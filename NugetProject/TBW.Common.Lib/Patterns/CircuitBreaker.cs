using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TBW.Common.Lib.Patterns
{
    public class CircuitBreaker
    {
        private readonly ICircuitBreakerStateStore stateStore = null; // CircuitBreakerStateStoreFactory.GetCircuitBreakerStateStore();

        private readonly object halfOpenSyncObject = new object();

        private readonly TimeSpan OpenToHalfOpenWaitTime = TimeSpan.FromSeconds(10);

        public bool IsClosed { get { return stateStore.IsClosed; } }

        public bool IsOpen { get { return !IsClosed; } }

        public void ExecuteAction(Action action)
        {

            if (IsOpen)
            {
                // The circuit breaker is Open.
                //... (see code sample below for details)

                // The circuit breaker is Open. Check if the Open timeout has expired.
                // If it has, set the state to HalfOpen. Another approach might be to
                // check for the HalfOpen state that had be set by some other operation.
                if (stateStore.LastStateChangedDateUtc + OpenToHalfOpenWaitTime < DateTime.UtcNow)
                {
                    // The Open timeout has expired. Allow one operation to execute. Note that, in
                    // this example, the circuit breaker is set to HalfOpen after being
                    // in the Open state for some period of time. An alternative would be to set
                    // this using some other approach such as a timer, test method, manually, and
                    // so on, and check the state here to determine how to handle execution
                    // of the action.
                    // Limit the number of threads to be executed when the breaker is HalfOpen.
                    // An alternative would be to use a more complex approach to determine which
                    // threads or how many are allowed to execute, or to execute a simple test
                    // method instead.
                    bool lockTaken = false;
                    try
                    {
                        Monitor.TryEnter(halfOpenSyncObject, ref lockTaken);
                        if (lockTaken)
                        {
                            // Set the circuit breaker state to HalfOpen.
                            stateStore.HalfOpen();

                            // Attempt the operation.
                            action();

                            // If this action succeeds, reset the state and allow other operations.
                            // In reality, instead of immediately returning to the Closed state, a counter
                            // here would record the number of successful operations and return the
                            // circuit breaker to the Closed state only after a specified number succeed.
                            this.stateStore.Reset();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        // If there's still an exception, trip the breaker again immediately.
                        this.stateStore.Trip(ex);

                        // Throw the exception so that the caller knows which exception occurred.
                        throw;
                    }
                    finally
                    {
                        if (lockTaken)
                        {
                            Monitor.Exit(halfOpenSyncObject);
                        }
                    }
                }
                // The Open timeout hasn't yet expired. Throw a CircuitBreakerOpen exception to
                // inform the caller that the call was not actually attempted,
                // and return the most recent exception received.
                throw new CircuitBreakerOpenException(stateStore.LastException);
            }

            // The circuit breaker is Closed, execute the action.
            try
            {
                action();
            }
            catch (Exception ex)
            {
                // If an exception still occurs here, simply
                // retrip the breaker immediately.
                this.TrackException(ex);

                // Throw the exception so that the caller can tell
                // the type of exception that was thrown.
                throw;
            }
        }

        private void TrackException(Exception ex)
        {
            // For simplicity in this example, open the circuit breaker on the first exception.
            // In reality this would be more complex. A certain type of exception, such as one
            // that indicates a service is offline, might trip the circuit breaker immediately.
            // Alternatively it might count exceptions locally or across multiple instances and
            // use this value over time, or the exception/success ratio based on the exception
            // types, to open the circuit breaker.
            this.stateStore.Trip(ex);
        }
    }

    public class CircuitBreakerStateStoreFactory
    {
        //public static ICircuitBreakerStateStore GetCircuitBreakerStateStore()
        //{
        //    return new CircuitBreakerStateStore();
        //}

        public static ICircuitBreakerStateStore GetCircuitBreakerStateStore(AbstractCircuitBreakerStateStore stateStore)
        {
            return stateStore;
        }
    }

    public abstract class AbstractCircuitBreakerStateStore : ICircuitBreakerStateStore
    {
        public virtual CircuitBreakerStateEnum State => throw new NotImplementedException();

        public  Exception LastException => throw new NotImplementedException();

        public virtual DateTime LastStateChangedDateUtc => throw new NotImplementedException();

        public virtual bool IsClosed => throw new NotImplementedException();

        public virtual void HalfOpen()
        {
            throw new NotImplementedException();
        }

        public virtual void Reset()
        {
            throw new NotImplementedException();
        }

        public virtual void Trip(Exception ex)
        {
            throw new NotImplementedException();
        }
    }

    public interface ICircuitBreakerStateStore
    {
        /// <summary>
        /// 断路器的当前状态
        /// </summary>
        CircuitBreakerStateEnum State { get; }

        Exception LastException { get; }

        DateTime LastStateChangedDateUtc { get; }
        /// <summary>
        /// 将断路器的状态切换为打开状态，并记录导致状态更改的异常，以及异常发生的日期和时间。
        /// </summary>
        /// <param name="ex"></param>
        void Trip(Exception ex);
        /// <summary>
        /// 关闭断路器
        /// </summary>
        void Reset();

        /// <summary>
        /// 将断路器设置为半开
        /// </summary>
        void HalfOpen();

        /// <summary>
        /// 如果断路器处于关闭状态，则 IsClosed 属性应为 true，如果处于打开或半开状态，则为 false
        /// </summary>
        bool IsClosed { get; }
    }

    public enum CircuitBreakerStateEnum
    {
        /// <summary>
        /// 将来自应用程序的请求路由到操作。 代理维护最近失败次数的计数，如果对操作的调用不成功，代理将递增此计数。 如果在给定时间段内最近失败次数超过指定的阈值，则代理将置于打开状态。 此时，代理会启动超时计时器，并且当此计时器过期时，代理将置于半开状态。
        /// </summary>
        Closed,
        /// <summary>
        /// 来自应用程序的请求立即失败，并向应用程序返回异常。
        /// </summary>
        Open,
        /// <summary>
        /// 允许数量有限的来自应用程序的请求通过并调用操作。 如果这些请求成功，则假定先前导致失败的问题已被修复，并且断路器将切换到关闭状态（失败计数器重置）。 如果有任何请求失败，则断路器将假定故障仍然存在，因此它会恢复到打开状态，并重新启动超时计时器，再给系统一段时间来从故障中恢复。
        /// * 半开状态对于防止恢复服务突然被大量请求淹没很有用。 在服务恢复的同时，它或许能够支持数量有限的请求，直至恢复完成；但当恢复正在进行时，大量的工作可能导致服务超时或再次失败。
        /// </summary>
        HalfOpen
    }

    public class CircuitBreakerOpenException : Exception
    {
        public CircuitBreakerOpenException(Exception lastException):base(lastException.Message)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TBW.Common.Lib.Patterns
{
    /// <summary>
    /// 重试模式的实现
    /// </summary>
    public class Retry
    {
        private int RetryCount { get; } = 3;

        private TimeSpan DefaultDelay = TimeSpan.FromSeconds(1);

        public Retry(int retryCount)
        {
            this.RetryCount = retryCount;
        }

        public Retry(int retryCount, TimeSpan delay)
            : this(retryCount)
        {
            this.DefaultDelay = delay;
        }

        public Retry(int retryCount, int seconds)
            : this(retryCount, TimeSpan.FromSeconds(seconds))
        {

        }

        public async Task OperationWithBasicRetryAsync(Func<Task> transientOperationAsync)
        {
            int currentRetry = 0;
            for (; ; )
            {
                try
                {              
                    // Call external service.
                    await transientOperationAsync();

                    // Return or break.
                    break;
                }
                catch (Exception ex)
                {
                    currentRetry++;
                    if (currentRetry > RetryCount || !IsTransient(ex))
                    {
                        throw;
                    }
                }
                await Task.Delay(DefaultDelay);
            }
        }

        /// <summary>
        /// 方法检查是否存在与在其中运行代码的环境相关的一组特定异常。 暂时性异常的定义根据正在访问的资源以及在其中执行操作的环境而异。
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public virtual bool IsTransient(Exception ex)
        {
            // Determine if the exception is transient.
            // In some cases this is as simple as checking the exception type, in other
            // cases it might be necessary to inspect other properties of the exception.
            if (ex is OperationTransientException)
                return true;

            if (ex is WebException)
            {
                var webException = ex as WebException;
                if (webException != null)
                {
                    // If the web exception contains one of the following status values
                    // it might be transient.
                    return new[] {WebExceptionStatus.ConnectionClosed,
                  WebExceptionStatus.Timeout,
                  WebExceptionStatus.RequestCanceled }.
                            Contains(webException.Status);
                }
            }            

            // Additional exception checking logic goes here.
            return false;
        }
    }

    [Serializable]
    public class OperationTransientException : Exception
    {
        public OperationTransientException() { }

            
    }
}

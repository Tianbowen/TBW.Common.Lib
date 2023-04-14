using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace TBW.Common.Lib.Extension.AssertExtension
{    
    [DebuggerNonUserCode]
    [DebuggerStepThrough]
    public static class AssertEx
    {        
        public static void Fail(string message, Exception exception = null)
        {
            throw new Exception(message, exception);
        }
                
        public static void True(            
            bool condition,
            string message = null,            
            string argumentExpression = null)
        {
            if (!condition)
                throw new ArgumentException(message ?? "Expected condition to be true", message == null ? argumentExpression : null);
        }

        public static void False(            
            bool condition,
            string message = null,            
            string argumentExpression = null)
        {
            if (condition)
                throw new ArgumentException(message ?? "Expected condition to be false", message == null ? argumentExpression : null);
        }
    
        public static T NotNull<T>(           
            this T obj,
            string message = null,        
            string argumentExpression = null)
            where T : class
        {
            if (obj == null)
            {
                throw new ArgumentException(
                    message ?? $"Expected object of type '{typeof(T).FullName}' to be not null",
                    message == null ? argumentExpression : null);
            }

            return obj;
        }

        
        public static T? NotNull<T>(            
            this T? obj,
            string message = null,
            string argumentExpression = null)
            where T : struct
        {
            if (obj == null)
            {
                throw new ArgumentException(
                    message ?? $"Expected object of type '{typeof(T).FullName}' to be not null",
                    message == null ? argumentExpression : null);
            }

            return obj;
        }

        
        public static string NotNullOrEmpty(        
            this string str,
            string message = null,        
            string argumentExpression = null)
        {
            if (string.IsNullOrEmpty(str))
                throw new ArgumentException(message ?? "Expected string to be not null or empty", message == null ? argumentExpression : null);
            return str;
        }
                
        public static string NotNullOrWhiteSpace(            
            this string str,
            string message = null,            
            string argumentExpression = null)
        {
            if (string.IsNullOrWhiteSpace(str))
                throw new ArgumentException(message ?? "Expected string to be not null or whitespace", message == null ? argumentExpression : null);
            return str;
        }

        
        public static void NotEmpty<T>(        
            IReadOnlyCollection<T> collection,
            string message = null,        
            string argumentExpression = null)
        {
            if (collection.NotNull(argumentExpression: argumentExpression).Count == 0)
                throw new ArgumentException(message ?? "Expected collection to be not empty", message == null ? argumentExpression : null);
        }
                
        public static void Empty<T>(            
            IReadOnlyCollection<T> collection,
            string message = null,            
            string argumentExpression = null)
        {
            if (collection.NotNull(argumentExpression: argumentExpression).Count > 0)
                throw new ArgumentException(message ?? "Expected collection to be empty", message == null ? argumentExpression : null);
        }
                
        public static void Count<T>(            
            IReadOnlyCollection<T> collection,
            int length,
            string message = null,            
            string argumentExpression = null)
        {
            if (collection.NotNull(argumentExpression: argumentExpression).Count != length)
                throw new ArgumentException(message ?? $"Expected collection to have length of {length}", message == null ? argumentExpression : null);
        }
                
        public static void HasSingleItem<T>(            
            IReadOnlyCollection<T> collection,
            string message = null,            
            string argumentExpression = null)
        {
            Count(collection, length: 1, message, argumentExpression);
        }

        public static void FileExists(string path, string message = null,  string argumentExpression = null)
        {
            if (!File.Exists(path.NotNull(argumentExpression)))
                throw new ArgumentException(message ?? $"Expected file to exist: {path}", message == null ? argumentExpression : null);
        }

        public static void DirectoryExists(string path, string message = null, string argumentExpression = null)
        {
            if (!Directory.Exists(path.NotNull(argumentExpression)))
                throw new ArgumentException(message ?? $"Expected directory to exist: {path}", message == null ? argumentExpression : null);
        }
    }
}

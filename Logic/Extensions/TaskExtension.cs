using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Logic.Extensions
{
    public static class TaskExtension
    {
        public static async Task<ResultOrException<T>> WrapResultOrException<T>(this Task<T> task, T defaultValue, ILogger logger)
        {
            try
            {
                var result = await task;
                return new ResultOrException<T>(result);
            }
            catch (Exception ex)
            {
                logger.LogError("Task failed", ex);
                
                return new ResultOrException<T>(defaultValue);
            }
        }

    }

    public class ResultOrException<T>
    {
        public ResultOrException(T result)
        {
            Result = result;
        }

        public T Result { get; set; }
    }
}
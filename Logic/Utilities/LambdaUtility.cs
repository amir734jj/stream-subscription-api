using System;

namespace Logic.Utilities
{
    public static class LambdaUtility
    {
        public static TResult IgnoreException<TReturn, TResult>(Func<TReturn> handler, Func<TReturn, TResult> thenHandler, Func<Exception, TResult> defaultHandler)
        {
            try
            {
                return thenHandler(handler());
            }
            catch (Exception e)
            {
                return defaultHandler(e);
            }
        }

        public static TResult Then<TResult>(Action action, TResult result)
        {
            action();

            return result;
        }
    }
}
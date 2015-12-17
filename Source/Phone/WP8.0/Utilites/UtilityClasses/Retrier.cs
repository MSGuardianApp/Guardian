using System;
using System.Threading.Tasks;

namespace SOS.Phone.Utilites.UtilityClasses
{
    public class Retrier<TResult>
    {
        public async Task<bool> TryWithDelay(Func<Task<bool>> func, int maxRetries, int delayInMilliseconds)
        {
            bool returnValue = false;
            int numTries = 0;
            bool succeeded = false;
            while (numTries < maxRetries)
            {
                try
                {
                    returnValue = await func();
                    if (returnValue)
                        succeeded = true;
                }
                catch (Exception)
                {
                }
                finally
                {
                    numTries++;
                }
                if (succeeded)
                    return returnValue;
                System.Threading.Thread.Sleep(delayInMilliseconds);
            }
            return false;
        }
    }
}

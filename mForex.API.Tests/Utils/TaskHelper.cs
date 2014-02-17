using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace mForex.API.Tests.Utils
{
    public static class TaskHelper
    {
        public async static Task<T> ThrowsAsync<T>(Func<Task> testCode) where T : Exception
        {
            try
            {
                await testCode();
                Assert.Throws<T>(() => { });
            }
            catch (T exception)
            {
                return exception;
            }
            return null;
        }    
    }
}

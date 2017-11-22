using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Grid.Test.Tools
{
    public static class TestTools
    {
        public static async Task<Exception> ThrowsAsync<TException>(Func<Task> func)
        {
            var expected = typeof(TException);
            Exception exception = null;
            Type actual = null;
            try
            {
                await func();
            }
            catch (Exception e)
            {
                actual = e.GetType();
                exception = e;
            }
            Assert.NotNull(exception);
            Assert.Equal(expected, actual);
            return exception;
        }

        public static Microsoft.EntityFrameworkCore.DbContextOptions<DbContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new Microsoft.Extensions.DependencyInjection.ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<DbContext>();
            builder.UseInMemoryDatabase()
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        private static Random rand = new Random(DateTime.Now.Millisecond);
        public static int rInt(int exclUB, int incLB = 1)
        {
            int t = rand.Next(incLB, exclUB);
            return t;
        }
    }
}

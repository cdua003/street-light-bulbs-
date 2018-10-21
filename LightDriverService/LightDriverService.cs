using System;
using System.Threading.Tasks;
using System.Threading;

namespace LightDriverService
{
    public class LightDriverService : ILightDriverService
    {
        

        public LightDriverService()
        {

        }

        private int GetRandomDelay()
        {
            return Convert.ToInt32(1000 * new Random().NextDouble() + 1000);
        }

        private Task<T> CreateReturningTask<T>(T returnValue)
        {
            var task = new Task<T>(() =>
            {
                return returnValue;
            });

            Timer _timer = null;
            _timer = new Timer(state =>
            {
                _timer.Dispose();
                task.Start(); // starting task will result in it finishing immediately - effectively meaning that we have a random time to retrieve
            }, null, GetRandomDelay(), Timeout.Infinite);

            return task;
        }

        public Task<bool> SwitchOffBulb(Guid bulbId)
        {
            return CreateReturningTask<bool>(true); // we simulate always turning on
        }

        public Task<bool> SwitchOffLight(Guid lightId)
        {
            return CreateReturningTask<bool>(true);
        }

        public Task<bool> SwitchOnBulb(Guid bulbId)
        {
            return CreateReturningTask<bool>(true); // we simulate always turning on
        }
    }
}

using DbDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightDriverService;
using Common;

namespace TestApplication.Repositories
{
    public class StreetlightRepository : IStreetlightRepository
    {
        private readonly IDataService _dataService;
        private readonly ILightDriverService _lightDriver;

        public StreetlightRepository(IDataService dataService
            , ILightDriverService lightDriver)
        {
            _dataService = dataService;
            _lightDriver = lightDriver;
        }

        public Task<IEnumerable<StreetlightSummaryModel>> GetAllStreetlights()
        {
            return _dataService.GetStreetlightListing();
        }

        public Task<StreetlightModel> GetStreetlight(Guid lightId)
        {
            return _dataService.GetStreetlightDetail(lightId);
        }

        public Task<bool> RegisterBulbFailure(Guid bulbId)
        {
            return _dataService.UpdateBulbStatus(bulbId, false, Common.FaultCode.GeneralFailure);
        }

        public Task<bool> RegisterFault(Guid bulbId, FaultCode fault)
        {
            return _dataService.UpdateBulbStatus(bulbId, false, fault);
        }

        public async Task<bool> SetFault(Guid bulbId, FaultCode fault)
        {
            // get bulb
            BulbState bulb = await _dataService.GetBulbState(bulbId);
            if (fault == FaultCode.None && bulb.BulbCurrentState.BulbTemperature > bulb.BulbInformation.MaxTemperature)
            {
                // for repair, we need to reset bulb temperature if it exceeds the max temperature
                bulb.BulbCurrentState.BulbTemperature = 0;
            }
            await _dataService.UpdateBulbStatus(bulb, fault == FaultCode.None ? bulb.BulbCurrentState.IsOn : false, fault, bulb.BulbCurrentState);
            return true;
        }

        public Task<bool> RegisterBulbOff(Guid bulbId)
        {

            return _dataService.UpdateBulbStatus(bulbId, false);
        }

        public Task<bool> RegisterBulbOn(Guid bulbId)
        {
            return _dataService.UpdateBulbStatus(bulbId, true);
        }

        public Task<Guid> ReplaceBulb(Guid originalBulbId)
        {
            return _dataService.ReplaceBulb(originalBulbId);
        }

        // simulation
        public async Task<bool> SetBulbTemperature(Guid bulbId, double temperature)
        {
            // get bulb
            BulbState bulb = await _dataService.GetBulbState(bulbId);
            bulb.BulbCurrentState.BulbTemperature = temperature;
            if (bulb.BulbCurrentState.IsOn)
            {
                // check over temperature
                if (!CanSwitchBulbOn(bulb))
                {
                    // fault condition!
                    await Task.WhenAll(_lightDriver.SwitchOffBulb(bulbId)
                        , _dataService.UpdateBulbStatus(bulb, false, FaultCode.OverTemperature, bulb.BulbCurrentState));
                    return false;
                }
            }

            // the bulb is off or not over temperature - no worries - log the data
            bulb.BulbCurrentState.BulbTemperature = temperature;

            await _dataService.UpdateBulbStatus(bulb, false, null, bulb.BulbCurrentState); // fire-forget
            return false;
        }

        public async Task<bool> SwitchOffBulb(Guid bulbId)
        {
            await Task.WhenAll(_lightDriver.SwitchOffBulb(bulbId)
                , _dataService.UpdateBulbStatus(bulbId, false));

            return true;
        }

        public async Task<BulbState> GetBulbState(Guid bulbId)
        {
            return await _dataService.GetBulbState(bulbId);
        }

        public async Task<bool> SwitchOnBulb(Guid bulbId)
        {
            // check bulb temperature before attempting switch on
            var bulbData = await _dataService.GetBulbState(bulbId);

            if (!CanSwitchBulbOn(bulbData))
            {
                return false; // do not switch it on!
            }
            else // switch on as instructed
            {
                await Task.WhenAll(_lightDriver.SwitchOnBulb(bulbId), _dataService.UpdateBulbStatus(bulbData, true));
            }
            return true;
        }

        private bool CanSwitchBulbOn(BulbState bulbState)
        {
            if (bulbState.BulbCurrentState.FaultCondition.HasValue && bulbState.BulbCurrentState.FaultCondition.Value != FaultCode.None)
            {
                return false;
            }
            if (bulbState.BulbCurrentState.BulbTemperature > bulbState.BulbInformation.MaxTemperature)
            {
                return false;
            }
            if (bulbState.BulbCurrentState.BulbHours > bulbState.BulbInformation.MaxHours)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> SwitchOffLight(Guid lightId)
        {
            var success = await _lightDriver.SwitchOffLight(lightId);
            var streetlightDetail = await _dataService.GetStreetlightDetail(lightId); // kills power to the light
            try
            {
                var tasks = new List<Task>();
                var countBulbs = streetlightDetail.Bulbs.Count();
                //foreach-loop uses more stack space for local variables compare with for-loop
                for (int i = 0; i < countBulbs; i++)
                {
                    int taskNo = i; // copy local for closure
                    var task = new Task(() =>
                    {
                        _dataService.UpdateBulbStatus(streetlightDetail.Bulbs.ElementAt(taskNo).BulbInformation.Id, false); // shut off all bulbs without mercy
                    });
                    tasks.Add(task);

#if DEBUG
                    //To run synchronously for debugging uncomment following code
                    //task.RunSynchronously();
#endif
                    task.Start();
                }
                await Task.WhenAll(tasks);
            }
            catch (Exception e)
            {
                // catch this so that don't throw an exception
                // log and handle faulted tasks
            }

            await _dataService.UpdateStreetlightStatus(lightId, false);

            return true;
        }

        public async Task<IEnumerable<Guid>> SwitchOnLight(Guid lightId)
        {
            // we switch on only bulbs that are valid
            var streetlightDetail = await _dataService.GetStreetlightDetail(lightId);
            IList<Guid> bulbsSwitchedOn = new List<Guid>();
            try
            {
                var tasks = new List<Task>();
                var bulbs = streetlightDetail.Bulbs.Where(f => CanSwitchBulbOn(f)).ToList();
                var countBulbs = bulbs.Count();
                //foreach-loop uses more stack space for local variables compare with for-loop
                for (int i = 0; i < countBulbs; i++)
                {
                    int taskNo = i; // copy local for closure
                    tasks.Add(Task.Run(async () =>
                    {
                        if (!(await _lightDriver.SwitchOnBulb(bulbs[taskNo].BulbInformation.Id)))
                        {
                            return;
                        }
                        lock (bulbsSwitchedOn)
                        {
                            bulbsSwitchedOn.Add(bulbs[taskNo].BulbInformation.Id);
                        }
                        await _dataService.UpdateBulbStatus(bulbs[taskNo], true);
                    }));
                }
                await Task.WhenAll(tasks);
            }
            catch (Exception e)
            {
                // catch this so that don't throw an exception
                // log and handle faulted tasks
            }

            await _dataService.UpdateStreetlightStatus(lightId, true);

            return bulbsSwitchedOn;
        }

        public Task<bool> SetAmbientLightLevel(int lumens)
        {
            throw new NotImplementedException();
        }
    }
}
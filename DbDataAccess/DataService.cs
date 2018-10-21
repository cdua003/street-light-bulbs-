using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;

namespace DbDataAccess
{
    public class DataService : IDataService
    {
        private Task<T> GetTask<T>(T returnValue)
        {
            var task = new Task<T>(() =>
            {
                return returnValue;
            });

            task.Start();
            return task;
        }

        public async Task<BulbState> GetBulbState(Guid bulbId)
        {
            return await GetTask(DbData.Bulbs.Single(x => x.BulbInformation.Id == bulbId));
        }
        public async Task<StreetlightModel> GetStreetlightDetail(Guid lightId)
        {
            return await GetTask(DbData.Streetlights.Single(x => x.Id == lightId));
        }

        public async Task<IEnumerable<StreetlightSummaryModel>> GetStreetlightListing()
        {
            return await GetTask(DbData.StreetlightSummary);
        }

        public Task<Guid> ReplaceBulb(Guid bulbId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateBulbStatus(Guid bulbId, bool? isOn, FaultCode? fault = default(FaultCode?), BulbData bulbData = null)
        {
            var bulb = await GetBulbState(bulbId);

            return await UpdateBulbStatus(bulb, isOn, fault, bulbData);
        }

        public async Task<bool> UpdateBulbStatus(BulbState bulbState, bool? isOn, FaultCode? fault = default(FaultCode?), BulbData bulbData = null)
        {
            // get the current state
            var hasUpdate = false;
            if (bulbData != null)
            {
                bulbState.BulbCurrentState = bulbData;
                hasUpdate = true;
            }

            if (isOn.HasValue)
            {
                bulbState.BulbCurrentState.IsOn = isOn.Value;
                hasUpdate = true;
            }

            if (fault.HasValue)
            {
                bulbState.BulbCurrentState.FaultCondition = fault.Value;
                hasUpdate = true;
            }

            if (hasUpdate)
            {
                var bulb = await GetTask(DbData.Bulbs.Single(x => x.BulbInformation.Id == bulbState.BulbInformation.Id));
                bulb.BulbCurrentState = bulbState.BulbCurrentState;
                return true;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> UpdateStreetlightStatus(Guid lightId, bool isOn)
        {
            var streetlight = await (GetTask(DbData.Streetlights.Single(x => x.Id == lightId)));
            streetlight.IsSwitchedOn = isOn;

            return true;
        }
    }
}

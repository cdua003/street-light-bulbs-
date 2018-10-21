using Common;
using DbDataAccess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestApplication.Repositories
{
    public interface IStreetlightRepository
    {
        // the following allow the web interface to read data from the streetlight DB
        Task<IEnumerable<StreetlightSummaryModel>> GetAllStreetlights();

        Task<StreetlightModel> GetStreetlight(Guid lightId);

        Task<BulbState> GetBulbState(Guid bulbId);
        
        // the following allow the web interface / light-sensor to switch lights on/off
        Task<IEnumerable<Guid>> SwitchOnLight(Guid lightId);
        Task<bool> SwitchOffLight(Guid lightId);

        Task<bool> SwitchOnBulb(Guid bulbId);

        Task<bool> SwitchOffBulb(Guid bulbId);

        // the following are called by the light-driver and must not be changed
        Task<bool> RegisterBulbOn(Guid bulbId);
        Task<bool> RegisterBulbOff(Guid bulbId);
        Task<Guid> ReplaceBulb(Guid originalBulbId);
        Task<bool> RegisterFault(Guid bulbId, FaultCode fault);
        Task<bool> RegisterBulbFailure(Guid bulbId);
        Task<bool> SetBulbTemperature(Guid bulbId, double temperature);
        Task<bool> SetFault(Guid bulbId, FaultCode fault);


        // the following are used by the light-sensor-driver and must not be changed
        Task<bool> SetAmbientLightLevel(int lumens);
    }
}

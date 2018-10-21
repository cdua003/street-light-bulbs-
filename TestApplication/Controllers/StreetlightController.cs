using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using TestApplication.Repositories;
using DbDataAccess;
using Common;

namespace TestApplication.Controllers
{
    [RoutePrefix("api/streetlights")]
    public class StreetlightController : ApiController
    {
        private readonly IStreetlightRepository _dataLayer;
        public StreetlightController(IStreetlightRepository streetlightRepository)
        {
            _dataLayer = streetlightRepository;
        }

        [HttpPost]
        [Route("{lightId:guid}/on")]
        public async Task<IEnumerable<Guid>> SwitchLightOn(Guid lightId) // returns list of Bulbs that responded and switched on
        {
            return await _dataLayer.SwitchOnLight(lightId);
        }

        [HttpPost]
        [Route("{lightId:guid}/off")]
        public async Task<bool> SwitchLightOff(Guid lightId)
        {
            return await _dataLayer.SwitchOffLight(lightId);
        }

        [HttpGet]
        [Route("bulb/{bulbId:guid}")]
        public async Task<BulbState> GetBulbDetails(Guid bulbId)
        {
            return await _dataLayer.GetBulbState(bulbId);
        }

        [HttpPost]
        [Route("bulb/{bulbId:guid}/on")]
        public async Task<bool> SwitchBulbOn(Guid bulbId) // returns list of Bulbs that responded and switched on
        {
            return await _dataLayer.SwitchOnBulb(bulbId);
        }

        [HttpPost]
        [Route("bulb/{bulbId:guid}/off")]
        public async Task<bool> SwitchBulbOff(Guid bulbId)
        {
            return await _dataLayer.SwitchOffBulb(bulbId);
        }

        [Route("")]
        public async Task<IEnumerable<StreetlightSummaryModel>> GetAllStreetLights()
        {
            return await _dataLayer.GetAllStreetlights();
        }

        [Route("{lightId:guid}")]
        public async Task<StreetlightModel> GetStreetLight(Guid lightId)
        {
            return await _dataLayer.GetStreetlight(lightId);
        }

        // Simulation context options
        [HttpPost]
        [Route("fault/{bulbId:guid}/{faultCode:int}")]
        public async Task<bool> SimulateFault(Guid bulbId, int faultCode)
        {
            return await _dataLayer.RegisterFault(bulbId, (FaultCode)faultCode);
        }

        [HttpPost]
        [Route("fail/{bulbId:guid}")]
        public async Task<bool> FailBulb(Guid bulbId)
        {
            return await _dataLayer.RegisterBulbFailure(bulbId);
        }

        // Simulation bulb fault state
        [HttpPost]
        [Route("setfault/{bulbId:guid}/{faultCode:int}")]
        public async Task<bool> SetFault(Guid bulbId, int faultCode)
        {
            return await _dataLayer.SetFault(bulbId, (FaultCode)faultCode);
        }
    }
}
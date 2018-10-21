using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbDataAccess
{
    public interface IDataService
    {
        Task<bool> UpdateBulbStatus(BulbState bulbState, bool? isOn, FaultCode? fault = default(FaultCode?), BulbData bulbData = null);
        Task<bool> UpdateBulbStatus(Guid bulbId, bool? isOn, FaultCode? fault = default(FaultCode?), BulbData bulbData = null);
        Task<bool> UpdateStreetlightStatus(Guid lightId, bool isOn);
        Task<Guid> ReplaceBulb(Guid bulbId);
        Task<IEnumerable<StreetlightSummaryModel>> GetStreetlightListing();
        Task<StreetlightModel> GetStreetlightDetail(Guid lightId);

        Task<BulbState> GetBulbState(Guid bulbId);
    }
}

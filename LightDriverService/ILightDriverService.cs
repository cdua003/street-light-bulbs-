using System;
using System.Threading.Tasks;

namespace LightDriverService
{
    public interface ILightDriverService
    {
        // switch on the specified bulb
        Task<bool> SwitchOnBulb(Guid bulbId);
        // switch off the specified bulb
        Task<bool> SwitchOffBulb(Guid bulbId);

        // switch off the specified light
        Task<bool> SwitchOffLight(Guid lightId);
    }
}

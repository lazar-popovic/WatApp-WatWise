using API.Common;
using API.Models.ViewModels;
using API.Services.DeviceScheduling.Interfaces;

namespace API.Services.DeviceScheduling.Implementations;

public class DeviceScheduler : IDeviceScheduler
{
    public Task ExecuteJob(int deviceId, bool turn)
    {
        throw new NotImplementedException();
    }

    public Task<Response?> ScheduleJob(DeviceJobViewModel request)
    {
        throw new NotImplementedException();
    }
}
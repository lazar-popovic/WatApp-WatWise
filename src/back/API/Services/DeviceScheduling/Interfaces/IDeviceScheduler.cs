using API.Common;
using API.Models.ViewModels;

namespace API.Services.DeviceScheduling.Interfaces;

public interface IDeviceScheduler
{
    Task ExecuteJob(int deviceId, bool turn);
    Task<Response?> ScheduleJob(DeviceJobViewModel request);
}
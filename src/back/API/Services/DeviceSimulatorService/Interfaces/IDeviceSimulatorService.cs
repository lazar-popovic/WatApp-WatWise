
using API.Models.ViewModels;

namespace API.Services.DeviceSimulatorService.Interfaces;

public interface IDeviceSimulatorService
{
    Task HourlyUpdate();
    Task FillDataSinceJanuary1st(int type, int deviceId);
    Task SetCurrentDataTo0IfOff();
}
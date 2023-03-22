
using API.Models.ViewModels;

namespace API.Services.DeviceSimulatorService.Interfaces;

public interface IDeviceSimulatorService
{
    Task<List<ElectricalUsageViewModel>> GetUsageForDeviceBetweenDates(string device, DateTime startingDate, DateTime endingDate);
    Task HourlyUpdate();
}
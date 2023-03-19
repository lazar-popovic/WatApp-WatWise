using API.Models.Dto;

namespace API.Services.DeviceSimulatorService.Interfaces;

public interface IDeviceSimulatorService
{
    Task<List<ElectricalUsage>> GetUsageForDeviceBetweenDates(string device, DateTime startingDate, DateTime endingDate);
}
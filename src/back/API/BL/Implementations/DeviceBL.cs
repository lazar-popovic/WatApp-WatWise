using API.DAL.Implementations;
using API.DAL.Interfaces;
using API.Models.Entity;
using API.Models;
using API.BL.Interfaces;
using API.Models.ViewModels;
using API.Common;

namespace API.BL.Implementations
{
    public class DeviceBL : IDeviceBL
    {
        private readonly IDeviceDAL _ideviceDal;

        public DeviceBL(IDeviceDAL deviceDal)
        {
            _ideviceDal = deviceDal;
        }
        public async Task<Response<Device>> GetByIdAsync(int id)
        {
            var response = new Response<Device>();

            var device = await _ideviceDal.GetDeviceByIdAsync(id);

            if (device == null)
            {
                response.Errors.Add("Device doesn't exist!");
                response.Success = false;

                return response;
            }

            response.Data = device!;
            response.Success = response.Errors.Count() == 0;

            return response;
        }

        public async Task<Response<List<Device>>> GetDevice()
        {
            var response = new Response<List<Device>>();

            var devices = await _ideviceDal.GetAllDevicesAsync();

            if (devices == null)
            {
                response.Errors.Add("Error with displaying device from base!");
                response.Success = false;

                return response;
            }

            response.Data = devices!;
            response.Success = response.Errors.Count() == 0;

            return response;
        }
        public async Task<Response<String>> AddDevice(Device device)
        {
            var response = new Response<String>();
            var dev = device;
            if (dev == null)
            {
                response.Errors.Add("Device is null");
                response.Success = false;

                return response;
            }
            await _ideviceDal.AddDeviceAsync(device);
            response.Data = "Pass";

            response.Success = response.Errors.Count() == 0;

            return response;



        }
        public async Task<Response<String>> DeleteDevice(int id)
        {
            var response = new Response<String>();
            var findwithid = await _ideviceDal.GetDeviceByIdAsync(id);

            if (findwithid == null)
            {
                response.Errors.Add("Device doesn't exist");
                response.Success = false;

                return response;
            }
            await _ideviceDal.DeleteDeviceAsync(findwithid);

            response.Data = "Device has been removed successfully!";

            response.Success = response.Errors.Count() == 0;

            return response;
        }
        public async Task<Response<String>> UpdateDevice(int id, DeviceNameAndDataShareUpdateViewModel request)
        {
            var response = new Response<String>();

            var device = await _ideviceDal.GetWholeDeviceByIdAsync(id);

            if (device == null)
            {
                response.Errors.Add("Device with this id doesn't exist!");
                response.Success = response.Errors.Count == 0;

                return response;
            }

            if (!string.IsNullOrEmpty(request.Name))
                device.Name = request.Name;

            device.DataShare = request.DataShare!.Value;

            await _ideviceDal.UpdateDeviceAsync(device);

            response.Data = "Device name and data share status has been updated succesfully!";

            response.Success = response.Errors.Count() == 0;

            return response;

        }

        public async Task<Response<String>> AddDeviceViewModel(DeviceViewModel device)
        {
            var response = new Response<String>();
            var dev = device;

            if (string.IsNullOrEmpty(dev.Name?.Trim()))
            {
                response.Errors.Add("Name is required!");
            }
            if (dev.DeviceTypeId <= 0)
            {
                response.Errors.Add("Type must be selected!");
            }
            if (dev.DeviceSubtypeId <= 0)
            {
                response.Errors.Add("Subtype must be selected!");
            }

            response.Success = !response.Errors.Any();

            if (response.Success == false)
                return response;

            await _ideviceDal.AddDeviceViewModel(device);

            response.Data = $"Device {dev.Name} connected successfully! {dev.DeviceSubtypeId}";


            return response;



        }

        public async Task<Response<List<DeviceType>>> GetDeviceTypesByCategory(int id)
        {
            var response = new Response<List<DeviceType>>();

            var devices = await _ideviceDal.GetDeviceTypesByCategory(id);

            if (devices == null)
            {
                response.Errors.Add("Error with displaying device from base!");
                response.Success = false;

                return response;
            }

            response.Data = devices!;
            response.Success = response.Errors.Count() == 0;

            return response;
        }

        public Response<object> GetDevicesByUserId(int userId)
        {
            var response = new Response<object>();
            response.Success = true;
            response.Data = _ideviceDal.GetDevicesByUserId(userId);
            return response;
        }

        public async Task<Response<RegisterResponseViewModel>> TurnDevicesOnOff(DeviceControlViewModel request)
        {
            var response = new Response<RegisterResponseViewModel>();

            if (request.DevicesOn == false)
            {
                await _ideviceDal.TurnDevicesOff();

                response.Success = true;
                response.Data = new RegisterResponseViewModel { Message = "Devices turned off succesfully!" };

                return response;
            }
            else
            {
                await _ideviceDal.TurnDevicesOn();

                response.Success = true;
                response.Data = new RegisterResponseViewModel { Message = "Devices turned on succesfully!" };

                return response;
            }
        }

        public async Task<Response<RegisterResponseViewModel>> ShareDeviceDataWithDSO(DeviceControlViewModel request)
        {
            var response = new Response<RegisterResponseViewModel>();

            if (request.DevicesOn == false)
            {
                await _ideviceDal.TurnDataSharingOff();

                response.Success = true;
                response.Data = new RegisterResponseViewModel { Message = "Devices data sharing with DSO turned off succesfully!" };

                return response;
            }
            else
            {
                await _ideviceDal.TurnDataSharingOn();

                response.Success = true;
                response.Data = new RegisterResponseViewModel { Message = "Devices data sharing with DSO turned on succesfully!" };

                return response;
            }
        }

        public async Task<Response<object>> Top3DevicesByUserId(int userId)
        {
            var response = new Response<object>();
            response.Success = true;
            response.Data = await _ideviceDal.Top3DevicesByUserId(userId);
            return response;
        }

        public async Task<Response<RegisterResponseViewModel>> TurnDevicesOnOffById(DeviceControlViewModel request, int deviceId)
        {
            var response = new Response<RegisterResponseViewModel>();

            if (request.DevicesOn == false)
            {
                var resp = await _ideviceDal.TurnDeviceOffById(deviceId);

                if (resp.Success == false)
                {
                    resp.Errors.Add("Error! Failed to turn off the device!");
                    return resp;
                }

                response.Success = true;
                response.Data = new RegisterResponseViewModel { Message = "Device turned off succesfully!" };

                return response;
            }
            else
            {
                var resp = await _ideviceDal.TurnDeviceOnById(deviceId);

                if (resp.Success == false)
                {
                    resp.Errors.Add("Error! Failed to turn on the device!");
                    return resp;
                }

                response.Success = true;
                response.Data = new RegisterResponseViewModel { Message = "Device turned on succesfully!" };

                return response;
            }
        }

        public async Task<Response<RegisterResponseViewModel>> ShareDeviceDataWithDSOById(DeviceControlViewModel request, int deviceId)
        {
            var response = new Response<RegisterResponseViewModel>();

            if (request.DevicesOn == false)
            {
                var resp = await _ideviceDal.ShareDataOffById(deviceId);

                if (resp.Success == false)
                {
                    resp.Errors.Add("Error! Failed to turn off data sharing!");
                    return resp;
                }

                response.Success = true;
                response.Data = new RegisterResponseViewModel { Message = "Device data sharing with DSO turned off succesfully!" };

                return response;
            }
            else
            {
                var resp = await _ideviceDal.ShareDataOnById(deviceId);

                if (resp.Success == false)
                {
                    resp.Errors.Add("Error! Failed to turn on data sharing!");
                    return resp;
                }

                response.Success = true;
                response.Data = new RegisterResponseViewModel { Message = "Device data sharing with DSO turned on succesfully!" };

                return response;
            }
        }

        public async Task<Response> GetDeviceSubtypesByType(int deviceTypeId)
        {
            {
                var response = new Response();

                var subtypes = await _ideviceDal.GetDeviceSubtypesByType(deviceTypeId);

                if (subtypes == null)
                {
                    response.Errors.Add("Error with displaying sybtypes from database!");
                    response.Success = false;

                    return response;
                }

                response.Data = subtypes!;
                response.Success = response.Errors.Count() == 0;

                return response;
            }
        }
    }
}

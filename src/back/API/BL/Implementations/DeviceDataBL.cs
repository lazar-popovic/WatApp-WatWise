using API.BL.Interfaces;
using API.Common;
using API.DAL.Interfaces;
using API.Models;
using API.Models.Entity;
using System;

namespace API.BL.Implementations;

public class DeviceDataBL : IDeviceDataBL
{
    private readonly IDeviceDataDAL _deviceDataDal;

    public DeviceDataBL(IDeviceDataDAL deviceDataDal)
    {
        _deviceDataDal = deviceDataDal;
    }
    
    public async Task<Response<object>> GetDeviceDataForToday(int day, int month, int year, int deviceId)
    {
        var response = new Response<object>();

        response.Success = true;
        response.Data = await _deviceDataDal.GetDeviceDataForToday(day, month, year, deviceId);

        return response;
    }

    public async Task<Response<object>> GetAllDevicesDataWhereShareWithDsoIsAllowedForToday(int day, int month, int year)
    {
        var response = new Response<object>();

        response.Success = true;
        response.Data = await _deviceDataDal.GetAllDevicesDataWhereShareWithDsoIsAllowedForToday(day, month, year);

        return response;
    }

    public async Task<Response> GetAllDevicesDataWhereShareWithDsoIsAllowedForNextNDays(int numberOfDays)
    {
        var response = new Response();

        if(numberOfDays != 1 &&  numberOfDays != 3 && numberOfDays != 7)
        {
            response.Errors.Add("Can only return prediction for 1,3 and 7 days interval!");
            response.Success = response.Errors.Count == 0;

            return response;
        }

        if(numberOfDays == 1)
        {
            response.Success = true;
            response.Data = await _deviceDataDal.GetAllDevicesDataWhereShareWithDsoIsAllowedForTomorrowPrediction();

            return response;
        }
        else if(numberOfDays == 3)
        {
            response.Success = true;
            response.Data = await _deviceDataDal.GetAllDevicesDataWhereShareWithDsoIsAllowedForNext3DaysPrediction();

            return response;
        }
        else
        {
            response.Success = true;
            response.Data = await _deviceDataDal.GetAllDevicesDataWhereShareWithDsoIsAllowedForNext7DaysPrediction();

            return response;
        }
    }

    public async Task<Response> GetDeviceDataForNextNDays(int id, int numberOfDays)
    {
        var response = new Response();

        if (numberOfDays != 1 && numberOfDays != 3 && numberOfDays != 7)
        {
            response.Errors.Add("Can only return prediction for 1,3 and 7 days interval!");
            response.Success = response.Errors.Count == 0;

            return response;
        }

        if (numberOfDays == 1)
        {
            response.Success = true;
            response.Data = await _deviceDataDal.GetDeviceDataForTomorrowPrediction(id);

            return response;
        }
        else if (numberOfDays == 3)
        {
            response.Success = true;
            response.Data = await _deviceDataDal.GetDeviceDataForNext3DaysPrediction(id);

            return response;
        }
        else
        {
            response.Success = true;
            response.Data = await _deviceDataDal.GetDeviceDataForNext7DaysPrediction(id);

            return response;
        }
    }

    public async Task<Response<object>> GetDeviceDataForMonth( int month, int year, int deviceId)
    {
        var response = new Response<object>();

        response.Success = true;
        response.Data = await _deviceDataDal.GetDeviceDataForMonth(month, year,deviceId);

        return response;
    }

    public async Task<Response<object>> GetAllDevicesDataWhereShareWithDsoIsAllowedForMonth(int month, int year)
    {
        var response = new Response<object>();

        response.Success = true;
        response.Data = await _deviceDataDal.GetAllDevicesDataWhereShareWithDsoIsAllowedForMonth( month, year);

        return response;
    }

    public async Task<Response> GetDeviceDataForCategoryAndProsumerIdForMonth(int month, int year, int category, int userId)
    {
        var response = new Response();

        response.Success = true;
        response.Data = await _deviceDataDal.GetDeviceDataForCategoryAndProsumerIdForMonth(month, year, category, userId);

        return response;
    }

    public async Task<Response<object>> GetDeviceDataForYear( int year, int deviceId)
    {
        var response = new Response<object>();

        response.Success = true;
        response.Data = await _deviceDataDal.GetDeviceDataForYear( year, deviceId);

        return response;
    }

    public async Task<Response> GetAllDevicesDataWhereShareWithDsoIsAllowedForYear(int year)
    {
        var response = new Response();

        response.Success = true;
        response.Data = await _deviceDataDal.GetAllDevicesDataWhereShareWithDsoIsAllowedForYear(year);

        return response;
    }

    public async Task<Response> GetDeviceDataForCategoryAndProsumerIdForYear(int year, int category, int userId)
    {
        var response = new Response();

        response.Success = true;
        response.Data = await _deviceDataDal.GetDeviceDataForCategoryAndProsumerIdForYear(year, category, userId);

        return response;
    }

    public async Task<Response<object>> GetDayTotalProductionConsumptionByUserId( int day, int month, int year, int userId)
    {
        var response = new Response<object>();

        response.Success = true;
        response.Data = await _deviceDataDal.GetDayTotalProductionConsumptionByUserId( day, month, year, userId);

        return response;
    }

    public async Task<Response> GetTotalConsumptionForPrevious7HoursAndTotalProductionForNext7HoursForAllUsers()
    {
        var response = new Response();

        response.Success = true;
        response.Data = await _deviceDataDal.GetTotalConsumptionForPrevious7HoursAndTotalProductionForNext7HoursForAllUsers();

        return response;
    }
}
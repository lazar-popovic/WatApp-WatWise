﻿using API.DAL.Implementations;
using API.DAL.Interfaces;
using API.Models.Entity;
using API.Models;
using API.BL.Interfaces;

namespace API.BL.Implementations
{
    public class LocationBL:ILocationBL
    {
        private readonly ILocationDAL _ilocationDal;

        public LocationBL(ILocationDAL locationDAL)
        {
            _ilocationDal = locationDAL;
        }

        public async Task<Response<List<Location>>> GetAllLocation()
        {
            var response = new Response<List<Location>>();

            var locations = await _ilocationDal.GetAllLocationsAsync();

            if (locations == null)
            {
                response.Errors.Add("Error with displaying location from base!");
                response.Success = false;

                return response;
            }

            response.Data = locations;
            response.Success = response.Errors.Count() == 0;

            return response;
        }

    }
}
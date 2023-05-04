﻿using API.DAL.Implementations;
using API.DAL.Interfaces;
using API.Models.Entity;
using API.Models;
using API.BL.Interfaces;
using API.Models.DTOs;

namespace API.BL.Implementations
{
    public class LocationBL:ILocationBL
    {
        private readonly ILocationDAL _ilocationDal;

        public LocationBL(ILocationDAL locationDAL)
        {
            _ilocationDal = locationDAL;
        }

        public async Task<Response<List<LocationWithPowerUsageDTO>>> GetAllLocation()
        {
            var response = new Response<List<LocationWithPowerUsageDTO>>();

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

        public async Task<Response<List<String>>> GetAllLocationDistinctCity()
        {
            var response = new Response<List<String>>();

            var locations = await _ilocationDal.GetAllLocationsCity();
            if (locations == null)
            {
                response.Errors.Add("Error with displaying location!");
                response.Success = false;

                return response;
            }
            response.Data = locations;
            response.Success = response.Errors.Count() == 0;

            return response;


        }
        public async Task<Response<List<String>>> GetAllNeighborhood(string city)
        {
            var response = new Response<List<String>>();

            var neighborhood = await _ilocationDal.GetAllNeighborhood(city);
            if (neighborhood == null)
            {
                response.Errors.Add("Error with displaying neighborhood!");
                response.Success = false;

                return response;
            }
            response.Data = neighborhood;
            response.Success = response.Errors.Count() == 0;

            return response;


        }
        public async Task<Response<List<LocationWithPowerUsageDTO>>> GetAllLocationWithNeighborhood(string city, string neighborhood)
        {
            var response = new Response<List<LocationWithPowerUsageDTO>>();

            var locations = await _ilocationDal.GetAllLocationWithNeighborhood(city,neighborhood);
            if (locations == null)
            {
                response.Errors.Add("Error with displaying location!");
                response.Success = false;

                return response;
            }
            response.Data = locations;
            response.Success = response.Errors.Count() == 0;

            return response;


        }
    }
}

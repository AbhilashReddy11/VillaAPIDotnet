﻿using VillaAPI_Utility;
using VillaAPI_Web.Models.Dto;
using VillaAPI_Web.Services.IServices;

namespace VillaAPI_Web.Services
{
    public class VillaService : BaseService, IVillaService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string villaUrl;

        public VillaService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");

        }

        public Task<T> CreateAsync<T>(VillaCreateDTO dto, string Token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = villaUrl + "/api/v1/villaAPI",
                Token = Token

            });
        }

        public Task<T> DeleteAsync<T>(int id, string Token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = villaUrl + "/api/v1/villaAPI/" + id,
                Token = Token

            });
        }

        public Task<T> GetAllAsync<T>(string Token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaUrl + "/api/v1/villaAPI",
                Token = Token

            });
        }

        public Task<T> GetAsync<T>(int id, string Token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaUrl + "/api/v1/villaAPI/" + id,
                Token = Token

            });
        }

        public Task<T> UpdateAsync<T>(VillaUpdateDTO dto, string Token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = villaUrl + "/api/v1/villaAPI/" + dto.Id,
                Token = Token

            }) ;
        }
    }
}

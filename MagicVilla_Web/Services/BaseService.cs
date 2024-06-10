﻿using MagicVilla_Utility;
using MagicVilla_Web.Models;
using Newtonsoft.Json;
using System;
using System.Text;

namespace MagicVilla_Web.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get;set;}


        //this is uses to call the API
        public IHttpClientFactory httpClient {  get; set;}
        
        public BaseService(IHttpClientFactory httpClient)
        {
            this.responseModel = new();
            this.httpClient = httpClient;
        }
        //above http  client is used to call the API
        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                //Create HttpClient:


                var client = httpClient.CreateClient("MagicAPI");
                //Create HttpRequestMessage
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                //data will not be null in post/put http calls
                //Set Request Content (if any)
                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                          Encoding.UTF8, "application/json");
                    
                }
                //Set HTTP Method:
                switch (apiRequest.ApiType)
                {
                    case SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }
                HttpResponseMessage apiResponse = null;

                //Send the Request and Get Response
                apiResponse = await client.SendAsync(message);
                //Handle the Response
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                try
                {
                     APIResponse ApiResponse =JsonConvert.DeserializeObject<APIResponse>(apiContent);
                    if(ApiResponse.StatusCode==System.Net.HttpStatusCode.BadRequest
                        || ApiResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        ApiResponse.StatusCode =System.Net.HttpStatusCode.BadRequest;
                        ApiResponse.IsSuccess = false;
                        var res = JsonConvert.SerializeObject(ApiResponse);
                        var returnObj = JsonConvert.DeserializeObject<T>(res);
                        return returnObj;
                    }
                }
                catch (Exception ex)
                {
                    var exceptionResponse = JsonConvert.DeserializeObject<T>(apiContent);
                    return exceptionResponse;
                }
                var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return APIResponse;
            }
            catch (Exception ex)
            {
                var dto = new APIResponse
                {
                    ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
                    IsSuccess = false
                };
                
            var res = JsonConvert.SerializeObject(dto);
            var APIResponse = JsonConvert.DeserializeObject<T>(res);
            return APIResponse;
            }
                
        }
    }
}

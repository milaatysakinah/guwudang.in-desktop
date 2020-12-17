﻿using Velacro.Api;
using Velacro.Basic;
using System.Net.Http;
using guwudang.Model;
using System.Collections.Generic;
using guwudang.utils;
using System;

namespace guwudang.Product
{
    public class ProductController : MyController
    {
        private static string id;

        public ProductController(IMyView _myView) : base(_myView) { }

        public async void product()
        {
            var client = new ApiClient("http://localhost:8000/");
            var request = new ApiRequestBuilder();
            

            User user = new User();
            string token = user.getToken();
            client.setAuthorizationToken(token);

            var reqAccount = request
                .buildHttpRequest()
                .setEndpoint("api/authUser")
                .setRequestMethod(HttpMethod.Get);
            client.setOnSuccessRequest(setSuccessAuthorization);
            client.setOnFailedRequest(setFailedAuthorization);
            var response1 = await client.sendRequest(request.getApiRequestBundle());
            
            Console.WriteLine(response1.getHttpResponseMessage().Content);
           
            
        }

        public async void nextProduct(string id)
        {
            string _endpoint = "api/searchProductByUserID/?id=:id";

            _endpoint = _endpoint.Replace(":id", id);
            //Console.WriteLine(_endpoint);

            var client2 = new ApiClient("http://localhost:8000/");
            var request2 = new ApiRequestBuilder();
            var req = request2
                .buildHttpRequest()
                .setEndpoint(_endpoint)
                .setRequestMethod(HttpMethod.Get);
            client2.setOnSuccessRequest(setViewProductData);
            var response = await client2.sendRequest(request2.getApiRequestBundle());
        }

        public void searchProduct(string key)
        {
            var client = new ApiClient("http://localhost:8000/");
            var request = new ApiRequestBuilder();
            string _endpoint = "api/searchProduct/?id=:id&search=:search";

            _endpoint = _endpoint.Replace(":id", id);
            _endpoint = _endpoint.Replace(":search", key);
            var req = request
                .buildHttpRequest()
                .setEndpoint(_endpoint)
                .setRequestMethod(HttpMethod.Get);
            client.setOnSuccessRequest(setViewProductData);
            var response = client.sendRequest(request.getApiRequestBundle());
        }

        public async void deleteProduct(List<string> selectedItemsID)
        {
            var client = new ApiClient("http://localhost:8000/");
            var request = new ApiRequestBuilder();
            foreach (string item in selectedItemsID)
            {
                //Console.WriteLine(item);
                string _endpoint = "api/product/:id";

                _endpoint = _endpoint.Replace(":id", item);
                var req = request
                    .buildHttpRequest()
                    .setEndpoint(_endpoint)
                    .setRequestMethod(HttpMethod.Delete);
                var response =  client.sendRequest(request.getApiRequestBundle());
            }
            product();
        }

        private void setViewProductData(HttpResponseBundle _response)
        {
            if (_response.getHttpResponseMessage().Content != null)
            {
                string status = _response.getHttpResponseMessage().ReasonPhrase;
                getView().callMethod("setProduct", _response.getParsedObject<List<guwudang.Model.Product>>());
            }
        }

        private void setFailedAuthorization(HttpResponseBundle _response)
        {
            if (_response.getHttpResponseMessage().Content != null)
            {
                getView().callMethod("backToLogin");
            }
        }

        private void setSuccessAuthorization(HttpResponseBundle _response)
        {
            if (_response.getHttpResponseMessage().Content != null)
            {
                id = _response.getJObject()["user"]["id"].ToString();
                nextProduct(id);
            }
        }
    }
}

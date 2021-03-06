﻿using Velacro.Api;
using Velacro.Basic;
using System.Net.Http;
using guwudang.Model;
using System.Collections.Generic;
using guwudang.utils;
using System;
using System.IO;

namespace guwudang.CreateProduct
{
    class CreateProductController : MyController
    {
        private static string id;
        private static User user = new User();
        public CreateProductController(IMyView _myView) : base(_myView)
        {

        }

        public async void productType()
        {
            var client = new ApiClient(utils.urls.BASE_URL);
            var request = new ApiRequestBuilder();
            string _endpoint = "api/productType/";

            utils.User user = new utils.User();
            string token = user.getToken();
            client.setAuthorizationToken(token);

            var req = request
                .buildHttpRequest()
                .setEndpoint(_endpoint)
                .setRequestMethod(HttpMethod.Get);
            client.setOnSuccessRequest(setViewProductTypeData);
            var response = await client.sendRequest(request.getApiRequestBundle());
        }

        private void setViewProductTypeData(HttpResponseBundle _response)
        {
            if (_response.getHttpResponseMessage().Content != null)
            {
                string status = _response.getHttpResponseMessage().ReasonPhrase;
                getView().callMethod("setProductType", _response.getParsedObject<List<guwudang.Model.ProductType>>());
            }
        }

        public async void units()
        {
            var client = new ApiClient(utils.urls.BASE_URL);
            var request = new ApiRequestBuilder();
            
            utils.User user = new utils.User();
            string token = user.getToken();
            client.setAuthorizationToken(token);

            var req = request
                .buildHttpRequest()
                .setEndpoint("api/units/")
                .setRequestMethod(HttpMethod.Get);
            client.setOnSuccessRequest(setViewUnitsData);
            var response = await client.sendRequest(request.getApiRequestBundle());
        }

        private void setViewUnitsData(HttpResponseBundle _response)
        {
            if (_response.getHttpResponseMessage().Content != null)
            {
                string status = _response.getHttpResponseMessage().ReasonPhrase;
                getView().callMethod("setUnits", _response.getParsedObject<List<guwudang.Model.Units>>());
            }
        }

        public async void createProduct(Model.Product product, MyList<MyFile> fileImage)
        {
            var client = new ApiClient(utils.urls.BASE_URL);
            var request = new ApiRequestBuilder();
            String endpoint = "api/product/";
            try
            {
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(product.product_name), "product_name");
                content.Add(new StringContent(product.product_type_id), "product_type_id");
                content.Add(new StringContent(product.units), "units");
                content.Add(new StringContent(product.description), "description");
                //content.Add(new StringContent(product.product_picture), "product_picture");
                content.Add(new StringContent(product.price), "price");
                if (fileImage.Count > 0)
                    content.Add(new StreamContent(new MemoryStream(fileImage[0].byteArray)), "file", fileImage[0].fullFileName);

                var multiPartRequest = request
                .buildMultipartRequest(new MultiPartContent(content))
                .setEndpoint(endpoint)
                .setRequestMethod(HttpMethod.Post);

                utils.User user = new utils.User();
                string token = user.getToken();
                client.setAuthorizationToken(token);

                client.setOnFailedRequest(setViewFailed);
                client.setOnSuccessRequest(setViewSuccess);

                var response = await client.sendRequest(request.getApiRequestBundle());
            }
            catch (Exception e)
            {
                getView().callMethod("setStatusError", "Please fill the blank");
            }
            
        }

        private void setViewFailed(HttpResponseBundle _response)
        {
            if (_response.getHttpResponseMessage().Content != null)
            {
                getView().callMethod("setStatusError", _response.getHttpResponseMessage().ReasonPhrase.ToString());
            }
        }

        private void setViewSuccess(HttpResponseBundle _response)
        {
            if (_response.getHttpResponseMessage().Content != null)
            {
                getView().callMethod("createSuccess");
            }
        }
    }
}


















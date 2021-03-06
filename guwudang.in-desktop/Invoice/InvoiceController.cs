﻿using Velacro.Api;
using Velacro.Basic;
using System.Net.Http;
using guwudang.Model;
using System.Collections.Generic;
using guwudang.utils;
using System;
using System.Windows.Controls;

namespace guwudang.Invoice
{
    public class InvoiceController : MyController
    {
        private static string id;
        public InvoiceController(IMyView _myView) : base(_myView)
        {
            //getData();
        }

        public async void listInvoice()
        {
            var client = new ApiClient(utils.urls.BASE_URL);
            var request = new ApiRequestBuilder();
            string _endpoint = "api/searchInvoiceByUserID/";

            User user = new User();
            string token = user.getToken();
            client.setAuthorizationToken(token);
            
            var client2 = new ApiClient(utils.urls.BASE_URL);
            var request2 = new ApiRequestBuilder();

            client2.setAuthorizationToken(token);

            var req = request2
                .buildHttpRequest()
                .setEndpoint(_endpoint)
                .setRequestMethod(HttpMethod.Get);
            client2.setOnSuccessRequest(setViewInvoiceData);
            var response = await client2.sendRequest(request2.getApiRequestBundle());
            
            //Console.WriteLine(response.getJObject().ToString());
            //client.setAuthorizationToken(response.getJObject().ToString());
        }

        public void searchInvoice(String key)
        {
            var client = new ApiClient(utils.urls.BASE_URL);
            var request = new ApiRequestBuilder();
            string _endpoint = "api/searchInvoice/?id=:id&search=:search";

            _endpoint = _endpoint.Replace(":id", id);
            _endpoint = _endpoint.Replace(":search", key);

            User user = new User();
            string token = user.getToken();
            client.setAuthorizationToken(token);

            var req = request
                .buildHttpRequest()
                .setEndpoint(_endpoint)
                .setRequestMethod(HttpMethod.Get);
            client.setOnSuccessRequest(setViewInvoiceData);
            var response = client.sendRequest(request.getApiRequestBundle());
        }

        private void setViewInvoiceData(HttpResponseBundle _response)
        {
            if (_response.getHttpResponseMessage().Content != null)
            {
                Console.WriteLine("Invoice : " + _response.getHttpResponseMessage().ReasonPhrase);
                getView().callMethod("setInvoice", _response.getParsedObject<List<guwudang.Model.Invoice>>());
            }
        }

        private void setFailedAuthorization(HttpResponseBundle _response)
        {
            if (_response.getHttpResponseMessage().Content != null)
            {
                getView().callMethod("backToLogin");
            }
        }

        public void deleteInvoice(List<string> selectedItemsID)
        {
            var client = new ApiClient(utils.urls.BASE_URL);
            var request = new ApiRequestBuilder();
            foreach (string item in selectedItemsID)
            {
                Console.WriteLine("Delete Invoice : " + item);
                string _endpoint = "api/invoice/:id";

                _endpoint = _endpoint.Replace(":id", item);

                User user = new User();
                string token = user.getToken();
                client.setAuthorizationToken(token);

                var req = request
                    .buildHttpRequest()
                    .setEndpoint(_endpoint)
                    .setRequestMethod(HttpMethod.Delete);
                //client.setOnSuccessRequest(setViewInvoiceData);
                var response = client.sendRequest(request.getApiRequestBundle());
            }
            getView().callMethod("createSuccess");
        }
    }
}

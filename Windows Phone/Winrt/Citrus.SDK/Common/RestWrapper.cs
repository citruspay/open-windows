﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestWrapper.cs" company="Citrus Payment Solutions Pvt. Ltd.">
//   Copyright 2015 Citrus Payment Solutions Pvt. Ltd.
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//   http://www.apache.org/licenses/LICENSE-2.0
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// <summary>
//   Helper to support REST based operations
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Citrus.SDK.Common
{
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using Citrus.SDK.Entity;

    using Newtonsoft.Json;

    /// <summary>
    /// Helper to support REST based operations
    /// </summary>
    internal class RestWrapper
    {
        #region Public Methods and Operators

        /// <summary>
        /// Put object to Service
        /// </summary>
        /// <param name="relativeServicePath">
        /// Relative REST method path
        /// </param>
        /// <param name="urlParams">
        /// Parameters to be posted
        /// </param>
        /// <returns>
        /// Success or Failure
        /// </returns>
        public async Task<object> Put(string relativeServicePath, IEnumerable<KeyValuePair<string, string>> urlParams, AuthTokenType authTokenType)
        {
            var client = new HttpClient();
            HttpResponseMessage response;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await Session.GetAuthTokenAsync(authTokenType));
            var content = new FormUrlEncodedContent(urlParams);
            response = await client.PutAsync(Session.Config.Environment.GetEnumDescription() + relativeServicePath, content);

            if (response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }

            return new Error(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Get object from Service
        /// </summary>
        /// <param name="relativeServicePath">
        /// Relative REST method path
        /// </param>
        /// <typeparam name="T">
        /// Return object type
        /// </typeparam>
        /// <returns>
        /// Result Object
        /// </returns>
        public async Task<IEntity> Get<T>(string relativeServicePath, AuthTokenType authTokenType)
        {
            var client = new HttpClient();
            HttpResponseMessage response;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await Session.GetAuthTokenAsync(authTokenType));

            response = await client.GetAsync(Session.Config.Environment.GetEnumDescription() + relativeServicePath);

            if (response.IsSuccessStatusCode)
            {
                var serializer = new JsonSerializer();
                return
                    (IEntity)
                    serializer.Deserialize<T>(
                        new JsonTextReader(new StringReader(await response.Content.ReadAsStringAsync())));
            }

            return await this.ReturnError(response);
        }

        /// <summary>
        /// Post object to Service
        /// </summary>
        /// <param name="relativeServicePath">
        /// Relative REST method path
        /// </param>
        /// <param name="authTokenType">
        /// </param>
        /// <param name="objectToPost">
        /// Object to Post
        /// </param>
        /// <param name="isRaw">
        /// Flag to mention whether the object is to be posted in request body
        /// </param>
        /// <typeparam name="T">
        /// Return object type
        /// </typeparam>
        /// <returns>
        /// Result Object
        /// </returns>
        public async Task<IEntity> Post<T>(string relativeServicePath, AuthTokenType authTokenType, IEntity objectToPost = null, bool isRaw = false)
        {
            var client = new HttpClient();
            HttpResponseMessage response = null;
            var serializer = new JsonSerializer();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await Session.GetAuthTokenAsync(authTokenType));
            if (!isRaw)
            {
                if (objectToPost != null)
                {
                    var content = new FormUrlEncodedContent(objectToPost.ToKeyValuePair());
                    response =
                        await client.PostAsync(Session.Config.Environment.GetEnumDescription() + relativeServicePath, content);
                }
                else
                {
                    response =
                        await
                        client.PostAsync(
                            Session.Config.Environment.GetEnumDescription() + relativeServicePath,
                            new StringContent(string.Empty));
                }
            }
            else
            {
                var stringContent = new StringWriter();
                serializer.Serialize(stringContent, objectToPost);
                var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                response = await client.PostAsync(Session.Config.Environment.GetEnumDescription() + relativeServicePath, content);
            }

            if (response.IsSuccessStatusCode)
            {
                return
                    (IEntity)
                    serializer.Deserialize<T>(
                        new JsonTextReader(new StringReader(await response.Content.ReadAsStringAsync())));
            }

            return await this.ReturnError(response);
        }

        /// <summary>
        /// Post object to Service
        /// </summary>
        /// <param name="relativeServicePath">
        /// Relative REST method path
        /// </param>
        /// <param name="urlParams">
        /// Key value pair of values to be posted
        /// </param>
        /// <typeparam name="T">
        /// Return object type
        /// </typeparam>
        /// <returns>
        /// Result Object
        /// </returns>
        public async Task<IEntity> Post<T>(string relativeServicePath, IEnumerable<KeyValuePair<string, string>> urlParams, AuthTokenType authTokenType)
        {
            var client = new HttpClient();
            HttpResponseMessage response;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await Session.GetAuthTokenAsync(authTokenType));

            var content = new FormUrlEncodedContent(urlParams);
            response = await client.PostAsync(Session.Config.Environment.GetEnumDescription() + relativeServicePath, content);

            if (response.IsSuccessStatusCode)
            {
                var serializer = new JsonSerializer();
                var responseString = new StringReader(await response.Content.ReadAsStringAsync());
                var resp =
                    (IEntity)
                    serializer.Deserialize<T>(
                        new JsonTextReader(responseString));
                return resp;
            }

            return await this.ReturnError(response);
        }

        public async Task<HttpResponseMessage> Put(string relativeServicePath, string jsonToPost, AuthTokenType authTokenType)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await Session.GetAuthTokenAsync(authTokenType));
            var content = new StringContent(jsonToPost, Encoding.UTF8, "application/json");
            return await client.PutAsync(Session.Config.Environment.GetEnumDescription() + relativeServicePath, content);
        }

        public async Task<HttpResponseMessage> Delete(string relativeServicePath, AuthTokenType authTokenType)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await Session.GetAuthTokenAsync(authTokenType));            
            return await client.DeleteAsync(Session.Config.Environment.GetEnumDescription() + relativeServicePath);
        }

        public async Task<HttpResponseMessage> WithdrawMoney(string relativeServicePath, AuthTokenType authTokenType, HttpContent httpContent)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await Session.GetAuthTokenAsync(authTokenType));
            return await client.PostAsync(Session.Config.Environment.GetEnumDescription() + relativeServicePath, httpContent);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Return error details received from service
        /// </summary>
        /// <param name="response">
        /// Response received from the REST call
        /// </param>
        /// <returns>
        /// Error details
        /// </returns>
        private async Task<IEntity> ReturnError(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.Forbidden)
            {
                return new Error(await response.Content.ReadAsStringAsync());
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new ServiceException("Server is down at this time, Please try again later.");
            }

            throw new ServiceException("Something went wrong");
        }

        #endregion
    }
}
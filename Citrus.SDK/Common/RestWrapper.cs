// --------------------------------------------------------------------------------------------------------------------
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
        /// Post object to Service
        /// </summary>
        /// <param name="relativeServicePath">
        /// Relative REST method path
        /// </param>
        /// <param name="objectToPost">
        /// Object to Post
        /// </param>
        /// <typeparam name="T">
        /// Return object type
        /// </typeparam>
        /// <returns>
        /// Result Object
        /// </returns>
        public async Task<IEntity> Post<T>(string relativeServicePath, IEntity objectToPost = null)
        {
            var client = new HttpClient();
            HttpResponseMessage response;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session.GetAuthToken());

            if (objectToPost != null)
            {
                var content = new FormUrlEncodedContent(objectToPost.ToKeyValuePair());
                response =
                    await client.PostAsync(Config.Environment.GetEnumDescription() + relativeServicePath, content);
            }
            else
            {
                response =
                    await
                    client.PostAsync(
                        Config.Environment.GetEnumDescription() + relativeServicePath, 
                        new StringContent(string.Empty));
            }

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
        /// <param name="urlParams">
        /// Key value pair of values to be posted
        /// </param>
        /// <typeparam name="T">
        /// Return object type
        /// </typeparam>
        /// <returns>
        /// Result Object
        /// </returns>
        public async Task<IEntity> Post<T>(
            string relativeServicePath, 
            IEnumerable<KeyValuePair<string, string>> urlParams)
        {
            var client = new HttpClient();
            HttpResponseMessage response;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session.GetAuthToken());

            var content = new FormUrlEncodedContent(urlParams);
            response = await client.PostAsync(Config.Environment.GetEnumDescription() + relativeServicePath, content);

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
            return response.StatusCode == HttpStatusCode.BadRequest
                       ? new Error(await response.Content.ReadAsStringAsync())
                       : new Error();
        }

        #endregion
    }
}
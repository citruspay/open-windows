// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Error.cs" company="Citrus Payment Solutions Pvt. Ltd.">
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
//   Error
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Citrus.SDK.Entity
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    /// <summary>
    /// Error
    /// </summary>
    public class Error : IEntity
    {
        #region Constructors and Destructors

        /// <summary>
        /// Error ctor
        /// </summary>
        public Error()
        {
        }

        /// <summary>
        /// Error ctor
        /// </summary>
        /// <param name="response">
        /// Response string(JSON)
        /// </param>
        public Error(string response)
        {
            this.Response = response;
        }

        /// <summary>
        /// Error ctor
        /// </summary>
        /// <param name="code">
        /// Error code
        /// </param>
        /// <param name="message">
        /// Error detail
        /// </param>
        public Error(string code, string message)
        {
            this.Code = code;
            this.Message = message;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        [JsonProperty("error")]
        public string Code { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("error_description")]
        public string Message { get; set; }

        /// <summary>
        /// </summary>
        [JsonIgnore]
        public string Response { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("type")]
        public string ErrorType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("description")]
        public string ErrorDescription { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public IEnumerable<KeyValuePair<string, string>> ToKeyValuePair()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
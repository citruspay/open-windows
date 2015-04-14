// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OAuthToken.cs" company="Citrus Payment Solutions Pvt. Ltd.">
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
//   OAuth Token Entity
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Citrus.SDK.Entity
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    /// <summary>
    /// </summary>
    internal class OAuthToken : IEntity
    {
        #region Public Properties

        /// <summary>
        /// Access Token
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Access Token
        /// </summary>
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Expires In
        /// </summary>
        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }

        /// <summary>
        /// Scope
        /// </summary>
        [JsonProperty("scope")]
        public string Scope { get; set; }

        /// <summary>
        /// Token Type
        /// </summary>
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

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
﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SigninRequest.cs" company="Citrus Payment Solutions Pvt. Ltd.">
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
//   Sign In Request
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Citrus.SDK.Entity
{
    using System.Collections.Generic;

    using Citrus.SDK.Common;

    using Newtonsoft.Json;

    /// <summary>
    /// Sign In Request
    /// </summary>
    internal class SigninRequest : IEntity
    {
        #region Public Properties

        /// <summary>
        /// Client Id
        /// </summary>
        [JsonProperty("client_id")]
        public string ClientId
        {
            get
            {
                return Session.Config.SignInId;
            }
        }

        /// <summary>
        /// Client Secret
        /// </summary>
        [JsonProperty("client_secret")]
        public string ClientSecret
        {
            get
            {
                return Session.Config.SignInSecret;
            }
        }

        /// <summary>
        /// Grant Type
        /// </summary>
        [JsonProperty("grant_type")]
        public string GrantType
        {
            get
            {
                return "password";
            }
        }

        /// <summary>
        /// User
        /// </summary>
        [JsonIgnore]
        public User User { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Return the key value pair of properties
        /// </summary>
        /// <returns>
        /// Key value pair of properties to be posted
        /// </returns>
        public IEnumerable<KeyValuePair<string, string>> ToKeyValuePair()
        {
            return new List<KeyValuePair<string, string>>
                       {
                           new KeyValuePair<string, string>("client_id", this.ClientId), 
                           new KeyValuePair<string, string>(
                               "client_secret", 
                               this.ClientSecret), 
                           new KeyValuePair<string, string>(
                               "grant_type", 
                               this.GrantType), 
                           new KeyValuePair<string, string>(
                               "username", 
                               this.User.UserName), 
                           new KeyValuePair<string, string>(
                               "password", 
                               this.User.Password)
                       };
        }

        #endregion
    }
}
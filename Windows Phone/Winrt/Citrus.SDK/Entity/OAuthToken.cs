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
    using System.Threading.Tasks;

    using Citrus.SDK.Common;

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

        private string _expiresIn;

        /// <summary>
        /// Expires In
        /// </summary>
        [JsonProperty("expires_in")]
        public string ExpiresIn
        {
            get
            {
                return this._expiresIn;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                this.ExpirationTime = DateTime.Now.AddMilliseconds(Convert.ToDouble(value));

                this._expiresIn = value;
            }
        }

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

        internal DateTime ExpirationTime { get; set; }

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

        public async Task<string> GetActiveTokenAsync()
        {
            if (this.ExpirationTime != default(DateTime) && DateTime.Now > this.ExpirationTime)
            {
                if (Session.Config == null || string.IsNullOrEmpty(Session.Config.SignInId) || string.IsNullOrEmpty(Session.Config.SignInSecret))
                {
                    throw new ServiceException("Invalid Configuration: Client ID & Client Secret");
                }

                //Renew token
                var rest = new RestWrapper();
                var result = await rest.Post<OAuthToken>(
                        Service.Signin,
                        new List<KeyValuePair<string, string>>()
                            {
                                new KeyValuePair<string, string>("client_id", Session.Config.SignInId),
                                new KeyValuePair<string, string>("client_secret", Session.Config.SignInSecret),
                                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                                new KeyValuePair<string, string>("refresh_token", this.RefreshToken)
                            },
                        AuthTokenType.None);

                if (!(result is Error))
                {
                    var oauthToken = result as OAuthToken;
                    if (oauthToken != null)
                    {
                        this.AccessToken = oauthToken.AccessToken;
                        this.RefreshToken = oauthToken.RefreshToken;
                        this.ExpiresIn = oauthToken.ExpiresIn;
                        this.Scope = oauthToken.Scope;
                        this.TokenType = oauthToken.TokenType;
                    }
                }
            }

            return this.AccessToken;
        }

        #endregion
    }
}
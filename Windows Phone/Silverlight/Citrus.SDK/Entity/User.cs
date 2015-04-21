// --------------------------------------------------------------------------------------------------------------------
// <copyright file="User.cs" company="Citrus Payment Solutions Pvt. Ltd.">
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
//   User
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Citrus.SDK.Entity
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    /// <summary>
    /// User
    /// </summary>
    public class User : IEntity
    {
        #region Public Properties

        /// <summary>
        /// Email
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Mobile
        /// </summary>
        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }

        /// <summary>
        /// User Name
        /// </summary>
        [JsonProperty("username")]
        public string UserName { get; set; }

        /// <summary>
        /// Balance Amount
        /// </summary>
        [JsonProperty("value")]
        public string BalanceAmount { get; set; }

        /// <summary>
        /// Currency Format
        /// </summary>
        [JsonProperty("currency")]
        public string CurrencyFormat { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Return the key value pair of properties
        /// </summary>
        /// <returns>
        /// Key value pair of properties
        /// </returns>
        public IEnumerable<KeyValuePair<string, string>> ToKeyValuePair()
        {
            return new List<KeyValuePair<string, string>>
                       {
                           new KeyValuePair<string, string>("email", this.Email), 
                           new KeyValuePair<string, string>("mobile", this.Mobile)
                       };
        }

        #endregion
    }
}
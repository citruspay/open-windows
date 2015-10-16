using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citrus.SDK.Entity
{
    public class Prepaid_Pay_Token
    {
        /// <summary>
        /// Access Token
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

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

        /// <summary>
        /// Outer Access Token
        /// </summary>
        [JsonProperty("Outer_Access_Token")]
        public string OuterAccessToken { get; set; }

        internal DateTime ExpirationTime { get; set; }
    }
}

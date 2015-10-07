namespace Citrus.SDK.Entity
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class OTP : IEntity
    {
        #region Public Properties

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("otpType")]
        public string OTPType { get; set; }

        [JsonProperty("identity")]
        public string Identity { get; set; }

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
                    new KeyValuePair<string, string>("source", this.Source), 
                    new KeyValuePair<string, string>(
                        "otpType", 
                        this.OTPType), 
                    new KeyValuePair<string, string>(
                        "identity", 
                        this.Identity)
                };
        }

        #endregion
    }
}

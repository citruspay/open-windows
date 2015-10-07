namespace Citrus.SDK.Entity
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public class UserProfile : IEntity
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("emailVerified")]
        public string EmailVerified { get; set; }

        [JsonProperty("emailVerifiedDate")]
        public string EmailVerifiedDate { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("mobileVerified")]
        public string MobileVerified { get; set; }

        [JsonProperty("mobileVerifiedDate")]
        public string MobileVerifiedDate { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("uuid")]
        public string UUID { get; set; }

        public IEnumerable<KeyValuePair<string, string>> ToKeyValuePair()
        {
            return new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("email", this.Email), 
                    new KeyValuePair<string, string>(
                        "firstName", 
                        this.FirstName), 
                    new KeyValuePair<string, string>(
                        "lastName", 
                        this.LastName), 
                    new KeyValuePair<string, string>(
                        "mobile", 
                        this.Mobile)
                };
        }
    }

    public class UserProfileRequest : IEntity
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        public IEnumerable<KeyValuePair<string, string>> ToKeyValuePair()
        {
            return new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("email", this.Email), 
                    new KeyValuePair<string, string>(
                        "firstName", 
                        this.FirstName), 
                    new KeyValuePair<string, string>(
                        "lastName", 
                        this.LastName), 
                    new KeyValuePair<string, string>(
                        "mobile", 
                        this.Mobile)
                };
        }
    }
}

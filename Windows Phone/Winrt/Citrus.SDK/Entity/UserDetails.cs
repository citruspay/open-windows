namespace Citrus.SDK.Entity
{
    using Newtonsoft.Json;

    public class UserDetails
    {
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("mobileNo")]
        public string MobileNo { get; set; }

        [JsonProperty("address")]
        public Address Address { get; set; }
    }
}

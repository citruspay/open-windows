using Newtonsoft.Json;

namespace Citrus.SDK.Entity
{
    public class CashoutAccount
    {
        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("branch")]
        public string Branch { get; set; }
    }
}

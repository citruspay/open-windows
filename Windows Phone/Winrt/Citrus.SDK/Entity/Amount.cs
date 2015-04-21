namespace Citrus.SDK.Entity
{
    using Newtonsoft.Json;

    public class Amount
    {
        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("currency")]
        public string CurrencyType { get; set; }
    }
}

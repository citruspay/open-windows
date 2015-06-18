namespace Citrus.SDK.Entity
{
    using Newtonsoft.Json;

    public class Amount
    {
        private double value;
        [JsonProperty("value")]
        public double Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        [JsonProperty("currency")]
        public string CurrencyType { get; set; }
    }
}

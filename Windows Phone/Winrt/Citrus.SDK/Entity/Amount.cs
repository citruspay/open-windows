namespace Citrus.SDK.Entity
{
    using Newtonsoft.Json;
    using System;

    public class Amount
    {
        private Int32 value;
        [JsonProperty("value")]
        public Int32 Value
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

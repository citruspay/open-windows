﻿namespace Citrus.SDK.Entity
{
    using Newtonsoft.Json;

    public class Amount
    {
        private int value;
        [JsonProperty("value")]
        public int Value
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

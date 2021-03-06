﻿using Newtonsoft.Json;

namespace Citrus.SDK.Entity
{
    public class TransferMoneyRequest
    {
        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Citrus.SDK.Entity
{
    public class WithdrawInfoResponse : IEntity
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("cashoutAccount")]
        public CashoutAccount CashoutAccount { get; set; }

        public IEnumerable<KeyValuePair<string, string>> ToKeyValuePair()
        {
            throw new NotImplementedException();
        }

    }
}

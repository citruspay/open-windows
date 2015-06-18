using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citrus.SDK.Entity
{
    public class WithdrawMoneyResponse : IEntity
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("cutsomer")]
        public string Customer { get; set; }

        [JsonProperty("merchant")]
        public string Merchant { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("date")]
        public long Date { get; set; }

        [JsonProperty("amount")]
        public Amount Amount { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("balance")]
        public Amount Balance { get; set; }

        [JsonProperty("_ref")]
        public string Reference { get; set; }

        public IEnumerable<KeyValuePair<string, string>> ToKeyValuePair()
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Citrus.SDK.Entity
{
    public class UserWallet : IEntity
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("defaultOption")]
        public string DefaultOption { get; set; }

        [JsonProperty("paymentOptions")]
        public PaymentOption[] PaymentOptions { get; set; }

        public IEnumerable<KeyValuePair<string, string>> ToKeyValuePair()
        {
            throw new NotImplementedException();
        }
    }
}

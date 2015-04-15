using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citrus.SDK.Entity
{
    using Newtonsoft.Json;

    public class PrepaidBillRequest : IEntity
    {
        [JsonProperty("currency")]
        public string CurrencyType { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("redirect")]
        public string RedirectUrl { get; set; }

        public IEnumerable<KeyValuePair<string, string>> ToKeyValuePair()
        {
            throw new NotImplementedException();
        }
    }
}

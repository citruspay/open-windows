using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Citrus.SDK.Entity
{
    public class Bill : IEntity
    {
        [JsonProperty("amount")]
        public Amount BillAmount { get; set; }

        [JsonProperty("merchantAccessKey")]
        public string MerchantAccessKey { get; set; }

        [JsonProperty("returnUrl")]
        public string ReturnUrl { get; set; }

        [JsonProperty("notifyUrl")]
        public string NotifyUrl { get; set; }

        public virtual IEnumerable<KeyValuePair<string, string>> ToKeyValuePair()
        {
            throw new NotImplementedException();
        }
    }
}

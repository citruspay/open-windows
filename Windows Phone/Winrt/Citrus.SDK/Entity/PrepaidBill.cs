namespace Citrus.SDK.Entity
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class PrepaidBill : Bill
    {
        [JsonProperty("merchantTransactionId")]
        public string MerchantTransactionId { get; set; }

        [JsonProperty("merchant")]
        public string Merchant { get; set; }

        [JsonProperty("customer")]
        public string Customer { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }

        public override IEnumerable<KeyValuePair<string, string>> ToKeyValuePair()
        {
            throw new NotImplementedException();
        }
    }
}

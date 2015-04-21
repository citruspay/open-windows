namespace Citrus.SDK.Entity
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class PrepaidBill : IEntity
    {
        [JsonProperty("merchantTransactionId")]
        public string MerchantTransactionId { get; set; }

        [JsonProperty("merchant")]
        public string Merchant { get; set; }

        [JsonProperty("customer")]
        public string Customer { get; set; }

        [JsonProperty("amount")]
        public Amount BillAmount { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }

        [JsonProperty("merchantAccessKey")]
        public string MerchantAccessKey { get; set; }

        [JsonProperty("returnUrl")]
        public string ReturnUrl { get; set; }

        [JsonProperty("notifyUrl")]
        public string NotifyUrl { get; set; }

        public IEnumerable<KeyValuePair<string, string>> ToKeyValuePair()
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citrus.SDK.Entity
{
    using Newtonsoft.Json;

    public class LoadMoneyRequest : IEntity
    {
        [JsonProperty("returnUrl")]
        internal string ReturnUrl { get; set; }

        [JsonProperty("notifyUrl")]
        public string NotifyUrl { get; set; }

        [JsonProperty("amount")]
        public Amount BillAmount { get; set; }

        [JsonProperty("merchantAccessKey")]
        internal string MerchantAccessKey { get; set; }

        [JsonProperty("paymentToken")]
        public IPaymentMode PaymentDetails { get; set; }

        [JsonProperty("merchantTxnId")]
        internal string MerchantTransactionId { get; set; }

        [JsonProperty("requestSignature")]
        internal string Signature { get; set; }

        [JsonProperty("userDetails")]
        public UserDetails UserDetails { get; set; }


        [JsonIgnore]
        public string RedirectUrl { get; set; }

        public IEnumerable<KeyValuePair<string, string>> ToKeyValuePair()
        {
            throw new NotImplementedException();
        }
    }
}

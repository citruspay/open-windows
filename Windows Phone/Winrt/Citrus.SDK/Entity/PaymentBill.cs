using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Citrus.SDK.Entity
{
    public class PaymentBill : Bill
    {
        [JsonProperty("merchantTxnId")]
        public string MerchantTxnId { get; set; }
        
        [JsonProperty("requestSignature")]
        public string RequestSignature { get; set; }

        public bool IsValid()
        {
            if (!string.IsNullOrEmpty(this.MerchantTxnId) && this.BillAmount != null && !string.IsNullOrEmpty(this.RequestSignature)
                && !string.IsNullOrEmpty(this.MerchantAccessKey) && !string.IsNullOrEmpty(this.ReturnUrl))
            {

                return true;
            }

            return false;
        }
    }
}

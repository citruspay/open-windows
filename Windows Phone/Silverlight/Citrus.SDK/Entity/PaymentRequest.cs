using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Citrus.SDK.Entity
{
    public class PaymentRequest : PaymentBill
    {
        [JsonProperty("paymentToken")]
        public IPaymentMode PaymentMode { get; set; }

        [JsonProperty("userDetails")]
        public UserDetails User { get; set; }
    }
}

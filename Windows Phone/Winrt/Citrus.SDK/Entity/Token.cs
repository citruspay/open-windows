using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citrus.SDK.Entity
{
    using Citrus.SDK.Common;

    using Newtonsoft.Json;

    public class TokenPayment : PaymentDetailsBase, IPaymentMode
    {
        [JsonProperty("cvv")]
        public string CVV { get; set; }

        [JsonProperty("id")]
        public string TokenId { get; set; }
    }
}

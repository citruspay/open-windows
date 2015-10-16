using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citrus.SDK.Entity
{
    using Citrus.SDK.Common;

    using Newtonsoft.Json;

    public class NetBanking : IPaymentMode
    {
        [JsonProperty("type")]
        internal string Type
        {
            get
            {
                return PaymentType.NetBanking.GetEnumDescription();
            }
        }

        [JsonProperty("code")]
        public string Code { get; set; }
    }

    public class NetBankingPayment : PaymentDetailsBase, IPaymentMode
    {
        [JsonProperty("paymentMode")]
        public NetBanking NetBanking { get; set; }
    }

    public class TokenBankingPayment : IPaymentMode
    {
        [JsonProperty("id")]
        public string TokenId { get; set; }

        [JsonProperty("type")]
        internal string Type
        {
            get
            {
                return PaymentType.Token.GetEnumDescription();
            }
        }
    }
}

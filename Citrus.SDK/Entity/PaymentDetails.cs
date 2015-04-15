using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citrus.SDK.Entity
{
    using Citrus.SDK.Common;

    using Newtonsoft.Json;

    public class PaymentDetails
    {
        [JsonProperty("type")]
        internal string Type
        {
            get
            {
                if (this.PaymentType != PaymentType.Token)
                {
                    return PaymentType.Card.GetEnumDescription();
                }

                return PaymentType.Token.GetEnumDescription();
            }
        }

        [JsonIgnore]
        public PaymentType PaymentType { get; set; }

        [JsonIgnore]
        private IPaymentMode _paymentMode;

        [JsonProperty("paymentMode")]
        public IPaymentMode PaymentMode
        {
            get
            {
                return this._paymentMode;
            }

            set
            {
                this._paymentMode = value;
            }
        }

        [JsonProperty("cvv")]
        internal string CVV
        {
            get
            {
                if (this.PaymentType == PaymentType.Token && this.PaymentMode is Token)
                {
                    return (this.PaymentMode as Token).CVV;
                }

                return string.Empty;
            }
        }

        [JsonProperty("id")]
        internal string TokenId
        {
            get
            {
                if (this.PaymentType == PaymentType.Token && this.PaymentMode is Token)
                {
                    return (this.PaymentMode as Token).TokenId;
                }

                return string.Empty;
            }
        }
    }
}

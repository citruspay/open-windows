using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Citrus.SDK.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace Citrus.SDK.Entity
{
    public class PaymentOption
    {
        [JsonProperty("type")]
        internal string Type
        {
            get { return this.CardType.GetEnumDescription(); }
            set
            {
                CardType card;
                this.CardType = Enum.TryParse(value, true, out card) ? card : CardType.UnKnown;
            }
        }

        [JsonIgnore]
        public CardType CardType { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("owner")]
        public string CardHolder { get; set; }

        [JsonProperty("number")]
        public string CardNumber { get; set; }

        //[JsonProperty("expiryDate")]
        //public string ExpiryDate { get; set; }

        [JsonProperty("expiryDate")]
        internal string CardExpiryDate
        {
            get
            {
                return string.Format("{0}/{1}", this.ExpiryDate.Month < 10 ? "0" + this.ExpiryDate.Month : this.ExpiryDate.Month.ToString(CultureInfo.InvariantCulture), this.ExpiryDate.Year);
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var splits = value.ToString();
                    this.ExpiryDate = new CardExpiry()
                    {
                        Month = Convert.ToInt16(splits.Substring(0, 2)),
                        Year = Convert.ToInt16(splits.Substring(2, 4))
                    };
                }
            }
        }

        [JsonIgnore]
        public CardExpiry ExpiryDate { get; set; }

        [JsonProperty("scheme")]
        internal string Scheme
        {
            get
            {
                return this.CardScheme != CreditCardType.Unknown ? this.CardScheme.GetEnumDescription() : "";
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.CardScheme = (CreditCardType)Enum.Parse(typeof(CreditCardType), value, true);
                }
                else
                    this.CardScheme = CreditCardType.Unknown;
            }
        }

        [JsonProperty("bank")]
        public string Bank { get; set; }

        [JsonProperty("mmid")]
        public string MMID { get; set; }

        [JsonProperty("impsRegisteredMobile")]
        public string IMPSRegisteredMobile { get; set; }

        [JsonIgnore]
        public CreditCardType CardScheme
        {
            get;
            set;
        }

        public JObject ToJson()
        {
            var paymentOption = new JObject();

            if (!string.IsNullOrEmpty(this.Name))
            {
                paymentOption["name"] = this.Name;
            }

            if (this.CardType != CardType.UnKnown)
            {
                paymentOption["owner"] = this.CardHolder;
                paymentOption["number"] = this.CardNumber;
                paymentOption["scheme"] = this.Scheme;
                paymentOption["expiryDate"] = this.CardExpiryDate;
                paymentOption["type"] = this.Type;
            }
            else
            {
                paymentOption["owner"] = string.Empty;
                paymentOption["bank"] = this.Bank;
                paymentOption["mmid"] = this.MMID;
                paymentOption["impsRegisteredMobile"] = this.IMPSRegisteredMobile;
                paymentOption["type"] = "netbanking";
            }
            return paymentOption;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Citrus.SDK.Common;
using Newtonsoft.Json;

namespace Citrus.SDK.Entity
{
    public class PaymentOption
    {
        [JsonProperty("type")]
        public string Type
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

        [JsonProperty("expiryDate")]
        public string ExpiryDate { get; set; }

        [JsonProperty("bank")]
        public string Bank { get; set; }

        [JsonProperty("scheme")]
        public string Scheme { get; set; }

        [JsonProperty("mmid")]
        public object MMID { get; set; }

        [JsonProperty("impsRegisteredMobile")]
        public object IMPSRegisteredMobile { get; set; }

        [JsonIgnore]
        public CreditCardType CardScheme
        {
            get
            {
                if (!string.IsNullOrEmpty(Scheme))
                    return (CreditCardType)Enum.Parse(typeof(CreditCardType), Scheme, true);
                return CreditCardType.Unknown;
            }
        }
    }
}

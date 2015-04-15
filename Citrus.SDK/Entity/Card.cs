using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citrus.SDK.Entity
{
    using Citrus.SDK.Common;

    using Newtonsoft.Json;

    public class Card : IPaymentMode
    {
        public Card()
        {
            this.ExpiryDate = new CardExpiry();
        }

        [JsonProperty("cvv")]
        public string CVV { get; set; }

        [JsonProperty("holder")]
        public string AccountHolderName { get; set; }

        [JsonProperty("number")]
        public string CardNumber { get; set; }

        [JsonProperty("scheme")]
        public string Scheme
        {
            get
            {
                return this.CardScheme.GetEnumDescription();
            }
        }

        [JsonIgnore]
        public CardSchemeType CardScheme { get; set; }

        [JsonProperty("type")]
        public string Type
        {
            get
            {
                return this.CardType.GetEnumDescription();
            }
        }

        [JsonIgnore]
        public CardType CardType { get; set; }

        [JsonProperty("expiry")]
        public string CardExpiryDate
        {
            get
            {
                return string.Format("{0}/{1}", this.ExpiryDate.Month, this.ExpiryDate.Year);
            }
        }

        [JsonIgnore]
        public CardExpiry ExpiryDate { get; set; }
    }

    public interface IPaymentMode
    {
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citrus.SDK.Entity
{
    using System.Globalization;

    using Citrus.SDK.Common;

    using Newtonsoft.Json;

    public class Card 
    {
        public Card()
        {
            this.ExpiryDate = new CardExpiry();
        }

        [JsonProperty("cvv")]
        public string CVV { get; set; }

        [JsonProperty("holder")]
        public string AccountHolderName { get; set; }

        private string _cardNumber;

        [JsonProperty("number")]
        public string CardNumber
        {
            get
            {
                return this._cardNumber;
            }

            set
            {
                this._cardNumber = value;
                this.CardScheme = Utility.GetCardTypeFromNumber(value);
            }
        }

        [JsonProperty("scheme")]
        public string Scheme
        {
            get
            {
                return this.CardScheme.HasValue ? this.CardScheme.GetEnumDescription() : string.Empty;
            }
        }

        [JsonIgnore]
        internal CreditCardType? CardScheme { get; set; }

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
                return string.Format("{0}/{1}", this.ExpiryDate.Month < 10 ? "0" + this.ExpiryDate.Month : this.ExpiryDate.Month.ToString(CultureInfo.InvariantCulture), this.ExpiryDate.Year);
            }
        }

        [JsonIgnore]
        public CardExpiry ExpiryDate { get; set; }
    }

    public class CardPayment : PaymentDetailsBase, IPaymentMode
    {
        [JsonProperty("paymentMode")]
        public Card Card { get; set; }
    }

    public interface IPaymentMode
    {

    }
}

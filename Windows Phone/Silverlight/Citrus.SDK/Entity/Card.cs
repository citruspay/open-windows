﻿using System;
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
                if (this.CardType == CardType.Prepaid)
                    this.CardScheme = CreditCardType.Prepaid;
                else
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

        private CardType _CardType;

        [JsonIgnore]
        public CardType CardType
        {
            get
            {
                return _CardType;
            }
            set
            {
                _CardType = value;
                if (this.CardType == CardType.Prepaid)
                    this.CardScheme = CreditCardType.Prepaid;
            }
        }

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

        public bool IsValid()
        {
            if (this.CardScheme.HasValue && this.CardScheme.Value == CreditCardType.Mtro)
            {
                return validateNumber() && this.CardType == CardType.Debit;
            }
            else if (string.IsNullOrEmpty(this.CVV))
            {
                return validateNumber() && this.ExpiryDate.IsValid();
            }
            else
            {
                return validateNumber() && this.ExpiryDate.IsValid() && validateCVV();
            }
        }

        private bool validateNumber()
        {
            if (!this.CardScheme.HasValue || string.IsNullOrEmpty(_cardNumber))
                return false;

            if (CardScheme.Value == CreditCardType.Mtro)
            {
                return Utility.PassesLuhnTest(this._cardNumber);
            }

            String rawNumber = this._cardNumber.Trim().Replace("-", "");

            if (string.IsNullOrEmpty(rawNumber) || !Utility.PassesLuhnTest(this._cardNumber))
            {
                return false;
            }

            if (this.CardScheme == CreditCardType.Amex && (rawNumber.Length != 16 || rawNumber.Length != 15))
            {
                return false;
            }

            return true;
        }

        private bool validateCVV()
        {
            if (string.IsNullOrEmpty(this.CVV))
            {
                return false;
            }
            String cvcValue = CVV.Trim();

            bool validLength = ((this.CardScheme == null && cvcValue.Length >= 3 && cvcValue.Length <= 4)
                    || (CardScheme == CreditCardType.Amex && cvcValue.Length == 4) || (CardScheme != CreditCardType.Amex && cvcValue
                    .Length == 3));

            return validLength;
        }
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

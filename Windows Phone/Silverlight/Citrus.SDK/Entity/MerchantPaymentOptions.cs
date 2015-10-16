using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Citrus.SDK.Common;
using Newtonsoft.Json;

namespace Citrus.SDK.Entity
{
    public class MerchantPaymentOptions : IEntity
    {
        [JsonProperty("creditCard")]
        public string[] CreditCardSchemes { get; set; }

        [JsonProperty("debitCard")]
        public string[] DebitCardSchemes { get; set; }

        [JsonProperty("netBanking")]
        public NetBankingOption[] NetBankingOptions { get; set; }

        public IEnumerable<KeyValuePair<string, string>> ToKeyValuePair()
        {
            throw new NotImplementedException();
        }

        public bool ContainScheme(CreditCardType cardScheme, CardType cardType, string bank = null)
        {
            var scheme = string.Empty;

            if (cardScheme == CreditCardType.Visa)
            {
                scheme = "visa";
            }
            else if (cardScheme == CreditCardType.Mcrd)
            {
                scheme = "master card";
            }
            else if (cardScheme == CreditCardType.Amex)
            {
                scheme = "amex";
            }

            if (cardType == CardType.Credit)
            {
                return CreditCardSchemes.Any(sch => sch.ToLower() == scheme);
            }

            if (cardType == CardType.Debit)
            {
                return CreditCardSchemes.Any(sch => sch.ToLower() == scheme);
            }

            if (cardType == CardType.UnKnown && !string.IsNullOrEmpty(bank))
            {
                return NetBankingOptions.Any(sch => sch.BankName.ToLower() == bank.ToLower());
            }

            return false;
        }
    }

    public class NetBankingOption
    {
        [JsonProperty("bankName")]
        public string BankName { get; set; }

        [JsonProperty("issuerCode")]
        public string IssuerCode { get; set; }
    }
}

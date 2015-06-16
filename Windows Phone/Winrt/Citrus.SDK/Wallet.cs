using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Citrus.SDK
{
    using System;
    using System.Threading.Tasks;

    using Citrus.SDK.Common;
    using Citrus.SDK.Entity;
    using Newtonsoft.Json.Linq;

    public static class Wallet
    {
        private static MerchantPaymentOptions merchantPaymentOptions;

        private static async Task<PrepaidBill> GetPrepaidBillAsync(int amount, string currencyType, string redirectUrl)
        {
            var restWrapper = new RestWrapper();
            var result =
                await restWrapper.Post<PrepaidBill>(
                    Service.PrepaidBill,
                    AuthTokenType.SignIn,
                    new PrepaidBillRequest()
                        {
                            Amount = amount,
                            CurrencyType = currencyType,
                            RedirectUrl = redirectUrl
                        });
            if (!(result is Error))
            {
                return (PrepaidBill)result;
            }

            Utility.ParseAndThrowError(((Error)result).Response);
            return null;
        }

        public static async Task<Transaction> LoadMoneyAsync(LoadMoneyRequest request)
        {
            await Session.GetTokenIfEmptyAsync(AuthTokenType.SignIn);

            if (Session.signInToken == null || string.IsNullOrEmpty(Session.signInToken.AccessToken))
            {
                throw new UnauthorizedAccessException("User is not logged to perform this operation");
            }

            var cardPayment = request.PaymentDetails as CardPayment;
            if (cardPayment != null && !Utility.PassesLuhnTest(cardPayment.Card.CardNumber))
            {
                throw new ArgumentException("Invalid card number, Please provide a valid card detail");
            }

            var prepaidBill = await GetPrepaidBillAsync(
                request.BillAmount.Value,
                request.BillAmount.CurrencyType,
                request.RedirectUrl);
            if (prepaidBill != null)
            {
                request.ReturnUrl = prepaidBill.ReturnUrl;
                request.NotifyUrl = prepaidBill.NotifyUrl;
                request.BillAmount = prepaidBill.BillAmount;
                request.MerchantAccessKey = prepaidBill.MerchantAccessKey;
                request.MerchantTransactionId = prepaidBill.MerchantTransactionId;
                request.Signature = prepaidBill.Signature;
            }

            RestWrapper rest = new RestWrapper();
            var result = await rest.Post<Transaction>(Service.LoadMoney, AuthTokenType.SignIn, request, true);

            if (!(result is Error))
            {
                return (Transaction)result;
            }

            Utility.ParseAndThrowError(((Error)result).Response);
            return null;
        }

        public static async Task<List<PaymentOption>> GetWallet()
        {
            await Session.GetTokenIfEmptyAsync(AuthTokenType.Simple);
            var token = await Session.GetAuthTokenAsync(AuthTokenType.Simple);
            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("User is not logged to perform the action: Get Wallet");
            }

            var restWrapper = new RestWrapper();
            var response = await restWrapper.Get<UserWallet>(Service.Wallet, AuthTokenType.Simple);
            var options = new List<PaymentOption>();
            if (!(response is Error))
            {
                var wallet = response as UserWallet;
                if (wallet != null)
                {
                    if (merchantPaymentOptions != null)
                    {
                        options.AddRange(wallet.PaymentOptions.Where(option => merchantPaymentOptions.ContainScheme(option.CardScheme, option.CardType, option.Bank)));
                        return options;
                    }

                    return wallet.PaymentOptions.ToList();
                }
                return null;
            }

            Utility.ParseAndThrowError(((Error)response).Response);
            return null;
        }

        public static async Task GetMerchantPaymentOptions()
        {
            RestWrapper restWrapper = new RestWrapper();
            var paymentOptions = await restWrapper.Post<MerchantPaymentOptions>(Service.GetMerchantPaymentOptions, new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("vanity",Session.Config.Vanity)
            }, AuthTokenType.Simple);

            if (!(paymentOptions is Error))
            {
                merchantPaymentOptions = paymentOptions as MerchantPaymentOptions;
                return;
            }

            Utility.ParseAndThrowError(((Error)paymentOptions).Response);
        }

        public static async Task<bool> SavePaymentOptions(IEnumerable<PaymentOption> paymentOptions)
        {
            await Session.GetTokenIfEmptyAsync(AuthTokenType.Simple);
            var token = await Session.GetAuthTokenAsync(AuthTokenType.Simple);
            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("User is not logged to perform the action: Get Wallet");
            }

            RestWrapper restWrapper = new RestWrapper();
            var request = new JObject();
            request["type"] = "payment";

            var paymentOptionsList = new JArray();

            foreach (var option in paymentOptions)
            {
                paymentOptionsList.Add(option.ToJson());
            }

            request["paymentOptions"] = paymentOptionsList;

            var json = request.ToString();

            var response = await restWrapper.Put(Service.Wallet, json, AuthTokenType.Simple);

            return response.IsSuccessStatusCode;
        }

        public static async Task<bool> DeletePaymentOption(PaymentOption paymentOption)
        {
            await Session.GetTokenIfEmptyAsync(AuthTokenType.Simple);
            var token = await Session.GetAuthTokenAsync(AuthTokenType.Simple);

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("User is not logged in to perform the action: Delete Payment option");
            }

            if (paymentOption == null)
            {
                return false;
            }

            RestWrapper restWrapper = new RestWrapper();
            if (paymentOption.CardType != CardType.UnKnown)
            {
                var response = await restWrapper.Delete(Service.Wallet + "/" + paymentOption.CardNumber.Substring(12, 4) + ":" + paymentOption.Scheme, AuthTokenType.Simple);
                return response.IsSuccessStatusCode;
            }
            else
            {
                var response = await restWrapper.Delete(Service.Wallet + "/" + paymentOption.Token, AuthTokenType.Simple);
                return response.IsSuccessStatusCode;
            }

            return false;
        }
    }
}
namespace Citrus.SDK
{
    using System;
    using System.Threading.Tasks;

    using Citrus.SDK.Common;
    using Citrus.SDK.Entity;

    public static class Wallet
    {
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
    }
}

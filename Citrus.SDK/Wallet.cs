namespace Citrus.SDK
{
    using System.Threading.Tasks;

    using Citrus.SDK.Common;
    using Citrus.SDK.Entity;

    public static class Wallet
    {
        private static async Task<PrepaidBill> GetPrepaidBalanceAsync(int amount, string currencyType, string redirectUrl)
        {
            var restWrapper = new RestWrapper();
            var result =
                await restWrapper.Post<PrepaidBill>(
                    Service.GetBalance,
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
            var prepaidBill = await GetPrepaidBalanceAsync(
                request.BillAmount.Value,
                request.BillAmount.CurrencyType,
                request.RedirectUrl);
            if (prepaidBill != null)
            {
                request.ReturnUrl = prepaidBill.ReturnUrl;
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

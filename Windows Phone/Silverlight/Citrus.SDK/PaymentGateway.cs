using Citrus.SDK.Common;
using Citrus.SDK.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Citrus.SDK
{
    public class PaymentGateway
    {
        private PaymentBill _bill;
        private IPaymentMode _paymentMode;
        private UserDetails _user;

        public PaymentGateway(PaymentBill bill, IPaymentMode paymentMode, UserDetails userDetails)
        {
            _paymentMode = paymentMode;
            _bill = bill;
            _user = userDetails;
        }

        public async static Task<PaymentBill> GetBillAsync(string billUrl, Amount amount)
        {
            if (string.IsNullOrEmpty(billUrl) || amount == null)
            {
                throw new ArgumentException();
            }

            var billingUri = new UriBuilder(billUrl);
            billingUri.Query += ("amount=" + amount.Value);

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            var response = await httpClient.GetAsync(billingUri.Uri);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<PaymentBill>(await response.Content.ReadAsStringAsync());
            }

            return null;
        }

        public async Task<Transaction> ProcessPaymentAsync(string EmailId)
        {
            this._paymentMode = new CardPayment()
            {
                PaymentType = PaymentType.Prepaid,
                Card = new Card()
                {
                    AccountHolderName = EmailId,
                }
            };

            return await ProcessPaymentAsync();
        }

        public async Task<Transaction> ProcessPaymentAsync()
        {
            await Session.GetTokenIfEmptyAsync(AuthTokenType.SignIn);

            //if (Session.signInToken == null || string.IsNullOrEmpty(Session.signInToken.AccessToken))
            //{
            //    throw new UnauthorizedAccessException("User is not logged to perform this operation");
            //}

            if (this._bill == null || this._paymentMode == null || this._user == null)
            {
                throw new ArgumentException();
            }

            if (!this._bill.IsValid())
            {
                throw new ServiceException("Unable to get bill from server. Please try again.");
            }

            if (!ValidatePaymentMode())
            {
                return null;
            }

            IPaymentMode paymentMode = null;

            if (this._paymentMode is CardPayment)
            {
                var cardPayment = this._paymentMode as CardPayment;

                if (cardPayment.PaymentType == PaymentType.Card)
                {
                    if (cardPayment.Card.CardScheme.HasValue && cardPayment.Card.CardScheme.Value == CreditCardType.Mtro)
                    {
                        if (string.IsNullOrEmpty(cardPayment.Card.CVV))
                        {
                            cardPayment.Card.CVV = "123";
                        }
                        if (cardPayment.Card.ExpiryDate == null)
                        {
                            cardPayment.Card.ExpiryDate = new CardExpiry()
                            {
                                Month = 11,
                                Year = 2019 //Dummy value
                            };
                        }
                    }
                }
                else if (cardPayment.PaymentType == PaymentType.Prepaid)
                {
                    cardPayment.Card.CVV = "000";
                    cardPayment.Card.CardNumber = "1234561234561234";
                    cardPayment.Card.CardScheme = CreditCardType.Prepaid;
                    cardPayment.Card.CardType = CardType.Prepaid;
                    cardPayment.Card.ExpiryDate = new CardExpiry()
                    {
                        Month = 04,
                        Year = 2030
                    };

                    cardPayment.PaymentType = PaymentType.Card;
                }

                paymentMode = cardPayment;
            }
            else if (this._paymentMode is TokenPayment)
            {
                paymentMode = this._paymentMode;
            }
            else if (this._paymentMode is TokenBankingPayment)
            {
                paymentMode = this._paymentMode;
            }
            else if (this._paymentMode is NetBankingPayment)
            {
                paymentMode = this._paymentMode;
            }

            if (paymentMode == null)
            {
                throw new ArgumentException();
            }

            var paymentRequest = new PaymentRequest()
            {
                ReturnUrl = this._bill.ReturnUrl,
                NotifyUrl = this._bill.NotifyUrl,
                BillAmount = this._bill.BillAmount,
                MerchantAccessKey = this._bill.MerchantAccessKey,
                PaymentMode = paymentMode,
                MerchantTxnId = this._bill.MerchantTxnId,
                RequestSignature = this._bill.RequestSignature,
                User = this._user
            };

            var rest = new RestWrapper();
            var result = await rest.Post<Transaction>(Service.LoadMoney, AuthTokenType.SignIn, paymentRequest, true);

            if (!(result is Error))
            {
                return (Transaction)result;
            }

            Utility.ParseAndThrowError(((Error)result).Response);
            return null;
        }

        private bool ValidatePaymentMode()
        {
            if (this._paymentMode is CardPayment)
            {
                var card = this._paymentMode as CardPayment;
                if (card.PaymentType != PaymentType.Prepaid)
                {
                    if (string.IsNullOrEmpty(card.Card.CardNumber) || !card.Card.IsValid())
                    {
                        throw new ArgumentException("Invalid card");
                    }
                }
            }
            else if (this._paymentMode is TokenPayment)
            {
                var token = this._paymentMode as TokenPayment;
                if (string.IsNullOrEmpty(token.TokenId))
                {
                    throw new ArgumentException("Invalid Token");
                }
            }

            if (!ValidateMandatoryValues())
            {
                throw new ArgumentException("Bill or userdetails can not contain empty parameters");
            }

            return true;
        }

        private bool ValidateMandatoryValues()
        {
            if (string.IsNullOrEmpty(this._bill.MerchantAccessKey) || string.IsNullOrEmpty(this._bill.MerchantTxnId) ||
                string.IsNullOrEmpty(this._bill.RequestSignature) || string.IsNullOrEmpty(this._bill.ReturnUrl)
                || string.IsNullOrEmpty(_user.Email) || string.IsNullOrEmpty(_user.MobileNo) ||
                string.IsNullOrEmpty(_user.FirstName) || string.IsNullOrEmpty(_user.LastName))
            {
                return false;
            }

            return true;
        }

        #region WebView Functions

        private void OpenWebView(Transaction result)
        {
            if (result != null && !string.IsNullOrEmpty(result.RedirectUrl))
            {
                Microsoft.Phone.Tasks.WebBrowserTask webBrowserTask = new Microsoft.Phone.Tasks.WebBrowserTask();
                webBrowserTask.Uri = new Uri(result.RedirectUrl, UriKind.Absolute);
                webBrowserTask.Show();
            }
        }

        #endregion
    }

}

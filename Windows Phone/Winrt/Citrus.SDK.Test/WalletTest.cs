using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citrus.SDK.Test
{
    using Citrus.SDK.Common;
    using Citrus.SDK.Entity;

    using NUnit.Framework;

    [TestFixture]
    public class WalletTest
    {
        private UserDetails userDetails;

        private Amount amount;


        [SetUp]
        public void SetUp()
        {
            userDetails = new UserDetails()
                              {
                                  Address =
                                      new Address()
                                          {
                                              City = "Pune",
                                              Country = "India",
                                              State = "Maharastra",
                                              Street1 = "Street 1",
                                              Street2 = "Street 2",
                                              Zip = "620212"
                                          },
                                  Email = "user@email.com",
                                  FirstName = "Charles",
                                  LastName = "Pearson",
                                  MobileNo = "9876543210"
                              };

            amount = new Amount()
            {
                Value = 100,
                CurrencyType = "INR"
            };
        }

        [Test]
        public async void LoadMoney_ReturnsSuccess_OnValidCard()
        {
            await this.SignIn();
            var request = new LoadMoneyRequest();
            request.BillAmount = amount;
            request.RedirectUrl = "http://yourwebsite.com/return_url.php";
            request.UserDetails = userDetails;

            request.PaymentDetails = new CardPayment()
            {
                PaymentType = PaymentType.Card,
                Card = new Card()
                {
                    AccountHolderName = "Pearson Charles",
                    CVV = "123",
                    ExpiryDate =
                        new CardExpiry()
                        {
                            Month = 12,
                            Year = 2018
                        },
                    CardNumber = "4242424242424242",
                    CardType = CardType.Debit
                }
            };

            var result = await Wallet.LoadMoneyAsync(request);

            Assert.IsNotNull(result);

            Assert.IsNotNullOrEmpty(result.Code);

            Assert.IsNotNullOrEmpty(result.Status);
        }

        [Test]
        public async void LoadMoney_ReturnsSuccess_OnValidNetBanking()
        {
            await this.SignIn();
            var request = new LoadMoneyRequest();
            request.BillAmount = amount;
            request.RedirectUrl = "http://yourwebsite.com/return_url.php";
            request.UserDetails = userDetails;

            request.PaymentDetails = new NetBankingPayment()
            {
                PaymentType = PaymentType.NetBanking,
                NetBanking = new NetBanking()
                {
                    Code = "CID002"
                }
            };

            var result = await Wallet.LoadMoneyAsync(request);

            Assert.IsNotNull(result);

            Assert.IsNotNullOrEmpty(result.Code);

            Assert.IsNotNullOrEmpty(result.Status);
        }

        [Test]
        public async void LoadMoney_ReturnsSuccess_OnValidToken()
        {
            await this.SignIn();
            var request = new LoadMoneyRequest();
            request.BillAmount = amount;
            request.RedirectUrl = "http://yourwebsite.com/return_url.php";
            request.UserDetails = userDetails;
            request.PaymentDetails = new TokenPayment()
            {
                PaymentType = PaymentType.Token,
                CVV = "000",
                TokenId = "acdb59d66d55bc95dc76a86b1b99387c"
            };

            var result = await Wallet.LoadMoneyAsync(request);

            Assert.IsNotNull(result);

            Assert.IsNotNullOrEmpty(result.Code);

            Assert.IsNotNullOrEmpty(result.Status);
        }

        [Test]
        public void LoadMoney_ThrowsException_OnInvalidSession()
        {
            Session.SignOut();
            Assert.Throws<UnauthorizedAccessException>(async () => await Wallet.LoadMoneyAsync(new LoadMoneyRequest()));
        }

        [Test]
        public async void LoadMoney_ThrowsException_OnInvalidCardNumber()
        {
            await this.SignIn();
            var request = new LoadMoneyRequest();
            request.PaymentDetails = new CardPayment()
            {
                PaymentType = PaymentType.Card,
                Card = new Card()
                {
                    AccountHolderName = "Pearson Charles",
                    CVV = "123",
                    ExpiryDate =
                        new CardExpiry()
                        {
                            Month = 12,
                            Year = 2018
                        },
                    CardNumber = "123456789123",
                    CardType = CardType.Debit
                }
            };

            Assert.Throws<ArgumentException>(async () => await Wallet.LoadMoneyAsync(request));
        }

        private async Task SignIn()
        {
            await Session.SigninUser("username@yourwebsite.com", "password");
        }
    }
}

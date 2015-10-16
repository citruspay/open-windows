using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Citrus.SampleApp
{
    using Citrus.SDK;
    using Citrus.SDK.Common;
    using Citrus.SDK.Entity;
    using System.Threading.Tasks;

    public partial class Operations : PhoneApplicationPage
    {
        #region Init

        public Operations()
        {
            this.InitializeComponent();
            Config.Initialize(EnvironmentType.Sandbox, "test-signup", "c78ec84e389814a05d3ae46546d16d2e", "test-signin", "52f7e15efd4208cf5345dd554443fd99", "testing");
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private UserDetails UserDetails = new UserDetails()
        {
            Address = new Address()
            {
                City = "",
                Country = "",
                State = "",
                Street1 = "",
                Street2 = "",
                Zip = ""
            },
            Email = "maheshmutyal1@mailinator.com",
            FirstName = " ",
            LastName = " ",
            MobileNo = "9011094323"
        };
        int orderAmount = 100;
        private Amount amount = new Amount()
        {
            Value = 100,
            CurrencyType = "INR"
        };

        #endregion

        #region PG Payment

        private async void CardPayment_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingBar.Visibility = Visibility.Visible;

                string AccountHolderName = "Pearson Charles";
                string CVV = "123";
                Int32 ExpiryMonth = 1;
                Int32 ExpiryYear = 2020;
                string CardNumber = "4242424242424242";
                CardType objCardType = CardType.Debit;

                this.amount.Value = 1;
                this.UserDetails.Email = "user@email.com";
                this.UserDetails.MobileNo = "9876543210";
                orderAmount = this.amount.Value;
                var bill = await GetBillAsync(orderAmount);
                var payment = new CardPayment()
                {
                    PaymentType = PaymentType.Card,
                    Card = new Card()
                    {
                        AccountHolderName = AccountHolderName,
                        CVV = CVV,
                        ExpiryDate =
                            new CardExpiry()
                            {
                                Month = ExpiryMonth,
                                Year = ExpiryYear
                            },
                        CardNumber = CardNumber,
                        CardType = objCardType
                    }
                };

                var pg = new PaymentGateway(bill, payment, UserDetails);
                var result = await pg.ProcessPaymentAsync();
                if (result != null)
                {
                    MessageBox.Show("Result Code:" + result.Code + ", Status: " + result.Status + ", Redirect URL:" + result.RedirectUrl);
                }
                else
                {
                    MessageBox.Show("Something went wrong");
                }
            }
            catch (ServiceException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        private async void TokenPayment_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingBar.Visibility = Visibility.Visible;
                var bill = await GetBillAsync(orderAmount);
                var payment = new TokenPayment()
                {
                    PaymentType = PaymentType.Token,
                    TokenId = "e8c18a9aac39cfeb6f0d02f28ed4660b",
                    CVV = "123"
                };

                var pg = new PaymentGateway(bill, payment, UserDetails);
                var result = await pg.ProcessPaymentAsync();
                if (result != null)
                {
                    MessageBox.Show("Result Code:" + result.Code + ", Status: " + result.Status + ", Redirect URL:" + result.RedirectUrl);
                }
                else
                {
                    MessageBox.Show("Something went wrong");
                }
            }
            catch (ServiceException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        private async void NetBankingPayment_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingBar.Visibility = Visibility.Visible;
                string BankCode = "CID002";
                var bill = await GetBillAsync(orderAmount);
                var payment = new NetBankingPayment()
                {
                    PaymentType = PaymentType.NetBanking,
                    NetBanking = new NetBanking()
                    {
                        Code = BankCode
                    }
                };

                var pg = new PaymentGateway(bill, payment, UserDetails);
                var result = await pg.ProcessPaymentAsync();
                if (result != null)
                {
                    MessageBox.Show("Result Code:" + result.Code + ", Status: " + result.Status + ", Redirect URL:" + result.RedirectUrl);
                }
                else
                {
                    MessageBox.Show("Something went wrong");
                }
            }
            catch (ServiceException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        private async void NetBankingTokenPayment_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingBar.Visibility = Visibility.Visible;
                var bill = await GetBillAsync(orderAmount);
                var payment = new TokenBankingPayment()
                {
                    TokenId = "48ec899d5dd14be93dce01038a8af60d"
                };

                var pg = new PaymentGateway(bill, payment, UserDetails);
                var result = await pg.ProcessPaymentAsync();
                if (result != null)
                {
                    MessageBox.Show("Result Code:" + result.Code + ", Status: " + result.Status + ", Redirect URL:" + result.RedirectUrl);
                }
                else
                {
                    MessageBox.Show("Something went wrong");
                }
            }
            catch (ServiceException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        #region User Managment

        private async void SignUp_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string emailid = "user@email.com";
                string mobile = "9876543210";
                string password = "password";

                ResultPanel.DataContext = null;
                LoadingBar.Visibility = Visibility.Visible;
                ResultPanel.DataContext = await Session.SignupUser(emailid, mobile, password);
                MessageBox.Show("User signed up successfully");
            }
            catch (ServiceException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        private async void IsCitrusMember_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingBar.Visibility = Visibility.Visible;
                string emailid = "user@email.com";
                string mobile = "9876543210";
                string password = "password";

                var isCitrusMember = await Session.IsCitrusMemeber(emailid, password);
                if (isCitrusMember)
                {
                    var user = await Session.GetBalance();

                    MessageBox.Show("User Already A Citrus Member. Please Sign In User.");
                }
                else
                {
                    MessageBox.Show("User Not A Citrus Member. Please Sign Up User.");
                }
            }
            catch (ServiceException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        private async void SignIn_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string emailid = "user@email.com";
                string password = "password";

                loginStatus.Text = string.Empty;
                LoadingBar.Visibility = Visibility.Visible;
                if (await Session.SigninUser(emailid, password))
                {
                    loginStatus.Text = "Signed In";
                    MessageBox.Show("User signed in successfully");
                }
                else
                {
                    loginStatus.Text = "";
                    MessageBox.Show("User signed in failed");
                }

                //var obj = await Session.BindUser("joyce.bansode@mailinator.com", "7709829851");

            }
            catch (ServiceException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        private async void Bind_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string emailid = "user@email.com";
                string password = "password";

                loginStatus.Text = string.Empty;
                LoadingBar.Visibility = Visibility.Visible;

                var userObj = await Session.BindUser(emailid, password);
                if (userObj != null && !string.IsNullOrEmpty(userObj.UserName))
                {
                    MessageBox.Show("User signed in successfully");
                }
                else
                {
                    MessageBox.Show("User signed in failed");
                }

            }
            catch (ServiceException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        private void SignOut_OnClick(object sender, RoutedEventArgs e)
        {
            Session.SignOut();
            loginStatus.Text = "Signed out";
        }

        private async void Reset_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingBar.Visibility = Visibility.Visible;
                await Session.ResetPassword("user7@gmail.com");
                MessageBox.Show("Password reset completed");
            }
            catch (ServiceException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        private async void UpdateMobile_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingBar.Visibility = Visibility.Visible;
                string UMmobile = "9876543210";
                await Session.UpdateMobile(UMmobile);
                MessageBox.Show("Mobile update completed");
            }
            catch (ServiceException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        private async void GenerateOTP_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingBar.Visibility = Visibility.Visible;
                string UMmobile = "9876543210";
                string UMEmailId = "user@email.com";
                if (!string.IsNullOrEmpty(UMmobile))
                    await Session.GenerateOTPUsingMobile(UMmobile);
                else if (!string.IsNullOrEmpty(UMEmailId))
                    await Session.GenerateOTPUsingEmailId(UMEmailId);

                MessageBox.Show("OTP generate completed");
            }
            catch (ServiceException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        private async void SigninUsingOTP_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingBar.Visibility = Visibility.Visible;
                string UMOTP = "12345";
                string UMMobile = "9876543210";
                string UMEmailId = "user@email.com";
                bool flag = false;
                if (!string.IsNullOrEmpty(UMMobile))
                    flag = await Session.SignInUsingOTP(UMMobile, UMOTP);
                else if (!string.IsNullOrEmpty(UMEmailId))
                    flag = await Session.SignInUsingOTP(UMEmailId, UMOTP);

                if (flag)
                {
                    MessageBox.Show("User signed in successfully");
                }
                else
                {
                    MessageBox.Show("User signed in failed");
                }

            }
            catch (ServiceException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        private async void GetProfileInfo_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingBar.Visibility = Visibility.Visible;
                UserProfile result = await Session.GetProfileInfo();
                if (result != null)
                {
                    MessageBox.Show("First Name: " + result.FirstName + ", Last Name: " + result.LastName + ", Mobile:" + result.Mobile + ", MobileVerified: " + result.MobileVerified + ", Email:" + result.Email + ", EmailVerified: " + result.EmailVerified);
                }
                else
                {
                    MessageBox.Show("Something went wrong");
                }
            }
            catch (ServiceException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        private async void UpdateProfile_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingBar.Visibility = Visibility.Visible;
                string FirstName = "Charles";
                string LastName = "Pearson";

                UserProfile userProfile = new UserProfile()
                {
                    FirstName = FirstName,
                    LastName = LastName
                };
                await Session.UpdateProfile(userProfile);
                MessageBox.Show("Profile update completed");
            }
            catch (ServiceException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        #region Save Cards

        private async void GetWallet_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingBar.Visibility = Visibility.Visible;
                //UserPaymentOptionsListBox.ItemsSource = await Wallet.GetWallet();
            }
            catch (ServiceException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        private async void SavePayment_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingBar.Visibility = Visibility.Visible;
                var success = await Wallet.SavePaymentOptions(
                    new List<PaymentOption>()
                    {
                        //Debit card
                        new PaymentOption()
                        {
                            CardType=CardType.Debit,
                            CardNumber="4242424242424242",
                            CardHolder="DCPearson Charles",
                            CardScheme=CreditCardType.Visa,
                            ExpiryDate=new CardExpiry()
                            {
                                Month = 12,
                                Year = 2018
                            },
                            Name="MyDebitCard"                             
                        },
                        //Credit card
                        new PaymentOption()
                        {
                            CardType=CardType.Credit,
                            CardNumber="4242424242424241",
                            CardHolder="CCPearson Charles",
                            CardScheme=CreditCardType.Visa,
                            ExpiryDate=new CardExpiry()
                            {
                                Month = 12,
                                Year = 2018
                            },
                            Name="MyCreditCard"                             
                        },
                        //Netbanking
                        new PaymentOption()
                        {
                            Bank="HDFC Bank",
                            MMID="123456",
                            IMPSRegisteredMobile = "9876543210"
                        }
                    }
                );
                if (success)
                {
                    MessageBox.Show("Payment option(s) saved successfully");
                }
                else
                {
                    MessageBox.Show("Failed to save Payment option(s)");
                }

            }
            catch (ServiceException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        private async void DeletePaymentOption_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingBar.Visibility = Visibility.Visible;
                //var success = await Wallet.DeletePaymentOption(UserPaymentOptionsListBox.SelectedItem as PaymentOption);
                //if (success)
                //{
                //    MessageBox.Show("Payment option deleted successfully");
                //    GetWallet_OnClick(null, null);
                //}
                //else
                //{
                //    MessageBox.Show("Failed to delete Payment option");
                //}
            }
            catch (ServiceException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        #region Citrus Cash

        private async void Card_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string AccountHolderName = "Pearson Charles";
                string CVV = "123";
                Int32 ExpiryMonth = 1;
                Int32 ExpiryYear = 2020;
                string CardNumber = "4242424242424242";
                CardType objCardType = CardType.Debit;

                this.amount.Value = 1;
                this.UserDetails.Email = "user@email.com";
                this.UserDetails.MobileNo = "9876543210";

                LoadingBar.Visibility = Visibility.Visible;
                var request = new LoadMoneyRequest();
                request.BillAmount = this.amount;
                request.RedirectUrl = "http://yourwebsite.com/return_url.php";
                request.UserDetails = this.UserDetails;
                request.PaymentDetails = new CardPayment()
                {
                    PaymentType = PaymentType.Card,
                    Card = new Card()
                    {
                        AccountHolderName = AccountHolderName,
                        CVV = CVV,
                        ExpiryDate =
                            new CardExpiry()
                            {
                                Month = ExpiryMonth,
                                Year = ExpiryYear
                            },
                        CardNumber = CardNumber,
                        CardType = objCardType
                    }
                };

                var result = await Wallet.LoadMoneyAsync(request);
                if (result != null && !string.IsNullOrEmpty(result.RedirectUrl))
                {
                    MessageBox.Show("Result Code:" + result.Code + ", Status: " + result.Status);
                }
                else
                {
                    MessageBox.Show("Something went wrong");
                }
            }
            catch (ServiceException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        private async void Token_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingBar.Visibility = Visibility.Visible;
                var request = new LoadMoneyRequest();
                this.amount.Value = orderAmount;
                request.BillAmount = this.amount;
                request.RedirectUrl = "http://yourwebsite.com/return_url.php";
                request.UserDetails = this.UserDetails;
                request.PaymentDetails = new TokenPayment()
                {
                    PaymentType = PaymentType.Token,
                    CVV = "000",
                    TokenId = "acdb59d66d55bc95dc76a86b1b99387c"
                };

                var result = await Wallet.LoadMoneyAsync(request);
                if (result != null)
                {
                    MessageBox.Show("Result Code:" + result.Code + ", Status: " + result.Status);
                }
                else
                {
                    MessageBox.Show("Something went wrong");
                }
            }
            catch (ServiceException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        private async void NetBanking_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingBar.Visibility = Visibility.Visible;
                string BankCode = "CID002";
                var request = new LoadMoneyRequest();
                this.amount.Value = orderAmount;
                request.BillAmount = this.amount;
                request.RedirectUrl = "http://yourwebsite.com/return_url.php";
                request.UserDetails = this.UserDetails;
                request.PaymentDetails = new NetBankingPayment()
                {
                    PaymentType = PaymentType.NetBanking,
                    NetBanking = new NetBanking()
                    {
                        Code = BankCode
                    }
                };

                var result = await Wallet.LoadMoneyAsync(request);
                if (result != null)
                {
                    MessageBox.Show("Result Code:" + result.Code + ", Status: " + result.Status);
                }
                else
                {
                    MessageBox.Show("Something went wrong");
                }
            }
            catch (ServiceException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        private async void GetMerchantPaymentOptions_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingBar.Visibility = Visibility.Visible;
                await Wallet.GetMerchantPaymentOptions();
                MessageBox.Show("Merchant payment options retreived");
            }
            catch (ServiceException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        private async void WithdrawMoneyOption_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingBar.Visibility = Visibility.Visible;
                var result = await Wallet.WithdrawMoney(new WithdrawMoneyRequest()
                {
                    AccountNo = "042401523201",
                    Amount = 1,
                    IFSC = "ICIC0000424",
                    AccoutnHolderName = "Salil Godbole"

                });
                if (result.Status != "FAILED")
                {
                    MessageBox.Show("Amount withdrawn successfully.");
                }
                else
                {
                    MessageBox.Show("Failed to withdraw amount due to " + result.Reason);
                }
            }
            catch (ServiceException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        private async Task<PaymentBill> GetBillAsync(double orderAmount)
        {
            return await PaymentGateway.GetBillAsync("https://salty-plateau-1529.herokuapp.com/billGenerator.sandbox.php", new Amount()
            {
                Value = 1
            });
        }

        private async void CitrusCashPayment_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingBar.Visibility = Visibility.Visible;
                var bill = await GetBillAsync(orderAmount);
                var payment = new CardPayment()
                {
                    PaymentType = PaymentType.Prepaid,
                    Card = new Card()
                    {
                        AccountHolderName = "maheshmutyal1@mailinator.com",
                        //CVV = "000",
                        //ExpiryDate =
                        //    new CardExpiry()
                        //    {
                        //        Month = 11,
                        //        Year = 2030
                        //    },
                        //CardNumber = "1111111111111111",
                        //CardType = CardType.Prepaid
                    }
                };

                var pg = new PaymentGateway(bill, payment, UserDetails);
                var result = await pg.ProcessPaymentAsync();
                if (result != null && !string.IsNullOrEmpty(result.RedirectUrl))
                {
                    string cookieresult = await Wallet.GetCookie(payment.Card.AccountHolderName, "test@123");
                    if (!string.IsNullOrEmpty(cookieresult))
                    {
                        MessageBox.Show("Result Code:" + result.Code + ", Status: " + result.Status + ", Redirect URL:" + result.RedirectUrl);
                    }
                    else
                    {
                        MessageBox.Show("Something went wrong");
                    }
                }
                else
                {
                    MessageBox.Show("Something went wrong");
                }
            }
            catch (ServiceException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        private async void GetLoadMoneyPaymentOptions_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingBar.Visibility = Visibility.Visible;
                await Wallet.GetLoadMoneyPaymentOptions();
                MessageBox.Show("Merchant payment options retreived");
            }
            catch (ServiceException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        private async void TranMoneyEmail_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingBar.Visibility = Visibility.Visible;
                string to = "user@email.com";
                double amount = 1;
                string message = "my msg";

                var result = await Wallet.TransferMoneyUsingEmail(new TransferMoneyRequest()
                {
                    To = to,
                    Amount = amount,
                    Message = message,
                    Currency = "INR"

                });
                if (result.Status != "FAILED")
                {
                    MessageBox.Show("Amount transfer successfully.");
                }
                else
                {
                    MessageBox.Show("Failed to transfer amount due to " + result.Reason);
                }
            }
            catch (ServiceException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        private async void TranMoneyMobile_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingBar.Visibility = Visibility.Visible;
                string to = "user@email.com";
                double amount = 1;
                string message = "my msg";

                var result = await Wallet.TransferMoneyUsingMobile(new TransferMoneyRequest()
                {
                    To = to,
                    Amount = amount,
                    Message = message,
                    Currency = "INR"

                });
                if (result.Status != "FAILED")
                {
                    MessageBox.Show("Amount transfer successfully.");
                }
                else
                {
                    MessageBox.Show("Failed to transfer amount due to " + result.Reason);
                }
            }
            catch (ServiceException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        private async void GetWithdrawInfo_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingBar.Visibility = Visibility.Visible;
                var withdrawInfoResponse = await Wallet.GetWithdrawInfo();
                MessageBox.Show("Withdraw info Type: " + withdrawInfoResponse.Type + " Owner: " + withdrawInfoResponse.CashoutAccount.Owner + " Number: " + withdrawInfoResponse.CashoutAccount.Number + " Branch: " + withdrawInfoResponse.CashoutAccount.Branch);
            }
            catch (ServiceException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        private async void SaveWithdrawMoney_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingBar.Visibility = Visibility.Visible;
                string owner = "mahesh mutyal";
                string branch = "HSBC0000123";
                string number = "123456789987654";
                string type = "prepaid";

                var result = await Wallet.SaveWithdrawInfo(new WithdrawInfoResponse()
                {
                    Type = type,
                    Currency = "INR",
                    CashoutAccount = new CashoutAccount()
                    {
                        Owner = owner,
                        Branch = branch,
                        Number = number
                    }
                });

                MessageBox.Show("Save withdraw info successfully.");
            }
            catch (ServiceException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        #endregion
    }
}
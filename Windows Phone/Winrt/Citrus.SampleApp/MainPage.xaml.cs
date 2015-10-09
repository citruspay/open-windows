using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Citrus.SampleApp
{
    using Windows.UI.Popups;

    using Citrus.SDK;
    using Citrus.SDK.Common;
    using Citrus.SDK.Entity;
    using System.Text;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Init

        public MainPage()
        {
            this.InitializeComponent();
            //Config.Initialize(EnvironmentType.Sandbox, "test-signup", "c78ec84e389814a05d3ae46546d16d2e", "test-signin", "52f7e15efd4208cf5345dd554443fd99", "nativeSDK");
            //Config.Initialize(EnvironmentType.Sandbox, "o9s2w3ml3q-signup", "c8476d512e306f19265d532dae60b966", "o9s2w3ml3q-signin", "e74f57011fb12ff49c9b2e9ca4133f3a", "testedURL");
            Config.Initialize(EnvironmentType.Sandbox, "test-signup", "c78ec84e389814a05d3ae46546d16d2e", "test-signin", "52f7e15efd4208cf5345dd554443fd99", "testing");
            this.NavigationCacheMode = NavigationCacheMode.Required;
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

                string AccountHolderName = txtcpaymentAccountHolderName.Text;
                string CVV = txtcpaymentCVV.Text;
                Int32 ExpiryMonth = Convert.ToInt32(txtcpaymentExpiryMM.Text);
                Int32 ExpiryYear = Convert.ToInt32(txtcpaymentExpiryYYYY.Text);
                string CardNumber = txtcpaymentCardNumber.Text;
                string strCardType = txtcpaymentCardType.Text;
                CardType objCardType;
                if (strCardType.ToLower() == "debit")
                    objCardType = CardType.Debit;
                else if (strCardType.ToLower() == "credit")
                    objCardType = CardType.Credit;
                else if (strCardType.ToLower() == "prepaid")
                    objCardType = CardType.Prepaid;
                else
                    objCardType = CardType.UnKnown;

                this.amount.Value = Convert.ToInt32(txtcpaymentAmount.Text);
                this.UserDetails.Email = txtcpaymentemailid.Text;
                this.UserDetails.MobileNo = txtcpaymentmobile.Text;
                orderAmount = Convert.ToInt32(txtcpaymentAmount.Text);
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
                    OpenPaymentWebView(result.RedirectUrl, null);
                    new MessageDialog("Result Code:" + result.Code + ", Status: " + result.Status + ", Redirect URL:" + result.RedirectUrl).ShowAsync();
                }
                else
                {
                    new MessageDialog("Something went wrong").ShowAsync();
                }
            }
            catch (ServiceException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (ArgumentException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (UnauthorizedAccessException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
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
                    OpenPaymentWebView(result.RedirectUrl, null);
                    new MessageDialog("Result Code:" + result.Code + ", Status: " + result.Status + ", Redirect URL:" + result.RedirectUrl).ShowAsync();
                }
                else
                {
                    new MessageDialog("Something went wrong").ShowAsync();
                }
            }
            catch (ServiceException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (ArgumentException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (UnauthorizedAccessException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
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
                string BankCode = txtPaymentBankCode.Text;
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
                    OpenPaymentWebView(result.RedirectUrl, null);
                    new MessageDialog("Result Code:" + result.Code + ", Status: " + result.Status + ", Redirect URL:" + result.RedirectUrl).ShowAsync();
                }
                else
                {
                    new MessageDialog("Something went wrong").ShowAsync();
                }
            }
            catch (ServiceException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (ArgumentException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (UnauthorizedAccessException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
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
                    new MessageDialog("Result Code:" + result.Code + ", Status: " + result.Status + ", Redirect URL:" + result.RedirectUrl).ShowAsync();
                }
                else
                {
                    new MessageDialog("Something went wrong").ShowAsync();
                }
            }
            catch (ServiceException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (ArgumentException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (UnauthorizedAccessException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
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
                string emailid = txtemailid.Text;
                string mobile = txtmobile.Text;
                string password = txtpassword.Text;

                ResultPanel.DataContext = null;
                LoadingBar.Visibility = Visibility.Visible;
                ResultPanel.DataContext = await Session.SignupUser(emailid, mobile, password);
                new MessageDialog("User signed up successfully").ShowAsync();
            }
            catch (ServiceException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (ArgumentException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (UnauthorizedAccessException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
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
                string emailid = txtemailid.Text;
                string mobile = txtmobile.Text;
                string password = txtpassword.Text;

                var isCitrusMember = await Session.IsCitrusMemeber(emailid, password);
                if (isCitrusMember)
                {
                    var user = await Session.GetBalance();

                    new MessageDialog("User Already A Citrus Member. Please Sign In User.").ShowAsync();
                }
                else
                {
                    new MessageDialog("User Not A Citrus Member. Please Sign Up User.").ShowAsync();
                }
            }
            catch (ServiceException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (ArgumentException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (UnauthorizedAccessException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
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
                string emailid = txtsigninemailid.Text;
                string password = txtsigninpassword.Text;

                loginStatus.Text = string.Empty;
                LoadingBar.Visibility = Visibility.Visible;
                if (await Session.SigninUser(emailid, password))
                {
                    loginStatus.Text = "Signed In";
                    new MessageDialog("User signed in successfully").ShowAsync();
                }
                else
                {
                    loginStatus.Text = "";
                    new MessageDialog("User signed in failed").ShowAsync();
                }

                //if (await Session.SigninUser("salilgodbole@gmail.com", "Salil@123"))
                //{
                //    loginStatus.Text = "Signed In";
                //    new MessageDialog("User signed in successfully").ShowAsync();
                //}

                //var obj = await Session.BindUser("joyce.bansode@mailinator.com", "7709829851");

            }
            catch (ServiceException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (ArgumentException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (UnauthorizedAccessException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
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
                string emailid = txtsigninemailid.Text;
                string password = txtsigninpassword.Text;

                loginStatus.Text = string.Empty;
                LoadingBar.Visibility = Visibility.Visible;

                var userObj = await Session.BindUser(emailid, password);
                if (userObj != null && !string.IsNullOrEmpty(userObj.UserName))
                {
                    new MessageDialog("User signed in successfully").ShowAsync();
                }
                else
                {
                    new MessageDialog("User signed in failed").ShowAsync();
                }

            }
            catch (ServiceException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (ArgumentException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (UnauthorizedAccessException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
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
                new MessageDialog("Password reset completed").ShowAsync();
            }
            catch (ServiceException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (ArgumentException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (UnauthorizedAccessException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
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
                string UMmobile = txtUMmobile.Text;
                await Session.UpdateMobile(UMmobile);
                new MessageDialog("Mobile update completed").ShowAsync();
            }
            catch (ServiceException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (ArgumentException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (UnauthorizedAccessException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
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
                string UMmobile = txtUMmobile.Text;
                string UMEmailId = txtUMEmailId.Text;
                if (!string.IsNullOrEmpty(UMmobile))
                    await Session.GenerateOTPUsingMobile(UMmobile);
                else if (!string.IsNullOrEmpty(UMEmailId))
                    await Session.GenerateOTPUsingEmailId(UMEmailId);

                new MessageDialog("OTP generate completed").ShowAsync();
            }
            catch (ServiceException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (ArgumentException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (UnauthorizedAccessException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
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
                string UMOTP = txtUMOTP.Text;
                string UMMobile = txtUMmobile.Text;
                string UMEmailId = txtUMEmailId.Text;
                bool flag = false;
                if (!string.IsNullOrEmpty(UMMobile))
                    flag = await Session.SignInUsingOTP(UMMobile, UMOTP);
                else if (!string.IsNullOrEmpty(UMEmailId))
                    flag = await Session.SignInUsingOTP(UMEmailId, UMOTP);

                if (flag)
                {
                    new MessageDialog("User signed in successfully").ShowAsync();
                }
                else
                {
                    new MessageDialog("User signed in failed").ShowAsync();
                }

            }
            catch (ServiceException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (ArgumentException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (UnauthorizedAccessException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
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
                    new MessageDialog("First Name: " + result.FirstName + ", Last Name: " + result.LastName + ", Mobile:" + result.Mobile + ", MobileVerified: " + result.MobileVerified + ", Email:" + result.Email + ", EmailVerified: " + result.EmailVerified).ShowAsync();
                }
                else
                {
                    new MessageDialog("Something went wrong").ShowAsync();
                }
            }
            catch (ServiceException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (ArgumentException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (UnauthorizedAccessException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
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
                string FirstName = txtUMFirstName.Text;
                string LastName = txtUMLastName.Text;

                UserProfile userProfile = new UserProfile()
                {
                    FirstName = FirstName,
                    LastName = LastName
                };
                await Session.UpdateProfile(userProfile);
                new MessageDialog("Profile update completed").ShowAsync();
            }
            catch (ServiceException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (ArgumentException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (UnauthorizedAccessException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
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
                UserPaymentOptionsListBox.ItemsSource = await Wallet.GetWallet();
            }
            catch (ServiceException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (ArgumentException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (UnauthorizedAccessException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
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
                    new MessageDialog("Payment option(s) saved successfully").ShowAsync();
                }
                else
                {
                    new MessageDialog("Failed to save Payment option(s)").ShowAsync();
                }

            }
            catch (ServiceException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (ArgumentException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (UnauthorizedAccessException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
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
                var success = await Wallet.DeletePaymentOption(UserPaymentOptionsListBox.SelectedItem as PaymentOption);
                if (success)
                {
                    new MessageDialog("Payment option deleted successfully").ShowAsync();
                    GetWallet_OnClick(null, null);
                }
                else
                {
                    new MessageDialog("Failed to delete Payment option").ShowAsync();
                }
            }
            catch (ServiceException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (ArgumentException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (UnauthorizedAccessException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
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
                string AccountHolderName = txtcardAccountHolderName.Text;
                string CVV = txtcardCVV.Text;
                Int32 ExpiryMonth = Convert.ToInt32(txtcardExpiryMM.Text);
                Int32 ExpiryYear = Convert.ToInt32(txtcardExpiryYYYY.Text);
                string CardNumber = txtcardCardNumber.Text;
                string strCardType = txtcardCardType.Text;
                CardType objCardType;
                if (strCardType.ToLower() == "debit")
                    objCardType = CardType.Debit;
                else if (strCardType.ToLower() == "credit")
                    objCardType = CardType.Credit;
                else if (strCardType.ToLower() == "prepaid")
                    objCardType = CardType.Prepaid;
                else
                    objCardType = CardType.UnKnown;

                this.amount.Value = Convert.ToInt32(txtcardAmount.Text);
                this.UserDetails.Email = txtcardemailid.Text;
                this.UserDetails.MobileNo = txtcardmobile.Text;

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
                    OpenWebView(result.RedirectUrl, "");
                    new MessageDialog("Result Code:" + result.Code + ", Status: " + result.Status).ShowAsync();
                }
                else
                {
                    new MessageDialog("Something went wrong").ShowAsync();
                }
            }
            catch (ServiceException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (ArgumentException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (UnauthorizedAccessException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
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
                    new MessageDialog("Result Code:" + result.Code + ", Status: " + result.Status).ShowAsync();
                }
                else
                {
                    new MessageDialog("Something went wrong").ShowAsync();
                }
            }
            catch (ServiceException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (ArgumentException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (UnauthorizedAccessException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
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
                string BankCode = txtLoadBankCode.Text;
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
                    OpenWebView(result.RedirectUrl, null);
                    new MessageDialog("Result Code:" + result.Code + ", Status: " + result.Status).ShowAsync();
                }
                else
                {
                    new MessageDialog("Something went wrong").ShowAsync();
                }
            }
            catch (ServiceException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (ArgumentException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (UnauthorizedAccessException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
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
                new MessageDialog("Merchant payment options retreived").ShowAsync();
            }
            catch (ServiceException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (ArgumentException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (UnauthorizedAccessException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
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
                    new MessageDialog("Amount withdrawn successfully.").ShowAsync();

                }
                else
                {
                    new MessageDialog("Failed to withdraw amount due to " + result.Reason).ShowAsync();
                }
            }
            catch (ServiceException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (ArgumentException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (UnauthorizedAccessException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
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
                        OpenWebView(result.RedirectUrl, cookieresult);
                        new MessageDialog("Result Code:" + result.Code + ", Status: " + result.Status + ", Redirect URL:" + result.RedirectUrl).ShowAsync();
                    }
                    else
                    {
                        new MessageDialog("Something went wrong").ShowAsync();
                    }
                }
                else
                {
                    new MessageDialog("Something went wrong").ShowAsync();
                }
            }
            catch (ServiceException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (ArgumentException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (UnauthorizedAccessException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            finally
            {
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        private void OpenWebView(string RedirectUrl, string strcookie)
        {
            try
            {
                Uri baseUri = new Uri(RedirectUrl);
                if (!string.IsNullOrEmpty(strcookie))
                {
                    Windows.Web.Http.Filters.HttpBaseProtocolFilter filter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
                    Windows.Web.Http.HttpCookie cookie = new Windows.Web.Http.HttpCookie(baseUri.Host, baseUri.Host, "");
                    cookie.Value = strcookie;
                    filter.CookieManager.SetCookie(cookie, false);
                }

                Windows.Web.Http.HttpRequestMessage httpRequestMessage = new Windows.Web.Http.HttpRequestMessage(Windows.Web.Http.HttpMethod.Get, baseUri);
                citruswebview.NavigateWithHttpRequestMessage(httpRequestMessage);
            }
            catch (Exception exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
        }

        private void OpenPaymentWebView(string RedirectUrl, string strcookie)
        {
            try
            {
                //Uri targetUri = new Uri(result.RedirectUrl);
                //citruswebview.Navigate(targetUri);

                Uri baseUri = new Uri(RedirectUrl);
                Windows.Web.Http.Filters.HttpBaseProtocolFilter filter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
                Windows.Web.Http.HttpCookie cookie = new Windows.Web.Http.HttpCookie("cookieName", baseUri.Host, "/");
                cookie.Value = "cookieValue";
                filter.CookieManager.SetCookie(cookie, false);

                Windows.Web.Http.HttpRequestMessage httpRequestMessage = new Windows.Web.Http.HttpRequestMessage(Windows.Web.Http.HttpMethod.Get, baseUri);
                paymentwebview.NavigateWithHttpRequestMessage(httpRequestMessage);

            }
            catch (Exception exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
        }

        private async void GetLoadMoneyPaymentOptions_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingBar.Visibility = Visibility.Visible;
                await Wallet.GetLoadMoneyPaymentOptions();
                new MessageDialog("Merchant payment options retreived").ShowAsync();
            }
            catch (ServiceException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (ArgumentException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (UnauthorizedAccessException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
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
                string to = txtTranToEmail.Text.Trim();
                double amount = Convert.ToDouble(txtTranAmount.Text.Trim());
                string message = txtTranMessage.Text.Trim();

                var result = await Wallet.TransferMoneyUsingEmail(new TransferMoneyRequest()
                {
                    To = to,
                    Amount = amount,
                    Message = message,
                    Currency = "INR"

                });
                if (result.Status != "FAILED")
                {
                    new MessageDialog("Amount transfer successfully.").ShowAsync();
                }
                else
                {
                    new MessageDialog("Failed to transfer amount due to " + result.Reason).ShowAsync();
                }
            }
            catch (ServiceException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (ArgumentException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (UnauthorizedAccessException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
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
                string to = txtTranToMobile.Text.Trim();
                Int32 amount = Convert.ToInt32(txtTranAmount.Text.Trim());
                string message = txtTranMessage.Text.Trim();

                var result = await Wallet.TransferMoneyUsingMobile(new TransferMoneyRequest()
                {
                    To = to,
                    Amount = amount,
                    Message = message,
                    Currency = "INR"

                });
                if (result.Status != "FAILED")
                {
                    new MessageDialog("Amount transfer successfully.").ShowAsync();
                }
                else
                {
                    new MessageDialog("Failed to transfer amount due to " + result.Reason).ShowAsync();
                }
            }
            catch (ServiceException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (ArgumentException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (UnauthorizedAccessException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
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
                new MessageDialog("Withdraw info Type: " + withdrawInfoResponse.Type + " Owner: " + withdrawInfoResponse.CashoutAccount.Owner + " Number: " + withdrawInfoResponse.CashoutAccount.Number + " Branch: " + withdrawInfoResponse.CashoutAccount.Branch).ShowAsync();
            }
            catch (ServiceException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (ArgumentException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (UnauthorizedAccessException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
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
                string owner = txtWithdrawOwner.Text.Trim();
                string branch = txtWithdrawBranch.Text.Trim();
                string number = txtWithdrawNumber.Text.Trim();
                string type = txtWithdrawType.Text.Trim();

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

                new MessageDialog("Save withdraw info successfully.").ShowAsync();
            }
            catch (ServiceException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (ArgumentException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            catch (UnauthorizedAccessException exception)
            {
                new MessageDialog(exception.Message).ShowAsync();
            }
            finally
            {
                LoadingBar.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

    }
}

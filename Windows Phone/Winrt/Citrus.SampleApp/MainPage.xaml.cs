using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Config.Initialize(EnvironmentType.Sandbox, "test-signup", "c78ec84e389814a05d3ae46546d16d2e", "test-signin", "52f7e15efd4208cf5345dd554443fd99", "prepaid");
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private async void SignUp_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ResultPanel.DataContext = null;
                LoadingBar.Visibility = Visibility.Visible;
                ResultPanel.DataContext = await Session.SignupUser("user777@gmail.com", "9876543210", "password#123");
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

        private async void SignIn_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                loginStatus.Text = string.Empty;
                LoadingBar.Visibility = Visibility.Visible;
                if (await Session.SigninUser("user777@gmail.com", "password#123"))
                {
                    loginStatus.Text = "Signed In";
                    new MessageDialog("User signed in successfully").ShowAsync();
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

        private UserDetails UserDetails = new UserDetails()
        {
            Address = new Address()
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

        private Amount amount = new Amount()
        {
            Value = 100,
            CurrencyType = "INR"
        };

        private async void Card_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
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

        private async void Token_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingBar.Visibility = Visibility.Visible;
                var request = new LoadMoneyRequest();
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
                var request = new LoadMoneyRequest();
                request.BillAmount = this.amount;
                request.RedirectUrl = "http://yourwebsite.com/return_url.php";
                request.UserDetails = this.UserDetails;
                request.PaymentDetails = new NetBankingPayment()
                {
                    PaymentType = PaymentType.NetBanking,
                    NetBanking = new NetBanking()
                    {
                        Code = "CID002"
                    }
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

        private async void IsCitrusMember_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingBar.Visibility = Visibility.Visible;
                var isCitrusMember = await Session.IsCitrusMemeber("user777@gmail.com", "9876543210");
                if (isCitrusMember)
                {
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

        private async void WithdrawMoneyOption_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingBar.Visibility = Visibility.Visible;
                var result = await Wallet.WithdrawMoney(new WithdrawMoneyRequest()
                {
                    Account = "042401523201",
                    Amount = 1.00,
                    Currency = "INR",
                    IFSC_Code = "ICIC0000424",
                    Owner = "Salil Godbole"

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

    }
}

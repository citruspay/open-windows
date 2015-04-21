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

    public partial class Operations : PhoneApplicationPage
    {
        public Operations()
        {
            InitializeComponent();
            Config.Initialize(EnvironmentType.Sandbox, "test-signup", "c78ec84e389814a05d3ae46546d16d2e", "test-signin", "52f7e15efd4208cf5345dd554443fd99");
        }

        private async void SignUp_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ResultPanel.DataContext = null;
                LoadingBar.Visibility = Visibility.Visible;
                ResultPanel.DataContext = await Session.SignupUser("user7@gmail.com", "9876543210", "password#123");
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

        private async void SignIn_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                loginStatus.Text = string.Empty;
                LoadingBar.Visibility = Visibility.Visible;
                if (await Session.SigninUser("user5@email.com", "temp#123"))
                {
                    loginStatus.Text = "Signed In";
                    MessageBox.Show("User signed in successfully");
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
    }
}
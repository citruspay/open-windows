using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Citrus.SDK;
using Citrus.SDK.Common;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Citrus.SampleApp.Resources;

namespace Citrus.SampleApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            Config.Environment = EnvironmentType.Sandbox;
            Config.SignUpId = "test-signup";
            Config.SignUpSecret = "c78ec84e389814a05d3ae46546d16d2e";
            Config.SignInId = "meru-webapp-payment-v1";
            Config.SignInSecret = "579d6f2c6af04d1dfc605b46da51b450";

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        async void MainPage_Loaded(object sender, object e)
        {
            //RandomPasswordGenerator ps = new RandomPasswordGenerator();
            //var result = ps.Generate("kamal@gmail.com", "12324422");
            //var res1 = ps.Generate("ram@hotmail.com", "12548759");
            //var res = ps.Generate("ram@gmail.com", "5478458754");
            //var res2 = ps.Generate("ram@celerapps.com", "98972454872154");
            try
            {
                var res = await Session.SignupUser("ram121212@celerapps.com", "1245788956", "enter321");
            }
            catch (ServiceException exception)
            {
                MessageBox.Show(exception.Message);
            }
            //var result =await Session.ResetPassword();
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}
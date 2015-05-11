namespace Citrus.SDK.Test
{
    using System;

    using Citrus.SDK.Common;

    using NUnit.Framework;

    [TestFixture]
    public class SessionTest
    {
        private string email;
        private string mobile;
        private string password;

        private string registeredUserName;
        private string registeredPassword;
        private Random random;

        [SetUp]
        public void Setup()
        {
            Config.Initialize(EnvironmentType.Sandbox, "test-signup", "c78ec84e389814a05d3ae46546d16d2e", "test-signin", "52f7e15efd4208cf5345dd554443fd99");
            this.email = this.GetSignupEmail();
            this.mobile = "9876543210";
            this.password = "password#123";
            this.registeredUserName = "registereduser@citrus.com";
            this.registeredPassword = "password#123";
            random = new Random();
        }

        [Test]
        public async void SignupUser_Returns_UserBalance()
        {
            this.Setup();
            var user = await Session.SignupUser(email, mobile, password);

            Assert.IsNotNull(user);

            Assert.AreEqual(email, user.Email);

            Assert.IsNotNullOrEmpty(user.UserName);

            Assert.AreEqual(mobile, user.Mobile);

            Assert.AreEqual(password, user.Password);

            Assert.IsNotNullOrEmpty(user.BalanceAmount);

            Assert.IsNotNullOrEmpty(user.CurrencyFormat);
        }

        [Test]
        public void SignupUser_Fails_OnRegisteredEmail()
        {
            Assert.Throws<ServiceException>(async () => await Session.SignupUser(email, mobile, password));
        }

        [Test]
        public void SignupUser_ThrowsException_OnInvalidConfig()
        {
            Config.Reset();
            email = this.GetSignupEmail();
            Assert.Throws<ServiceException>(async () => await Session.SignupUser(email, mobile, password));
        }

        [Test]
        public void SignupUser_ThrowsException_OnInvalidInput()
        {
            this.Setup();

            Assert.Throws<ArgumentException>(async () => await Session.SignupUser("", "", ""));
        }

        private string GetSignupEmail()
        {
            return "user" + random.Next() + "@youremail.com";
        }

        [Test]
        public async void Signin_ReturnsSuccess_OnValidCredentials()
        {
            Assert.IsTrue(await Session.SigninUser(registeredUserName, registeredPassword));
        }

        [Test]
        public void Signin_ThrowsException_OnUnregisteredUser()
        {
            Assert.Throws<ServiceException>(async () => await Session.SigninUser(this.GetSignupEmail(), password));
        }

        [Test]
        public void Signin_ThrowsException_OnInvalidInput()
        {
            Assert.Throws<ArgumentException>(async () => await Session.SigninUser("", ""));
        }

        [Test]
        public void SigninUser_ThrowsException_OnInvalidConfig()
        {
            Config.Reset();
            Assert.Throws<ServiceException>(async () => await Session.SigninUser(email, password));
        }

        [Test]
        public async void UpdatePassword_ReturnsSuccess_OnValidInput()
        {
            await Session.SigninUser(registeredUserName, registeredPassword);
            var success = await Session.UpdatePassword(registeredPassword, registeredPassword);
            Assert.IsTrue(success);
        }

        [Test]
        public void UpdatePassword_ThrowsException_OnInvalidSession()
        {
            Session.SignOut();

            Assert.Throws<UnauthorizedAccessException>(async () => await Session.UpdatePassword(registeredPassword, registeredPassword));
        }

        [Test]
        public async void UpdatePassword_ThrowsException_OnInvalidInput()
        {
            await Session.SigninUser(registeredUserName, registeredPassword);
            Assert.Throws<ServiceException>(async () => await Session.UpdatePassword("", ""));
        }


        [Test]
        public async void GetBalance_ReturnsSuccess_OnValidSession()
        {
            await Session.SigninUser(registeredUserName, registeredPassword);

            var user = await Session.GetBalance();

            Assert.IsNotNullOrEmpty(user.BalanceAmount);

            Assert.IsNotNullOrEmpty(user.CurrencyFormat);
        }

        [Test]
        public void GetBalance_ThrowsException_OnInvalidSession()
        {
            Session.SignOut();

            Assert.Throws<UnauthorizedAccessException>(async () => await Session.GetBalance());
        }

        [Test]
        public async void ResetPassword_Succeeds_OnValidInput()
        {
            await Session.ResetPassword(registeredUserName);
        }

        [Test]
        public void ResetPassword_ThrowsException_OnInvalidInput()
        {
            Assert.Throws<ServiceException>(async () => await Session.ResetPassword(""));
        }

        [Test]
        public void ResetPassword_ThrowsException_OnInvalidConfig()
        {
            Config.Reset();

            Assert.Throws<ServiceException>(async () => await Session.ResetPassword(email));
        }
    }
}

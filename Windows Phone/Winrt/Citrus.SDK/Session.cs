// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Session.cs" company="Citrus Payment Solutions Pvt. Ltd.">
//   Copyright 2015 Citrus Payment Solutions Pvt. Ltd.
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//   http://www.apache.org/licenses/LICENSE-2.0
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// <summary>
//   Session state of the user
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Citrus.SDK
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    using Citrus.SDK.Common;
    using Citrus.SDK.Entity;

    /// <summary>
    /// Session
    /// </summary>
    public static class Session
    {
        #region Static Fields

        /// <summary>
        /// Sign Up Token
        /// </summary>
        internal static OAuthToken signUpToken;

        /// <summary>
        /// Sign Up Token
        /// </summary>
        internal static OAuthToken signInToken;


        internal static OAuthToken simpleToken;

        /// <summary>
        /// Session User
        /// </summary>
        private static User user;

        internal static Config Config;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Get Auth token.
        /// </summary>
        /// <param name="authTokenType">
        /// Type of auth token to get
        /// </param>
        /// <returns>
        /// Auth token
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        internal static async Task<string> GetAuthTokenAsync(AuthTokenType authTokenType)
        {
            switch (authTokenType)
            {
                case AuthTokenType.SignIn:
                    return signInToken != null ? await signInToken.GetActiveTokenAsync() : string.Empty;
                case AuthTokenType.SignUp:
                    return signUpToken != null ? await signUpToken.GetActiveTokenAsync() : string.Empty;
                case AuthTokenType.Simple:
                    return simpleToken != null ? await simpleToken.GetActiveTokenAsync() : string.Empty;
                default:
                    return string.Empty;
            }
        }

        internal static async Task GetTokenIfEmptyAsync(AuthTokenType authTokenType)
        {
            if (authTokenType == AuthTokenType.SignIn && signInToken == null)
            {
                signInToken = Utility.ReadFromLocalStorage<OAuthToken>(Utility.SignInTokenKey);
                if (signInToken != null)
                    await signInToken.GetActiveTokenAsync();
            }
            else if (authTokenType == AuthTokenType.SignUp && signUpToken == null)
            {
                signUpToken = Utility.ReadFromLocalStorage<OAuthToken>(Utility.SignUpTokenKey);
                if (signUpToken != null)
                    await signUpToken.GetActiveTokenAsync();
            }
            else if (authTokenType == AuthTokenType.Simple && simpleToken == null)
            {
                simpleToken = Utility.ReadFromLocalStorage<OAuthToken>(Utility.SimpleTokenKey);
                if (simpleToken != null)
                    await simpleToken.GetActiveTokenAsync();
            }
        }

        /// <summary>
        /// Gets the User account's balance.
        /// </summary>
        /// <returns>
        /// Account balance
        /// </returns>
        public static async Task<User> GetBalance()
        {
            await GetTokenIfEmptyAsync(AuthTokenType.Simple);

            if (simpleToken == null || string.IsNullOrEmpty(simpleToken.AccessToken))
            {
                throw new UnauthorizedAccessException("User is not logged to perform Get Balance");
            }

            var rest = new RestWrapper();
            var result = await rest.Get<User>(Service.GetBalance, AuthTokenType.Simple);

            if (!(result is Error))
            {
                var balance = (User)result;
                user.BalanceAmount = balance.BalanceAmount;
                user.CurrencyFormat = balance.CurrencyFormat;
                return user;
            }

            Utility.ParseAndThrowError((result as Error).Response);

            return null;
        }

        /// <summary>
        /// Reset the password.
        /// </summary>
        /// <returns>
        /// Reset state: true for success, false for failure
        /// </returns>
        public static async Task ResetPassword(string userName)
        {
            if (string.IsNullOrEmpty(Config.SignUpId) || string.IsNullOrEmpty(Config.SignUpSecret))
            {
                throw new ServiceException("Invalid Configuration: Client ID & Client Secret");
            }

            await GetSignupToken();
            var rest = new RestWrapper();
            var result =
                await
                rest.Post<User>(
                    Service.ResetPassword,
                    new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>(
                                "username", 
                                userName)
                        },
                    AuthTokenType.SignUp);

            if (result is Error)
            {
                Utility.ParseAndThrowError((result as Error).Response);
            }
        }

        /// <summary>
        /// Sign in the user account.
        /// </summary>
        /// <param name="userName">
        /// Citrus Pay UserName.
        /// </param>
        /// <param name="password">
        /// Citrus Pay Password.
        /// </param>
        /// <returns>
        /// Sign In state, true for success, false for failure
        /// </returns>
        public static async Task<bool> SigninUser(string userName, string password)
        {
            if (string.IsNullOrEmpty(Config.SignInId) || string.IsNullOrEmpty(Config.SignInSecret))
            {
                throw new ServiceException("Invalid Configuration: Client ID & Client Secret");
            }

            var request = new SigninRequest { User = new User { UserName = userName, Password = password } };

            var rest = new RestWrapper();
            var result = await rest.Post<OAuthToken>(Service.Signin, AuthTokenType.None, request);
            if (!(result is Error))
            {
                signInToken = result as OAuthToken;
                simpleToken = result as OAuthToken;
                Utility.SaveToLocalStorage(Utility.SignInTokenKey, signInToken);
                Utility.SaveToLocalStorage(Utility.SimpleTokenKey, simpleToken);
                return signInToken != null && !string.IsNullOrEmpty(signInToken.AccessToken);
            }

            Utility.ParseAndThrowError((result as Error).Response);
            return false;
        }



        /// <summary>
        /// Sign up an user account.
        /// </summary>
        /// <param name="email">
        /// Email
        /// </param>
        /// <param name="mobile">
        /// Mobile
        /// </param>
        /// <param name="password"></param>
        /// <returns>
        /// Logged In user's detail
        /// </returns>
        public static async Task<User> SignupUser(string email, string mobile, string password)
        {
            if (string.IsNullOrEmpty(Config.SignUpId) || string.IsNullOrEmpty(Config.SignUpSecret))
            {
                throw new ServiceException("Invalid Configuration: Client ID & Client Secret");
            }

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Invalid parameter", "email");
            }

            if (string.IsNullOrEmpty(mobile))
            {
                throw new ArgumentException("Invalid parameter", "mobile");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Invalid parameter", "password");
            }

            await GetSignupToken();
            var objectToPost = new User { Email = email, Mobile = mobile };
            user = objectToPost;
            var rest = new RestWrapper();
            var result = await rest.Post<User>(Service.Signup, AuthTokenType.SignUp, objectToPost);
            if (!(result is Error))
            {
                user.UserName = ((User)result).UserName;
                if (!string.IsNullOrEmpty(user.UserName))
                {
                    var randomPasswordGenerator = new RandomPasswordGenerator();
                    user.Password = randomPasswordGenerator.Generate(user.Email, user.Mobile);

                    var success = await SigninUser(user.UserName, user.Password);
                    if (success)
                    {
                        success = await UpdatePassword(user.Password, password);
                        if (success)
                        {
                            user.Password = password;
                            return await GetBalance();
                        }
                    }
                }
            }
            else
            {
                Utility.ParseAndThrowError((result as Error).Response);
            }

            return new User();
        }

        /// <summary>
        /// Update old password with new password
        /// </summary>
        /// <param name="oldPassword">
        /// Old Password.
        /// </param>
        /// <param name="newPassword">
        /// New Password.
        /// </param>
        /// <returns>
        /// Success or Failure
        /// </returns>
        public static async Task<bool> UpdatePassword(string oldPassword, string newPassword)
        {
            await GetTokenIfEmptyAsync(AuthTokenType.SignIn);

            if (signInToken == null || string.IsNullOrEmpty(signInToken.AccessToken))
            {
                throw new UnauthorizedAccessException("User is not logged to perform the action: Update Password");
            }

            var request = new List<KeyValuePair<string, string>>
                              {
                                  new KeyValuePair<string, string>("old", oldPassword), 
                                  new KeyValuePair<string, string>("new", newPassword)
                              };
            var rest = new RestWrapper();
            var result = await rest.Put(Service.UpdatePassword, request, AuthTokenType.SignIn);

            if (result is bool && (bool)result)
            {
                return true;
            }
            else
            {
                var error = result as Error;
                if (error != null)
                {
                    Utility.ParseAndThrowError(error.Response);
                }
            }

            return false;
        }

        public static async Task<User> BindUser(string email, string mobile)
        {
            if (string.IsNullOrEmpty(Config.SignUpId) || string.IsNullOrEmpty(Config.SignUpSecret))
            {
                throw new ServiceException("Invalid Configuration: Client ID & Client Secret");
            }

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Invalid parameter", "email");
            }

            if (string.IsNullOrEmpty(mobile))
            {
                throw new ArgumentException("Invalid parameter", "mobile");
            }

            await GetSignupToken();
            var objectToPost = new User { Email = email, Mobile = mobile };
            user = objectToPost;
            var rest = new RestWrapper();
            var result = await rest.Post<User>(Service.BindUser, AuthTokenType.SignUp, objectToPost);
            if (!(result is Error))
            {
                user.UserName = ((User)result).UserName;
                if (!string.IsNullOrEmpty(user.UserName))
                {
                    var signInRequest = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("client_id", Config.SignInId),
                        new KeyValuePair<string, string>("client_secret", Config.SignInSecret),
                        new KeyValuePair<string, string>("grant_type", "username"),
                        new KeyValuePair<string, string>("username", user.Email)
                    };

                    var authTokenResult = await rest.Post<OAuthToken>(Service.Signin, signInRequest, AuthTokenType.None);

                    if (!(authTokenResult is Error))
                    {
                        signInToken = null;
                        Utility.RemoveEntry(Utility.SignInTokenKey);
                        simpleToken = authTokenResult as OAuthToken;
                        return user;
                    }

                    Utility.ParseAndThrowError((authTokenResult as Error).Response);
                    return null;
                }
            }
            else
            {
                Utility.ParseAndThrowError((result as Error).Response);
            }

            return null;
        }

        public static async Task<bool> IsCitrusMemeber(string email, string mobile)
        {
            var newUser = await BindUser(email, mobile);
            var randomPasswordGenerator = new RandomPasswordGenerator();
            newUser.Password = randomPasswordGenerator.Generate(newUser.Email, newUser.Mobile);

            var request = new SigninRequest { User = new User { UserName = newUser.UserName, Password = newUser.Password } };

            var rest = new RestWrapper();
            var result = await rest.Post<OAuthToken>(Service.Signin, AuthTokenType.None, request);
            if (!(result is Error))
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the Signup Token.
        /// </summary>
        /// <returns>
        /// </returns>
        private static async Task GetSignupToken()
        {
            await GetTokenIfEmptyAsync(AuthTokenType.SignUp);

            if (signUpToken != null)
            {
                return;
            }

            var rest = new RestWrapper();
            var result = await rest.Post<OAuthToken>(Service.SignUpToken, AuthTokenType.None, new SignupTokenRequest());
            if (!(result is Error))
            {
                signUpToken = (OAuthToken)result;
                Utility.SaveToLocalStorage(Utility.SignUpTokenKey, signUpToken);
            }
            else
            {
                Utility.ParseAndThrowError((result as Error).Response);
            }
        }

        public static void SignOut()
        {
            signInToken = new OAuthToken();
            signUpToken = new OAuthToken();
            simpleToken = new OAuthToken();
            user = new User();
            Utility.RemoveAllEntries();
        }

        #endregion

        #region User Management

        public static async Task<bool> UpdateMobile(string newMobile)
        {
            await GetTokenIfEmptyAsync(AuthTokenType.SignIn);

            if (signInToken == null || string.IsNullOrEmpty(signInToken.AccessToken))
            {
                throw new UnauthorizedAccessException("User is not logged to perform the action: Update Password");
            }

            var request = new List<KeyValuePair<string, string>>
                              {
                                  new KeyValuePair<string, string>("mobile", newMobile), 
                              };
            var rest = new RestWrapper();
            var result = await rest.Put(Service.UpdateMobile, request, AuthTokenType.SignIn);

            if (result is bool && (bool)result)
            {
                return true;
            }
            else
            {
                var error = result as Error;
                if (error != null)
                {
                    Utility.ParseAndThrowError(error.Response);
                }
            }

            return false;
        }

        public static async Task<bool> GenerateOTPUsingMobile(string Mobile)
        {
            await GetTokenIfEmptyAsync(AuthTokenType.SignIn);

            //if (signInToken == null || string.IsNullOrEmpty(signInToken.AccessToken))
            //{
            //    throw new UnauthorizedAccessException("User is not logged to perform the action: Generate OTP");
            //}

            var request = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("source", "RIO"), 
                        new KeyValuePair<string, string>("otpType", "Mobile"), 
                        new KeyValuePair<string, string>("identity", Mobile)
                    };
            var objOTP = new OTP() {
                Source = "RIO",
                OTPType = "Mobile",
                Identity = Mobile,
            };

            var rest = new RestWrapper();
            var result = await rest.Post<ResponseEntity>(Service.GenerateOTP, AuthTokenType.None, objOTP, true);

            if (!(result is Error))
            {
                return true;
            }
            else
            {
                var error = result as Error;
                if (error != null)
                {
                    Utility.ParseAndThrowError(error.Response);
                }
            }

            return false;
        }

        public static async Task<bool> GenerateOTPUsingEmailId(string EmailId)
        {
            await GetTokenIfEmptyAsync(AuthTokenType.SignIn);

            //if (signInToken == null || string.IsNullOrEmpty(signInToken.AccessToken))
            //{
            //    throw new UnauthorizedAccessException("User is not logged to perform the action: Generate OTP");
            //}

            var request = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("source", "RIO"), 
                    new KeyValuePair<string, string>("otpType", "Email"), 
                    new KeyValuePair<string, string>("identity", EmailId)
                };
            var rest = new RestWrapper();
            var result = await rest.Post<OAuthToken>(Service.GenerateOTP, request, AuthTokenType.None);

            if (!(result is Error))
            {
                return true;
            }
            else
            {
                var error = result as Error;
                if (error != null)
                {
                    Utility.ParseAndThrowError(error.Response);
                }
            }
            return false;
        }

        public static async Task<bool> SignInUsingOTP(string EmailId, string OTP)
        {
            await GetTokenIfEmptyAsync(AuthTokenType.SignIn);

            if (Session.Config == null || string.IsNullOrEmpty(Session.Config.SignInId) || string.IsNullOrEmpty(Session.Config.SignInSecret))
            {
                throw new ServiceException("Invalid Configuration: Client ID & Client Secret");
            }

            //Renew token
            var rest = new RestWrapper();
            var result = await rest.Post<OAuthToken>(
                    Service.Signin,
                    new List<KeyValuePair<string, string>>()
                            {
                                new KeyValuePair<string, string>("client_id", Session.Config.SignInId),
                                new KeyValuePair<string, string>("client_secret", Session.Config.SignInSecret),
                                new KeyValuePair<string, string>("grant_type", "onetimepass"),
                                new KeyValuePair<string, string>("username", EmailId),
                                new KeyValuePair<string, string>("password", OTP)
                            },
                    AuthTokenType.None);

            if (!(result is Error))
            {
                signInToken = result as OAuthToken;
                simpleToken = result as OAuthToken;
                Utility.SaveToLocalStorage(Utility.SignInTokenKey, signInToken);
                Utility.SaveToLocalStorage(Utility.SimpleTokenKey, simpleToken);
                return signInToken != null && !string.IsNullOrEmpty(signInToken.AccessToken);
            }

            Utility.ParseAndThrowError((result as Error).Response);
            return false;
        }

        public static async Task<UserProfile> GetProfileInfo()
        {
            await GetTokenIfEmptyAsync(AuthTokenType.SignIn);

            if (signInToken == null || string.IsNullOrEmpty(signInToken.AccessToken))
            {
                throw new UnauthorizedAccessException("User is not logged to perform the action: Get Profile Info");
            }

            var restWrapper = new RestWrapper();
            var response = await restWrapper.Get<UserProfile>(Service.GetProfileInfo, AuthTokenType.SignIn);
            if (!(response is Error))
            {
                var user = response as UserProfile;
                return user;
            }

            return null;
        }

        public static async Task<bool> UpdateProfile(UserProfile userProfile)
        {
            await GetTokenIfEmptyAsync(AuthTokenType.SignIn);

            if (signInToken == null || string.IsNullOrEmpty(signInToken.AccessToken))
            {
                throw new UnauthorizedAccessException("User is not logged to perform the action: Update Password");
            }

            //var request = new List<KeyValuePair<string, string>>
            //    {
            //        new KeyValuePair<string, string>("firstname", userProfile.FirstName), 
            //        new KeyValuePair<string, string>("lastname", userProfile.LastName),
            //    };
            UserProfileRequest userprofilepostData = new UserProfileRequest()
            {
                Email = "",
                Mobile = "",
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
            };
            
            var rest = new RestWrapper();
            var response = await rest.Put<UserProfile>(Service.UpdateProfile, AuthTokenType.SignIn, userprofilepostData, true);
            if (!(response is Error))
            {
                var user = response as ResponseEntity;
                return true;
            }

            return false;
        }


        #endregion
    }
}
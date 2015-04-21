﻿// --------------------------------------------------------------------------------------------------------------------
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
        public static async Task<string> GetAuthTokenAsync(AuthTokenType authTokenType)
        {
            switch (authTokenType)
            {
                case AuthTokenType.SignIn:
                    return signInToken != null ? await signInToken.GetActiveTokenAsync() : string.Empty;
                case AuthTokenType.SignUp:
                    return signUpToken != null ? await signUpToken.GetActiveTokenAsync() : string.Empty;
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
        }

        /// <summary>
        /// Gets the User account's balance.
        /// </summary>
        /// <returns>
        /// Account balance
        /// </returns>
        public static async Task<User> GetBalance()
        {
            await GetTokenIfEmptyAsync(AuthTokenType.SignIn);

            if (signInToken == null || string.IsNullOrEmpty(signInToken.AccessToken))
            {
                throw new UnauthorizedAccessException("User is not logged to perform Get Balance");
            }

            var rest = new RestWrapper();
            var result = await rest.Get<User>(Service.GetBalance, AuthTokenType.SignIn);

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
                Utility.SaveToLocalStorage(Utility.SignInTokenKey, signInToken);
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
            user = new User();
            Utility.RemoveAllEntries();
        }

        #endregion
    }
}
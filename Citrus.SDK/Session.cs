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
        private static OAuthToken signUpToken;

        /// <summary>
        /// Sign Up Token
        /// </summary>
        private static OAuthToken signInToken;

        /// <summary>
        /// Session User
        /// </summary>
        private static User user;

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
        public static string GetAuthToken(AuthTokenType authTokenType)
        {
            switch (authTokenType)
            {
                case AuthTokenType.SignIn:
                    return signInToken != null ? signInToken.AccessToken : string.Empty;
                case AuthTokenType.SignUp:
                    return signUpToken != null ? signUpToken.AccessToken : string.Empty;
                default:
                    return string.Empty;
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
            if (string.IsNullOrEmpty(user.UserName))
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
        public static async Task<bool> ResetPassword()
        {
            if (string.IsNullOrEmpty(user.UserName))
            {
                throw new UnauthorizedAccessException("User is not logged to perform reset password");
            }

            var rest = new RestWrapper();
            var result =
                await
                rest.Post<User>(
                    Service.ResetPassword,
                    new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>(
                                "username", 
                                user.UserName)
                        },
                    AuthTokenType.SignIn);

            if (!(result is Error))
            {
                return !string.IsNullOrEmpty(result.ToString());
            }

            Utility.ParseAndThrowError((result as Error).Response);

            return false;
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
            var request = new SigninRequest { User = new User { UserName = userName, Password = password } };

            var rest = new RestWrapper();
            var result = await rest.Post<OAuthToken>(Service.Signin, AuthTokenType.None, request);
            if (!(result is Error))
            {
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
        /// <returns>
        /// Logged In user's detail
        /// </returns>
        public static async Task<User> SignupUser(string email, string mobile, string password)
        {
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
                user = (User)result;
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
            var request = new List<KeyValuePair<string, string>>
                              {
                                  new KeyValuePair<string, string>("old", oldPassword), 
                                  new KeyValuePair<string, string>("new", newPassword)
                              };
            var rest = new RestWrapper();
            return await rest.Put(Service.UpdatePassword, request, AuthTokenType.SignIn);
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
            if (signUpToken != null)
            {
                return;
            }

            var rest = new RestWrapper();
            var result = await rest.Post<OAuthToken>(Service.SignUpToken, AuthTokenType.None, new SignupTokenRequest());
            if (!(result is Error))
            {
                signUpToken = (OAuthToken)result;
            }
            else
            {
                Utility.ParseAndThrowError((result as Error).Response);
            }
        }

        #endregion
    }
}
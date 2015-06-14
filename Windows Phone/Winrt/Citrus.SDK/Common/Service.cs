// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Service.cs" company="Citrus Payment Solutions Pvt. Ltd.">
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
//  Service related constants   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Citrus.SDK.Common
{
    /// <summary>
    /// Service related constants
    /// </summary>
    internal static class Service
    {
        #region Constants

        /// <summary>
        /// Get Balance endpoint
        /// </summary>
        public const string GetBalance = ServiceVersion + "prepayment/balance";

        /// <summary>
        /// Random Password endpoint
        /// </summary>
        public const string PrepaidBill = ServiceVersion + "prepayment/load";

        /// <summary>
        /// Reset Password endpoint
        /// </summary>
        public const string ResetPassword = ServiceVersion + "identity/passwords/reset";

        /// <summary>
        /// Get Sign up token endpoint
        /// </summary>
        public const string SignUpToken = "oauth/token";

        /// <summary>
        /// Get Sign in token endpoint
        /// </summary>
        public const string Signin = "oauth/token";

        /// <summary>
        /// Sign up endpoint
        /// </summary>
        public const string Signup = ServiceVersion + "identity/new";

        /// <summary>
        /// Update Password endpoint
        /// </summary>
        public const string UpdatePassword = ServiceVersion + "identity/me/password";

        /// <summary>
        /// Load Money endpoint
        /// </summary>
        public const string LoadMoney = "service/moto/authorize/struct/payment";

        /// <summary>
        /// Base service path with version
        /// </summary>
        private const string ServiceVersion = "service/v2/";

        public const string BindUser = ServiceVersion + "identity/bind";

        public const string Wallet = ServiceVersion + "profile/me/payment";

        public const string GetMercharPaymentOptions = "service/v1/merchant/pgsetting";

        #endregion
    }
}
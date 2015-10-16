// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Enum.cs" company="Citrus Payment Solutions Pvt. Ltd.">
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
//   Custom Enum types
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Citrus.SDK.Common
{
    /// <summary>
    ///     Target Citrus Pay Service Type
    /// </summary>
    public enum EnvironmentType
    {
        /// <summary>
        ///     The sandbox environment for development and testing
        /// </summary>
        [Value("https://sandboxadmin.citruspay.com/")]
        Sandbox,

        /// <summary>
        ///     The Production Environment for Real time or Live Services
        /// </summary>
        [Value("https://admin.citruspay.com/")]
        Production
    }

    /// <summary>
    ///     Authentication Token Type
    /// </summary>
    public enum AuthTokenType
    {
        /// <summary>
        /// Unauthorized access
        /// </summary>
        None,

        /// <summary>
        ///     Sign In Token Type
        /// </summary>
        SignIn,

        /// <summary>
        ///     Sign Up Token Type
        /// </summary>
        SignUp,

        Simple,

        /// <summary>
        ///     Prepaid Sign In Token Type
        /// </summary>
        PrepaidSignIn,
    }

    public enum CardType
    {
        [Value("")]
        UnKnown,
        [Value("debit")]
        Debit,
        [Value("credit")]
        Credit,
        [Value("prepaid")]
        Prepaid
    }

    public enum PaymentType
    {
        [Value("paymentOptionToken")]
        Card,
        [Value("paymentOptionIdToken")]
        Token,
        [Value("netbanking")]
        NetBanking,
        [Value("prepaid")]
        Prepaid
    }

    public enum CreditCardType
    {
        [Value("VISA")]
        Visa,
        [Value("DISCOVER")]
        Discover,
        [Value("AMEX")]
        Amex,
        [Value("MTRO")]
        Mtro,
        [Value("MCRD")]
        Mcrd,
        [Value("DINERS")]
        Diners,
        [Value("JCB")]
        Jcb,
        [Value("UNKNOWN")]
        Unknown,
        [Value("CPAY")]
        Prepaid
    }
}
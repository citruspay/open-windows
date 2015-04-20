#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Utility.cs" company="Citrus Payment Solutions Pvt. Ltd.">
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
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Citrus.SDK.Common
{
    using System;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Text.RegularExpressions;

    using Citrus.SDK.Entity;

    using Newtonsoft.Json;

    /// <summary>
    ///     Utility methods
    /// </summary>
    internal static class Utility
    {
        #region Constants

        /// <summary>
        /// </summary>
        private const string cardRegex =
            "^(?:(?<Visa>4\\d{3})|(?<MasterCard>5[1-5]\\d{2})|(?<Discover>6011)|(?<DinersClub>(?:3[68]\\d{2})|(?:30[0-5]\\d))|(?<Amex>3[47]\\d{2}))([ -]?)(?(DinersClub)(?:\\d{6}\\1\\d{4})|(?(Amex)(?:\\d{6}\\1\\d{5})|(?:\\d{4}\\1\\d{4}\\1\\d{4})))$";

        public const string SignUpTokenKey = "SignUpToken";
        public const string SignInTokenKey = "SignInToken";
        public const string ConfigKey = "CitrusMerchatConfig";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="cardNum">
        /// </param>
        /// <returns>
        /// </returns>
        public static CreditCardType? GetCardTypeFromNumber(string cardNum)
        {
            // Create new instance of Regex comparer with our
            // credit card regex patter
            var cardTest = new Regex(cardRegex);

            // Compare the supplied card number with the regex
            // pattern and get reference regex named groups
            GroupCollection gc = cardTest.Match(cardNum).Groups;

            // Compare each card type to the named groups to 
            // determine which card type the number matches
            if (gc[CreditCardType.Amex.ToString()].Success)
            {
                return CreditCardType.Amex;
            }

            if (gc[CreditCardType.MasterCard.ToString()].Success)
            {
                return CreditCardType.MasterCard;
            }

            if (gc[CreditCardType.Visa.ToString()].Success)
            {
                return CreditCardType.Visa;
            }

            if (gc[CreditCardType.Discover.ToString()].Success)
            {
                return CreditCardType.Discover;
            }

            // Card type is not supported by our system, return null
            // (You can modify this code to support more (or less)
            // card types as it pertains to your application)
            return null;
        }

        /// <summary>
        /// Parse the error message from REST and throw it as exception
        /// </summary>
        /// <param name="response">
        /// Response string(JSON)
        /// </param>
        /// <exception cref="Exception">
        /// Custom exception for the error response
        /// </exception>
        public static void ParseAndThrowError(string response)
        {
            if (string.IsNullOrEmpty(response))
            {
                return;
            }

            var serializer = new JsonSerializer();
            var error = serializer.Deserialize<Error>(new JsonTextReader(new StringReader(response)));
            throw new ServiceException(string.IsNullOrEmpty(error.Code) ? error.ErrorDescription : error.Message);
        }

        public static void SaveToLocalStorage<T>(string key, T value) where T : class
        {
            IsolatedStorageSettings storageSettings = IsolatedStorageSettings.ApplicationSettings;
            var serializer = new JsonSerializer();
            var jsonObject = new StringWriter();
            serializer.Serialize(jsonObject, value);
            if (storageSettings.Contains(key))
            {
                storageSettings[key] = jsonObject;
            }
            else
            {
                storageSettings.Add(key, jsonObject);
            }

            storageSettings.Save();
        }

        public static T ReadFromLocalStorage<T>(string key) where T : class
        {
            IsolatedStorageSettings storageSettings = IsolatedStorageSettings.ApplicationSettings;

            if (storageSettings.Contains(key))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<T>(new JsonTextReader(new StringReader(storageSettings[key].ToString())));
            }

            return null;
        }

        public static void RemoveEntry(string key)
        {
            IsolatedStorageSettings storageSettings = IsolatedStorageSettings.ApplicationSettings;

            if (storageSettings.Contains(key))
            {
                storageSettings.Remove(key);
                storageSettings.Save();
            }
        }

        public static void RemoveAllEntries()
        {
            IsolatedStorageSettings storageSettings = IsolatedStorageSettings.ApplicationSettings;
            if (storageSettings.Contains(SignInTokenKey))
            {
                storageSettings.Remove(SignInTokenKey);
            }

            if (storageSettings.Contains(SignUpTokenKey))
            {
                storageSettings.Remove(SignUpTokenKey);
            }
            
            if (storageSettings.Contains(ConfigKey))
            {
                storageSettings.Remove(ConfigKey);
            }

            storageSettings.Save();
        }

        public static bool PassesLuhnTest(string cardNumber)
        {
            //Clean the card number- remove dashes and spaces
            cardNumber = cardNumber.Replace("-", "").Replace(" ", "");

            //Convert card number into digits array
            int[] digits = new int[cardNumber.Length];
            for (int len = 0; len < cardNumber.Length; len++)
            {
                digits[len] = Int32.Parse(cardNumber.Substring(len, 1));
            }

            //Luhn Algorithm
            //Adapted from code availabe on Wikipedia at
            //http://en.wikipedia.org/wiki/Luhn_algorithm
            int sum = 0;
            bool alt = false;
            for (int i = digits.Length - 1; i >= 0; i--)
            {
                int curDigit = digits[i];
                if (alt)
                {
                    curDigit *= 2;
                    if (curDigit > 9)
                    {
                        curDigit -= 9;
                    }
                }
                sum += curDigit;
                alt = !alt;
            }

            //If Mod 10 equals 0, the number is good and this will return true
            return sum % 10 == 0;
        }

        #endregion
    }
}
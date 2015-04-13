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
//   Utility methods
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Citrus.SDK.Common
{
    using System;
    using System.IO;

    using Citrus.SDK.Entity;

    using Newtonsoft.Json;

    /// <summary>
    /// Utility methods
    /// </summary>
    internal static class Utility
    {
        #region Public Methods and Operators

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
            throw new Exception(error.Message);
        }

        #endregion
    }
}
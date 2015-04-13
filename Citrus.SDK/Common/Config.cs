// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Config.cs" company="Citrus Payment Solutions Pvt. Ltd.">
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
//   Configurations
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Citrus.SDK.Common
{
    /// <summary>
    ///     Config related to accessing Citrus REST end points
    /// </summary>
    public static class Config
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the Target Environment Type ; Sandbox for development and testing, Production for real time or live
        /// </summary>
        public static EnvironmentType Environment { get; set; }

        /// <summary>
        ///     Gets or sets the Client Id for Sign In
        /// </summary>
        public static string SignInId { get; set; }

        /// <summary>
        ///     Gets or sets the Client Secret for Sign In
        /// </summary>
        public static string SignInSecret { get; set; }

        /// <summary>
        ///     Gets or sets the Client Id for Sign Up
        /// </summary>
        public static string SignUpId { get; set; }

        /// <summary>
        ///     Gets or sets the Client Secret for Sign Up
        /// </summary>
        public static string SignUpSecret { get; set; }

        #endregion
    }
}
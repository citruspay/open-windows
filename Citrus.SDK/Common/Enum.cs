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
    using System.ComponentModel;

    /// <summary>
    ///     Target Citrus Pay Service Type
    /// </summary>
    public enum EnvironmentType
    {
        /// <summary>
        ///     The sandbox environment for development and testing
        /// </summary>
        [Description("https://sandboxadmin.citruspay.com/")]
        Sandbox, 

        /// <summary>
        ///     The Production Environment for Real time or Live Services
        /// </summary>
        [Description("https://admin.citruspay.com/")]
        Production
    };
}
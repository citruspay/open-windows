// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEntity.cs" company="Citrus Payment Solutions Pvt. Ltd.">
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
//   Interface for all entities
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Citrus.SDK.Entity
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for all entities
    /// </summary>
    internal interface IEntity
    {
        #region Public Methods and Operators

        /// <summary>
        /// Return the key value pair of properties
        /// </summary>
        /// <returns>
        /// Key value pair of properties
        /// </returns>
        IEnumerable<KeyValuePair<string, string>> ToKeyValuePair();

        #endregion
    }
}
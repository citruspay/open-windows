// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Citrus Payment Solutions Pvt. Ltd.">
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
//   Extension methods
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Citrus.SDK.Common
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    ///     Extensions for built in types
    /// </summary>
    public static class Extensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// Get value of Description attribute
        /// </summary>
        /// <param name="value">
        /// Source Enum
        /// </param>
        /// <returns>
        /// Description string
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
            Justification = "Reviewed. Suppression is OK here.")]
        public static string GetEnumDescription(this Enum value)
        {
            Type type = value.GetType();
            FieldInfo fi = type.GetRuntimeField(value.ToString());
            var stringValueAttribute = fi.GetCustomAttributes(typeof(ValueAttribute), false).FirstOrDefault() as ValueAttribute;
            if (stringValueAttribute != null)
            {
                return stringValueAttribute.Value;
            }

            return string.Empty;
        }

        #endregion
    }

    public class ValueAttribute : Attribute
    {
        private string _value;
        public ValueAttribute(string value)
        {
            this._value = value;
        }

        public string Value
        {
            get { return this._value; }
        }
    }
}
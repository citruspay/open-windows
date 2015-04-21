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
    public class Config
    {
        private EnvironmentType environment;

        private string signInId;

        private string signInSecret;

        private string signUpId;

        private string signUpSecret;

        #region Public Properties

        /// <summary>
        ///     Gets or sets the Target Environment Type ; Sandbox for development and testing, Production for real time or live
        /// </summary>
        public EnvironmentType Environment
        {
            get
            {
                return this.environment;
            }
            private set
            {
                this.environment = value;
            }
        }

        /// <summary>
        ///     Gets or sets the Client Id for Sign In
        /// </summary>
        public string SignInId
        {
            get
            {
                return this.signInId;
            }
            private set
            {
                this.signInId = value;
            }
        }

        /// <summary>
        ///     Gets or sets the Client Secret for Sign In
        /// </summary>
        public string SignInSecret
        {
            get
            {
                return this.signInSecret;
            }
            private set
            {
                this.signInSecret = value;
            }
        }

        /// <summary>
        ///     Gets or sets the Client Id for Sign Up
        /// </summary>
        public string SignUpId
        {
            get
            {
                return this.signUpId;
            }
            private set
            {
                this.signUpId = value;
            }
        }

        /// <summary>
        ///     Gets or sets the Client Secret for Sign Up
        /// </summary>
        public string SignUpSecret
        {
            get
            {
                return this.signUpSecret;
            }
            private set
            {
                this.signUpSecret = value;
            }
        }

        public static void Initialize(EnvironmentType environmentType, string signUpClientId, string signUpClientSecret, string signInClientId, string signInClientSecret)
        {
            var config = new Config()
                                 {
                                     Environment = environmentType,
                                     SignUpId = signUpClientId,
                                     SignUpSecret = signUpClientSecret,
                                     SignInId = signInClientId,
                                     SignInSecret = signInClientSecret
                                 };

            var localConfig = Utility.ReadFromLocalStorage<Config>(Utility.ConfigKey);

            if (!config.Equals(localConfig))
            {
                Utility.RemoveAllEntries();
            }

            Session.Config = config;
            Utility.SaveToLocalStorage(
                Utility.ConfigKey, Session.Config);
        }

        public static void Reset()
        {
            Session.SignOut();
            Session.Config = new Config();
        }

        public override bool Equals(object obj)
        {
            var config = obj as Config;

            if (config == null)
            {
                return false;
            }

            if (config.environment == this.environment && config.signInId == this.signInId
                && config.signInSecret == this.signInSecret && config.signUpId == this.signUpId
                && config.signUpSecret == this.signInSecret)
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
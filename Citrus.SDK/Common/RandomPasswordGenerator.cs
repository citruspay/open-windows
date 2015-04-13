﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RandomPasswordGenerator.cs" company="Citrus Payment Solutions Pvt. Ltd.">
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
//   Random Password generator
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Citrus.SDK.Common
{
    using System;
    using System.Text;

    using BigMath;

    /// <summary>
    ///     Random Password generator
    /// </summary>
    public class RandomPasswordGenerator
    {
        #region Public Methods and Operators

        /// <summary>
        /// Generate a random password for the email
        /// </summary>
        /// <param name="email">
        /// Email address
        /// </param>
        /// <param name="mobile">
        /// Mobile number
        /// </param>
        /// <returns>
        /// Autogenerated password
        /// </returns>
        public string Generate(string email, string mobile)
        {
            var ran = new PseudoRandomNumberGenerator(this.GenerateSeed(email));
            var builder = new StringBuilder();
            for (int i = 0; i < 8; i++)
            {
                builder.Append(ran.NextLetter());
            }

            return builder.ToString();
        }

        /// <summary>
        /// Generates a non-random positive number from a string.
        ///     @param data the string to generate seed from.
        ///     @return the value of the 3 highest bytes of the MD5 sum of data.
        /// </summary>
        /// <param name="data">
        /// Input string
        /// </param>
        /// <returns>
        /// Seed to populate password
        /// </returns>
        public int GenerateSeed(string data)
        {
            var md5 = MD5.Create("MD5");
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(data));
            hash = RangeCopy(hash, hash.Length - 3, hash.Length);
            return new BigInteger(1, hash).IntValue;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Copy the range of values from byte array.
        /// </summary>
        /// <param name="original">
        /// Source bytes
        /// </param>
        /// <param name="from">
        /// Start index
        /// </param>
        /// <param name="to">
        /// End index
        /// </param>
        /// <returns>
        /// Copied range based on the from and to values
        /// </returns>
        private static byte[] RangeCopy(byte[] original, int from, int to)
        {
            int lengthNew = to - from;
            if (lengthNew < 0)
            {
                throw new ArgumentOutOfRangeException(from + " > " + to);
            }

            var hash = new byte[lengthNew];
            Array.Copy(original, from, hash, 0, Math.Min(original.Length - from, lengthNew));
            return hash;
        }

        #endregion
    }

    /// <summary>
    ///     A pseudo random integer generator.
    /// </summary>
    public class PseudoRandomNumberGenerator
    {
        #region Fields

        /// <summary>
        ///     Seed or state
        /// </summary>
        private int state;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     PseudoRandomNumberGenerator ctor
        /// </summary>
        public PseudoRandomNumberGenerator()
        {
        }

        /// <summary>
        /// Creates a new pseudo random number generator. param seed the seed to start the generation from.
        /// </summary>
        /// <param name="seed">
        /// Seed value
        /// </param>
        public PseudoRandomNumberGenerator(int seed)
        {
            this.state = seed;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Generates the next pseudo-random alphabetical character. The letter is in the [a-zA-Z] range.
        ///     @return the next letter in the pseudo-random suite.
        /// </summary>
        /// <returns>
        ///     Next Random Letter
        /// </returns>
        public char NextLetter()
        {
            int n = this.NextInt(52);
            return (char)(n + ((n < 26) ? 'A' : 'a' - 26));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Generates the next pseudo-random integer. The number is positive in the [0-max] interval.
        ///     @param max the exclusive upper boundary of the interval to generate the pseudo-random in.
        ///     @return the next number in the pseudo-random suite modulo max.
        /// </summary>
        /// <param name="max">
        /// max value
        /// </param>
        /// <returns>
        /// next random integer
        /// </returns>
        private int NextInt(int max)
        {
            this.state = 7 * this.state % 3001;
            return (this.state - 1) % max;
        }

        #endregion
    }
}
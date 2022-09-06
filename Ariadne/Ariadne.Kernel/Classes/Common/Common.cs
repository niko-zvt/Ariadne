// Copyright 2022 Nikolay V. Zhivotenko
// Licensed under the Apache License, Version 2.0
// E-mail: niko.zvt@gmail.com

using System;
using System.Text;

namespace Ariadne.Kernel.Common
{
    /// <summary>
    /// A class for implementing static hashing methods
    /// </summary>
    public static class Hasher
    {
        /// <summary>
        /// Hash a string
        /// </summary>
        /// <param name="input">Input source string</param>
        /// <returns>Encoded string</returns>
        public static string Hash(string input)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(inputBytes);
        }

        /// <summary>
        /// Unhash a string
        /// </summary>
        /// <param name="input">Input encoded string</param>
        /// <returns>Source string</returns>
        public static string Unhash(string input)
        {
            var base64EncodedBytes = Convert.FromBase64String(input);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}

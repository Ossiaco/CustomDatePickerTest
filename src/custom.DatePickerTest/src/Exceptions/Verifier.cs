// Copyright (c) PlaceholderCompany. All rights reserved.

namespace Custom.DatePickerTest.Exceptions
{
    using System.Linq;
#nullable enable
    using System.Runtime.CompilerServices;

    /// <summary>
    /// A verifier.
    /// </summary>
#if INTERNAL_GUARD
    internal class Verifier
#else
    public class Verifier
#endif
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Verifier"/> class.
        /// </summary>
        internal Verifier()
        {
        }

        /// <summary>
        /// Not empty or null.
        /// </summary>
        ///
        /// <param name="value">        The value. </param>
        /// <param name="paramName">    Name of the parameter. </param>
        ///
        /// <returns>
        /// A string.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string IsNotEmptyOrNull(string? value, string paramName)
        {
            Guard.Throw.ArgumentNullException(string.IsNullOrEmpty(value), paramName, $"{paramName} cannot be null or empty");
            return value;
        }

        /// <summary>
        /// Not empty or null.
        /// </summary>
        ///
        /// <param name="value">        The value. </param>
        /// <param name="paramName">    Name of the parameter. </param>
        /// <param name="message">      The message. </param>
        ///
        /// <returns>
        /// A string.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string IsNotEmptyOrNull(string? value, string paramName, string message)
        {
            Guard.Throw.ArgumentNullException(string.IsNullOrEmpty(value), paramName, message);
            return value;
        }

        /// <summary>
        /// First not empty or null.
        /// </summary>
        ///
        /// <param name="args"> A variable-length parameters list containing arguments. </param>
        ///
        /// <returns>
        /// A string.
        /// </returns>
        public string? FirstNotEmptyOrNull(params string?[] args) => args.FirstOrDefault(a => !string.IsNullOrEmpty(a));
    }
}
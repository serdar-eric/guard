﻿namespace Dawn
{
    using System;

    /// <content>Provides preconditions for <see cref="Uri" /> arguments.</content>
    public static partial class Guard
    {
        /// <summary>The URI scheme for Hypertext Transfer Protocol (HTTP).</summary>
        private const string HttpUriScheme = "http"; // Uri.UriSchemeHttp

        /// <summary>The URI scheme for Secure Hypertext Transfer Protocol (HTTPS).</summary>
        private const string HttpsUriScheme = "https"; // Uri.UriSchemeHttps

        /// <summary>Requires the argument value to be an absolute URI.</summary>
        /// <param name="argument">The URI argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not
        ///     <c>null</c> and is not an absolute URI.
        /// </exception>
        public static ref readonly ArgumentInfo<Uri> Absolute(
            in this ArgumentInfo<Uri> argument, Func<Uri, string> message = null)
        {
            if (argument.HasValue() && !argument.Value.IsAbsoluteUri)
            {
                var m = message?.Invoke(argument.Value) ?? Messages.UriAbsolute(argument);
                throw new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>Requires the argument value to be a relative URI.</summary>
        /// <param name="argument">The URI argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not
        ///     <c>null</c> and is not a relative URI.
        /// </exception>
        public static ref readonly ArgumentInfo<Uri> Relative(
            in this ArgumentInfo<Uri> argument, Func<Uri, string> message = null)
        {
            if (argument.HasValue() && argument.Value.IsAbsoluteUri)
            {
                var m = message?.Invoke(argument.Value) ?? Messages.UriRelative(argument);
                throw new ArgumentException(m, argument.Name);
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have an absolute
        ///     URI with the specified scheme.
        /// </summary>
        /// <param name="argument">The URI argument.</param>
        /// <param name="scheme">The URI scheme to compare.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c>
        ///     and does not have the specified scheme.
        /// </exception>
        public static ref readonly ArgumentInfo<Uri> Scheme(
            in this ArgumentInfo<Uri> argument, string scheme, Func<Uri, string, string> message = null)
        {
            if (argument.HasValue())
            {
                if (!argument.Value.IsAbsoluteUri ||
                    !argument.Value.Scheme.Equals(scheme, StringComparison.OrdinalIgnoreCase))
                {
                    var m = message?.Invoke(argument.Value, scheme) ?? Messages.UriScheme(argument, scheme);
                    throw new ArgumentException(m, argument.Name);
                }
            }

            return ref argument;
        }

        /// <summary>
        ///     Requires the argument to have an absolute
        ///     URI with the HTTP or HTTPS scheme.
        /// </summary>
        /// <param name="argument">The URI argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c>
        ///     and its scheme is neither HTTP nor HTTPS.
        /// </exception>
        public static ref readonly ArgumentInfo<Uri> Http(
            in this ArgumentInfo<Uri> argument, Func<Uri, string> message = null)
            => ref argument.Http(true, message);

        /// <summary>
        ///     Requires the argument to have an absolute
        ///     URI with the HTTP or HTTPS scheme.
        /// </summary>
        /// <param name="argument">The URI argument.</param>
        /// <param name="allowHttps">
        ///     Pass <c>true</c> to allow both the HTTP and HTTPS schemes
        ///     or <c>false</c> to allow only the HTTP scheme.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c>
        ///     and does not have one of the required schemes.
        /// </exception>
        public static ref readonly ArgumentInfo<Uri> Http(
            in this ArgumentInfo<Uri> argument, bool allowHttps, Func<Uri, string> message = null)
        {
            if (!argument.HasValue())
                return ref argument;

            if (argument.Value.IsAbsoluteUri)
            {
                if (argument.Value.Scheme == HttpUriScheme)
                    return ref argument;

                if (allowHttps && argument.Value.Scheme == HttpsUriScheme)
                    return ref argument;
            }

            var m = message?.Invoke(argument.Value) ?? Messages.UriHttp(argument);
            throw new ArgumentException(m, argument.Name);
        }

        /// <summary>
        ///     Requires the argument to have an absolute URI with the HTTPS scheme.
        /// </summary>
        /// <param name="argument">The URI argument.</param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception that
        ///     will be thrown if the precondition is not satisfied.
        /// </param>
        /// <returns><paramref name="argument" />.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="argument" /> value is not <c>null</c>
        ///     and does not have the HTTPS scheme.
        /// </exception>
        public static ref readonly ArgumentInfo<Uri> Https(
            in this ArgumentInfo<Uri> argument, Func<Uri, string> message = null)
        {
            if (argument.HasValue())
                if (!argument.Value.IsAbsoluteUri || argument.Value.Scheme != HttpsUriScheme)
                {
                    var m = message?.Invoke(argument.Value) ?? Messages.UriHttps(argument);
                    throw new ArgumentException(m, argument.Name);
                }

            return ref argument;
        }
    }
}

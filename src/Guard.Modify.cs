﻿namespace Dawn
{
    using System;

    /// <content>Provides safe modification functions to normalize arguments.</content>
    public static partial class Guard
    {
        /// <summary>
        ///     Returns a new argument with the same name and the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="argument">The existing argument.</param>
        /// <param name="value">The new argument value.</param>
        /// <returns>A new <see cref="ArgumentInfo{T}" />.</returns>
        public static ArgumentInfo<T> Modify<T>(in this ArgumentInfo<T> argument, T value)
            => new ArgumentInfo<T>(value, argument.Name, true);

        /// <summary>
        ///     Returns a new argument with the same name and a value that
        ///     is created using the specified conversion function.
        /// </summary>
        /// <typeparam name="TSource">The type of the existing argument.</typeparam>
        /// <typeparam name="TTarget">The type of the new argument.</typeparam>
        /// <param name="argument">The existing argument.</param>
        /// <param name="convert">
        ///     A function that accepts the existing argument's value and
        ///     returns a new object to be used as the new argument's value.
        /// </param>
        /// <returns>A new <see cref="ArgumentInfo{TTarget}" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="convert" /> is <c>null</c>.
        /// </exception>
        public static ArgumentInfo<TTarget> Modify<TSource, TTarget>(
            in this ArgumentInfo<TSource> argument, Func<TSource, TTarget> convert)
        {
            Argument(convert, nameof(convert)).NotNull();
            return new ArgumentInfo<TTarget>(convert(argument.Value), argument.Name, true);
        }

        /// <summary>
        ///     <para>
        ///         Returns a new argument with the same name and a value that
        ///         is created using the specified conversion function.
        ///     </para>
        ///     <para>
        ///         If the conversion function throws an exception, it will
        ///         be wrapped in an <see cref="ArgumentException" />.
        ///     </para>
        /// </summary>
        /// <typeparam name="TSource">The type of the existing argument.</typeparam>
        /// <typeparam name="TTarget">The type of the new argument.</typeparam>
        /// <param name="argument">The existing argument.</param>
        /// <param name="convert">
        ///     A function that accepts the existing argument's value and
        ///     returns a new object to be used as the new argument's value.
        /// </param>
        /// <param name="message">
        ///     The factory to initialize the message of the exception
        ///     that will be thrown if the conversion function fails.
        /// </param>
        /// <returns>A new <see cref="ArgumentInfo{TTarget}" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="convert" /> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="convert" /> threw an exception.
        /// </exception>
        public static ArgumentInfo<TTarget> Wrap<TSource, TTarget>(
            in this ArgumentInfo<TSource> argument,
            Func<TSource, TTarget> convert,
            Func<TSource, string> message = null)
        {
            Argument(convert, nameof(convert)).NotNull();
            try
            {
                return new ArgumentInfo<TTarget>(convert(argument.Value), argument.Name, true);
            }
            catch (Exception x)
            {
                var m = message?.Invoke(argument.Value) ?? Messages.Require(argument);
                throw new ArgumentException(m, argument.Name, x);
            }
        }

#if !NETSTANDARD1_0
        /// <summary>
        ///     Returns a new argument with the same name
        ///     and a shallow clone of the original value.
        /// </summary>
        /// <typeparam name="T">The type of the cloneable argument.</typeparam>
        /// <param name="argument">The cloneable argument.</param>
        /// <returns>A new <see cref="ArgumentInfo{T}" />.</returns>
        public static ArgumentInfo<T> Clone<T>(in this ArgumentInfo<T> argument)
            where T : class, ICloneable
        {
            return argument.HasValue()
                ? new ArgumentInfo<T>(argument.Value.Clone() as T, argument.Name, argument.Modified)
                : argument;
        }
#endif
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace PerformanceTools
{
    /// <summary>
    /// A helper class for calculating size of types
    /// </summary>
    public static class SizeUtility
    {
        /// <summary>
        /// Get size in bytes of the specified type parameter (using occurrences count)
        /// </summary>
        /// <param name="type">the type</param>
        /// <param name="occurrences">the number of type object</param>
        /// <returns>The size in bytes of the specified type parameter</returns>
        /// <exception cref="NotSupportedException"/>
        public static int GetSizeOf(Type type, int occurrences)
        {
            if (type == typeof(string))
                throw new NotSupportedException($"string. Use {nameof(GetSizeOfStringType)} instead");
            return GetSizeOfInternal(type, occurrences);
        }

        /// <summary>
        /// Get size in bytes of the specified type parameter (using occurrences count)
        /// </summary>
        /// <typeparam name="TType">the type</typeparam>
        /// <param name="occurrences">the number of type object</param>
        /// <returns>The size in bytes of the specified type parameter</returns>
        /// <exception cref="NotSupportedException"/>
        public static int GetSizeOf<TType>(int occurrences)
            => GetSizeOf(typeof(TType), occurrences);

        /// <summary>
        /// Get the total size in bytes of the specified objects parameter
        /// </summary>
        /// <param name="context">
        /// Key: type of the object
        /// Value: occurrences of the object
        /// </param>
        /// <returns>The total size in bytes of the specified object parameter</returns>
        public static int GetSizeOf(Dictionary<Type, int> context)
        {
            if (context == null)
                return 0;

            var result = 0;
            foreach (var kvp in context.Where(pair => pair.Value != 0))
            {
                result += GetSizeOf(kvp.Key, kvp.Value);
            }

            return result;
        }

        /// <summary>
        /// Get size in bytes of a string object of according to its length
        /// </summary>
        /// <param name="length"></param>
        /// <param name="nbOfStringObjects"></param>
        /// <returns></returns>
        public static int GetSizeOfStringType(int length)
            => GetSizeOfInternal(typeof(string), length);

        /// <summary>
        /// Get size internal implementation
        /// </summary>
        /// <param name="type"></param>
        /// <param name="occurrences"></param>
        /// <returns></returns>
        private static int GetSizeOfInternal(Type type, int occurrences)
        {
            var t = (type == typeof(string))
                ? typeof(char*)
                : type;
            try
            {
                return occurrences == 0 ? 0 : occurrences * Marshal.SizeOf(t);
            }
            catch (ArgumentException ex)
            {
                throw new NotSupportedException($"{type}", ex);
            }
        }
    }
}

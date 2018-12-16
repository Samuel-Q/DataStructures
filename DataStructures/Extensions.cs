using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures
{
    public static class Extensions
    {
        public static bool AreMembersOfTheSameSet<T>(this DisjointSets<T> @this, T x, T y) where T : IEquatable<T>
        {
            return @this.FindSet(x).Equals(@this.FindSet(y));
        }

        /// <summary>
        /// Computes the least non-negative residue.
        /// </summary>
        /// <param name="a">The dividend.</param>
        /// <param name="b">The divisor.</param>
        /// <returns>a mod b.</returns>
        /// <remarks>The % operator returns the <em>remainder</em>, which may be different from the
        /// least non-negative residue when the dividend is negative, when the dividend is divided
        /// by the divisor.</remarks>
        public static int Modulo(this int a, int b)
        {
            var result = a % b;
            if (result < 0) result += b;
            return result;
        }

        public static CircularList<T> ToCircularList<T>(this IEnumerable<T> @this)
        {
            if (@this is null) throw new ArgumentNullException(nameof(@this));
            return new CircularList<T>(@this);
        }
    }
}

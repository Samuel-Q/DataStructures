using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures
{
    /// <summary>
    /// A reimplementation of <see cref="System.Collections.BitArray"/> that allows for bit arrays
    /// of more than <see cref="int.MaxValue"/> number of elements.
    /// </summary>
    public class BitArray : IEnumerable<bool>
    {
        // Currently not used
        private static readonly UInt64[] bitMasks;

        private readonly UInt64[] longArray;

        /// <summary>
        /// Gets the number of elements contained in the <see cref="BitArray"/>.
        /// </summary>
        public UInt64 Count { get; private set; }

        /// <summary>
        /// Gets or sets the value of the bit at a specific position in the <see cref="BitArray"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the value to get or set.</param>
        /// <returns>The value of the bit at position <paramref name="index"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is greater than
        /// or equal to <see cref="Count"/>.</exception>
        public bool this[UInt64 index]
        {
            get
            {
                ValidateIndex(index);
                var (arrayIndex, bitIndex) = GetIndices(index);
                bool bit = (longArray[arrayIndex] >> bitIndex & 1) != 0;

                // Other versions with seemingly the same performance (assuming I have understood
                // the profiler correctly and this getter is a bottleneck)
                //bool bit = (longArray[arrayIndex] & 1ul << bitIndex) != 0;
                //bool bit = (longArray[arrayIndex] & bitMasks[bitIndex]) != 0;
                //bool bit = (longArray[arrayIndex] >> bitIndex % 2) != 0;
                //bool bit = (longArray[arrayIndex] & bitMasks[bitIndex]) == bitMasks[bitIndex];

                // Requires instance-level variables
                //UInt64 value;
                //if (arrayIndex == cachedIndex) value = cachedValue;
                //else
                //{
                //    cachedIndex = index;
                //    cachedValue = value = longArray[arrayIndex];
                //}
                //bool bit = ((value >> bitIndex) & 1) != 0;

                return bit;
            }
            set
            {
                ValidateIndex(index);
                var (arrayIndex, bitIndex) = GetIndices(index);
                if (value) longArray[arrayIndex] |= 1ul << bitIndex;
                else longArray[arrayIndex] &= ~(1ul << bitIndex);

                //if (value) longArray[arrayIndex] |= bitMasks[bitIndex];
                //else longArray[arrayIndex] &= ~bitMasks[bitIndex]; // Consider having another array for these bit masks
            }
        }

        /// <summary>
        /// Gets or sets the value of the bit at a specific position in the <see cref="BitArray"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the value to get or set.</param>
        /// <returns>The value of the bit at position <paramref name="index"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is negative,
        /// or <paramref name="index"/> is greater than or equal to <see cref="Count"/>.</exception>
        public bool this[Int64 index]
        {
            get
            {
                ValidateIndex(index);
                return this[(UInt64)index];
            }
            set
            {
                ValidateIndex(index);
                this[(UInt64)index] = value;
            }
        }

        static BitArray()
        {
            bitMasks = new UInt64[64];
            for (int i = 0; i < 64; i++)
            {
                bitMasks[i] = 1ul << i;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitArray"/> class that can hold the
        /// specified number of bit values, all of which are initially set to
        /// <see langword="false"/>.
        /// </summary>
        /// <param name="length">The number of bit values in the new <see cref="BitArray"/>.</param>
        public BitArray(UInt64 length) : this(length, defaultValue: false)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitArray"/> class that contains bit values
        /// copied from the specified array of <see cref="bool"/>s.
        /// </summary>
        /// <param name="values">An array of <see cref="bool"/>s containing the values to copy.</param>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is <see langword="null"/>.</exception>
        public BitArray(IEnumerable<bool> values)
        {
            var longList = new List<UInt64>();
            UInt64 count = 0;

            while (true)
            {
                var bits = values.Take(64).ToList();
                if (bits.Count == 0) break;
                values = values.Skip(64);
                longList.Add(BitsToUInt64(bits));
                count += (UInt64)bits.Count;
            }

            UInt64 BitsToUInt64(List<bool> bits)
            {
                UInt64 val = 0;
                for (int i = 0; i < bits.Count; i++)
                {
                    var bit = bits[i];
                    if (bit) val |= (1ul << i++);
                }

                return val;
            }

            longArray = longList.ToArray();
            Count = count;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitArray"/> class that can hold the
        /// specified number of bit values, all of which are initially set to the specified value.
        /// </summary>
        /// <param name="length">The number of bit values in the new <see cref="BitArray"/>.</param>
        public BitArray(UInt64 length, bool defaultValue)
        {
            var longArrayLength = length % 64 == 0 ? length >> 6 : (length >> 6) + 1;
            longArray = new UInt64[longArrayLength];
            Count = length;

            if (defaultValue)
            {
                for (UInt64 i = 0; i < longArrayLength; i++)
                    longArray[i] = 0xffffffffffffffffu; // Don't care about the most significant bits in the last element.
            }
        }

        private void ValidateIndex(UInt64 index)
        {
            if (index < 0 || index >= Count) throw new ArgumentOutOfRangeException(nameof(index));
        }

        private void ValidateIndex(Int64 index)
        {
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));
            ValidateIndex((UInt64)index);
        }

        private UInt64 GetArrayIndex(UInt64 bitIndex) => bitIndex >> 6;

        private int GetIndexInLong(UInt64 bitIndex) => (int)(bitIndex % (1 << 6));

        /// <summary>
        /// Gets the index into <see cref="longArray"/> and the index into the corresponding
        /// element of <see cref="longArray"/> for a given index into the bit array.
        /// </summary>
        /// <param name="bitIndex"></param>
        /// <returns></returns>
        private (UInt64, int) GetIndices(UInt64 bitIndex) => (GetArrayIndex(bitIndex), GetIndexInLong(bitIndex));

        public IEnumerator<bool> GetEnumerator()
        {
            for (UInt64 i = 0; i < Count; i++) yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

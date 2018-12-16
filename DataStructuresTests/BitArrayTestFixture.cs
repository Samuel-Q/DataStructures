using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;
using DataStructures;

namespace DataStructuresTests
{
    [TestFixture]
    public class BitArrayTestFixture
    {
        private BitArray CreateBitArray(UInt64 count)
        {
            return new BitArray(count);
        }

        [TestCase(0u)]
        [TestCase(1u)]
        [TestCase(10u)]
        [TestCase(31u)]
        [TestCase(32u)]
        [TestCase(33u)]
        [TestCase(63u)]
        [TestCase(64u)]
        [TestCase(65u)]
        public void Constructor_UInt64_Works_ForSmallLengths(uint length)
        {
            var bitArray = new BitArray(length);
            Assert.That(bitArray.Count, Is.EqualTo(length));
            Assert.That(bitArray.ToList(), Is.EqualTo(Enumerable.Repeat(false, unchecked((int)length))));
        }


#if LONG_TESTS
        [TestCase(0x100000000u)] // Takes a few minutes to run I think
#endif
        public void Constructor_UInt64_Works_ForLargeLengths(UInt64 length)
        {
            var bitArray = new BitArray(length);
            Assert.That(bitArray.Count, Is.EqualTo(length));
            Assert.That(bitArray, Has.All.False);
        }

        [TestCase(0u, false)]
        [TestCase(1u, false)]
        [TestCase(10u, false)]
        [TestCase(31u, false)]
        [TestCase(32u, false)]
        [TestCase(33u, false)]
        [TestCase(63u, false)]
        [TestCase(64u, false)]
        [TestCase(65u, false)]
        [TestCase(0u, true)]
        [TestCase(1u, true)]
        [TestCase(10u, true)]
        [TestCase(31u, true)]
        [TestCase(32u, true)]
        [TestCase(33u, true)]
        [TestCase(63u, true)]
        [TestCase(64u, true)]
        [TestCase(65u, true)]
        public void Constructor_UInt64Bool_Works_ForSmallLengths(uint length, bool defaultValue)
        {
            var bitArray = new BitArray(length, defaultValue);
            Assert.That(bitArray.Count, Is.EqualTo(length));
            Assert.That(bitArray.ToList(), Is.EqualTo(Enumerable.Repeat(defaultValue, (int)length)));
        }

        static IEnumerable<TestCaseData> SmallCollections_TestCaseSource()
        {
            yield return new TestCaseData(new bool[] { });
            yield return new TestCaseData(new bool[] { false });
            yield return new TestCaseData(new bool[] { true });
            var bits = Enumerable.Repeat(new[] { false, true }, 50).SelectMany(x => x);
            yield return new TestCaseData(bits.Take(31));
            yield return new TestCaseData(bits.Take(32));
            yield return new TestCaseData(bits.Take(33));
            yield return new TestCaseData(bits.Take(63));
            yield return new TestCaseData(bits.Take(64));
            yield return new TestCaseData(bits.Take(65));
            bits = Enumerable.Repeat(new[] { true, false }, 50).SelectMany(x => x);
            yield return new TestCaseData(bits.Take(31));
            yield return new TestCaseData(bits.Take(32));
            yield return new TestCaseData(bits.Take(33));
            yield return new TestCaseData(bits.Take(63));
            yield return new TestCaseData(bits.Take(64));
            yield return new TestCaseData(bits.Take(65));
        }

        [TestCaseSource(nameof(SmallCollections_TestCaseSource))]
        public void Constructor_IEnumerableOfBool_Works_ForSmallCollections(IEnumerable<bool> values)
        {
            var bitArray = new BitArray(values);
            Assert.That(bitArray, Is.EqualTo(values));
        }

        [TestCase(0u)]
        [TestCase(1u)]
        [TestCase(10u)]
        [TestCase(0xffffffffu)]
        [TestCase(0x100000000u)]
        public void Indexer_Get_UInt64_ThrowsArgumentOutOfRangeException(UInt64 count)
        {
            var bitArray = CreateBitArray(count);
            Assert.That(() => bitArray[count], Throws.InstanceOf<ArgumentOutOfRangeException>());
            if (count > UInt64.MaxValue - 1) return;
            Assert.That(() => bitArray[count + 1], Throws.InstanceOf<ArgumentOutOfRangeException>());
            if (count > UInt64.MaxValue - 10) return;
            Assert.That(() => bitArray[count + 10], Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(0u)]
        [TestCase(1u)]
        [TestCase(10u)]
        [TestCase(0xffffffffu)]
        [TestCase(0x100000000u)]
        public void Indexer_Get_Int64_ThrowsArgumentOutOfRangeException(UInt64 count)
        {
            var bitArray = CreateBitArray(count);

            Assert.That(() => bitArray[-1], Throws.InstanceOf<ArgumentOutOfRangeException>());
            Assert.That(() => bitArray[-10], Throws.InstanceOf<ArgumentOutOfRangeException>());
            Assert.That(() => bitArray[Int32.MinValue], Throws.InstanceOf<ArgumentOutOfRangeException>());
            Assert.That(() => bitArray[Int64.MinValue], Throws.InstanceOf<ArgumentOutOfRangeException>());

            if (count > Int64.MaxValue) throw new InvalidTestCaseException();
            Int64 index = (Int64)count;
            Assert.That(() => bitArray[index], Throws.InstanceOf<ArgumentOutOfRangeException>());

            if (count + 1 > Int64.MaxValue) throw new InvalidTestCaseException();
            index = (Int64)count + 1;
            Assert.That(() => bitArray[count + 1], Throws.InstanceOf<ArgumentOutOfRangeException>());

            if (count + 10 > Int64.MaxValue - 10) throw new InvalidTestCaseException();
            index = (Int64)count + 10;
            Assert.That(() => bitArray[count + 10], Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCaseSource(nameof(SmallCollections_TestCaseSource))]
        public void Indexer_Get_UInt64_Works_ForSmallCollections(IEnumerable<bool> values)
        {
            var valuesList = values.ToList();
            if (valuesList.Count > Int32.MaxValue) throw new InvalidTestCaseException();

            var bitArray = new BitArray(values);
            for (uint i = 0; i < valuesList.Count; i++)
            {
                Assert.That(bitArray[i], Is.EqualTo(valuesList[(int)i]));
            }
        }

        [TestCaseSource(nameof(SmallCollections_TestCaseSource))]
        public void Indexer_Get_Int64_Works_ForSmallCollections(IEnumerable<bool> values)
        {
            var valuesList = values.ToList();

            var bitArray = new BitArray(values);
            for (int i = 0; i < valuesList.Count; i++)
            {
                Assert.That(bitArray[i], Is.EqualTo(valuesList[i]));
            }
        }
    }
}

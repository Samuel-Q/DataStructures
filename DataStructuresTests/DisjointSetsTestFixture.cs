using DataStructures;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DataStructuresTests
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks><see cref="DisjointSets{T}.FindSet(T)"/> is currently preferred over
    /// <see cref="DisjointSets{T}.GetSet(T)"/> for the simple tests, because the former has a much
    /// simpler (in the sense of likeliness not to fail and in my opinion) implementation than the latter.</remarks>
    [TestFixture]
    public class DisjointSetsTestFixture
    {
        [Test]
        public void Constructor_TypicalCase()
        {
            var elements = new int[] { 1, 2, 3, 4, 5 };
            var sets = new DisjointSets<int>(elements);
            foreach (var element in elements)
                Assert.That(sets.FindSet(element), Is.EqualTo(element));
        }

        [Test]
        public void Constructor_ThrowsOnDuplicates()
        {
            var elements = new int[] { 1, 2, 1, 4, 5 };
            Assert.That(() => new DisjointSets<int>(elements), Throws.ArgumentException);
        }

        [Test]
        public void MakeSet_WithoutPreExistingElements()
        {
            var sets = new DisjointSets<int>();
            foreach (int i in Enumerable.Range(1, 10))
                sets.MakeSet(i);

            foreach (int i in Enumerable.Range(1, 10))
                Assert.That(sets.FindSet(i), Is.EqualTo(i));
        }

        [Test]
        public void MakeSet_WithPreExistingElements()
        {
            var sets = new DisjointSets<int>(Enumerable.Range(1, 10));
            foreach (int i in Enumerable.Range(11, 10))
                sets.MakeSet(i);

            foreach (int i in Enumerable.Range(1, 20))
                Assert.That(sets.FindSet(i), Is.EqualTo(i));
        }

        [Test]
        public void MakeSet_ThrowsOnAlreadyExistingElement()
        {
            var sets = new DisjointSets<int>();
            sets.MakeSet(1);
            Assert.That(() => sets.MakeSet(1), Throws.ArgumentException);
        }

        [Test]
        public void MakeSet_ThrowsOnAlreadyExistingElement2()
        {
            var sets = new DisjointSets<int>(new int[] { 1, 2, 3 });
            Assert.That(() => sets.MakeSet(1), Throws.ArgumentException);
            Assert.That(() => sets.MakeSet(2), Throws.ArgumentException);
            Assert.That(() => sets.MakeSet(3), Throws.ArgumentException);
        }

        [Test]
        public void FindSet_WithoutPreExistingElements()
        {
            var sets = new DisjointSets<int>();
            foreach (int i in Enumerable.Range(1, 10))
            {
                sets.MakeSet(i);
                foreach (int j in Enumerable.Range(1, i))
                    Assert.That(sets.FindSet(j), Is.EqualTo(j));
            }

            foreach (int i in Enumerable.Range(1, 10))
                Assert.That(sets.FindSet(i), Is.EqualTo(i));
        }

        [Test]
        public void FindSet_WithPreExistingElements()
        {
            var elements = new int[] { 1, 2, 3, 4 };
            var sets = new DisjointSets<int>(elements);
            foreach (var element in elements)
            {
                Assert.That(sets.FindSet(element), Is.EqualTo(element));
            }
        }

        [Test]
        public void FindSet_ThrowsOnNonExistentElement()
        {
            var sets = new DisjointSets<int>(new int[] { 1, 2, 3 });
            Assert.That(() => sets.FindSet(4), Throws.ArgumentException);
        }

        [Test]
        public void Union()
        {
            var sets = new DisjointSets<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            sets.Union(1, 2);
            Assert.That(sets.FindSet(1), Is.EqualTo(1).Or.EqualTo(2));
            Assert.That(sets.FindSet(2), Is.EqualTo(1).Or.EqualTo(2));
            foreach (int i in new int[] { 3, 4, 5, 6 }) Assert.That(sets.FindSet(i), Is.EqualTo(i));
        }

        [Test]
        public void Union_TypicalCase()
        {
            var sets = new DisjointSets<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            sets.Union(1, 2);
            sets.Union(2, 3);
            sets.Union(3, 4);
            sets.Union(5, 6);
            sets.Union(3, 7);
            foreach (int i in new int[] { 1, 2, 3, 4, 5, 6, 7})
                Assert.That(new int[] { sets.FindSet(i) }, Is.SubsetOf(new int[] { 1, 2, 3, 4, 5, 6, 7 })); // Sort of hacky; Is.MemberOf would be better

            foreach (int i in new int[] { 8, 9, 10 })
                Assert.That(sets.FindSet(i), Is.EqualTo(i));
        }

        [Test]
        public void Union_ThrowsOnFirstElementNonExistent()
        {
            var sets = new DisjointSets<int>(new int[] { 1, 2 });
            Assert.That(() => sets.Union(3, 1), Throws.ArgumentException);
        }

        [Test]
        public void Union_ThrowsOnSecondElementNonExistent()
        {
            var sets = new DisjointSets<int>(new int[] { 1, 2 });
            Assert.That(() => sets.Union(1, 3), Throws.ArgumentException);
        }

        [Test]
        public void Union_ThrowsOnElementsInTheSameSet1()
        {
            var sets = new DisjointSets<int>(new int[] { 1, 2, 3, 4 });
            Assert.That(() => sets.Union(1, 1), Throws.ArgumentException);
        }

        [Test]
        public void Union_ThrowsOnElementsInTheSameSet2()
        {
            var sets = new DisjointSets<int>(new int[] { 1, 2, 3, 4 });
            sets.Union(4, 3);
            sets.Union(3, 2);
            sets.Union(2, 1);
            Assert.That(() => sets.Union(1, 4), Throws.ArgumentException);
        }

        [Test]
        public void GetSet_TypicalCase()
        {
            var elements = new int[] { 1, 2, 3, 4, 5 };
            var sets = new DisjointSets<int>(elements);
            foreach (int element in elements)
            {
                Assert.That(sets.GetSet(element), Is.EquivalentTo(new SortedSet<int>(new int[] { element })));
            }
        }

        [Test]
        public void GetSet_TypicalCase2()
        {
            var sets = new DisjointSets<int>(new int[] { 1, 2, 3, 4, 5, 6 });
            sets.Union(1, 2);
            sets.Union(1, 6);
            sets.Union(3, 4);

            Assert.That(sets.GetSet(1), Is.EquivalentTo(new HashSet<int>(new int[] { 1, 2, 6 })));
            Assert.That(sets.GetSet(2), Is.EquivalentTo(new HashSet<int>(new int[] { 1, 2, 6 })));
            Assert.That(sets.GetSet(6), Is.EquivalentTo(new HashSet<int>(new int[] { 1, 2, 6 })));
            Assert.That(sets.GetSet(3), Is.EquivalentTo(new HashSet<int>(new int[] { 3, 4 })));
            Assert.That(sets.GetSet(4), Is.EquivalentTo(new HashSet<int>(new int[] { 3, 4 })));
            Assert.That(sets.GetSet(5), Is.EquivalentTo(new HashSet<int>(new int[] { 5 })));
        }

        [Test]
        public void GetSet_ThrowsOnNonExistentElement()
        {
            var sets = new DisjointSets<int>(new int[] { 1, 2 });
            Assert.That(() => sets.GetSet(3), Throws.ArgumentException);
        }
    }
}

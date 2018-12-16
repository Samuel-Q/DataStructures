using DataStructures;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructuresTests
{
    [TestFixture]
    public class CircularListTestFixture
    {

        [Test]
        public void IndexerGet_ThrowsOnEmptyList()
        {
            var clist = new CircularList<int>();
            Assert.That(() => clist[0], Throws.InstanceOf<InvalidOperationException>());
            Assert.That(() => clist[-1], Throws.InstanceOf<InvalidOperationException>());
            Assert.That(() => clist[1], Throws.InstanceOf<InvalidOperationException>());
            Assert.That(() => clist[4], Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void IndexerSet_ThrowsOnEmptyList()
        {
            var clist = new CircularList<int>();
            Assert.That(() => clist[0] = 123, Throws.InstanceOf<InvalidOperationException>());
            Assert.That(() => clist[-1] = 123, Throws.InstanceOf<InvalidOperationException>());
            Assert.That(() => clist[1] = 123, Throws.InstanceOf<InvalidOperationException>());
            Assert.That(() => clist[4] = 123, Throws.InstanceOf<InvalidOperationException>());
        }

        [TestCase(new int[] { })]
        [TestCase(new int[] { 4 })]
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6 })]
        public void Count_AfterConstruction<T>(IEnumerable<T> collection)
        {
            var clist = new CircularList<T>(collection);
            Assert.That(clist.Count, Is.EqualTo(collection.Count()));
        }

        [TestCase(new int[] { }, new int[] { 1 })]
        [TestCase(new int[] { 4 }, new int[] { 1 })]
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1 })]
        [TestCase(new int[] { }, new int[] { 1, 2, 3 })]
        [TestCase(new int[] { 4 }, new int[] { 1, 2, 3 })]
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1, 2, 3 })]
        public void Count_AfterAddingElements<T>(IEnumerable<T> collection, IEnumerable<T> itemsToAdd)
        {
            var clist = new CircularList<T>(collection);
            foreach (var item in itemsToAdd) clist.Add(item);
            Assert.That(clist.Count, Is.EqualTo(collection.Count() + itemsToAdd.Count()));
        }

        [Test]
        public void DefaultConstructor()
        {
            var clist = new CircularList<int>();
            Assert.That(clist, Is.EqualTo(new int[0]));
        }

        [Test]
        public void Constructor_ThrowsOnNullCollection()
        {
            Assert.That(() => new CircularList<int>(null), Throws.ArgumentNullException);
        }

        [TestCase(new int[] { 123 })]
        [TestCase(new int[] { 123, 456, 789 })]
        [TestCase(new int[] { 1, 1, 1, 1, 1 })]
        public void Constructor_TypicalCase<T>(IEnumerable<T> collection)
        {
            var clist = new CircularList<T>(collection);
            Assert.That(clist, Is.EqualTo(collection));
        }

        [TestCase(0, new int[] { }, new int[] { })]
        [TestCase(1, new int[] { }, new int[] { })]
        [TestCase(-1, new int[] { }, new int[] { })]
        [TestCase(9, new int[] { }, new int[] { })]
        [TestCase(0, new int[] { 1 }, new int[] { 1 })]
        [TestCase(1, new int[] { 1 }, new int[] { 1 })]
        [TestCase(-1, new int[] { 1 }, new int[] { 1 })]
        [TestCase(9, new int[] { 1 }, new int[] { 1 })]
        [TestCase(0, new int[] { 1, 2, 3, 4 }, new int[] { 1, 2, 3, 4 })]
        [TestCase(1, new int[] { 1, 2, 3, 4 }, new int[] { 2, 3, 4, 1 })]
        [TestCase(-1, new int[] { 1, 2, 3, 4 }, new int[] { 4, 1, 2, 3 })]
        [TestCase(9, new int[] { 1, 2, 3, 4 }, new int[] { 2, 3, 4, 1 })]
        public void RotateLeft_AfterConstruction<T>(int count, IEnumerable<T> collection, IEnumerable<T> expected)
        {
            var clist = new CircularList<T>(collection);
            clist.RotateLeft(count);
            Assert.That(clist, Is.EqualTo(expected));
        }

        [TestCase(0, new int[] { }, new int[] { })]
        [TestCase(1, new int[] { }, new int[] { })]
        [TestCase(-1, new int[] { }, new int[] { })]
        [TestCase(9, new int[] { }, new int[] { })]
        [TestCase(0, new int[] { 1 }, new int[] { 1 })]
        [TestCase(1, new int[] { 1 }, new int[] { 1 })]
        [TestCase(-1, new int[] { 1 }, new int[] { 1 })]
        [TestCase(9, new int[] { 1 }, new int[] { 1 })]
        [TestCase(0, new int[] { 1, 2, 3, 4 }, new int[] { 1, 2, 3, 4 })]
        [TestCase(1, new int[] { 1, 2, 3, 4 }, new int[] { 4, 1, 2, 3 })]
        [TestCase(-1, new int[] { 1, 2, 3, 4 }, new int[] { 2, 3, 4, 1 })]
        [TestCase(9, new int[] { 1, 2, 3, 4 }, new int[] { 4, 1, 2, 3 })]
        public void RotateRight_AfterConstruction<T>(int count, IEnumerable<T> collection, IEnumerable<T> expected)
        {
            var clist = new CircularList<T>(collection);
            clist.RotateRight(count);
            Assert.That(clist, Is.EqualTo(expected));
        }

        [Test]
        public void AddRange_ThrowsArgumentNullException_OnNullCollection()
        {
            var clist = new CircularList<int>();
            Assert.That(() => clist.AddRange(null), Throws.ArgumentNullException);
            clist = new CircularList<int> { 1, 2, 3, 4, 5 };
            Assert.That(() => clist.AddRange(null), Throws.ArgumentNullException);
        }

        [TestCase(new int[] { }, new int[] { }, new int[] { })]
        [TestCase(new int[] { 1 }, new int[] { }, new int[] { 1 })]
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { }, new int[] { 1, 2, 3, 4, 5, 6 })]
        [TestCase(new int[] { 1, 2, 1, 3, 1, 2 }, new int[] { }, new int[] { 1, 2, 1, 3, 1, 2 })]
        [TestCase(new int[] { }, new int[] { 1 }, new int[] { 1 })]
        [TestCase(new int[] { }, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1, 2, 3, 4, 5, 6 })]
        [TestCase(new int[] { }, new int[] { 1, 2, 1, 1, 3, 4 }, new int[] { 1, 2, 1, 1, 3, 4 })]
        [TestCase(new int[] { 1 }, new int[] { 2 }, new int[] { 1, 2 })]
        [TestCase(new int[] { 1 }, new int[] { 2, 3, 4, 5 }, new int[] { 1, 2, 3, 4, 5 })]
        [TestCase(new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 1, 2, 3, 4, 5 })]
        [TestCase(new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 1, 2, 3, 4, 5, 6, 7, 8 })]
        [TestCase(new int[] { 1, 2, 1, 4 }, new int[] { 5, 6, 7, 6 }, new int[] { 1, 2, 1, 4, 5, 6, 7, 6 })]
        public void AddRange_AfterConstruction<T>(IEnumerable<T> collection, IEnumerable<T> itemsToAdd, IEnumerable<T> expected)
        {
            var clist = new CircularList<T>(collection);
            clist.AddRange(itemsToAdd);
            Assert.That(clist, Is.EqualTo(expected));
        }

        [TestCase(1, new int[] { 1 }, new int[] { }, new int[] { 1 })]
        [TestCase(1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { }, new int[] { 2, 3, 4, 5, 6, 1 })]
        [TestCase(1, new int[] { 1, 2, 1, 3, 1, 2 }, new int[] { }, new int[] { 2, 1, 3, 1, 2, 1 })]
        [TestCase(1, new int[] { 1 }, new int[] { 2 }, new int[] { 1, 2 })]
        [TestCase(1, new int[] { 1 }, new int[] { 2, 3, 4, 5 }, new int[] { 1, 2, 3, 4, 5 })]
        [TestCase(1, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 2, 3, 4, 1, 5 })]
        [TestCase(1, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 2, 3, 4, 1, 5, 6, 7, 8 })]
        [TestCase(1, new int[] { 1, 2, 1, 4 }, new int[] { 5, 6, 7, 6 }, new int[] { 2, 1, 4, 1, 5, 6, 7, 6 })]
        [TestCase(3, new int[] { 1, 2, 1, 4 }, new int[] { 5, 6, 7, 6 }, new int[] { 4, 1, 2, 1, 5, 6, 7, 6 })]
        public void AddRange_AfterLeftRotation<T>(int count, IEnumerable<T> collection, IEnumerable<T> itemsToAdd, IEnumerable<T> expected)
        {
            var clist = new CircularList<T>(collection);
            clist.RotateLeft(count);
            clist.AddRange(itemsToAdd);
            Assert.That(clist, Is.EqualTo(expected));
        }

        [TestCase(1, new int[] { 1 }, new int[] { }, new int[] { 1 })]
        [TestCase(1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { }, new int[] { 6, 1, 2, 3, 4, 5 })]
        [TestCase(1, new int[] { 1, 2, 1, 3, 1, 2 }, new int[] { }, new int[] { 2, 1, 2, 1, 3, 1 })]
        [TestCase(1, new int[] { 1 }, new int[] { 2 }, new int[] { 1, 2 })]
        [TestCase(1, new int[] { 1 }, new int[] { 2, 3, 4, 5 }, new int[] { 1, 2, 3, 4, 5 })]
        [TestCase(1, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 4, 1, 2, 3, 5 })]
        [TestCase(1, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 4, 1, 2, 3, 5, 6, 7, 8 })]
        [TestCase(1, new int[] { 1, 2, 1, 4 }, new int[] { 5, 6, 7, 6 }, new int[] { 4, 1, 2, 1, 5, 6, 7, 6 })]
        [TestCase(3, new int[] { 1, 2, 1, 4 }, new int[] { 5, 6, 7, 6 }, new int[] { 2, 1, 4, 1, 5, 6, 7, 6 })]
        public void AddRange_AfterRightRotation<T>(int count, IEnumerable<T> collection, IEnumerable<T> itemsToAdd, IEnumerable<T> expected)
        {
            var clist = new CircularList<T>(collection);
            clist.RotateRight(count);
            clist.AddRange(itemsToAdd);
            Assert.That(clist, Is.EqualTo(expected));
        }

        [TestCase(0, -1, new int[] { 1 })]
        [TestCase(1, -1, new int[] { 1 })]
        [TestCase(-1, -1, new int[] { 1 })]
        [TestCase(0, -1, new int[] { 1, 2, 3, 4 })]
        [TestCase(3, -1, new int[] { 1, 2, 3, 4 })]
        [TestCase(-1, -1, new int[] { 1, 2, 3, 4 })]
        [TestCase(0, -9, new int[] { 1 })]
        [TestCase(1, -9, new int[] { 1 })]
        [TestCase(-1, -9, new int[] { 1 })]
        [TestCase(0, -9, new int[] { 1, 2, 3, 4 })]
        [TestCase(3, -9, new int[] { 1, 2, 3, 4 })]
        [TestCase(-1, -9, new int[] { 1, 2, 3, 4 })]
        public void GetRange_ThrowsArgumentOutOfRangeException_OnNegativeCount<T>(int index, int count, IEnumerable<T> collection)
        {
            var clist = new CircularList<T>(collection);
            Assert.That(() => clist.GetRange(index, count), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(0, 2, new int[] { 1 })]
        [TestCase(1, 2, new int[] { 1 })]
        [TestCase(-1, 2, new int[] { 1 })]
        [TestCase(0, 5, new int[] { 1, 2, 3, 4 })]
        [TestCase(3, 5, new int[] { 1, 2, 3, 4 })]
        [TestCase(-1, 5, new int[] { 1, 2, 3, 4 })]
        [TestCase(0, 9, new int[] { 1 })]
        [TestCase(1, 9, new int[] { 1 })]
        [TestCase(-1, 9, new int[] { 1 })]
        [TestCase(0, 9, new int[] { 1, 2, 3, 4 })]
        [TestCase(3, 9, new int[] { 1, 2, 3, 4 })]
        [TestCase(-1, 9, new int[] { 1, 2, 3, 4 })]
        public void GetRange_ThrowsArgumentOutOfRangeException_OnCountTooLarge<T>(int index, int count, IEnumerable<T> collection)
        {
            var clist = new CircularList<T>(collection);
            Assert.That(() => clist.GetRange(index, count), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(0, 0)]
        [TestCase(1, 0)]
        [TestCase(-1, 0)]
        [TestCase(5, 0)]
        [TestCase(-5, 0)]
        public void GetRange_ThrowsInvalidOperationException_WhenListIsEmpty(int index, int count)
        {
            var clist = new CircularList<int>();
            Assert.That(() => clist.GetRange(index, count), Throws.InstanceOf<InvalidOperationException>());
        }


        [TestCase(0, 0, new int[] { 1 }, new int[] { })]
        [TestCase(1, 0, new int[] { 1 }, new int[] { })]
        [TestCase(-1, 0, new int[] { 1 }, new int[] { })]
        [TestCase(0, 0, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { })]
        [TestCase(1, 0, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { })]
        [TestCase(-1, 0, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { })]
        [TestCase(7, 0, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { })]
        [TestCase(0, 1, new int[] { 1 }, new int[] { 1 })]
        [TestCase(1, 1, new int[] { 1 }, new int[] { 1 })]
        [TestCase(-1, 1, new int[] { 1 }, new int[] { 1 })]
        [TestCase(0, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1 })]
        [TestCase(1, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 2 })]
        [TestCase(-1, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 6 })]
        [TestCase(7, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 2 })]
        [TestCase(0, 5, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1, 2, 3, 4, 5 })]
        [TestCase(1, 5, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 2, 3, 4, 5, 6 })]
        [TestCase(-1, 5, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 6, 1, 2, 3, 4 })]
        [TestCase(7, 5, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 2, 3, 4, 5, 6 })]
        [TestCase(0, 6, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1, 2, 3, 4, 5, 6 })]
        [TestCase(1, 6, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 2, 3, 4, 5, 6, 1 })]
        [TestCase(-1, 6, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 6, 1, 2, 3, 4, 5 })]
        [TestCase(7, 6, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 2, 3, 4, 5, 6, 1 })]
        public void GetRange_AfterConstruction<T>(int index, int count, IEnumerable<T> collection, IEnumerable<T> expected)
        {
            var clist = new CircularList<T>(collection);
            var range = clist.GetRange(index, count);
            Assert.That(range, Is.EqualTo(expected));
        }

        [TestCase(1, 0, 0, new int[] { 1 }, new int[] { })]
        [TestCase(1, 1, 0, new int[] { 1 }, new int[] { })]
        [TestCase(1, -1, 0, new int[] { 1 }, new int[] { })]
        [TestCase(1, 0, 0, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { })]
        [TestCase(1, 1, 0, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { })]
        [TestCase(1, -1, 0, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { })]
        [TestCase(1, 7, 0, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { })]
        [TestCase(1, 0, 1, new int[] { 1 }, new int[] { 1 })]
        [TestCase(1, 1, 1, new int[] { 1 }, new int[] { 1 })]
        [TestCase(1, -1, 1, new int[] { 1 }, new int[] { 1 })]
        [TestCase(1, 0, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 2 })]
        [TestCase(1, 1, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 3 })]
        [TestCase(1, -1, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1 })]
        [TestCase(1, 7, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 3 })]
        [TestCase(1, 0, 5, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 2, 3, 4, 5, 6 })]
        [TestCase(1, 1, 5, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 3, 4, 5, 6, 1 })]
        [TestCase(1, -1, 5, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1, 2, 3, 4, 5 })]
        [TestCase(1, 7, 5, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 3, 4, 5, 6, 1 })]
        [TestCase(1, 0, 6, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 2, 3, 4, 5, 6, 1 })]
        [TestCase(1, 1, 6, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 3, 4, 5, 6, 1, 2 })]
        [TestCase(1, -1, 6, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1, 2, 3, 4, 5, 6 })]
        [TestCase(1, 7, 6, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 3, 4, 5, 6, 1, 2 })]
        [TestCase(7, 0, 0, new int[] { 1 }, new int[] { })]
        [TestCase(7, 1, 0, new int[] { 1 }, new int[] { })]
        [TestCase(7, -1, 0, new int[] { 1 }, new int[] { })]
        [TestCase(7, 0, 0, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { })]
        [TestCase(7, 1, 0, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { })]
        [TestCase(7, -1, 0, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { })]
        [TestCase(7, 7, 0, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { })]
        [TestCase(7, 0, 1, new int[] { 1 }, new int[] { 1 })]
        [TestCase(7, 1, 1, new int[] { 1 }, new int[] { 1 })]
        [TestCase(7, -1, 1, new int[] { 1 }, new int[] { 1 })]
        [TestCase(7, 0, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 2 })]
        [TestCase(7, 1, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 3 })]
        [TestCase(7, -1, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1 })]
        [TestCase(7, 7, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 3 })]
        [TestCase(7, 0, 5, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 2, 3, 4, 5, 6 })]
        [TestCase(7, 1, 5, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 3, 4, 5, 6, 1 })]
        [TestCase(7, -1, 5, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1, 2, 3, 4, 5 })]
        [TestCase(7, 7, 5, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 3, 4, 5, 6, 1 })]
        [TestCase(7, 0, 6, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 2, 3, 4, 5, 6, 1 })]
        [TestCase(7, 1, 6, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 3, 4, 5, 6, 1, 2 })]
        [TestCase(7, -1, 6, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1, 2, 3, 4, 5, 6 })]
        [TestCase(7, 7, 6, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 3, 4, 5, 6, 1, 2 })]
        [TestCase(-5, 0, 0, new int[] { 1 }, new int[] { })]
        [TestCase(-5, 1, 0, new int[] { 1 }, new int[] { })]
        [TestCase(-5, -1, 0, new int[] { 1 }, new int[] { })]
        [TestCase(-5, 0, 0, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { })]
        [TestCase(-5, 1, 0, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { })]
        [TestCase(-5, -1, 0, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { })]
        [TestCase(-5, 7, 0, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { })]
        [TestCase(-5, 0, 1, new int[] { 1 }, new int[] { 1 })]
        [TestCase(-5, 1, 1, new int[] { 1 }, new int[] { 1 })]
        [TestCase(-5, -1, 1, new int[] { 1 }, new int[] { 1 })]
        [TestCase(-5, 0, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 2 })]
        [TestCase(-5, 1, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 3 })]
        [TestCase(-5, -1, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1 })]
        [TestCase(-5, 7, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 3 })]
        [TestCase(-5, 0, 5, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 2, 3, 4, 5, 6 })]
        [TestCase(-5, 1, 5, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 3, 4, 5, 6, 1 })]
        [TestCase(-5, -1, 5, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1, 2, 3, 4, 5 })]
        [TestCase(-5, 7, 5, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 3, 4, 5, 6, 1 })]
        [TestCase(-5, 0, 6, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 2, 3, 4, 5, 6, 1 })]
        [TestCase(-5, 1, 6, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 3, 4, 5, 6, 1, 2 })]
        [TestCase(-5, -1, 6, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1, 2, 3, 4, 5, 6 })]
        [TestCase(-5, 7, 6, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 3, 4, 5, 6, 1, 2 })]
        public void GetRange_AfterLeftRotation<T>(int rotCount, int index, int count, IEnumerable<T> collection, IEnumerable<T> expected)
        {
            var clist = new CircularList<T>(collection);
            clist.RotateLeft(rotCount);
            var range = clist.GetRange(index, count);
            Assert.That(range, Is.EqualTo(expected));
        }

        [TestCase(1, 0, 0, new int[] { 1 }, new int[] { })]
        [TestCase(1, 1, 0, new int[] { 1 }, new int[] { })]
        [TestCase(1, -1, 0, new int[] { 1 }, new int[] { })]
        [TestCase(1, 0, 0, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { })]
        [TestCase(1, 1, 0, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { })]
        [TestCase(1, -1, 0, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { })]
        [TestCase(1, 7, 0, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { })]
        [TestCase(1, 0, 1, new int[] { 1 }, new int[] { 1 })]
        [TestCase(1, 1, 1, new int[] { 1 }, new int[] { 1 })]
        [TestCase(1, -1, 1, new int[] { 1 }, new int[] { 1 })]
        [TestCase(1, 0, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 6 })]
        [TestCase(1, 1, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1 })]
        [TestCase(1, -1, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 5 })]
        [TestCase(1, 7, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1 })]
        [TestCase(1, 0, 5, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 6, 1, 2, 3, 4 })]
        [TestCase(1, 1, 5, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1, 2, 3, 4, 5 })]
        [TestCase(1, -1, 5, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 5, 6, 1, 2, 3 })]
        [TestCase(1, 7, 5, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1, 2, 3, 4, 5 })]
        [TestCase(1, 0, 6, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 6, 1, 2, 3, 4, 5 })]
        [TestCase(1, 1, 6, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1, 2, 3, 4, 5, 6 })]
        [TestCase(1, -1, 6, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 5, 6, 1, 2, 3, 4 })]
        [TestCase(1, 7, 6, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1, 2, 3, 4, 5, 6 })]
        [TestCase(7, 0, 0, new int[] { 1 }, new int[] { })]
        [TestCase(7, 1, 0, new int[] { 1 }, new int[] { })]
        [TestCase(7, -1, 0, new int[] { 1 }, new int[] { })]
        [TestCase(7, 0, 0, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { })]
        [TestCase(7, 1, 0, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { })]
        [TestCase(7, -1, 0, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { })]
        [TestCase(7, 7, 0, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { })]
        [TestCase(7, 0, 1, new int[] { 1 }, new int[] { 1 })]
        [TestCase(7, 1, 1, new int[] { 1 }, new int[] { 1 })]
        [TestCase(7, -1, 1, new int[] { 1 }, new int[] { 1 })]
        [TestCase(7, 0, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 6 })]
        [TestCase(7, 1, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1 })]
        [TestCase(7, -1, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 5 })]
        [TestCase(7, 7, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1 })]
        [TestCase(7, 0, 5, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 6, 1, 2, 3, 4 })]
        [TestCase(7, 1, 5, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1, 2, 3, 4, 5 })]
        [TestCase(7, -1, 5, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 5, 6, 1, 2, 3 })]
        [TestCase(7, 7, 5, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1, 2, 3, 4, 5 })]
        [TestCase(7, 0, 6, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 6, 1, 2, 3, 4, 5 })]
        [TestCase(7, 1, 6, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1, 2, 3, 4, 5, 6 })]
        [TestCase(7, -1, 6, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 5, 6, 1, 2, 3, 4 })]
        [TestCase(7, 7, 6, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1, 2, 3, 4, 5, 6 })]
        [TestCase(-5, 0, 0, new int[] { 1 }, new int[] { })]
        [TestCase(-5, 1, 0, new int[] { 1 }, new int[] { })]
        [TestCase(-5, -1, 0, new int[] { 1 }, new int[] { })]
        [TestCase(-5, 0, 0, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { })]
        [TestCase(-5, 1, 0, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { })]
        [TestCase(-5, -1, 0, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { })]
        [TestCase(-5, 7, 0, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { })]
        [TestCase(-5, 0, 1, new int[] { 1 }, new int[] { 1 })]
        [TestCase(-5, 1, 1, new int[] { 1 }, new int[] { 1 })]
        [TestCase(-5, -1, 1, new int[] { 1 }, new int[] { 1 })]
        [TestCase(-5, 0, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 6 })]
        [TestCase(-5, 1, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1 })]
        [TestCase(-5, -1, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 5 })]
        [TestCase(-5, 7, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1 })]
        [TestCase(-5, 0, 5, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 6, 1, 2, 3, 4 })]
        [TestCase(-5, 1, 5, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1, 2, 3, 4, 5 })]
        [TestCase(-5, -1, 5, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 5, 6, 1, 2, 3 })]
        [TestCase(-5, 7, 5, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1, 2, 3, 4, 5 })]
        [TestCase(-5, 0, 6, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 6, 1, 2, 3, 4, 5 })]
        [TestCase(-5, 1, 6, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1, 2, 3, 4, 5, 6 })]
        [TestCase(-5, -1, 6, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 5, 6, 1, 2, 3, 4 })]
        [TestCase(-5, 7, 6, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { 1, 2, 3, 4, 5, 6 })]
        public void GetRange_AfterRightRotation<T>(int rotCount, int index, int count, IEnumerable<T> collection, IEnumerable<T> expected)
        {
            var clist = new CircularList<T>(collection);
            clist.RotateRight(rotCount);
            var range = clist.GetRange(index, count);
            Assert.That(range, Is.EqualTo(expected));
        }

        [Test]
        public void InsertRange_ThrowsArgumentNullException_OnNullCollection()
        {
            var clist = new CircularList<int>();
            Assert.That(() => clist.InsertRange(0, null), Throws.ArgumentNullException);
            Assert.That(() => clist.InsertRange(4, null), Throws.ArgumentNullException);
            clist = new CircularList<int> { 1, 2, 3, 4, 5 };
            Assert.That(() => clist.InsertRange(0, null), Throws.ArgumentNullException);
            Assert.That(() => clist.InsertRange(4, null), Throws.ArgumentNullException);
        }

        [TestCase(0, new int[] { })]
        [TestCase(1, new int[] { })]
        [TestCase(-1, new int[] { })]
        [TestCase(9, new int[] { })]
        [TestCase(0, new int[] { 4 })]
        [TestCase(1, new int[] { 4 })]
        [TestCase(-1, new int[] { 4 })]
        [TestCase(9, new int[] { 4 })]
        [TestCase(0, new int[] { 1, 2, 3, 4 })]
        [TestCase(1, new int[] { 1, 2, 3, 4 })]
        [TestCase(-1, new int[] { 1, 2, 3, 4 })]
        [TestCase(9, new int[] { 1, 2, 3, 4 })]
        public void InsertRange_ThrowsInvalidOperationException_WhenListIsEmpty<T>(int index, IEnumerable<T> itemsToInsert)
        {
            var clist = new CircularList<T>();
            Assert.That(() => clist.InsertRange(index, itemsToInsert), Throws.InstanceOf<InvalidOperationException>());
        }

        [TestCase(0, new int[] { 1 }, new int[] { }, new int[] { 1 })]
        [TestCase(0, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { }, new int[] { 1, 2, 3, 4, 5, 6 })]
        [TestCase(0, new int[] { 1, 2, 1, 3, 1, 2 }, new int[] { }, new int[] { 1, 2, 1, 3, 1, 2 })]
        [TestCase(0, new int[] { 1 }, new int[] { 2 }, new int[] { 2, 1 })]
        [TestCase(0, new int[] { 1 }, new int[] { 2, 3, 4, 5 }, new int[] { 2, 3, 4, 5, 1 })]
        [TestCase(0, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 5, 1, 2, 3, 4 })]
        [TestCase(0, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 5, 6, 7, 8, 1, 2, 3, 4 })]
        [TestCase(0, new int[] { 1, 2, 1, 4 }, new int[] { 5, 6, 7, 6 }, new int[] { 5, 6, 7, 6, 1, 2, 1, 4 })]
        [TestCase(1, new int[] { 1 }, new int[] { }, new int[] { 1 })]
        [TestCase(1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { }, new int[] { 1, 2, 3, 4, 5, 6 })]
        [TestCase(1, new int[] { 1, 2, 1, 3, 1, 2 }, new int[] { }, new int[] { 1, 2, 1, 3, 1, 2 })]
        [TestCase(1, new int[] { 1 }, new int[] { 2 }, new int[] { 2, 1 })] // Pecular but the way it is
        [TestCase(1, new int[] { 1 }, new int[] { 2, 3, 4, 5 }, new int[] { 2, 3, 4, 5, 1 })] // Pecular but the way it is
        [TestCase(1, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 1, 5, 2, 3, 4 })]
        [TestCase(1, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 1, 5, 6, 7, 8, 2, 3, 4 })]
        [TestCase(1, new int[] { 1, 2, 1, 4 }, new int[] { 5, 6, 7, 6 }, new int[] { 1, 5, 6, 7, 6, 2, 1, 4 })]
        [TestCase(-1, new int[] { 1 }, new int[] { }, new int[] { 1 })]
        [TestCase(-1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { }, new int[] { 1, 2, 3, 4, 5, 6 })]
        [TestCase(-1, new int[] { 1, 2, 1, 3, 1, 2 }, new int[] { }, new int[] { 1, 2, 1, 3, 1, 2 })]
        [TestCase(-1, new int[] { 1 }, new int[] { 2 }, new int[] { 2, 1 })] // Pecular but the way it is
        [TestCase(-1, new int[] { 1 }, new int[] { 2, 3, 4, 5 }, new int[] { 2, 3, 4, 5, 1 })] // Pecular but the way it is
        [TestCase(-1, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 1, 2, 3, 5, 4 })]
        [TestCase(-1, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 1, 2, 3, 5, 6, 7, 8, 4 })]
        [TestCase(-1, new int[] { 1, 2, 1, 4 }, new int[] { 5, 6, 7, 6 }, new int[] { 1, 2, 1, 5, 6, 7, 6, 4 })]
        public void InsertRange_AfterConstruction<T>(int index, IEnumerable<T> collection, IEnumerable<T> itemsToInsert, IEnumerable<T> expected)
        {
            var clist = new CircularList<T>(collection);
            clist.InsertRange(index, itemsToInsert);
            Assert.That(clist, Is.EqualTo(expected));
        }

        [TestCase(0, 1, new int[] { 1 }, new int[] { }, new int[] { 1 })]
        [TestCase(0, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { }, new int[] { 2, 3, 4, 5, 6, 1 })]
        [TestCase(0, 1, new int[] { 1, 2, 1, 3, 1, 2 }, new int[] { }, new int[] { 2, 1, 3, 1, 2, 1 })]
        [TestCase(0, 1, new int[] { 1 }, new int[] { 2 }, new int[] { 2, 1 })]
        [TestCase(0, 1, new int[] { 1 }, new int[] { 2, 3, 4, 5 }, new int[] { 2, 3, 4, 5, 1 })]
        [TestCase(0, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 5, 2, 3, 4, 1 })]
        [TestCase(0, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 5, 6, 7, 8, 2, 3, 4, 1 })]
        [TestCase(0, 1, new int[] { 1, 2, 1, 4 }, new int[] { 5, 6, 7, 6 }, new int[] { 5, 6, 7, 6, 2, 1, 4, 1 })]
        [TestCase(0, 3, new int[] { 1, 2, 1, 4 }, new int[] { 5, 6, 7, 6 }, new int[] { 5, 6, 7, 6, 4, 1, 2, 1 })]
        [TestCase(1, 1, new int[] { 1 }, new int[] { }, new int[] { 1 })]
        [TestCase(1, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { }, new int[] { 2, 3, 4, 5, 6, 1 })]
        [TestCase(1, 1, new int[] { 1, 2, 1, 3, 1, 2 }, new int[] { }, new int[] { 2, 1, 3, 1, 2, 1 })]
        [TestCase(1, 1, new int[] { 1 }, new int[] { 2 }, new int[] { 2, 1 })] // Pecular but the way it is
        [TestCase(1, 1, new int[] { 1 }, new int[] { 2, 3, 4, 5 }, new int[] { 2, 3, 4, 5, 1 })] // Peculiar but the way it is
        [TestCase(1, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 2, 5, 3, 4, 1 })]
        [TestCase(1, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 2, 5, 6, 7, 8, 3, 4, 1 })]
        [TestCase(1, 1, new int[] { 1, 2, 1, 4 }, new int[] { 5, 6, 7, 6 }, new int[] { 2, 5, 6, 7, 6, 1, 4, 1 })]
        [TestCase(1, 3, new int[] { 1, 2, 1, 4 }, new int[] { 5, 6, 7, 6 }, new int[] { 4, 5, 6, 7, 6, 1, 2, 1 })]
        [TestCase(-1, 1, new int[] { 1 }, new int[] { }, new int[] { 1 })]
        [TestCase(-1, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { }, new int[] { 2, 3, 4, 5, 6, 1 })]
        [TestCase(-1, 1, new int[] { 1, 2, 1, 3, 1, 2 }, new int[] { }, new int[] { 2, 1, 3, 1, 2, 1 })]
        [TestCase(-1, 1, new int[] { 1 }, new int[] { 2 }, new int[] { 2, 1 })] // Pecular but the way it is
        [TestCase(-1, 1, new int[] { 1 }, new int[] { 2, 3, 4, 5 }, new int[] { 2, 3, 4, 5, 1 })] // Peculiar but the way it is
        [TestCase(-1, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 2, 3, 4, 5, 1 })]
        [TestCase(-1, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 2, 3, 4, 5, 6, 7, 8, 1 })]
        [TestCase(-1, 1, new int[] { 1, 2, 1, 4 }, new int[] { 5, 6, 7, 6 }, new int[] { 2, 1, 4, 5, 6, 7, 6, 1 })]
        [TestCase(-1, 3, new int[] { 1, 2, 1, 4 }, new int[] { 5, 6, 7, 6 }, new int[] { 4, 1, 2, 5, 6, 7, 6, 1 })]
        public void InsertRange_AfterLeftRotation<T>(int index, int count, IEnumerable<T> collection, IEnumerable<T> itemsToInsert, IEnumerable<T> expected)
        {
            var clist = new CircularList<T>(collection);
            clist.RotateLeft(count);
            clist.InsertRange(index, itemsToInsert);
            Assert.That(clist, Is.EqualTo(expected));
        }

        [TestCase(0, 1, new int[] { 1 }, new int[] { }, new int[] { 1 })]
        [TestCase(0, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { }, new int[] { 6, 1, 2, 3, 4, 5 })]
        [TestCase(0, 1, new int[] { 1, 2, 1, 3, 1, 2 }, new int[] { }, new int[] { 2, 1, 2, 1, 3, 1 })]
        [TestCase(0, 1, new int[] { 1 }, new int[] { 2 }, new int[] { 2, 1 })]
        [TestCase(0, 1, new int[] { 1 }, new int[] { 2, 3, 4, 5 }, new int[] { 2, 3, 4, 5, 1 })]
        [TestCase(0, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 5, 4, 1, 2, 3 })]
        [TestCase(0, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 5, 6, 7, 8, 4, 1, 2, 3 })]
        [TestCase(0, 1, new int[] { 1, 2, 1, 4 }, new int[] { 5, 6, 7, 6 }, new int[] { 5, 6, 7, 6, 4, 1, 2, 1 })]
        [TestCase(0, 3, new int[] { 1, 2, 1, 4 }, new int[] { 5, 6, 7, 6 }, new int[] { 5, 6, 7, 6, 2, 1, 4, 1 })]
        [TestCase(1, 1, new int[] { 1 }, new int[] { }, new int[] { 1 })]
        [TestCase(1, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { }, new int[] { 6, 1, 2, 3, 4, 5 })]
        [TestCase(1, 1, new int[] { 1, 2, 1, 3, 1, 2 }, new int[] { }, new int[] { 2, 1, 2, 1, 3, 1 })]
        [TestCase(1, 1, new int[] { 1 }, new int[] { 2 }, new int[] { 2, 1 })] // Pecular but the way it is
        [TestCase(1, 1, new int[] { 1 }, new int[] { 2, 3, 4, 5 }, new int[] { 2, 3, 4, 5, 1 })] // Peculiar but the way it is
        [TestCase(1, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 4, 5, 1, 2, 3 })]
        [TestCase(1, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 4, 5, 6, 7, 8, 1, 2, 3 })]
        [TestCase(1, 1, new int[] { 1, 2, 1, 4 }, new int[] { 5, 6, 7, 6 }, new int[] { 4, 5, 6, 7, 6, 1, 2, 1 })]
        [TestCase(1, 3, new int[] { 1, 2, 1, 4 }, new int[] { 5, 6, 7, 6 }, new int[] { 2, 5, 6, 7, 6, 1, 4, 1 })]
        [TestCase(-1, 1, new int[] { 1 }, new int[] { }, new int[] { 1 })]
        [TestCase(-1, 1, new int[] { 1, 2, 3, 4, 5, 6 }, new int[] { }, new int[] { 6, 1, 2, 3, 4, 5 })]
        [TestCase(-1, 1, new int[] { 1, 2, 1, 3, 1, 2 }, new int[] { }, new int[] { 2, 1, 2, 1, 3, 1 })]
        [TestCase(-1, 1, new int[] { 1 }, new int[] { 2 }, new int[] { 2, 1 })] // Pecular but the way it is
        [TestCase(-1, 1, new int[] { 1 }, new int[] { 2, 3, 4, 5 }, new int[] { 2, 3, 4, 5, 1 })] // Peculiar but the way it is
        [TestCase(-1, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 4, 1, 2, 5, 3 })]
        [TestCase(-1, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 4, 1, 2, 5, 6, 7, 8, 3 })]
        [TestCase(-1, 1, new int[] { 1, 2, 1, 4 }, new int[] { 5, 6, 7, 6 }, new int[] { 4, 1, 2, 5, 6, 7, 6, 1 })]
        [TestCase(-1, 3, new int[] { 1, 2, 1, 4 }, new int[] { 5, 6, 7, 6 }, new int[] { 2, 1, 4, 5, 6, 7, 6, 1 })]
        public void InsertRange_AfterRightRotation<T>(int index, int count, IEnumerable<T> collection, IEnumerable<T> itemsToInsert, IEnumerable<T> expected)
        {
            var clist = new CircularList<T>(collection);
            clist.RotateRight(count);
            clist.InsertRange(index, itemsToInsert);
            Assert.That(clist, Is.EqualTo(expected));
        }

        //[TestCase(0,  -1, new int[] { })] // These clash with the InvalidOperationException condition (empty list)
        //[TestCase(1,  -1, new int[] { })]
        //[TestCase(-1, -1, new int[] { })]
        //[TestCase(0,  -9, new int[] { })]
        //[TestCase(1,  -9, new int[] { })]
        //[TestCase(-1, -9, new int[] { })]
        [TestCase(0,  -1, new int[] { 1 })]
        [TestCase(1,  -1, new int[] { 1 })]
        [TestCase(-1, -1, new int[] { 1 })]
        [TestCase(0,  -9, new int[] { 1 })]
        [TestCase(1,  -9, new int[] { 1 })]
        [TestCase(-1, -9, new int[] { 1 })]
        [TestCase(0,  -9, new int[] { 1, 2, 3, 4, 5, 6, 7, 8 })]
        [TestCase(1,  -9, new int[] { 1, 2, 3, 4, 5, 6, 7, 8 })]
        [TestCase(-1, -9, new int[] { 1, 2, 3, 4, 5, 6, 7, 8 })]
        public void RemoveRange_ThrowsArgumentOutOfRangeException_OnNegativeCount<T>(int index, int count, IEnumerable<T> collection)
        {
            var clist = new CircularList<T>(collection);
            Assert.That(() => clist.RemoveRange(index, count), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        //[TestCase(0,  1, new int[] { })] // These clash with the InvalidOperationException condition (empty list)
        //[TestCase(1,  1, new int[] { })]
        //[TestCase(-1, 1, new int[] { })]
        //[TestCase(0,  9, new int[] { })]
        //[TestCase(1,  9, new int[] { })]
        //[TestCase(-1, 9, new int[] { })]
        [TestCase(0,  2, new int[] { 1 })]
        [TestCase(1,  2, new int[] { 1 })]
        [TestCase(-1, 2, new int[] { 1 })]
        [TestCase(0,  9, new int[] { 1 })]
        [TestCase(1,  9, new int[] { 1 })]
        [TestCase(-1, 9, new int[] { 1 })]
        [TestCase(0,  9, new int[] { 1, 2, 3, 4, 5, 6, 7, 8 })]
        [TestCase(1,  9, new int[] { 1, 2, 3, 4, 5, 6, 7, 8 })]
        [TestCase(-1, 9, new int[] { 1, 2, 3, 4, 5, 6, 7, 8 })]
        public void RemoveRange_ThrowsArgumentOutOfRangeException_OnTooLargeCount<T>(int index, int count, IEnumerable<T> collection)
        {
            var clist = new CircularList<T>(collection);
            Assert.That(() => clist.RemoveRange(index, count), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void RemoveRange_ThrowsInvalidOperationException_WhenListIsEmpty()
        {
            var clist = new CircularList<int>();
            Assert.That(() => clist.RemoveRange(0, 0), Throws.InstanceOf<InvalidOperationException>());
            Assert.That(() => clist.RemoveRange(1, 0), Throws.InstanceOf<InvalidOperationException>());
            Assert.That(() => clist.RemoveRange(-1, 0), Throws.InstanceOf<InvalidOperationException>());
        }

        [TestCase(0,  0, new int[] { 1 }, new int[] { 1 })]
        [TestCase(0,  0, new int[] { 1, 2, 3, 4 }, new int[] { 1, 2, 3, 4 })]
        [TestCase(1,  0, new int[] { 1 }, new int[] { 1 })]
        [TestCase(1,  0, new int[] { 1, 2, 3, 4 }, new int[] { 1, 2, 3, 4 })]
        [TestCase(-1, 0, new int[] { 1 }, new int[] { 1 })]
        [TestCase(-1, 0, new int[] { 1, 2, 3, 4 }, new int[] { 1, 2, 3, 4 })]
        [TestCase(0,  1, new int[] { 1 }, new int[] { })]
        [TestCase(0,  1, new int[] { 1, 2, 3, 4 }, new int[] { 2, 3, 4 })]
        [TestCase(1,  1, new int[] { 1 }, new int[] { })]
        [TestCase(1,  1, new int[] { 1, 2, 3, 4 }, new int[] { 1, 3, 4 })]
        [TestCase(-1, 1, new int[] { 1 }, new int[] { })]
        [TestCase(-1, 1, new int[] { 1, 2, 3, 4 }, new int[] { 1, 2, 3 })]
        [TestCase(0,  3, new int[] { 1, 2, 3, 4 }, new int[] { 4 })]
        [TestCase(1,  3, new int[] { 1, 2, 3, 4 }, new int[] { 1 })]
        [TestCase(-1, 3, new int[] { 1, 2, 3, 4 }, new int[] { 3 })]
        public void RemoveRange_AfterConstruction<T>(int index, int count, IEnumerable<T> collection, IEnumerable<T> expected)
        {
            var clist = new CircularList<T>(collection);
            clist.RemoveRange(index, count);
            Assert.That(clist, Is.EqualTo(expected));
        }

        [TestCase(1, 0, 0, new int[] { 1 }, new int[] { 1 })]
        [TestCase(1, 0, 0, new int[] { 1, 2, 3, 4 }, new int[] { 2, 3, 4, 1 })]
        [TestCase(1, 1, 0, new int[] { 1 }, new int[] { 1 })]
        [TestCase(1, 1, 0, new int[] { 1, 2, 3, 4 }, new int[] { 2, 3, 4, 1 })]
        [TestCase(1, -1, 0, new int[] { 1 }, new int[] { 1 })]
        [TestCase(1, -1, 0, new int[] { 1, 2, 3, 4 }, new int[] { 2, 3, 4, 1 })]
        [TestCase(1, 0, 1, new int[] { 1 }, new int[] { })]
        [TestCase(1, 0, 1, new int[] { 1, 2, 3, 4 }, new int[] { 3, 4, 1 })]
        [TestCase(1, 1, 1, new int[] { 1 }, new int[] { })]
        [TestCase(1, 1, 1, new int[] { 1, 2, 3, 4 }, new int[] { 2, 4, 1 })]
        [TestCase(1, -1, 1, new int[] { 1 }, new int[] { })]
        [TestCase(1, -1, 1, new int[] { 1, 2, 3, 4 }, new int[] { 2, 3, 4 })]
        [TestCase(1, 0, 3, new int[] { 1, 2, 3, 4 }, new int[] { 1 })]
        [TestCase(1, 1, 3, new int[] { 1, 2, 3, 4 }, new int[] { 2 })]
        [TestCase(1, -1, 3, new int[] { 1, 2, 3, 4 }, new int[] { 4 })]
        public void RemoveRange_AfterLeftRotation<T>(int rotCount, int index, int count, IEnumerable<T> collection, IEnumerable<T> expected)
        {
            var clist = new CircularList<T>(collection);
            clist.RotateLeft(rotCount);
            clist.RemoveRange(index, count);
            Assert.That(clist, Is.EqualTo(expected));
        }

        [TestCase(1, 0, 0, new int[] { 1 }, new int[] { 1 })]
        [TestCase(1, 0, 0, new int[] { 1, 2, 3, 4 }, new int[] { 4, 1, 2, 3 })]
        [TestCase(1, 1, 0, new int[] { 1 }, new int[] { 1 })]
        [TestCase(1, 1, 0, new int[] { 1, 2, 3, 4 }, new int[] { 4, 1, 2, 3 })]
        [TestCase(1, -1, 0, new int[] { 1 }, new int[] { 1 })]
        [TestCase(1, -1, 0, new int[] { 1, 2, 3, 4 }, new int[] { 4, 1, 2, 3 })]
        [TestCase(1, 0, 1, new int[] { 1 }, new int[] { })]
        [TestCase(1, 0, 1, new int[] { 1, 2, 3, 4 }, new int[] { 1, 2, 3 })]
        [TestCase(1, 1, 1, new int[] { 1 }, new int[] { })]
        [TestCase(1, 1, 1, new int[] { 1, 2, 3, 4 }, new int[] { 4, 2, 3 })]
        [TestCase(1, -1, 1, new int[] { 1 }, new int[] { })]
        [TestCase(1, -1, 1, new int[] { 1, 2, 3, 4 }, new int[] { 4, 1, 2 })]
        [TestCase(1, 0, 3, new int[] { 1, 2, 3, 4 }, new int[] { 3 })]
        [TestCase(1, 1, 3, new int[] { 1, 2, 3, 4 }, new int[] { 4 })]
        [TestCase(1, -1, 3, new int[] { 1, 2, 3, 4 }, new int[] { 2 })]
        public void RemoveRange_AfterRightRotation<T>(int rotCount, int index, int count, IEnumerable<T> collection, IEnumerable<T> expected)
        {
            var clist = new CircularList<T>(collection);
            clist.RotateRight(rotCount);
            clist.RemoveRange(index, count);
            Assert.That(clist, Is.EqualTo(expected));
        }

        [Test]
        public void ReplaceRange_ThrowsArgumentNullException_OnNullCollection()
        {
            var clist = new CircularList<int> { 1, 2, 3, 4, 5 };
            Assert.That(() => clist.ReplaceRange(0, 0, null), Throws.ArgumentNullException);
            Assert.That(() => clist.ReplaceRange(0, 3, null), Throws.ArgumentNullException);
            Assert.That(() => clist.ReplaceRange(3, 0, null), Throws.ArgumentNullException);
            Assert.That(() => clist.ReplaceRange(3, 3, null), Throws.ArgumentNullException);
        }

        [Test]
        public void ReplaceRange_ThrowsArgumentOutOfRangeException_OnNegativeCount()
        {
            var clist = new CircularList<int> { 1, 2, 3, 4, 5 };
            var replacement = new int[] { 6, 7, 8 };
            Assert.That(() => clist.ReplaceRange(0, -1, replacement), Throws.InstanceOf<ArgumentOutOfRangeException>());
            Assert.That(() => clist.ReplaceRange(0, -3, replacement), Throws.InstanceOf<ArgumentOutOfRangeException>());
            Assert.That(() => clist.ReplaceRange(3, -1, replacement), Throws.InstanceOf<ArgumentOutOfRangeException>());
            Assert.That(() => clist.ReplaceRange(3, -3, replacement), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ReplaceRange_ThrowsArgumentOutOfRangeException_OnCountTooLarge()
        {
            var clist = new CircularList<int> { 1, 2, 3, 4, 5 };
            var replacement = new int[] { 6, 7, 8 };
            Assert.That(() => clist.ReplaceRange(0, 6, replacement), Throws.InstanceOf<ArgumentOutOfRangeException>());
            Assert.That(() => clist.ReplaceRange(0, 99, replacement), Throws.InstanceOf<ArgumentOutOfRangeException>());
            Assert.That(() => clist.ReplaceRange(3, 6, replacement), Throws.InstanceOf<ArgumentOutOfRangeException>());
            Assert.That(() => clist.ReplaceRange(3, 99, replacement), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ReplaceRange_ThrowsInvalidOperationException_WhenListIsEmpty()
        {
            var clist = new CircularList<int> { };
            var replacement = new int[] { 1, 2, 3 };
            Assert.That(() => clist.ReplaceRange(0, 0, new int[] { }), Throws.InstanceOf<InvalidOperationException>());
            Assert.That(() => clist.ReplaceRange(0, 0, new int[] { 1 }), Throws.InstanceOf<InvalidOperationException>());
            Assert.That(() => clist.ReplaceRange(0, 0, new int[] { 1, 2, 3 }), Throws.InstanceOf<InvalidOperationException>());
        }

        [TestCase(0, 0, new int[] { 1 }, new int[] { }, new int[] { 1 })]
        [TestCase(0, 0, new int[] { 1 }, new int[] { 2 }, new int[] { 2, 1 })]
        [TestCase(1, 0, new int[] { 1 }, new int[] { 2 }, new int[] { 2, 1 })] // Pecular but the way it is
        [TestCase(2, 0, new int[] { 1 }, new int[] { 2 }, new int[] { 2, 1 })] // Pecular but the way it is
        [TestCase(-1, 0, new int[] { 1 }, new int[] { 2 }, new int[] { 2, 1 })] // Pecular but the way it is
        [TestCase(0, 0, new int[] { 1, 2, 3, 4 }, new int[] { }, new int[] { 1, 2, 3, 4 })]
        [TestCase(0, 0, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 5, 1, 2, 3, 4 })]
        [TestCase(1, 0, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 1, 5, 2, 3, 4 })]
        [TestCase(5, 0, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 1, 5, 2, 3, 4 })]
        [TestCase(-1, 0, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 1, 2, 3, 5, 4 })]
        [TestCase(0, 0, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 5, 6, 7, 8, 1, 2, 3, 4 })]
        [TestCase(1, 0, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 1, 5, 6, 7, 8, 2, 3, 4 })]
        [TestCase(5, 0, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 1, 5, 6, 7, 8, 2, 3, 4 })]
        [TestCase(-1, 0, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 1, 2, 3, 5, 6, 7, 8, 4 })]
        [TestCase(0, 1, new int[] { 1 }, new int[] { }, new int[] { })]
        [TestCase(0, 1, new int[] { 1 }, new int[] { 2 }, new int[] { 2 })]
        [TestCase(1, 1, new int[] { 1 }, new int[] { 2 }, new int[] { 2 })]
        [TestCase(2, 1, new int[] { 1 }, new int[] { 2 }, new int[] { 2 })]
        [TestCase(-1, 1, new int[] { 1 }, new int[] { 2 }, new int[] { 2 })]
        [TestCase(0, 1, new int[] { 1, 2, 3, 4 }, new int[] { }, new int[] { 2, 3, 4 })]
        [TestCase(0, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 5, 2, 3, 4 })]
        [TestCase(1, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 1, 5, 3, 4 })]
        [TestCase(5, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 1, 5, 3, 4 })]
        [TestCase(-1, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 1, 2, 3, 5 })]
        [TestCase(0, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 5, 6, 7, 8, 2, 3, 4 })]
        [TestCase(1, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 1, 5, 6, 7, 8, 3, 4 })]
        [TestCase(5, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 1, 5, 6, 7, 8, 3, 4 })]
        [TestCase(-1, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 1, 2, 3, 5, 6, 7, 8 })]
        [TestCase(3, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 1, 2, 3, 5, 6, 7, 8 })]
        [TestCase(0, 3, new int[] { 1, 2, 3, 4 }, new int[] { }, new int[] { 4 })]
        [TestCase(0, 3, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 5, 4 })]
        [TestCase(1, 3, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 1, 5 })]
        [TestCase(5, 3, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 1, 5 })]
        [TestCase(-1, 3, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 3, 5 })]
        [TestCase(0, 3, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 5, 6, 7, 8, 4 })]
        [TestCase(1, 3, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 1, 5, 6, 7, 8 })]
        [TestCase(5, 3, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 1, 5, 6, 7, 8 })]
        [TestCase(-1, 3, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 3, 5, 6, 7, 8 })]
        [TestCase(3, 3, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 3, 5, 6, 7, 8 })]
        [TestCase(0, 4, new int[] { 1, 2, 3, 4 }, new int[] { }, new int[] { })]
        [TestCase(0, 4, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 5 })]
        [TestCase(1, 4, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 5 })]
        [TestCase(5, 4, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 5 })]
        [TestCase(-1, 4, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 5 })]
        [TestCase(0, 4, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 5, 6, 7, 8 })]
        [TestCase(1, 4, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 5, 6, 7, 8 })]
        [TestCase(5, 4, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 5, 6, 7, 8 })]
        [TestCase(3, 4, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 5, 6, 7, 8 })]
        [TestCase(-1, 4, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 5, 6, 7, 8 })]
        [TestCase(3, 2, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 2, 3, 5 })]
        [TestCase(14, 2, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 }, new int[] { 15 }, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 15 })]
        public void ReplaceRange_AfterConstruction<T>(int index, int count, IEnumerable<T> collection, IEnumerable<T> replacement, IEnumerable<T> expected)
        {
            var clist = new CircularList<T>(collection);
            clist.ReplaceRange(index, count, replacement);
            Assert.That(clist, Is.EqualTo(expected));
        }

        [TestCase(1, 0, 0, new int[] { 1 }, new int[] { }, new int[] { 1 })]
        [TestCase(1, 0, 0, new int[] { 1 }, new int[] { 2 }, new int[] { 2, 1 })]
        [TestCase(1, 1, 0, new int[] { 1 }, new int[] { 2 }, new int[] { 2, 1 })] // Pecular but the way it is
        [TestCase(1, 2, 0, new int[] { 1 }, new int[] { 2 }, new int[] { 2, 1 })] // Pecular but the way it is
        [TestCase(1, -1, 0, new int[] { 1 }, new int[] { 2 }, new int[] { 2, 1 })] // Pecular but the way it is
        [TestCase(1, 0, 0, new int[] { 1, 2, 3, 4 }, new int[] { }, new int[] { 2, 3, 4, 1 })]
        [TestCase(1, 0, 0, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 5, 2, 3, 4, 1 })]
        [TestCase(1, 1, 0, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 2, 5, 3, 4, 1 })]
        [TestCase(1, 5, 0, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 2, 5, 3, 4, 1 })]
        [TestCase(1, -1, 0, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 2, 3, 4, 5, 1 })]
        [TestCase(1, 0, 0, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 5, 6, 7, 8, 2, 3, 4, 1 })]
        [TestCase(1, 1, 0, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 2, 5, 6, 7, 8, 3, 4, 1 })]
        [TestCase(1, 5, 0, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 2, 5, 6, 7, 8, 3, 4, 1 })]
        [TestCase(1, -1, 0, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 2, 3, 4, 5, 6, 7, 8, 1 })]
        [TestCase(1, 0, 1, new int[] { 1 }, new int[] { }, new int[] { })]
        [TestCase(1, 0, 1, new int[] { 1 }, new int[] { 2 }, new int[] { 2 })]
        [TestCase(1, 1, 1, new int[] { 1 }, new int[] { 2 }, new int[] { 2 })]
        [TestCase(1, 2, 1, new int[] { 1 }, new int[] { 2 }, new int[] { 2 })]
        [TestCase(1, -1, 1, new int[] { 1 }, new int[] { 2 }, new int[] { 2 })]
        [TestCase(1, 0, 1, new int[] { 1, 2, 3, 4 }, new int[] { }, new int[] { 3, 4, 1 })]
        [TestCase(1, 0, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 5, 3, 4, 1 })]
        [TestCase(1, 1, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 2, 5, 4, 1 })]
        [TestCase(1, 5, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 2, 5, 4, 1 })]
        [TestCase(1, -1, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 2, 3, 4, 5 })]
        [TestCase(1, 0, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 5, 6, 7, 8, 3, 4, 1 })]
        [TestCase(1, 1, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 2, 5, 6, 7, 8, 4, 1 })]
        [TestCase(1, 5, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 2, 5, 6, 7, 8, 4, 1 })]
        [TestCase(1, -1, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 2, 3, 4, 5, 6, 7, 8 })]
        [TestCase(1, 3, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 2, 3, 4, 5, 6, 7, 8 })]
        [TestCase(1, 0, 3, new int[] { 1, 2, 3, 4 }, new int[] { }, new int[] { 1 })]
        [TestCase(1, 0, 3, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 5, 1 })]
        [TestCase(1, 1, 3, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 2, 5 })]
        [TestCase(1, 5, 3, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 2, 5 })]
        [TestCase(1, -1, 3, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 4, 5 })]
        [TestCase(1, 0, 3, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 5, 6, 7, 8, 1 })]
        [TestCase(1, 1, 3, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 2, 5, 6, 7, 8 })]
        [TestCase(1, 5, 3, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 2, 5, 6, 7, 8 })]
        [TestCase(1, -1, 3, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 4, 5, 6, 7, 8 })]
        [TestCase(1, 3, 3, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 4, 5, 6, 7, 8 })]
        [TestCase(1, 0, 4, new int[] { 1, 2, 3, 4 }, new int[] { }, new int[] { })]
        [TestCase(1, 0, 4, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 5 })]
        [TestCase(1, 1, 4, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 5 })]
        [TestCase(1, 5, 4, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 5 })]
        [TestCase(1, -1, 4, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 5 })]
        [TestCase(1, 0, 4, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 5, 6, 7, 8 })]
        [TestCase(1, 1, 4, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 5, 6, 7, 8 })]
        [TestCase(1, 5, 4, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 5, 6, 7, 8 })]
        [TestCase(1, 3, 4, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 5, 6, 7, 8 })]
        [TestCase(1, -1, 4, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 5, 6, 7, 8 })]
        public void ReplaceRange_AfterLeftRotation<T>(int rotCount, int index, int count, IEnumerable<T> collection, IEnumerable<T> replacement, IEnumerable<T> expected)
        {
            var clist = new CircularList<T>(collection);
            clist.RotateLeft(rotCount);
            clist.ReplaceRange(index, count, replacement);
            Assert.That(clist, Is.EqualTo(expected));
        }

        [TestCase(1, 0, 0, new int[] { 1 }, new int[] { }, new int[] { 1 })]
        [TestCase(1, 0, 0, new int[] { 1 }, new int[] { 2 }, new int[] { 2, 1 })]
        [TestCase(1, 1, 0, new int[] { 1 }, new int[] { 2 }, new int[] { 2, 1 })] // Pecular but the way it is
        [TestCase(1, 2, 0, new int[] { 1 }, new int[] { 2 }, new int[] { 2, 1 })] // Pecular but the way it is
        [TestCase(1, -1, 0, new int[] { 1 }, new int[] { 2 }, new int[] { 2, 1 })] // Pecular but the way it is
        [TestCase(1, 0, 0, new int[] { 1, 2, 3, 4 }, new int[] { }, new int[] { 4, 1, 2, 3 })]
        [TestCase(1, 0, 0, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 5, 4, 1, 2, 3 })]
        [TestCase(1, 1, 0, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 4, 5, 1, 2, 3 })]
        [TestCase(1, 5, 0, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 4, 5, 1, 2, 3 })]
        [TestCase(1, -1, 0, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 4, 1, 2, 5, 3 })]
        [TestCase(1, 0, 0, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 5, 6, 7, 8, 4, 1, 2, 3 })]
        [TestCase(1, 1, 0, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 4, 5, 6, 7, 8, 1, 2, 3 })]
        [TestCase(1, 5, 0, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 4, 5, 6, 7, 8, 1, 2, 3 })]
        [TestCase(1, -1, 0, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 4, 1, 2, 5, 6, 7, 8, 3 })]
        [TestCase(1, 0, 1, new int[] { 1 }, new int[] { }, new int[] { })]
        [TestCase(1, 0, 1, new int[] { 1 }, new int[] { 2 }, new int[] { 2 })]
        [TestCase(1, 1, 1, new int[] { 1 }, new int[] { 2 }, new int[] { 2 })]
        [TestCase(1, 2, 1, new int[] { 1 }, new int[] { 2 }, new int[] { 2 })]
        [TestCase(1, -1, 1, new int[] { 1 }, new int[] { 2 }, new int[] { 2 })]
        [TestCase(1, 0, 1, new int[] { 1, 2, 3, 4 }, new int[] { }, new int[] { 1, 2, 3 })]
        [TestCase(1, 0, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 5, 1, 2, 3 })]
        [TestCase(1, 1, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 4, 5, 2, 3 })]
        [TestCase(1, 5, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 4, 5, 2, 3 })]
        [TestCase(1, -1, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 4, 1, 2, 5 })]
        [TestCase(1, 0, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 5, 6, 7, 8, 1, 2, 3 })]
        [TestCase(1, 1, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 4, 5, 6, 7, 8, 2, 3 })]
        [TestCase(1, 5, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 4, 5, 6, 7, 8, 2, 3 })]
        [TestCase(1, -1, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 4, 1, 2, 5, 6, 7, 8 })]
        [TestCase(1, 3, 1, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 4, 1, 2, 5, 6, 7, 8 })]
        [TestCase(1, 0, 3, new int[] { 1, 2, 3, 4 }, new int[] { }, new int[] { 3 })]
        [TestCase(1, 0, 3, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 5, 3 })]
        [TestCase(1, 1, 3, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 4, 5 })]
        [TestCase(1, 5, 3, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 4, 5 })]
        [TestCase(1, -1, 3, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 2, 5 })]
        [TestCase(1, 0, 3, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 5, 6, 7, 8, 3 })]
        [TestCase(1, 1, 3, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 4, 5, 6, 7, 8 })]
        [TestCase(1, 5, 3, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 4, 5, 6, 7, 8 })]
        [TestCase(1, -1, 3, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 2, 5, 6, 7, 8 })]
        [TestCase(1, 3, 3, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 2, 5, 6, 7, 8 })]
        [TestCase(1, 0, 4, new int[] { 1, 2, 3, 4 }, new int[] { }, new int[] { })]
        [TestCase(1, 0, 4, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 5 })]
        [TestCase(1, 1, 4, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 5 })]
        [TestCase(1, 5, 4, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 5 })]
        [TestCase(1, -1, 4, new int[] { 1, 2, 3, 4 }, new int[] { 5 }, new int[] { 5 })]
        [TestCase(1, 0, 4, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 5, 6, 7, 8 })]
        [TestCase(1, 1, 4, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 5, 6, 7, 8 })]
        [TestCase(1, 5, 4, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 5, 6, 7, 8 })]
        [TestCase(1, 3, 4, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 5, 6, 7, 8 })]
        [TestCase(1, -1, 4, new int[] { 1, 2, 3, 4 }, new int[] { 5, 6, 7, 8 }, new int[] { 5, 6, 7, 8 })]
        public void ReplaceRange_AfterRightRotation<T>(int rotCount, int index, int count, IEnumerable<T> collection, IEnumerable<T> replacement, IEnumerable<T> expected)
        {
            var clist = new CircularList<T>(collection);
            clist.RotateRight(rotCount);
            clist.ReplaceRange(index, count, replacement);
            Assert.That(clist, Is.EqualTo(expected));
        }

        [TestCase(new int[] { }, new int[] { 123 }, new int[] { 123 })]
        [TestCase(new int[] { }, new int[] { 123, 456 }, new int[] { 123, 456 })]
        [TestCase(new int[] { 123 }, new int[] { 456 }, new int[] { 123, 456 })]
        [TestCase(new int[] { 123 }, new int[] { 456, 789 }, new int[] { 123, 456, 789 })]
        public void Add_AfterConstruction<T>(IEnumerable<T> collection, IEnumerable<T> itemsToAdd, IEnumerable<T> expected)
        {
            var clist = new CircularList<T>(collection);
            foreach (var item in itemsToAdd) clist.Add(item);
            Assert.That(clist, Is.EqualTo(expected));
        }

        [TestCase(1, new int[] { 1, 2, 3 }, new int[] { 4 }, new int[] { 2, 3, 1, 4 })]
        [TestCase(1, new int[] { 1, 2, 3 }, new int[] { 4, 5, 6 }, new int[] { 2, 3, 1, 4, 5, 6 })]
        [TestCase(2, new int[] { 1, 2, 3 }, new int[] { 4 }, new int[] { 3, 1, 2, 4 })]
        [TestCase(2, new int[] { 1, 2, 3 }, new int[] { 4, 5, 6 }, new int[] { 3, 1, 2, 4, 5, 6 })]
        [TestCase(3, new int[] { 1, 2, 3 }, new int[] { 4 }, new int[] { 1, 2, 3, 4 })]
        [TestCase(3, new int[] { 1, 2, 3 }, new int[] { 4, 5, 6 }, new int[] { 1, 2, 3, 4, 5, 6 })]
        public void Add_AfterLeftRotation<T>(int count, IEnumerable<T> collection, IEnumerable<T> itemsToAdd, IEnumerable<T> expected)
        {
            var clist = new CircularList<T>(collection);
            clist.RotateLeft(count);
            foreach (var item in itemsToAdd) clist.Add(item);
            Assert.That(clist, Is.EqualTo(expected));
        }

        [TestCase(1, new int[] { 1, 2, 3 }, new int[] { 4 }, new int[] { 3, 1, 2, 4 })]
        [TestCase(1, new int[] { 1, 2, 3 }, new int[] { 4, 5, 6 }, new int[] { 3, 1, 2, 4, 5, 6 })]
        [TestCase(2, new int[] { 1, 2, 3 }, new int[] { 4 }, new int[] { 2, 3, 1, 4 })]
        [TestCase(2, new int[] { 1, 2, 3 }, new int[] { 4, 5, 6 }, new int[] { 2, 3, 1, 4, 5, 6 })]
        [TestCase(3, new int[] { 1, 2, 3 }, new int[] { 4 }, new int[] { 1, 2, 3, 4 })]
        [TestCase(3, new int[] { 1, 2, 3 }, new int[] { 4, 5, 6 }, new int[] { 1, 2, 3, 4, 5, 6 })]
        public void Add_AfterRightRotation<T>(int count, IEnumerable<T> collection, IEnumerable<T> itemsToAdd, IEnumerable<T> expected)
        {
            var clist = new CircularList<T>(collection);
            clist.RotateRight(count);
            foreach (var item in itemsToAdd) clist.Add(item);
            Assert.That(clist, Is.EqualTo(expected));
        }

        [TestCase(new int[] { })]
        [TestCase(new int[] { 123 })]
        [TestCase(new int[] { 123, 456, 789 })]
        public void Clear_AfterConstruction<T>(IEnumerable<T> collection)
        {
            var clist = new CircularList<T>(collection);
            clist.Clear();
            Assert.That(clist, Has.Count.EqualTo(0));
            Assert.That(clist, Is.EqualTo(new T[0]));
        }

        [TestCase(new int[] { }, 123, false)]
        [TestCase(new int[] { 1, 2, 3, 4, 5 }, 123, false)]
        [TestCase(new int[] { 123 }, 123, true)]
        [TestCase(new int[] { 123, 456, 789 }, 123, true)]
        [TestCase(new int[] { 456, 123, 789 }, 123, true)]
        [TestCase(new int[] { 456, 789, 123 }, 123, true)]
        public void Contains_AfterConstruction<T>(IEnumerable<T> collection, T item, bool expected)
        {
            var clist = new CircularList<T>(collection);
            Assert.That(clist.Contains(item), Is.EqualTo(expected));
        }

        [Test]
        public void CopyTo_ThrowsArgumentNullException_OnNullArray()
        {
            var clist = new CircularList<int>();
            Assert.That(() => clist.CopyTo(null, 0), Throws.ArgumentNullException);
        }

        [Test]
        public void CopyTo_ThrowsArgumentOutOfRangeException_OnNegativeIndex()
        {
            var clist = new CircularList<int>();
            var array = new int[100];
            Assert.That(() => clist.CopyTo(array, -1), Throws.InstanceOf<ArgumentOutOfRangeException>());
            clist = new CircularList<int>(new int[] { 1, 2, 3, 4, 5 });
            Assert.That(() => clist.CopyTo(array, -1), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="arrayEnumerable"></param>
        /// <param name="arrayIndex"></param>
        /// <remarks>There are two issues with this test (and other tests in this fixture; I won't
        /// repeat this disclaimer all over the test fixture): (1) you cannot specify an array in an
        /// attribute without some array initializer (the implicit initialization of e.g.
        /// new int[10] is rejected), and (2) the NUnit test runner (or something) crashes when
        /// using array parameters, so use an enumerable parameter instead.</remarks>
        [TestCase(new int[] { 123 }, new int[0], 0)]
        [TestCase(new int[] { 123 }, new int[1] { 0 }, 1)]
        [TestCase(new int[] { 123 }, new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 10)]
        [TestCase(new int[] { 1, 2, 3, 4 }, new int[4] { 0, 0, 0, 0 }, 1)]
        [TestCase(new int[] { 1, 2, 3, 4 }, new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 7)]
        public void CopyTo_ThrowsArgumentOutOfRangeException_OnTooLittleSpaceLeft<T>(IEnumerable<T> collection, IEnumerable<T> arrayEnumerable, int arrayIndex)
        {
            var array = arrayEnumerable.ToArray();
            var clist = new CircularList<T>(collection);
            Assert.That(() => clist.CopyTo(array, arrayIndex), Throws.InstanceOf<ArgumentException>());
        }

        [TestCase(new int[] { }, new int[2] { 0, 0 }, 0, new int[2] { 0, 0 })]
        [TestCase(new int[] { 1, 2, 3 }, new int[3] { 0, 0, 0 }, 0, new int[3] { 1, 2, 3 })]
        [TestCase(new int[] { 1, 2, 3 }, new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 0, new int[10] { 1, 2, 3, 0, 0, 0, 0, 0, 0, 0 })]
        [TestCase(new int[] { 1, 2, 3 }, new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 2, new int[10] { 0, 0, 1, 2, 3, 0, 0, 0, 0, 0 })]
        [TestCase(new int[] { 1, 2, 3 }, new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 7, new int[10] { 0, 0, 0, 0, 0, 0, 0, 1, 2, 3 })]
        public void CopyTo_AfterConstruction_TypicalCase<T>(IEnumerable<T> collection, IEnumerable<T> arrayEnumerable, int arrayIndex, IEnumerable<T> expected)
        {
            var array = arrayEnumerable.ToArray();
            var clist = new CircularList<T>(collection);
            clist.CopyTo(array, arrayIndex);
            Assert.That(array, Is.EqualTo(expected));
        }

        [TestCase(1, new int[] { 1, 2, 3 }, new int[3] { 0, 0, 0 }, 0, new int[3] { 2, 3, 1 })]
        [TestCase(1, new int[] { 1, 2, 3 }, new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 0, new int[10] { 2, 3, 1, 0, 0, 0, 0, 0, 0, 0 })]
        [TestCase(1, new int[] { 1, 2, 3 }, new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 2, new int[10] { 0, 0, 2, 3, 1, 0, 0, 0, 0, 0 })]
        [TestCase(1, new int[] { 1, 2, 3 }, new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 7, new int[10] { 0, 0, 0, 0, 0, 0, 0, 2, 3, 1 })]
        public void CopyTo_AfterLeftRotation_TypicalCase<T>(int count, IEnumerable<T> collection, IEnumerable<T> arrayEnumerable, int arrayIndex, IEnumerable<T> expected)
        {
            var array = arrayEnumerable.ToArray();
            var clist = new CircularList<T>(collection);
            clist.RotateLeft(count);
            clist.CopyTo(array, arrayIndex);
            Assert.That(array, Is.EqualTo(expected));
        }

        [TestCase(1, new int[] { 1, 2, 3 }, new int[3] { 0, 0, 0 }, 0, new int[3] { 3, 1, 2 })]
        [TestCase(1, new int[] { 1, 2, 3 }, new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 0, new int[10] { 3, 1, 2, 0, 0, 0, 0, 0, 0, 0 })]
        [TestCase(1, new int[] { 1, 2, 3 }, new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 2, new int[10] { 0, 0, 3, 1, 2, 0, 0, 0, 0, 0 })]
        [TestCase(1, new int[] { 1, 2, 3 }, new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 7, new int[10] { 0, 0, 0, 0, 0, 0, 0, 3, 1, 2 })]
        public void CopyTo_AfterRightRotation_TypicalCase<T>(int count, IEnumerable<T> collection, IEnumerable<T> arrayEnumerable, int arrayIndex, IEnumerable<T> expected)
        {
            var array = arrayEnumerable.ToArray();
            var clist = new CircularList<T>(collection);
            clist.RotateRight(count);
            clist.CopyTo(array, arrayIndex);
            Assert.That(array, Is.EqualTo(expected));
        }

        [TestCase(new int[] { 1 }, new int[] { 123 }, new int[] { 1 })]
        [TestCase(new int[] { 1 }, new int[] { 1 }, new int[] { })]
        [TestCase(new int[] { 1, 2, 3, 4, 5 }, new int[] { 1 }, new int[] { 2, 3, 4, 5 })]
        [TestCase(new int[] { 1, 2, 3, 4, 5 }, new int[] { 3 }, new int[] { 1, 2, 4, 5 })]
        [TestCase(new int[] { 1, 2, 3, 4, 5 }, new int[] { 5 }, new int[] { 1, 2, 3, 4 })]
        [TestCase(new int[] { 1, 2, 3, 4, 5 }, new int[] { 1, 2, 3 }, new int[] { 4, 5 })]
        [TestCase(new int[] { 1, 2, 3, 4, 5 }, new int[] { 3, 4, 5 }, new int[] { 1, 2 })]
        [TestCase(new int[] { 1, 2, 3, 4, 5 }, new int[] { 5, 1, 3 }, new int[] { 2, 4 })]
        [TestCase(new int[] { 1, 2, 3, 4, 5 }, new int[] { 1, 2, 3, 4, 5 }, new int[] { })]
        [TestCase(new int[] { 1, 2, 3, 4, 5 }, new int[] { 5, 4, 3, 2, 1 }, new int[] { })]
        [TestCase(new int[] { 1, 2, 1, 4, 1 }, new int[] { 1, 1 }, new int[] { 2, 4, 1 })]
        public void Remove_AfterConstruction<T>(IEnumerable<T> collection, IEnumerable<T> itemsToRemove, IEnumerable<T> expected)
        {
            var clist = new CircularList<T>(collection);
            foreach (var item in itemsToRemove) clist.Remove(item);
            Assert.That(clist, Is.EqualTo(expected));
        }

        [TestCase(1, new int[] { 1, 2, 3, 4, 5 }, new int[] { 1 }, new int[] { 2, 3, 4, 5 })]
        [TestCase(1, new int[] { 1, 2, 3, 4, 5 }, new int[] { 3 }, new int[] { 2, 4, 5, 1 })]
        [TestCase(1, new int[] { 1, 2, 3, 4, 5 }, new int[] { 5 }, new int[] { 2, 3, 4, 1 })]
        [TestCase(1, new int[] { 1, 2, 3, 4, 5 }, new int[] { 1, 2, 3 }, new int[] { 4, 5 })]
        [TestCase(1, new int[] { 1, 2, 3, 4, 5 }, new int[] { 3, 4, 5 }, new int[] { 2, 1 })]
        [TestCase(1, new int[] { 1, 2, 3, 4, 5 }, new int[] { 5, 1, 3 }, new int[] { 2, 4 })]
        [TestCase(1, new int[] { 1, 2, 3, 4, 5 }, new int[] { 1, 2, 3, 4, 5 }, new int[] { })]
        [TestCase(1, new int[] { 1, 2, 3, 4, 5 }, new int[] { 5, 4, 3, 2, 1 }, new int[] { })]
        [TestCase(1, new int[] { 1, 2, 1, 4, 1 }, new int[] { 1, 1 }, new int[] { 2, 4, 1 })]
        public void Remove_AfterLeftRotation<T>(int count, IEnumerable<T> collection, IEnumerable<T> itemsToRemove, IEnumerable<T> expected)
        {
            var clist = new CircularList<T>(collection);
            clist.RotateLeft(count);
            foreach (var item in itemsToRemove) clist.Remove(item);
            Assert.That(clist, Is.EqualTo(expected));
        }

        [TestCase(1, new int[] { 1, 2, 3, 4, 5 }, new int[] { 1 }, new int[] { 5, 2, 3, 4 })]
        [TestCase(1, new int[] { 1, 2, 3, 4, 5 }, new int[] { 3 }, new int[] { 5, 1, 2, 4 })]
        [TestCase(1, new int[] { 1, 2, 3, 4, 5 }, new int[] { 5 }, new int[] { 1, 2, 3, 4 })]
        [TestCase(1, new int[] { 1, 2, 3, 4, 5 }, new int[] { 1, 2, 3 }, new int[] { 5, 4 })]
        [TestCase(1, new int[] { 1, 2, 3, 4, 5 }, new int[] { 3, 4, 5 }, new int[] { 1, 2 })]
        [TestCase(1, new int[] { 1, 2, 3, 4, 5 }, new int[] { 5, 1, 3 }, new int[] { 2, 4 })]
        [TestCase(1, new int[] { 1, 2, 3, 4, 5 }, new int[] { 1, 2, 3, 4, 5 }, new int[] { })]
        [TestCase(1, new int[] { 1, 2, 3, 4, 5 }, new int[] { 5, 4, 3, 2, 1 }, new int[] { })]
        [TestCase(1, new int[] { 1, 2, 1, 4, 1 }, new int[] { 1, 1 }, new int[] { 2, 1, 4 })]
        public void Remove_AfterRightRotation<T>(int count, IEnumerable<T> collection, IEnumerable<T> itemsToRemove, IEnumerable<T> expected)
        {
            var clist = new CircularList<T>(collection);
            clist.RotateRight(count);
            foreach (var item in itemsToRemove) clist.Remove(item);
            Assert.That(clist, Is.EqualTo(expected));
        }

        [TestCase(new int[] { }, 1, -1)]
        [TestCase(new int[] { 123 }, 1, -1)]
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6 }, 123, -1)]
        [TestCase(new int[] { 1 }, 1, 0)]
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6 }, 1, 0)]
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6 }, 3, 2)]
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6 }, 6, 5)]
        [TestCase(new int[] { 1, 2, 3, 1, 5, 6 }, 1, 0)]
        [TestCase(new int[] { 1, 2, 5, 4, 5, 6 }, 5, 2)]
        public void IndexOf_AfterConstruction<T>(IEnumerable<T> collection, T item, int expected)
        {
            var clist = new CircularList<T>(collection);
            Assert.That(clist.IndexOf(item), Is.EqualTo(expected));
        }

        [TestCase(1, new int[] { 1, 2, 3, 4, 5, 6 }, 123, -1)]
        [TestCase(1, new int[] { 1, 2, 3, 4, 5, 6 }, 1, 5)]
        [TestCase(1, new int[] { 1, 2, 3, 4, 5, 6 }, 3, 1)]
        [TestCase(1, new int[] { 1, 2, 3, 4, 5, 6 }, 6, 4)]
        [TestCase(1, new int[] { 1, 2, 3, 1, 5, 6 }, 1, 2)]
        [TestCase(1, new int[] { 1, 2, 5, 4, 5, 6 }, 5, 1)]
        public void IndexOf_AfterLeftRotation<T>(int count, IEnumerable<T> collection, T item, int expected)
        {
            var clist = new CircularList<T>(collection);
            clist.RotateLeft(count);
            Assert.That(clist.IndexOf(item), Is.EqualTo(expected));
        }

        [TestCase(1, new int[] { 1, 2, 3, 4, 5, 6 }, 123, -1)]
        [TestCase(1, new int[] { 1, 2, 3, 4, 5, 6 }, 1, 1)]
        [TestCase(1, new int[] { 1, 2, 3, 4, 5, 6 }, 3, 3)]
        [TestCase(1, new int[] { 1, 2, 3, 4, 5, 6 }, 6, 0)]
        [TestCase(1, new int[] { 1, 2, 3, 1, 5, 6 }, 1, 1)]
        [TestCase(1, new int[] { 1, 2, 5, 4, 5, 6 }, 5, 3)]
        public void IndexOf_AfterRightRotation<T>(int count, IEnumerable<T> collection, T item, int expected)
        {
            var clist = new CircularList<T>(collection);
            clist.RotateRight(count);
            Assert.That(clist.IndexOf(item), Is.EqualTo(expected));
        }

        [Test]
        public void Insert_ThrowsInvalidOperationException_WhenListIsEmpty()
        {
            var clist = new CircularList<string>();
            Assert.That(() => clist.Insert(0, "abc"), Throws.InstanceOf<InvalidOperationException>());
        }

        [TestCase(new int[] { 123 }, new int[] { 0 }, new int[] { 234 }, new int[] { 234, 123 })]
        [TestCase(new int[] { 123 }, new int[] { 1 }, new int[] { 234 }, new int[] { 234, 123 })] // This is peculiar but the way it works
        [TestCase(new int[] { 123, 234 }, new int[] { 0 }, new int[] { 345 }, new int[] { 345, 123, 234 })]
        [TestCase(new int[] { 123, 234 }, new int[] { 1 }, new int[] { 345 }, new int[] { 123, 345, 234 })]
        [TestCase(new int[] { 123, 234 }, new int[] { 2 }, new int[] { 345 }, new int[] { 345, 123, 234 })] // This is pecular but the way it works
        public void Insert_AfterConstruction<T>(IEnumerable<T> collection, IEnumerable<int> indices, IEnumerable<T> items, IEnumerable<T> expected)
        {
            var clist = new CircularList<T>(collection);
            var insertData = indices.Zip(items, (x, y) => (x, y));
            foreach ((int index, T item) in insertData) clist.Insert(index, item);
            Assert.That(clist, Is.EqualTo(expected));
        }

        [TestCase(1, new int[] { 123, 234 }, new int[] { 0 }, new int[] { 345 }, new int[] { 345, 234, 123 })]
        [TestCase(1, new int[] { 123, 234 }, new int[] { 1 }, new int[] { 345 }, new int[] { 234, 345, 123 })]
        [TestCase(1, new int[] { 123, 234 }, new int[] { 2 }, new int[] { 345 }, new int[] { 345, 234, 123 })] // This is pecular but the way it works
        [TestCase(2, new int[] { 123, 234 }, new int[] { 0 }, new int[] { 345 }, new int[] { 345, 123, 234 })]
        [TestCase(2, new int[] { 123, 234 }, new int[] { 1 }, new int[] { 345 }, new int[] { 123, 345, 234 })]
        [TestCase(2, new int[] { 123, 234 }, new int[] { 2 }, new int[] { 345 }, new int[] { 345, 123, 234 })] // This is pecular but the way it works
        [TestCase(1, new int[] { 123, 234, 345 }, new int[] { 0 }, new int[] { 456 }, new int[] { 456, 234, 345, 123 })]
        [TestCase(1, new int[] { 123, 234, 345 }, new int[] { 1 }, new int[] { 456 }, new int[] { 234, 456, 345, 123 })]
        [TestCase(1, new int[] { 123, 234, 345 }, new int[] { 2 }, new int[] { 456 }, new int[] { 234, 345, 456, 123 })]
        public void Insert_AfterLeftRotation<T>(int count, IEnumerable<T> collection, IEnumerable<int> indices, IEnumerable<T> items, IEnumerable<T> expected)
        {
            var clist = new CircularList<T>(collection);
            clist.RotateLeft(count);
            var insertData = indices.Zip(items, (x, y) => (x, y));
            foreach ((int index, T item) in insertData) clist.Insert(index, item);
            Assert.That(clist, Is.EqualTo(expected));
        }

        [TestCase(1, new int[] { 123, 234 }, new int[] { 0 }, new int[] { 345 }, new int[] { 345, 234, 123 })]
        [TestCase(1, new int[] { 123, 234 }, new int[] { 1 }, new int[] { 345 }, new int[] { 234, 345, 123 })]
        [TestCase(1, new int[] { 123, 234 }, new int[] { 2 }, new int[] { 345 }, new int[] { 345, 234, 123 })] // This is pecular but the way it works
        [TestCase(2, new int[] { 123, 234 }, new int[] { 0 }, new int[] { 345 }, new int[] { 345, 123, 234 })]
        [TestCase(2, new int[] { 123, 234 }, new int[] { 1 }, new int[] { 345 }, new int[] { 123, 345, 234 })]
        [TestCase(2, new int[] { 123, 234 }, new int[] { 2 }, new int[] { 345 }, new int[] { 345, 123, 234 })] // This is pecular but the way it works
        [TestCase(1, new int[] { 123, 234, 345 }, new int[] { 0 }, new int[] { 456 }, new int[] { 456, 345, 123, 234 })]
        [TestCase(1, new int[] { 123, 234, 345 }, new int[] { 1 }, new int[] { 456 }, new int[] { 345, 456, 123, 234 })]
        [TestCase(1, new int[] { 123, 234, 345 }, new int[] { 2 }, new int[] { 456 }, new int[] { 345, 123, 456, 234 })]
        public void Insert_AfterRightRotation<T>(int count, IEnumerable<T> collection, IEnumerable<int> indices, IEnumerable<T> items, IEnumerable<T> expected)
        {
            var clist = new CircularList<T>(collection);
            clist.RotateRight(count);
            var insertData = indices.Zip(items, (x, y) => (x, y));
            foreach ((int index, T item) in insertData) clist.Insert(index, item);
            Assert.That(clist, Is.EqualTo(expected));
        }

        [Test]
        public void RemoveAt_ThrowsInvalidOperationException_WhenListIsEmpty()
        {
            var clist = new CircularList<int>();
            Assert.That(() => clist.RemoveAt(0), Throws.InstanceOf<InvalidOperationException>());
            Assert.That(() => clist.RemoveAt(-1), Throws.InstanceOf<InvalidOperationException>());
            Assert.That(() => clist.RemoveAt(1), Throws.InstanceOf<InvalidOperationException>());
            Assert.That(() => clist.RemoveAt(4), Throws.InstanceOf<InvalidOperationException>());
        }

        [TestCase(new int[] { 1 }, new int[] { 0 }, new int[] { })]
        [TestCase(new int[] { 1 }, new int[] { -1 }, new int[] { })]
        [TestCase(new int[] { 1 }, new int[] { 1 }, new int[] { })]
        [TestCase(new int[] { 1 }, new int[] { 8 }, new int[] { })]
        [TestCase(new int[] { 1, 2 }, new int[] { 0 }, new int[] { 2 })]
        [TestCase(new int[] { 1, 2 }, new int[] { 1 }, new int[] { 1 })]
        [TestCase(new int[] { 1, 2 }, new int[] { 2 }, new int[] { 2 })]
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6, 7 }, new int[] { 0 }, new int[] { 2, 3, 4, 5, 6, 7 })]
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6, 7 }, new int[] { -1 }, new int[] { 1, 2, 3, 4, 5, 6 })]
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6, 7 }, new int[] { 0, 0, 0 }, new int[] { 4, 5, 6, 7 })]
        [TestCase(new int[] { 1, 2, 3, 4, 5, 6, 7 }, new int[] { 6, 4, 2, 0 }, new int[] { 2, 4, 6 })]
        public void RemoveAt_AfterConstruction_TypicalCase<T>(IEnumerable<T> collection, IEnumerable<int> indices, IEnumerable<T> expected)
        {
            var clist = new CircularList<T>(collection);
            foreach (var index in indices) clist.RemoveAt(index);
            Assert.That(clist, Is.EqualTo(expected));
        }

        [TestCase(1, new int[] { 1, 2 }, new int[] { 0 }, new int[] { 1 })]
        [TestCase(1, new int[] { 1, 2 }, new int[] { 1 }, new int[] { 2 })]
        [TestCase(1, new int[] { 1, 2 }, new int[] { 2 }, new int[] { 1 })]
        [TestCase(1, new int[] { 1, 2, 3, 4, 5, 6, 7 }, new int[] { 0 }, new int[] { 3, 4, 5, 6, 7, 1 })]
        [TestCase(1, new int[] { 1, 2, 3, 4, 5, 6, 7 }, new int[] { -1 }, new int[] { 2, 3, 4, 5, 6, 7 })]
        [TestCase(1, new int[] { 1, 2, 3, 4, 5, 6, 7 }, new int[] { 0, 0, 0 }, new int[] { 5, 6, 7, 1 })]
        [TestCase(1, new int[] { 1, 2, 3, 4, 5, 6, 7 }, new int[] { 6, 4, 2, 0 }, new int[] { 3, 5, 7 })]
        public void RemoveAt_AfterLeftRotation_TypicalCase<T>(int count, IEnumerable<T> collection, IEnumerable<int> indices, IEnumerable<T> expected)
        {
            var clist = new CircularList<T>(collection);
            clist.RotateLeft(count);
            foreach (var index in indices) clist.RemoveAt(index);
            Assert.That(clist, Is.EqualTo(expected));
        }

        [TestCase(1, new int[] { 1, 2 }, new int[] { 0 }, new int[] { 1 })]
        [TestCase(1, new int[] { 1, 2 }, new int[] { 1 }, new int[] { 2 })]
        [TestCase(1, new int[] { 1, 2 }, new int[] { 2 }, new int[] { 1 })]
        [TestCase(1, new int[] { 1, 2, 3, 4, 5, 6, 7 }, new int[] { 0 }, new int[] { 1, 2, 3, 4, 5, 6 })]
        [TestCase(1, new int[] { 1, 2, 3, 4, 5, 6, 7 }, new int[] { -1 }, new int[] { 7, 1, 2, 3, 4, 5 })]
        [TestCase(1, new int[] { 1, 2, 3, 4, 5, 6, 7 }, new int[] { 0, 0, 0 }, new int[] { 3, 4, 5, 6 })]
        [TestCase(1, new int[] { 1, 2, 3, 4, 5, 6, 7 }, new int[] { 6, 4, 2, 0 }, new int[] { 1, 3, 5 })]
        public void RemoveAt_AfterRightRotation_TypicalCase<T>(int count, IEnumerable<T> collection, IEnumerable<int> indices, IEnumerable<T> expected)
        {
            var clist = new CircularList<T>(collection);
            clist.RotateRight(count);
            foreach (var index in indices) clist.RemoveAt(index);
            Assert.That(clist, Is.EqualTo(expected));
        }
    }
}

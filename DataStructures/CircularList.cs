using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures
{
    /// <summary>
    /// This class represents a circular list (&quot;list&quot; not in the sense of linked lists
    /// but rather in the sense of <see cref="List{T}"/>, which is really more of a dynamic array).
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <remarks>
    /// <para>Compared to <see cref="List{T}"/>, an index weakly greater than <see cref="Count"/>
    /// wraps around to the start of the list instead of being out of range. Negative indices wrap
    /// around to the end of the list in a similar way.</para>
    /// </remarks>
    public class CircularList<T> : IList<T>, ICollection<T>, IEnumerable<T>
    {
        /// <summary>
        /// The underlying list.
        /// </summary>
        private List<T> list;

        /// <summary>
        /// The index in <see cref="list"/> of the first element in the circular list.
        /// </summary>
        /// <remarks>
        /// <para>If the list is empty, this value is 0. Otherwise, this value is always between
        /// 0 (inclusive) and <see cref="Count"/> (exclusive).</para></remarks>
        private int firstIndex;

        /// <summary>
        /// Gets the actual index into the underlying list <see cref="list"/>.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <remarks>This method assumes that the list is non-empty.</remarks>
        private int GetActualIndex(int index)
        {
            return (firstIndex + index).Modulo(Count);
        }

        private void NormalizeFirstIndex()
        {
            firstIndex = firstIndex.Modulo(Count);
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at index <paramref name="index"/>.</returns>
        /// <exception cref="InvalidOperationException">The list is empty.</exception>
        public T this[int index]
        {
            get
            {
                if (Count == 0) throw new InvalidOperationException("The list is empty.");
                return list[GetActualIndex(index)];
            }
            set
            {
                if (Count == 0) throw new InvalidOperationException("The list is empty.");
                list[GetActualIndex(index)] = value;
            }
        }

        /// <inheritdoc/>
        public int Count { get => list.Count; }

        public bool IsReadOnly => false;

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularList{T}"/> class that contains no
        /// elements.
        /// </summary>
        public CircularList() : this(new T[0]) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularList{T}"/> class that contains
        /// elements copied from the specified collection.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
        public CircularList(IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            list = new List<T>(collection);
            firstIndex = 0;
        }

        #region CircularList<T> methods
        /// <summary>
        /// Rotates the list to the left, decreasing the indices of elements until they wrap around.
        /// </summary>
        /// <param name="count">The number of elements to rotate by.</param>
        public void RotateLeft(int count)
        {
            if (Count == 0) return;

            firstIndex += count;
            NormalizeFirstIndex();
        }

        /// <summary>
        /// Rotates the list to the right, increasing indices of elements until they wrap around.
        /// </summary>
        /// <param name="count">The number of elements to rotate by.</param>
        public void RotateRight(int count)
        {
            if (Count == 0) return;

            firstIndex -= count;
            NormalizeFirstIndex();
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of the list.
        /// </summary>
        /// <param name="collection">The collection whose elements should be added to the end of
        /// the list.</param>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
        public void AddRange(IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            // An optimization would be to insert at Count if firstIndex == 0 and not modify the index
            list.InsertRange(firstIndex, collection);
            firstIndex += collection.Count();
            // Normalization not necessary
        }

        /// <summary>
        /// Gets a range of elements in the list, as an ordinary non-circular list.
        /// </summary>
        /// <param name="index">The zero-based index at which the range starts.</param>
        /// <param name="count">The number of elements in the range.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative, or
        /// <paramref name="count"/> is greater than <see cref="Count"/>.</exception>
        /// <exception cref="InvalidOperationException">The list is empty.</exception>
        /// Design consideration: Skip the InvalidOperationException; *some* exception is thrown
        /// anyway, except in the case count == 0 and Count == 0, in which case one could return
        /// an empty list.
        public IList<T> GetRange(int index, int count)
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (count > Count) throw new ArgumentOutOfRangeException(nameof(count));
            if (Count == 0) throw new InvalidOperationException("The list is empty.");

            // Handle the case count == Count specially, because then (the first index) == lastIndex
            // In hindsight, it might be cleaner to have a LINQ implementation for the other case too
            // (and then perhaps not have to distinguish between the two cases)
            if (count == Count)
            {
                int startIndex = GetActualIndex(index);
                return list.Skip(startIndex).Concat(list.Take(startIndex)).ToList();
            }

            var output = new List<T>();
            int lastIndex = (GetActualIndex(index) + count).Modulo(Count); // exclusive
            for (int actualIndex = GetActualIndex(index); actualIndex != lastIndex; actualIndex = (actualIndex+1).Modulo(Count))
            {
                output.Add(list[actualIndex]);
            }

            return output;
        }

        /// <summary>
        /// Inserts the elements of a collection into the list at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the new elements should be inserted.</param>
        /// <param name="collection">The collection whose elements should be inserted into the list.</param>
        /// <remarks>
        /// <para>This method cannot be used to add elements at the end of the list, unlike for
        /// instance <see cref="List{T}.InsertRange(int, IEnumerable{T})"/>.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">The list is empty.</exception>
        public void InsertRange(int index, IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (Count == 0) throw new InvalidOperationException("The list is empty.");

            int actualIndex = GetActualIndex(index);
            list.InsertRange(actualIndex, collection);
            if (actualIndex < firstIndex) firstIndex += collection.Count();
            // Normalization not necessary
        }

        /// <summary>
        /// Removes a range of elements from the list.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range of elements to remove.</param>
        /// <param name="count">The number of elements to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative, or
        /// <paramref name="count"/> is greater than <see cref="Count"/>.</exception>
        /// <exception cref="InvalidOperationException">The list is empty.</exception>
        /// Design consideration: Skip the InvalidOperationException; *some* exception is thrown
        /// anyway, except in the case count == 0 and Count == 0, which could be a no-op.
        public void RemoveRange(int index, int count)
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (count > Count) throw new ArgumentOutOfRangeException(nameof(count));
            if (Count == 0) throw new InvalidOperationException("The list is empty.");

            int actualStartIndex1 = GetActualIndex(index);
            int countToEndOfList = list.Count - actualStartIndex1;
            int count1 = Math.Min(count, countToEndOfList); // count for the first removal (end of the list)
            int actualEndIndex1 = actualStartIndex1 + count1;

            // First removal
            list.RemoveRange(actualStartIndex1, count1);

            // If firstIndex is below the removed range, do nothing
            // If firstIndex is in the removed range, shift it down a little
            // If firstIndex is above the removed range, shift it down by all of count
            if (actualStartIndex1 <= firstIndex && firstIndex < actualEndIndex1) firstIndex -= (firstIndex - actualStartIndex1);
            else if (actualEndIndex1 <= firstIndex) firstIndex -= count1;

            int count2 = count - count1;
            if (count2 > 0)
            {
                int actualStartIndex2 = 0;
                int actualEndIndex2 = actualStartIndex2 + count2;
                list.RemoveRange(actualStartIndex2, count2);

                // If firstIndex is below the removed range, do nothing
                // If firstIndex is in the removed range, shift it down a little
                // If firstIndex is above the removed range, shift it down by all of count
                if (actualStartIndex2 <= firstIndex && firstIndex < actualEndIndex2) firstIndex -= (firstIndex - actualStartIndex2);
                else if (actualEndIndex2 <= firstIndex) firstIndex -= count2;
            }
        }

        /// <summary>
        /// Replaces a range of elements in the list with the elements of a specified collection.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range of elements to remove.</param>
        /// <param name="count">The number of elements to remove.</param>
        /// <param name="collection">The collection whose elements should be inserted into the list.</param>
        /// <remarks>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative, or
        /// <paramref name="count"/> is greater than <see cref="Count"/>.</exception>
        /// <exception cref="InvalidOperationException">The list is empty.</exception>
        public void ReplaceRange(int index, int count, IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (count > Count) throw new ArgumentOutOfRangeException(nameof(count));
            if (Count == 0) throw new InvalidOperationException("The list is empty.");

            // If we reach the end of the list (even without wrapping around), add the new elements to the end of the list (with AddRange)
            index = index.Modulo(Count);
            bool shouldAddElementsToEndOfList = (index + count >= Count);

            RemoveRange(index, count);
            if (shouldAddElementsToEndOfList) AddRange(collection);
            else InsertRange(index, collection);
        }
        #endregion


        #region IEnumerable<T>
        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            for (int index = firstIndex; index < Count; ++index)
            {
                yield return list[index];
            }

            for (int index = 0; index < firstIndex; ++index)
            {
                yield return list[index];
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region ICollection<T>
        /// <summary>
        /// Adds an item to the end of the list.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(T item)
        {
            // An optimization would be to insert at Count if firstIndex == 0 and not modify the index
            list.Insert(firstIndex, item);
            firstIndex += 1;
        }

        /// <inheritdoc/>
        public void Clear()
        {
            list.Clear();
            firstIndex = 0;
        }

        /// <inheritdoc/>
        public bool Contains(T item)
        {
            return list.Contains(item);
        }

        /// <inheritdoc/>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            int spaceInArray = array.Length - arrayIndex;
            if (Count > spaceInArray) throw new ArgumentException($"The number of elements to copy, {Count}, is greater than the available space, {spaceInArray}, at the end of the array.");

            // This might be inefficient (hopefully more efficient than using this.ToArray() and Array.Copy(...) or so at least)
            int numElementsAtEndOfList = list.Count - firstIndex;
            list.GetRange(firstIndex, numElementsAtEndOfList).CopyTo(array, arrayIndex);
            list.GetRange(0, firstIndex).CopyTo(array, arrayIndex + numElementsAtEndOfList);
        }

        /// <inheritdoc/>
        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index == -1) return false;
            RemoveAt(index);
            return true;
        }
        #endregion

        #region IList<T>
        /// <summary>
        /// Determines the index of a specified item in the <see cref="CircularList{T}"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="CircularList{T}"/>.</param>
        /// <returns>The index of <paramref name="item"/> if found in the list; otherwise, -1.</returns>
        /// <remarks>
        /// <para>If an item occurs multiple times in the list, the <see cref="IndexOf(T)"/> method
        /// always returns the index of the first instance found.</para>
        /// <para>If <paramref name="item"/> is found, the returned index is &quot;normalized&quot,
        /// i.e., in the range from 0 (inclusive) to <see cref="Count"/> (exclusive).</para>
        /// </remarks>
        public int IndexOf(T item)
        {
            int index = 0;
            foreach (var listItem in this)
            {
                if (item.Equals(listItem)) return index;
                index++;
            }

            return -1;
        }

        /// <summary>
        /// Inserts an item into the list at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be
        /// inserted.</param>
        /// <param name="item">The object to insert into the list.</param>
        /// <remarks>
        /// <para>This method cannot be used to add elements at the end of the list, unlike for
        /// instance <see cref="List{T}.Insert(int, T)"/>.</para>
        /// </remarks>
        /// <exception cref="InvalidOperationException">The list is empty.</exception>
        public void Insert(int index, T item)
        {
            if (Count == 0) throw new InvalidOperationException("The list is empty.");

            int actualIndex = GetActualIndex(index);
            list.Insert(actualIndex, item);
            if (actualIndex < firstIndex) firstIndex += 1;
        }

        /// <summary>
        /// Removes the list item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="InvalidOperationException">The list is empty.</exception>
        public void RemoveAt(int index)
        {
            if (Count == 0) throw new InvalidOperationException("The list is empty.");

            int actualIndex = GetActualIndex(index);
            list.RemoveAt(actualIndex);
            if (actualIndex < firstIndex) firstIndex -= 1;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataStructures
{
    /// <summary>
    /// <para>This data structure represents a collection of disjoint sets and is optimized for adding
    /// singletons (expanding the collection of elements), taking the (destructive) union of two
    /// disjoint sets, and determining which set an element belongs to.</para>
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <remarks>
    /// <para>Note to self: This is the disjoint-set data structure covered in AD2 and CLRS
    /// representing each set as a tree and the entire collection of disjoint sets as a forest of
    /// trees. More specifically, this implementation uses the heuristics union by rank and path
    /// compression on FindSet.</para>
    /// <para>This implementation differs from that covered by CLRS in that the elements are not
    /// assumed to be integers 0 to n-1. Instead, this implementation allows elements of any type
    /// by using a hash table (actually, a <see cref="Dictionary"/>).</para>
    /// <para>The methods are named as in CLRS, which uses names not really fitting for C#.</para>
    /// </remarks>
    public class DisjointSets<T>
    {
        /// <summary>
        /// Maps an element to its corresponding node.
        /// </summary>
        private Dictionary<T, DisjointSetsNode<T>> dict = new Dictionary<T, DisjointSetsNode<T>>();

        /// <summary>
        /// Gets the number of sets in the collection.
        /// </summary>
        public int NumberOfSets { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisjointSets{T}"/> class representing an
        /// empty collection of sets.
        /// </summary>
        public DisjointSets() : this(new HashSet<T>()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisjointSets{T}"/> class representing a
        /// collection of singletons.
        /// </summary>
        /// <param name="elements">The elements of the singletons.</param>
        /// <exception cref="ArgumentNullException"><paramref name="elements"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">The enumerable contains duplicates.</exception>
        /// TODO: Call the sets constructor overload for consistency?
        public DisjointSets(IEnumerable<T> elements)
        {
            if (elements == null)
            {
                throw new ArgumentNullException(nameof(elements));
            }

            NumberOfSets = 0;

            foreach (var element in elements)
            {
                try
                {
                    MakeSet(element);
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException($"The enumerable contains more than one instance of {element}.", nameof(elements), ex);
                }
            }
        }

        public DisjointSets(IEnumerable<ISet<T>> sets)
        {
            if (sets == null)
            {
                throw new ArgumentNullException(nameof(sets));
            }

            foreach (var set in sets)
            {
                if (set == null) throw new ArgumentException("The enumerable contains a null value.", nameof(sets));
                try
                {
                    MakeBigSet(set);
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException("The sets are not disjoint.", ex);
                }
            }
        }

        public DisjointSets(DisjointSets<T> otherDisjointSets) : this(otherDisjointSets?.GetSets() ?? throw new ArgumentNullException(nameof(otherDisjointSets)))
        { }

        /// <summary>
        /// Inserts a singleton into the collection of disjoint sets.
        /// </summary>
        /// <param name="x">The element of the singleton to insert.</param>
        /// <exception cref="ArgumentException"><paramref name="x"/> is already exists in the collection of disjoint sets.</exception>
        public void MakeSet(T x)
        {
            if (dict.ContainsKey(x))
            {
                throw new ArgumentException($"The element {x} already exists in the collection of disjoint sets.", nameof(x));
            }

            var node = new DisjointSetsNode<T>(x);
            dict[x] = node;
            NumberOfSets++;
        }

        /// <summary>
        /// Returns the representative from the unique set containing a given element.
        /// </summary>
        /// <param name="x">The element for whose set to find a representative.</param>
        /// <returns>The representative for the set containing <paramref name="x"/>.</returns>
        /// <remarks>The same representative is returned if the <see cref="DisjointSets{T}"/>
        /// collection is not modified between calls.</remarks>
        /// <exception cref="ArgumentException"><paramref name="x"/> does not exist in the collection of disjoint sets.</exception>
        public T FindSet(T x)
        {
            if (!dict.ContainsKey(x))
            {
                throw new ArgumentException($"The element {x} does not exist in the collection of disjoint sets", nameof(x));
            }

            return InternalFindSet(x).Element;
        }

        /// <summary>
        /// Returns the representative <em>node</em> from the unique set containing a given element.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        /// <remarks>This method assumes that <paramref name="x"/> is present in the collection of disjoint sets.</remarks>
        private DisjointSetsNode<T> InternalFindSet(T x)
        {
            var findPath = new List<DisjointSetsNode<T>>(); // For path compression
            var node = dict[x];
            while (!node.IsRoot)
            {
                findPath.Add(node);
                node = node.Parent;
            }

            var root = node;
            foreach (var nonrootNode in findPath)
            {
                nonrootNode.Parent = root;
            }

            return root;
        }

        /// <summary>
        /// Unites two disjoint sets.
        /// </summary>
        /// <param name="x">An element of the first set.</param>
        /// <param name="y">An element of the second set.</param>
        /// <remarks>Performs path compression.</remarks>
        /// <exception cref="ArgumentException"><paramref name="x"/> or <paramref name="y"/> does
        /// not exist in the collection of disjoint sets, or <paramref name="x"/> and <paramref name="y"/>
        /// are members of the same set.</exception>
        public void Union(T x, T y)
        {
            if (!dict.ContainsKey(x))
            {
                throw new ArgumentException($"The element {x} does not exist in the collection of disjoint sets", nameof(x));
            }

            if (!dict.ContainsKey(y))
            {
                throw new ArgumentException($"The element {y} does not exist in the collection of disjoint sets", nameof(y));
            }

            var xNode = InternalFindSet(x);
            var yNode = InternalFindSet(y);
            if (xNode == yNode)
            {
                throw new ArgumentException($"The elements {x} and {y} belong to the same set.");
            }

            Link(xNode, yNode);
            NumberOfSets--;
        }

        /// <summary>
        /// Unites two disjoint sets given the root elements of the sets.
        /// </summary>
        /// <param name="xNode">The root of (the tree representing) the first set.</param>
        /// <param name="yNode">The root of (the tree representing) the second set.</param>
        /// <remarks><paramref name="xNode"/> and <paramref name="yNode"/> are assumed to
        /// correspond to elements of different sets.</remarks>
        private void Link(DisjointSetsNode<T> xNode, DisjointSetsNode<T> yNode)
        {
            DisjointSetsNode<T> parentNode, childNode;
            if (xNode.Rank > yNode.Rank)
            {
                childNode = yNode;
                parentNode = xNode;
            }
            else
            {
                childNode = xNode;
                parentNode = yNode;
            }

            childNode.Parent = parentNode;
            if (childNode.Rank == parentNode.Rank)
            {
                parentNode.Rank += 1;
            }

            // Swap nexts
            var tempNode = parentNode.Next;
            parentNode.Next = childNode.Next;
            childNode.Next = tempNode;
        }

        /// <summary>
        /// Returns the unique set containing a given element.
        /// </summary>
        /// <param name="x">The element whose set to return.</param>
        /// <returns>The set containing <paramref name="x"/>.</returns>
        /// <remarks>This is essentially the implementation of Print-Set in CLRS hinted to in exercise 21.3-4.</remarks>
        /// <exception cref="ArgumentException"><paramref name="x"/> does not exist in the collection of disjoint sets.</exception>
        public ISet<T> GetSet(T x)
        {
            if (!dict.ContainsKey(x))
            {
                throw new ArgumentException($"The element {x} does not exist in the collection of disjoint sets", nameof(x));
            }

            var startNode = dict[x];
            var set = new HashSet<T>();
            var node = startNode;
            do
            {
                set.Add(node.Element);
            } while ((node = node.Next) != startNode);

            return set;
        }

        /// <summary>
        /// Indicates whether any of the disjoint sets contain a specified element.
        /// </summary>
        /// <param name="x">The element whose existence to determine.</param>
        /// <returns><see langword="true"/> if <paramref name="x"/> is present; <see langword="false"/> otherwise.</returns>
        public bool Contains(T x)
        {
            return dict.ContainsKey(x);
        }

        /// <summary>
        /// Returns this collection of disjoint sets as a set of sets.
        /// </summary>
        /// <returns>This collection of disjoint sets as a set of sets.</returns>
        /// <remarks>This implementation is naïve.</remarks>
        public ISet<ISet<T>> GetSets()
        {
            var setOfSets = new HashSet<ISet<T>>();
            foreach (var x in dict.Keys.Select(x => FindSet(x)).Distinct())
            {
                var set = GetSet(x);
                setOfSets.Add(set);
            }

            return setOfSets;
        }

        public void MakeBigSet(IEnumerable<T> xs)
        {
            var enumerator = xs.GetEnumerator();
            if (!enumerator.MoveNext()) return;
            var x0 = enumerator.Current;
            MakeSet(x0);
            while (enumerator.MoveNext())
            {
                var x = enumerator.Current;
                MakeSet(x);
                Union(x, x0);
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder("{");
            bool deleteTrailingComma = false;
            foreach (var set in GetSets())
            {
                builder.Append("{");
                builder.Append(String.Join(", ", set));
                builder.Append("}, ");
                deleteTrailingComma = true;
            }

            if (deleteTrailingComma) builder.Length -= 2;
            builder.Append("}");
            return builder.ToString();
        }
    }
}

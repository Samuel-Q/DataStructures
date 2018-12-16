using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures
{
    /// <summary>
    /// This class represents an entry in a <see cref="DisjointSets{T}"/> structure. It is really
    /// just a node with some data and a pointer to its unique parent in the 
    /// </summary>
    /// <typeparam name="T">The type of the elements in the <see cref="DisjointSets{T}"/>.</typeparam>
    internal class DisjointSetsNode<T>
    {
        public T Element { get; private set; }

        /// <summary>
        /// The parent node, or <see langword="null"/> if this node is a root.
        /// </summary>
        public DisjointSetsNode<T> Parent { get; internal set; }

        public bool IsRoot { get { return Parent == null; } }

        /// <summary>
        /// An upper bound of the height of the tree rooted at this node. In other words, an upper
        /// bound of the number of edges in the longest (simple) path from this node to a leaf in
        /// said tree.
        /// </summary>
        public int Rank { get; internal set; }

        /// <summary>
        /// A pointer to the next node in the tree that together with the <see cref="Next"/> field
        /// of the other nodes form a circular linked list containing all the nodes in the set.
        /// </summary>
        /// <remarks>This property is never <see langword="null"/>.</remarks>
        public DisjointSetsNode<T> Next { get; internal set; }

        public DisjointSetsNode(T x)
        {
            Element = x;
            Parent = null;
            Rank = 0;
            Next = this;
        }

        public override string ToString()
        {
            if (Parent == null) return String.Format("(Element: {0}, Parent: null, Rank: {1}, Next: {2})", Element, Rank, Next.Element);
            else return String.Format("(Element: {0}, Parent element: {1}, Rank: {2}, Next: {3})", Element, Parent.Element, Rank, Next.Element);
        }
    }
}

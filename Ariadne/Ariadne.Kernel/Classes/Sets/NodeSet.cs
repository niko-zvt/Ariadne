using System;
using System.Collections;
using System.Collections.Generic;

namespace Ariadne.Kernel
{
    /// <summary>
    /// Class for storing a generalized set of nodes.
    /// A generalized set is needed to hide a specific implementation of the object storage mechanism. 
    /// The source mechanism can be an array, a list, a tree, or other structure.
    /// </summary>
    public class NodeSet : IEnumerable
    {
        /// <summary>
        /// Specific storage collection
        /// </summary>
        private Dictionary<int, Node> _collection;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public NodeSet()
        {
            _collection = new Dictionary<int, Node>();
        }

        /// <summary>
        /// The method returns the node according to its ID
        /// </summary>
        /// <param name="id">Node ID</param>
        /// <returns>Specific node</returns>
        public Node GetByID(int id)
        {
            return _collection.TryGetValue(id, out Node value) ? value : null;
        }

        /// <summary>
        /// The method returns the nodes according to its IDs
        /// </summary>
        /// <param name="ids">Node IDs</param>
        /// <returns>Specific nodes</returns>
        public NodeSet GetByIDs(IntSet ids)
        {
            var list = new NodeSet();
            foreach (int id in ids)
            {
                var item = _collection.TryGetValue(id, out Node value) ? value : null;
                list.Add(id, item);
            }
            return list;
        }

        public List<Math.Vector3D> GetAllCoords()
        {
            var list = new List<Math.Vector3D>();
            foreach(int id in _collection.Keys)
            {
                var item = _collection.TryGetValue(id, out Node value) ? value : null;
                list.Add(new Math.Vector3D ( item.Coords.X, item.Coords.Y, item.Coords.Z ));
            }
            return list;
        }

        /// <summary>
        /// The method returns the node according to its ordinal index in the collection
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>Specific node</returns>
        public Node GetByIndex(int index)
        {
            if (index < 0 || index >= _collection.Count)
                return null;

            int i = 0;
            foreach (var item in _collection)
            {
                if (i == index)
                    return item.Value;
                i++;
            }

            return null;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the Dictionary<int, Node>
        /// </summary>
        /// <returns>A Dictionary<int, Node>.Enumerator structure for the Dictionary<int, Node></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Implementation for the GetEnumerator method
        /// </summary>
        /// <returns>Node enumerator</returns>
        public NodeEnumerator GetEnumerator()
        {
            return new NodeEnumerator(_collection);
        }

        /// <summary>
        /// Adds the specified key and value to the collection
        /// </summary>
        /// <param name="id">Node ID</param>
        /// <param name="value">Specific node</param>
        public void Add(int id, Node value)
        {
            _collection.Add(id, value);
        }

        /// <summary>
        /// Indexer declaration
        /// </summary>
        /// <param name="id">Node ID</param>
        /// <returns>Specific node</returns>
        public Node this[int id]
        {
            get { return GetByID(id); }
            set { Add(id, value); }
        }

        /// <summary>
        /// Gets the number of ID/Nodes pairs contained in the collection
        /// </summary>
        public int Count
        {
            get { return _collection.Count; }
        }
    }

    /// <summary>
    /// Node enumerator class
    /// </summary>
    public class NodeEnumerator : IEnumerator
    {
        /// <summary>
        /// Array for enumeration
        /// </summary>
        private KeyValuePair<int, Node>[] _array;

        /// <summary>
        /// Position
        /// </summary>
        int position = -1;

        /// <summary>
        /// Сonstructor for enumerator
        /// </summary>
        /// <param name="dic">Input dictionary</param>
        public NodeEnumerator(Dictionary<int, Node> dic)
        {
            var list = new List<KeyValuePair<int, Node>>(dic);
            _array = list.ToArray();
        }

        /// <summary>
        /// Move to the next one
        /// </summary>
        /// <returns>Returns true if the position is less than the length of the array</returns>
        public bool MoveNext()
        {
            position++;
            return (position < _array.Length);
        }

        /// <summary>
        /// Reset of the position
        /// </summary>
        public void Reset()
        {
            position = -1;
        }

        /// <summary>
        /// Return the current enumerator
        /// </summary>
        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        /// <summary>
        /// Returns the current specific object
        /// </summary>
        public Node Current
        {
            get
            {
                try
                {
                    return _array[position].Value;
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
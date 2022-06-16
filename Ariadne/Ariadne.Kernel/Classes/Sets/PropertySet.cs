using System;
using System.Collections;
using System.Collections.Generic;

namespace Ariadne.Kernel
{
    /// <summary>
    /// Class for storing a generalized set of properties.
    /// A generalized set is needed to hide a specific implementation of the object storage mechanism. 
    /// The source mechanism can be an array, a list, a tree, or other structure.
    /// </summary>
    public class PropertySet : IEnumerable
    {
        /// <summary>
        /// Specific storage collection
        /// </summary>
        private Dictionary<int, Property> _collection;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public PropertySet()
        {
            _collection = new Dictionary<int, Property>();
        }

        /// <summary>
        /// The method returns the property according to its ID
        /// </summary>
        /// <param name="id">Property ID</param>
        /// <returns>Specific property</returns>
        public Property GetByID(int id)
        {
            return _collection.TryGetValue(id, out Property value) ? value : null;
        }

        /// <summary>
        /// The method returns the node according to its ordinal index in the collection
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>Specific property</returns>
        public Property GetByIndex(int index)
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
        /// Returns an enumerator that iterates through the Dictionary<int, Property>
        /// </summary>
        /// <returns>A Dictionary<int, Property>.Enumerator structure for the Dictionary<int, Property></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Implementation for the GetEnumerator method
        /// </summary>
        /// <returns>Property enumerator</returns>
        public PropertyEnumerator GetEnumerator()
        {
            return new PropertyEnumerator(_collection);
        }

        /// <summary>
        /// Adds the specified key and value to the collection
        /// </summary>
        /// <param name="id">Property ID</param>
        /// <param name="value">Specific property</param>
        public void Add(int id, Property value)
        {
            _collection.Add(id, value);
        }

        /// <summary>
        /// Indexer declaration
        /// </summary>
        /// <param name="id">Property ID</param>
        /// <returns>Specific property</returns>
        public Property this[int id]
        {
            get { return GetByID(id); }
            set { Add(id, value); }
        }

        /// <summary>
        /// Gets the number of ID/Properties pairs contained in the collection
        /// </summary>
        public int Count
        {
            get { return _collection.Count; }
        }
    }

    /// <summary>
    /// Property enumerator class
    /// </summary>
    public class PropertyEnumerator : IEnumerator
    {
        /// <summary>
        /// Array for enumeration
        /// </summary>
        private KeyValuePair<int, Property>[] _array;

        /// <summary>
        /// Position
        /// </summary>
        int position = -1;

        /// <summary>
        /// Сonstructor for enumerator
        /// </summary>
        /// <param name="dic">Input dictionary</param>
        public PropertyEnumerator(Dictionary<int, Property> dic)
        {
            var list = new List<KeyValuePair<int, Property>>(dic);
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
        public Property Current
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
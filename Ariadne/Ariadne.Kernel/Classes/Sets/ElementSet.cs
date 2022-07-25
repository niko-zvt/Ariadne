using System;
using System.Collections;
using System.Collections.Generic;

namespace Ariadne.Kernel
{
    /// <summary>
    /// Class for storing a generalized set of elements.
    /// A generalized set is needed to hide a specific implementation of the object storage mechanism. 
    /// The source mechanism can be an array, a list, a tree, or other structure.
    /// </summary>
    public class ElementSet : IEnumerable
    {
        /// <summary>
        /// Specific storage collection
        /// </summary>
        private Dictionary<int, Element> _collection;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ElementSet()
        {
            _collection = new Dictionary<int, Element>();
        }

        /// <summary>
        /// The method returns the element according to its ID
        /// </summary>
        /// <param name="id">Element ID</param>
        /// <returns>Specific element</returns>
        public Element GetByID(int id)
        {
            return _collection.TryGetValue(id, out Element value) ? value : null;
        }

        /// <summary>
        /// The method returns the element according to its ordinal index in the collection
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>Specific element</returns>
        public Element GetByIndex(int index)
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
        /// Returns an enumerator that iterates through the Dictionary<int, Element>
        /// </summary>
        /// <returns>A Dictionary<int, Element>.Enumerator structure for the Dictionary<int, Element></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Implementation for the GetEnumerator method
        /// </summary>
        /// <returns>Element enumerator</returns>
        public ElementEnumerator GetEnumerator()
        {
            return new ElementEnumerator(_collection);
        }

        /// <summary>
        /// Adds the specified key and value to the collection
        /// </summary>
        /// <param name="id">Element ID</param>
        /// <param name="value">Specific element</param>
        public bool TryAdd(int id, Element value)
        {
            if (_collection is null)
                return false;

            return _collection.TryAdd(id, value);
        }

        /// <summary>
        /// Indexer declaration
        /// </summary>
        /// <param name="id">Element ID</param>
        /// <returns>Specific element</returns>
        public Element this[int id]
        {
            get { return GetByID(id); }
            set { TryAdd(id, value); }
        }

        /// <summary>
        /// Gets the number of ID/Elements pairs contained in the collection
        /// </summary>
        public int Count
        {
            get { return _collection.Count; }
        }
    }

    /// <summary>
    /// Element enumerator class
    /// </summary>
    public class ElementEnumerator : IEnumerator
    {
        /// <summary>
        /// Array for enumeration
        /// </summary>
        private KeyValuePair<int, Element>[] _array;

        /// <summary>
        /// Position
        /// </summary>
        int position = -1;

        /// <summary>
        /// Сonstructor for enumerator
        /// </summary>
        /// <param name="dic">Input dictionary</param>
        public ElementEnumerator(Dictionary<int, Element> dic)
        {
            var list = new List<KeyValuePair<int, Element>>(dic);
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
        public Element Current
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
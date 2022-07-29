using System;
using System.Collections;
using System.Collections.Generic;

namespace Ariadne.Kernel
{
    /// <summary>
    /// Class for storing a generalized set of materials.
    /// A generalized set is needed to hide a specific implementation of the object storage mechanism. 
    /// The source mechanism can be an array, a list, a tree, or other structure.
    /// </summary>
    public class MaterialSet : IEnumerable
    {
        /// <summary>
        /// Specific storage collection
        /// </summary>
        private List<Material> _collection;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MaterialSet()
        {
            _collection = new List<Material>();
        }

        /// <summary>
        /// The method returns the material according to its ID
        /// </summary>
        /// <param name="id">Material ID</param>
        /// <returns>Specific material</returns>
        public MaterialSet GetByID(int id)
        {
            var materials = new MaterialSet();
            
            foreach(var material in _collection)
            {
                if (material != null && material.ID == id)
                    materials.Add(material);
            }

            return materials;
        }

        /// <summary>
        /// The method returns the node according to its ordinal index in the collection
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>Specific material</returns>
        public Material GetByIndex(int index)
        {
            if (index < 0 || index >= _collection.Count)
                return null;

            return _collection[index];
        }

        /// <summary>
        /// Returns an enumerator that iterates through the Dictionary<int, Material>
        /// </summary>
        /// <returns>A Dictionary<int, Material>.Enumerator structure for the Dictionary<int, Material></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Implementation for the GetEnumerator method
        /// </summary>
        /// <returns>Material enumerator</returns>
        public MaterialEnumerator GetEnumerator()
        {
            return new MaterialEnumerator(_collection);
        }

        /// <summary>
        /// Adds the specified key and value to the collection
        /// </summary>
        /// <param name="id">Material ID</param>
        /// <param name="value">Specific material</param>
        public void Add(Material value)
        {
            _collection.Add(value);
        }

        /// <summary>
        /// Indexer declaration
        /// </summary>
        /// <param name="id">Material ID</param>
        /// <returns>Specific material</returns>
        public MaterialSet this[int id]
        {
            get { return GetByID(id); }
        }

        /// <summary>
        /// Gets the number of ID/Materials pairs contained in the collection
        /// </summary>
        public int Count
        {
            get { return _collection.Count; }
        }
    }

    /// <summary>
    /// Material enumerator class
    /// </summary>
    public class MaterialEnumerator : IEnumerator
    {
        /// <summary>
        /// Array for enumeration
        /// </summary>
        private Material[] _array;

        /// <summary>
        /// Position
        /// </summary>
        int position = -1;

        /// <summary>
        /// Сonstructor for enumerator
        /// </summary>
        /// <param name="list">Input list</param>
        public MaterialEnumerator(List<Material> list)
        {
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
        public Material Current
        {
            get
            {
                try
                {
                    return _array[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
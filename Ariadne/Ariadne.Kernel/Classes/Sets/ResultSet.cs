﻿// Copyright 2022 Nikolay V. Zhivotenko
// Licensed under the Apache License, Version 2.0
// E-mail: niko.zvt@gmail.com

using System;
using System.Collections;
using System.Collections.Generic;

namespace Ariadne.Kernel
{
    /// <summary>
    /// Class for storing a generalized set of results.
    /// A generalized set is needed to hide a specific implementation of the object storage mechanism. 
    /// The source mechanism can be an array, a list, a tree, or other structure.
    /// </summary>
    public class ResultSet : IEnumerable
    {
        /// <summary>
        /// Specific storage collection
        /// </summary>
        private Dictionary<int, Result> _collection;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ResultSet()
        {
            _collection = new Dictionary<int, Result>();
        }

        /// <summary>
        /// The method returns the result according to its ID
        /// </summary>
        /// <param name="id">Result ID</param>
        /// <returns>Specific result</returns>
        public Result GetByID(int id)
        {
            return _collection.TryGetValue(id, out Result value) ? value : null;
        }

        /// <summary>
        /// The method returns the result according to its ordinal index in the collection
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>Specific result</returns>
        public Result GetByIndex(int index)
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
        /// Returns an enumerator that iterates through the Dictionary<int, Result>
        /// </summary>
        /// <returns>A Dictionary<int, Result>.Enumerator structure for the Dictionary<int, Result></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Implementation for the GetEnumerator method
        /// </summary>
        /// <returns>Result enumerator</returns>
        public ResultEnumerator GetEnumerator()
        {
            return new ResultEnumerator(_collection);
        }

        /// <summary>
        /// Adds the specified key and value to the collection
        /// </summary>
        /// <param name="id">Result ID</param>
        /// <param name="value">Specific result</param>
        public void Add(int id, Result value)
        {
            _collection.Add(id, value);
        }

        /// <summary>
        /// Indexer declaration
        /// </summary>
        /// <param name="id">Result ID</param>
        /// <returns>Specific result</returns>
        public Result this[int id]
        {
            get { return GetByID(id); }
            set { Add(id, value); }
        }

        /// <summary>
        /// Gets the number of ID/Results pairs contained in the collection
        /// </summary>
        public int Count
        {
            get { return _collection.Count; }
        }

        /// <summary>
        /// Overriding the convert method to a string
        /// </summary>
        /// <returns>Returns a string representation of the result set</returns>
        public override string ToString()
        {
            var str = "";
            str += $"{this.GetType()}\n{this.Count} items in the set.\n";

            foreach (var element in _collection)
            {
                str += $"\t{element.Key}\t{element.Value}\n";
            }
            return str;
        }
    }

    /// <summary>
    /// Result enumerator class
    /// </summary>
    public class ResultEnumerator : IEnumerator
    {
        /// <summary>
        /// Array for enumeration
        /// </summary>
        private KeyValuePair<int, Result>[] _array;

        /// <summary>
        /// Position
        /// </summary>
        int position = -1;

        /// <summary>
        /// Сonstructor for enumerator
        /// </summary>
        /// <param name="dic">Input dictionary</param>
        public ResultEnumerator(Dictionary<int, Result> dic)
        {
            var list = new List<KeyValuePair<int, Result>>(dic);
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
        public Result Current
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
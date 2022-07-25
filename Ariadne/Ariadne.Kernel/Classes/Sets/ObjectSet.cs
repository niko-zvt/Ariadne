using System;
using System.Collections.Generic;

namespace Ariadne.Kernel
{
    /// <summary>
    /// Class for storing a generalized set of objects.
    /// A generalized set is needed to hide a specific implementation of the object storage mechanism. 
    /// The source mechanism can be an array, a list, a tree, or other structure.
    /// </summary>
    public class Set<T> : List<T>
    {
        /// <summary>
        /// Convert an array of objects to a set of objects
        /// </summary>
        /// <param name="array">Array of objects</param>
        /// <returns>Set of objects</returns>
        public static Set<T> FromArray(T[] array)
        {
            Set<T> set = new Set<T>();
            for (int i = 0; i < array.Length; i++)
            {
                set.Add(array[i]);
            }
            return set;
        }

        /// <summary>
        /// Convert a set of objects to an array of objects
        /// </summary>
        /// <param name="set">Set of objects</param>
        /// <returns>Array of objects</returns>
        public static T[] ToArray(Set<T> set)
        {
            if (set == null)
                return new T[0];

            var count = set.Count;

            if (count <= 0)
                return new T[0];

            var array = new T[count];

            for (int i = 0; i < count; i++)
            {
                array[i] = set[i];
            }

            return array;
        }

        /// <summary>
        /// Get type of objects in set.
        /// </summary>
        /// <returns>Type of objects.</returns>
        public Type GetObjectTypeInSet()
        {
            return typeof(T);
        }
    }
}

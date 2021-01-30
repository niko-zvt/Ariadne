using System.Collections.Generic;

namespace Ariadne.Kernel
{
    /// <summary>
    /// Class for storing a generalized set of int numbers.
    /// A generalized set is needed to hide a specific implementation of the object storage mechanism. 
    /// The source mechanism can be an array, a list, a tree, or other structure.
    /// </summary>
    class IntSet : List<int>
    {
        /// <summary>
        /// Converts an array of int numbers to a generalized set of int numbers
        /// </summary>
        /// <param name="array">Source array of int numbers</param>
        /// <returns>Set of int numbers</returns>
        public static IntSet FromArray(int[] array)
        {
            IntSet set = new IntSet();

            foreach(var e in array)
            {
                set.Add(e);
            }

            return set;
        }
    }
}

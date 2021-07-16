using System.Collections.Generic;

namespace Ariadne.Kernel
{
    /// <summary>
    /// Class for storing a generalized set of numbers.
    /// A generalized set is needed to hide a specific implementation of the object storage mechanism. 
    /// The source mechanism can be an array, a list, a tree, or other structure.
    /// </summary>
    class IntSet : List<int>
    {
        /// <summary>
        /// Convert an array of numbers to a set of numbers
        /// </summary>
        /// <param name="array">Array of numbers</param>
        /// <returns>Set of numbers</returns>
        public static IntSet FromArray(int[] array)
        {
            IntSet set = new IntSet();
            for (int i = 0; i < array.Length; i++)
            {
                set.Add(array[i]);
            }
            return set;
        }

        /// <summary>
        /// Convert a set of numbers to an array of numbers
        /// </summary>
        /// <param name="set">Set of numbers</param>
        /// <returns>Array of numbers</returns>
        public static int[] ToArray(IntSet set)
        {
            if (set == null)
                return new int[0];

            var count = set.Count;

            if (count <= 0)
                return new int[0];

            var array = new int[count];

            for (int i = 0; i < count; i++)
            {
                array[i] = set[i];
            }

            return array;
        }
    }
}

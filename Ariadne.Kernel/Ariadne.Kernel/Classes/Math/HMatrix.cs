using System;

namespace Ariadne.Kernel.Math
{
    public struct HVector : IEquatable<HVector>, IFormattable
    {
        VectorND Vector = null;
        float Float = float.NaN;
        bool IsVector = true;

        public HVector(float value)
        {
            Vector = null;
            Float = value;
            IsVector = false;
        }

        public HVector(float[] vector)
        {
            Vector = new VectorND(vector);
            Float = float.NaN;
            IsVector = true;
        }

        public bool Equals(HVector other)
        {
            if (IsVector == true)
                return Vector.Equals(other.Vector);

            return Float.Equals(other.Float);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (IsVector == true)
                return Vector.ToString();

            return Float.ToString();
        }

        public static HVector operator /(HVector a, HVector b)
        {
            if (a.IsVector == true && b.IsVector == true)
            {
                throw new ArgumentException("a is vector, b is vector!");
            }
            else if (a.IsVector == true && b.IsVector == false)
            {
                return new HVector() { IsVector = true, Vector = a.Vector / b.Float, Float = float.NaN };
            }
            else if (a.IsVector == false && b.IsVector == true)
            {
                throw new ArgumentException("a is float, b is vector!");
            }
            else
            {
                return new HVector() { IsVector = false, Vector = null, Float = a.Float / b.Float };
            }
        }

        public static HVector operator *(HVector a, HVector b)
        {
            if (a.IsVector == true && b.IsVector == true)
            {
                return new HVector() { IsVector = false, Vector = null, Float = a.Vector * b.Vector };
            }
            else if (a.IsVector == true && b.IsVector == false)
            {
                return new HVector() { IsVector = true, Vector = a.Vector * b.Float, Float = float.NaN };
            }
            else if (a.IsVector == false && b.IsVector == true)
            {
                return new HVector() { IsVector = true, Vector = a.Float * b.Vector, Float = float.NaN };
            }
            else
            {
                return new HVector() { IsVector = false, Vector = null, Float = a.Float * b.Float };
            }
        }

        public static HVector operator +(HVector a, HVector b)
        {
            if (a.IsVector == true && b.IsVector == true)
            {
                return new HVector() { IsVector = true, Vector = a.Vector + b.Vector, Float = float.NaN };
            }
            else if (a.IsVector == true && b.IsVector == false)
            {
                throw new ArgumentException("a is float, b is vector!");
            }
            else if (a.IsVector == false && b.IsVector == true)
            {
                throw new ArgumentException("a is vector, b is float!");
            }
            else
            {
                return new HVector() { IsVector = false, Vector = null, Float = a.Float + b.Float };
            }
        }

        public static HVector operator -(HVector a, HVector b)
        {
            if (a.IsVector == true && b.IsVector == true)
            {
                return new HVector() { IsVector = true, Vector = a.Vector - b.Vector, Float = float.NaN };
            }
            else if (a.IsVector == true && b.IsVector == false)
            {
                throw new ArgumentException("a is float, b is vector!");
            }
            else if (a.IsVector == false && b.IsVector == true)
            {
                throw new ArgumentException("a is vector, b is float!");
            }
            else
            {
                return new HVector() { IsVector = false, Vector = null, Float = a.Float - b.Float };
            }
        }

        public static HVector operator -(HVector a)
        {
            if (a.IsVector == true)
            {
                return new HVector() { IsVector = true, Vector = -a.Vector, Float = float.NaN };
            }
            else
            {
                return new HVector() { IsVector = false, Vector = null, Float = -a.Float };
            }// Statements  
        }
    }

    class HMatrix
    {
        private MathNet.Numerics.LinearAlgebra.Matrix<HVector> _values;

        /// <summary>
        /// Constructor by HVector array
        /// </summary>
        /// <param name="array">Array of size NxM</param>
        public HMatrix(HVector[,] array)
        {
            _values = MathNet.Numerics.LinearAlgebra.Matrix<HVector>.Build.DenseOfArray(array);
        }

        /// <summary>
        /// Constructor by matrix
        /// </summary>
        /// <param name="matrix">Matrix</param>
        public HMatrix(HMatrix matrix)
        {
            _values = matrix._values;
        }

        /// <summary>
        /// Trace of a square matrix is defined to be the sum of elements on the main diagonal
        /// </summary>
        /// <returns>Return trace of matrix</returns>
        public HVector Trace()
        {
            return _values.Trace();
        }

        /// <summary>
        /// Inverse the matrix
        /// </summary>
        /// <returns></returns>
        public void Inverse()
        {
            _values = _values.Inverse();
        }

        /// <summary>
        /// Transpose the matrix
        /// </summary>
        public void Transpose()
        {
            _values = _values.Transpose();
        }

        /// <summary>
        /// Computes the determinant of this matrix
        /// </summary>
        /// <returns>The determinant of this matrix</returns>
        public HVector Determinant()
        {
            return _values.Determinant();
        }

        /// <summary>
        /// Checking whether the matrix is square
        /// </summary>
        /// <returns>True if the matrix is square, otherwise - false</returns>
        public bool IsSquare()
        {
            return _values.RowCount == _values.ColumnCount;
        }

        /// <summary>
        /// Component
        /// </summary>
        public HVector GetValueAt(int i, int j)
        {
            if (i < 0 || j < 0)
                throw new System.ArgumentOutOfRangeException("i or j < 0");

            if (i > _values.RowCount || j > _values.ColumnCount)
                throw new System.ArgumentOutOfRangeException("i or j > matrix size");

            return _values[i, j];
        }

        /// <summary>
        /// The method returns representations of the specific matrix as a string
        /// </summary>
        /// <returns>A string that represents the current object</returns>
        public override string ToString()
        {
            return _values.ToString();
        }
    }
}

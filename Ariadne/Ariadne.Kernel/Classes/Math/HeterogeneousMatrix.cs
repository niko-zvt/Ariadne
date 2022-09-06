// Copyright 2022 Nikolay V. Zhivotenko
// Licensed under the Apache License, Version 2.0
// E-mail: niko.zvt@gmail.com

using System;

namespace Ariadne.Kernel.Math
{
    public struct HValue : IEquatable<HValue>, IFormattable
    {
        public VectorND Vector = null;
        public float Float = float.NaN;
        public bool IsVector = true;

        public HValue(float value)
        {
            Vector = null;
            Float = value;
            IsVector = false;
        }

        public HValue(float[] vector)
        {
            Vector = new VectorND(vector);
            Float = float.NaN;
            IsVector = true;
        }

        public HValue(Vector3D vector)
        {
            Vector = new VectorND(vector.ToArray());
            Float = float.NaN;
            IsVector = true;
        }

        public bool Equals(HValue other)
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

        public static HValue operator *(float a, HValue b)
        {
            if (b.IsVector == true)
            {
                return new HValue() { IsVector = true, Vector = b.Vector * a, Float = float.NaN };
            }
            else
            {
                return new HValue() { IsVector = false, Vector = null, Float = b.Float * a };
            }
        }

        public static HValue operator *(HValue a, float b)
        {
            if (a.IsVector == true)
            {
                return new HValue() { IsVector = true, Vector = a.Vector * b, Float = float.NaN };
            }
            else
            {
                return new HValue() { IsVector = false, Vector = null, Float = a.Float * b };
            }
        }

        public static HValue operator /(HValue a, HValue b)
        {
            if (a.IsVector == true && b.IsVector == true)
            {
                throw new ArgumentException("a is vector, b is vector!");
            }
            else if (a.IsVector == true && b.IsVector == false)
            {
                return new HValue() { IsVector = true, Vector = a.Vector / b.Float, Float = float.NaN };
            }
            else if (a.IsVector == false && b.IsVector == true)
            {
                throw new ArgumentException("a is float, b is vector!");
            }
            else
            {
                return new HValue() { IsVector = false, Vector = null, Float = a.Float / b.Float };
            }
        }

        public static HValue operator *(HValue a, HValue b)
        {
            if (a.IsVector == true && b.IsVector == true)
            {
                return new HValue() { IsVector = false, Vector = null, Float = a.Vector * b.Vector };
            }
            else if (a.IsVector == true && b.IsVector == false)
            {
                return new HValue() { IsVector = true, Vector = a.Vector * b.Float, Float = float.NaN };
            }
            else if (a.IsVector == false && b.IsVector == true)
            {
                return new HValue() { IsVector = true, Vector = a.Float * b.Vector, Float = float.NaN };
            }
            else
            {
                return new HValue() { IsVector = false, Vector = null, Float = a.Float * b.Float };
            }
        }

        public static HValue operator +(HValue a, HValue b)
        {
            if (a.IsVector == true && b.IsVector == true)
            {
                return new HValue() { IsVector = true, Vector = a.Vector + b.Vector, Float = float.NaN };
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
                return new HValue() { IsVector = false, Vector = null, Float = a.Float + b.Float };
            }
        }

        public static HValue operator -(HValue a, HValue b)
        {
            if (a.IsVector == true && b.IsVector == true)
            {
                return new HValue() { IsVector = true, Vector = a.Vector - b.Vector, Float = float.NaN };
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
                return new HValue() { IsVector = false, Vector = null, Float = a.Float - b.Float };
            }
        }

        public static HValue operator -(HValue a)
        {
            if (a.IsVector == true)
            {
                return new HValue() { IsVector = true, Vector = -a.Vector, Float = float.NaN };
            }
            else
            {
                return new HValue() { IsVector = false, Vector = null, Float = -a.Float };
            }// Statements  
        }

        public static explicit operator HValue(float value)
        {
            return new HValue(value);
        }

        public static explicit operator HValue(float[] value)
        {
            return new HValue(value);
        }

        public static explicit operator HValue(Vector3D value)
        {
            return new HValue(value);
        }
    }

    sealed class HMatrix
    {
        private MathNet.Numerics.LinearAlgebra.Matrix<HValue> _values;

        /// <summary>
        /// Constructor by HValue array
        /// </summary>
        /// <param name="array">Array of size NxM</param>
        public HMatrix(HValue[,] array)
        {
            _values = MathNet.Numerics.LinearAlgebra.Matrix<HValue>.Build.DenseOfArray(array);
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
        public HValue Trace()
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
        public HValue Determinant()
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
        public HValue GetValueAt(int i, int j)
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
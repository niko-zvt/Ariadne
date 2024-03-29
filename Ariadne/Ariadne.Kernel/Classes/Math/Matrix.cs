﻿// Copyright 2022 Nikolay V. Zhivotenko
// Licensed under the Apache License, Version 2.0
// E-mail: niko.zvt@gmail.com

using MathNet.Numerics.LinearAlgebra.Double.Solvers;
using MathNet.Numerics.LinearAlgebra.Solvers;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Ariadne.Kernel.Math
{
    /// <summary>
    /// Abstract class for hiding the implementation of the Matrix class
    /// </summary>
    public abstract class Matrix
    {
        /// <summary>
        /// Matrix specific data
        /// </summary>
        protected MathNet.Numerics.LinearAlgebra.Matrix<float> _values;

        /// <summary>
        /// Virtual method to return the type of the matrix
        /// </summary>
        /// <returns>Type of matrix</returns>
        public abstract MatrixType GetMatrixType();

        /// <summary>
        /// Dimension of matrix
        /// </summary>
        /// <returns>Dimension</returns>
        public int GetDimension()
        {
            return GetSize().Length;
        }

        /// <summary>
        /// Inverse the matrix
        /// </summary>
        public void InverseSelf()
        {
            _values = _values.Inverse();
        }

        /// <summary>
        /// Inverse the matrix
        /// </summary>
        /// <returns>New matrix</returns>
        public abstract Matrix Inverse();

        /// <summary>
        /// Size of matrix
        /// </summary>
        /// <returns>Size as array</returns>
        public abstract int[] GetSize();

        /// <summary>
        /// Returns this matrix as a multidimensional array. The returned array will be independent
        /// from this matrix. A new memory block will be allocated for the array.
        /// </summary>
        /// <returns>A multidimensional containing the values of this matrix.</returns>
        public float[,] ToArray()
        {
            return _values.ToArray();
        }

        /// <summary>
        /// Define the indexer to allow client code to use [] notation.
        /// </summary>
        /// <param name="i">First index (row)</param>
        /// <param name="j">Second index (column)</param>
        /// <returns></returns>
        public float this[int i, int j]
        {
            get 
            {
                return _values.ToArray()[i,j];
            }
            set 
            {
                var newValues = _values.ToArray();
                newValues[i,j] = value;
                _values = MathNet.Numerics.LinearAlgebra.Matrix<float>.Build.DenseOfArray(newValues);
            }
        }

        /// <summary>
        /// Check matrix is valid.
        /// </summary>
        /// <returns>True if success result, otherwise - false.</returns>
        public bool IsValid(bool forceCheckNaN)
        {
            if (_values == null ||
                _values.RowCount <= 0 ||
                _values.ColumnCount <= 0 ||
                _values.ToArray().Length <= 0 ||
                forceCheckNaN ? IsContain(float.NaN) : true)
                return false;

            return true;
        }

        /// <summary>
        /// Check the matrix for the content of a specific value.
        /// </summary>
        /// <param name="value">The value we are looking for in the matrix.</param>
        /// <returns>True if success result, otherwise - false.</returns>
        public bool IsContain(float value)
        {
            if (_values == null)
                return false;

            var array = _values.AsArray();

            foreach (var element in array)
            {
                if(element == value)
                    return true;
            }

            return false;
        }
    }

    
    /// <summary>
    /// Enumeration of all available types of matrix
    /// </summary>
    public enum MatrixType
    {
        Matrix3x3,
        MatrixNxM,
    }

    /// <summary>
    /// Class of a matrix in three-dimensional space 3x3
    /// </summary>
    public class Matrix3x3 : Matrix
    {
        /// <summary>
        /// Default constructor. Creates a zero matrix
        /// </summary>
        public Matrix3x3()
        {
            float[,] values = { { 0.0f, 0.0f, 0.0f },
                                { 0.0f, 0.0f, 0.0f },
                                { 0.0f, 0.0f, 0.0f }};

            _values = MathNet.Numerics.LinearAlgebra.Matrix<float>.Build.DenseOfArray(values);
        }

        /// <summary>
        /// Constructor by symmetry float values
        /// </summary>
        /// <param name="xx">Component [0, 0]</param>
        /// <param name="yy">Component [1, 1]</param>
        /// <param name="zz">Component [2, 2]</param>
        public Matrix3x3(float xx, float yy, float zz)
        {
            float o = 0.0f;
            float[,] values = { { xx, o, o },
                                { o, yy, o },
                                { o, o, zz }};

            _values = MathNet.Numerics.LinearAlgebra.Matrix<float>.Build.DenseOfArray(values);
        }

        /// <summary>
        /// Constructor by symmetry float values
        /// </summary>
        /// <param name="xx">Component [0, 0]</param>
        /// <param name="yy">Component [1, 1]</param>
        /// <param name="zz">Component [2, 2]</param>
        /// <param name="xy">Component [0, 1]</param>
        /// <param name="yz">Component [2, 3]</param>
        /// <param name="zx">Component [3, 1]</param>
        public Matrix3x3(float xx, float yy, float zz,
                         float xy, float yz, float zx)
        {
            float[,] values = { { xx, xy, zx },
                                { xy, yy, yz },
                                { zx, yz, zz }};

            _values = MathNet.Numerics.LinearAlgebra.Matrix<float>.Build.DenseOfArray(values);
        }

        /// <summary>
        /// Constructor by symmetry double values
        /// </summary>
        /// <param name="xx">Component [0, 0]</param>
        /// <param name="yy">Component [1, 1]</param>
        /// <param name="zz">Component [2, 2]</param>
        /// <param name="xy">Component [0, 1]</param>
        /// <param name="yz">Component [2, 3]</param>
        /// <param name="zx">Component [3, 1]</param>
        public Matrix3x3(double xx, double yy, double zz,
                         double xy, double yz, double zx)
        {
            float[,] values = { { (float)xx, (float)xy, (float)zx },
                                { (float)xy, (float)yy, (float)yz },
                                { (float)zx, (float)yz, (float)zz }};

            _values = MathNet.Numerics.LinearAlgebra.Matrix<float>.Build.DenseOfArray(values);
        }

        /// <summary>
        /// Constructor by float values
        /// </summary>
        /// <param name="xx">Component [0, 0]</param>
        /// <param name="xy">Component [0, 1]</param>
        /// <param name="xz">Component [0, 2]</param>
        /// <param name="yx">Component [1, 0]</param>
        /// <param name="yy">Component [1, 1]</param>
        /// <param name="yz">Component [1, 2]</param>
        /// <param name="zx">Component [2, 0]</param>
        /// <param name="zy">Component [2, 1]</param>
        /// <param name="zz">Component [2, 2]</param>
        public Matrix3x3(float xx, float xy, float xz,
                         float yx, float yy, float yz,
                         float zx, float zy, float zz)
        {
            float[,] values = { { xx, xy, xz },
                                { yx, yy, yz },
                                { zx, zy, zz }};

            _values = MathNet.Numerics.LinearAlgebra.Matrix<float>.Build.DenseOfArray(values);
        }

        /// <summary>
        /// Constructor by double values
        /// </summary>
        /// <param name="xx">Component [0, 0]</param>
        /// <param name="xy">Component [0, 1]</param>
        /// <param name="xz">Component [0, 2]</param>
        /// <param name="yx">Component [1, 0]</param>
        /// <param name="yy">Component [1, 1]</param>
        /// <param name="yz">Component [1, 2]</param>
        /// <param name="zx">Component [2, 0]</param>
        /// <param name="zy">Component [2, 1]</param>
        /// <param name="zz">Component [2, 2]</param>
        public Matrix3x3(double xx, double xy, double xz,
                         double yx, double yy, double yz,
                         double zx, double zy, double zz)
        {
            float[,] values = { { (float)xx, (float)xy, (float)xz },
                                 { (float)yx, (float)yy, (float)yz },
                                 { (float)zx, (float)zy, (float)zz }};

            _values = MathNet.Numerics.LinearAlgebra.Matrix<float>.Build.DenseOfArray(values);
        }

        /// <summary>
        /// Constructor by float array
        /// </summary>
        /// <param name="array">Array of size 3x3</param>
        public Matrix3x3(float[,] array)
        {
            var l1 = array.GetLength(0);
            var l2 = array.GetLength(1);

            if (l1 != 3 || l2 != 3)
                throw new System.ArgumentException("Incorrect dimension of the matrix");

            _values = MathNet.Numerics.LinearAlgebra.Matrix<float>.Build.DenseOfArray(array);
        }

        /// <summary>
        /// Constructor by double array
        /// </summary>
        /// <param name="array">Array of size 3x3</param>
        public Matrix3x3(double[,] array)
        {
            var l1 = array.GetLength(0);
            var l2 = array.GetLength(1);

            if (l1 != 3 || l2 != 3)
                throw new System.ArgumentException("Incorrect dimension of the matrix");

            float[,] floatArray = new float[l1, l2];

            for (int i = 0; i < l1; i++)
                for (int j = 0; j < l2; j++)
                    floatArray[i, j] = (float)array[i, j];

            _values = MathNet.Numerics.LinearAlgebra.Matrix<float>.Build.DenseOfArray(floatArray);
        }

        /// <summary>
        /// Constructor by matrix
        /// </summary>
        /// <param name="matrix">Matrix</param>
        public Matrix3x3(Matrix3x3 matrix)
        {
            _values = matrix._values;
        }

        /// <summary>
        /// Private constructor for transformations
        /// </summary>
        /// <param name="matrix">Matrix</param>
        private Matrix3x3(MathNet.Numerics.LinearAlgebra.Matrix<float> matrix)
        {
            if (matrix.RowCount != 3 || matrix.ColumnCount != 3)
                throw new System.ArgumentException("Matrix is not square 3x3");

            _values = matrix;
        }

        /// <summary>
        /// Build a diagonal matrix 3x3.
        /// XX = YY = ZZ;
        /// XY, YX, YZ, ZY, XZ, ZX - zero.
        /// </summary>
        /// <param name="x">XX, YY, ZZ values.</param>
        /// <returns>Diagonal matrix 3x3</returns>
        public static Matrix3x3 BuildDiagonal(float x)
        {
            return BuildDiagonal(x, x, x);
        }

        /// <summary>
        /// Build a diagonal matrix 3x3.
        /// XY, YX, YZ, ZY, XZ, ZX - zero.
        /// </summary>
        /// <param name="xx">XX value.</param>
        /// <param name="yy">YY value.</param>
        /// <param name="zz">ZZ value.</param>
        /// <returns>Diagonal matrix 3x3</returns>
        public static Matrix3x3 BuildDiagonal(float xx, float yy, float zz)
        {
            return new Matrix3x3(xx, yy, zz, 0.0f, 0.0f, 0.0f);
        }

        /// <summary>
        /// Trace of a square matrix is defined to be the sum of elements on the main diagonal
        /// </summary>
        /// <returns>Return trace of matrix</returns>
        public float Trace()
        {
            return _values.Trace();
        }

        /// <summary>
        /// The components of the matrix are squared and the trace of this matrix is determined
        /// </summary>
        /// <returns>Trace of the matrix square</returns>
        public float SquareTrace()
        {
            return Power(2).Trace();
        }

        /// <summary>
        /// Raises this square matrix to a positive integer exponent and places the result into the result matrix
        /// </summary>
        /// <param name="exponent">Positive exponent</param>
        /// <returns>Matrix raised to a power of</returns>
        public Matrix3x3 Power(int exponent)
        {
            if (exponent < 0)
                throw new System.ArgumentException("The exponent cannot be a negative number");

            MathNet.Numerics.LinearAlgebra.Matrix<float> resultValues = 
                MathNet.Numerics.LinearAlgebra.Matrix<float>.Build.Dense(3, 3);
            _values.Power(exponent, resultValues);
            return new Matrix3x3(resultValues);
        }

        /// <summary>
        /// Computes the determinant of this matrix
        /// </summary>
        /// <returns>The determinant of this matrix</returns>
        public float Determinant()
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
        /// Component XX
        /// </summary>
        public float XX { get { return _values[0, 0]; } set { _values[0, 0] = value; } }

        /// <summary>
        /// Component XY
        /// </summary>
        public float XY { get { return _values[0, 1]; } set { _values[0, 1] = value; } }

        /// <summary>
        /// Component XZ
        /// </summary>
        public float XZ { get { return _values[0, 2]; } set { _values[0, 2] = value; } }

        /// <summary>
        /// Component YX
        /// </summary>
        public float YX { get { return _values[1, 0]; } set { _values[1, 0] = value; } }

        /// <summary>
        /// Component YY
        /// </summary>
        public float YY { get { return _values[1, 1]; } set { _values[1, 1] = value; } }

        /// <summary>
        /// Component YZ
        /// </summary>
        public float YZ { get { return _values[1, 2]; } set { _values[1, 2] = value; } }

        /// <summary>
        /// Component ZX
        /// </summary>
        public float ZX { get { return _values[2, 0]; } set { _values[2, 0] = value; } }

        /// <summary>
        /// Component ZY
        /// </summary>
        public float ZY { get { return _values[2, 1]; } set { _values[2, 1] = value; } }

        /// <summary>
        /// Component ZZ
        /// </summary>
        public float ZZ { get { return _values[2, 2]; } set { _values[2, 2] = value; } }

        /// <summary>
        /// Dot multiplication operator.
        /// </summary>
        /// <param name="a">Matrix</param>
        /// <param name="b">Vector</param>
        /// <returns>Resulting vector after dot multiplication.</returns>
        public static Vector3D operator *(Matrix3x3 a, Vector3D b)
        {
            var vector = MathNet.Numerics.LinearAlgebra.Vector<float>.Build.DenseOfArray(b.ToArray());
            var newVector = a._values.Multiply(vector);
            return new Vector3D(newVector.ToArray());
        }

        /// <summary>
        /// Dot multiplication operator.
        /// </summary>
        /// <param name="a">Vector</param>
        /// <param name="b">Matrix</param>
        /// <returns>Resulting vector after dot multiplication.</returns>
        public static Vector3D operator *(Vector3D a, Matrix3x3 b)
        {
            var vector = MathNet.Numerics.LinearAlgebra.Vector<float>.Build.DenseOfArray(a.ToArray());
            var newVector = b._values.Multiply(vector);
            return new Vector3D(newVector.ToArray());
        }

        /// <summary>
        /// Dot multiplication operator.
        /// </summary>
        /// <param name="a">Matrix</param>
        /// <param name="b">Matrix</param>
        /// <returns>Resulting matrix after dot multiplication.</returns>
        public static Matrix3x3 operator *(Matrix3x3 a, Matrix3x3 b)
        {
            var newMatrix = a._values * b._values;
            return new Matrix3x3(newMatrix.ToArray());
        }

        /// <summary>
        /// Dot multiplication operator.
        /// </summary>
        /// <param name="a">Value</param>
        /// <param name="b">Matrix</param>
        /// <returns>Resulting matrix after dot multiplication.</returns>
        public static Matrix3x3 operator *(float a, Matrix3x3 b)
        {
            var newMatrix = a * b._values;
            return new Matrix3x3(newMatrix.ToArray());
        }

        /// <summary>
        /// Returns the matrix type
        /// </summary>
        /// <returns>Matrix type</returns>
        public override MatrixType GetMatrixType()
        {
            return MatrixType.Matrix3x3;
        }

        /// <summary>
        /// The method returns representations of the specific matrix as a string
        /// </summary>
        /// <returns>A string that represents the current object</returns>
        public override string ToString()
        {
            return GetMatrixType().ToString();
        }

        /// <summary>
        /// Size of matrix
        /// </summary>
        /// <returns>Size as array</returns>
        public override int[] GetSize()
        {
            return new int[] { 3, 3 };
        }

        /// <summary>
        /// Inverse the matrix
        /// </summary>
        /// <returns>New matrix</returns>
        public override Matrix Inverse()
        {
            return new Matrix3x3(_values.Inverse());
        }
    }

    /// <summary>
    /// Class of a matrix in three-dimensional space NxN
    /// </summary>
    public class MatrixNxM : Matrix
    {
        /// <summary>
        /// Constructor by float array
        /// </summary>
        /// <param name="array">Array of size NxM</param>
        public MatrixNxM(float[,] array)
        {
            _values = MathNet.Numerics.LinearAlgebra.Matrix<float>.Build.DenseOfArray(array);
        }

        /// <summary>
        /// Constructor by double array
        /// </summary>
        /// <param name="array">Array of size 3x3</param>
        public MatrixNxM(double[,] array)
        {
            var l1 = array.GetLength(0);
            var l2 = array.GetLength(1);
            float[,] floatArray = new float[l1, l2];

            for (int i = 0; i < l1; i++)
                for (int j = 0; j < l2; j++)
                    floatArray[i, j] = (float)array[i, j];

            _values = MathNet.Numerics.LinearAlgebra.Matrix<float>.Build.DenseOfArray(floatArray);
        }

        /// <summary>
        /// Constructor by matrix
        /// </summary>
        /// <param name="matrix">Matrix</param>
        public MatrixNxM(MatrixNxM matrix)
        {
            _values = matrix._values;
        }

        /// <summary>
        /// Constructor by vectors
        /// </summary>
        /// <param name="v1">First vector</param>
        /// <param name="v2">Second vector</param>
        /// <param name="v3">Third vector</param>
        /// <param name="isHorizontal">true - transverse matrix</param>
        public MatrixNxM(VectorND v1, VectorND v2, VectorND v3, bool isHorizontal = false)
        {
            if (v1.Size != v2.Size ||
                v1.Size != v3.Size ||
                v2.Size != v3.Size)
                throw new System.ArgumentException("Vectors is not one size!");

            var _v1 = MathNet.Numerics.LinearAlgebra.Vector<float>.Build.DenseOfArray(v1.ToArray());
            var _v2 = MathNet.Numerics.LinearAlgebra.Vector<float>.Build.DenseOfArray(v2.ToArray());
            var _v3 = MathNet.Numerics.LinearAlgebra.Vector<float>.Build.DenseOfArray(v3.ToArray());

            _values = MathNet.Numerics.LinearAlgebra.Matrix<float>.Build.DenseOfColumnVectors(_v1, _v2, _v3);
            if (isHorizontal == false)
               _values = _values.Transpose();
        }

        /// <summary>
        /// Private constructor for transformations
        /// </summary>
        /// <param name="matrix">Matrix</param>
        private MatrixNxM(MathNet.Numerics.LinearAlgebra.Matrix<float> matrix)
        {
            if (matrix.RowCount != matrix.ColumnCount)
                throw new System.ArgumentException("Matrix is not square NxN");

            _values = matrix;
        }

        /// <summary>
        /// Trace of a square matrix is defined to be the sum of elements on the main diagonal
        /// </summary>
        /// <returns>Return trace of matrix</returns>
        public float Trace()
        {
            return _values.Trace();
        }

        /// <summary>
        /// Transpose the matrix
        /// </summary>
        public void Transpose()
        {
            _values = _values.Transpose();
        }

        /// <summary>
        /// The components of the matrix are squared and the trace of this matrix is determined
        /// </summary>
        /// <returns>Trace of the matrix square</returns>
        public float SquareTrace()
        {
            return Power(2).Trace();
        }

        public VectorND Solve(VectorND vector)
        {
            var b = MathNet.Numerics.LinearAlgebra.Vector<float>.Build.DenseOfArray(vector.ToArray());

            var x = _values.Solve(b);

            return new VectorND(x.ToArray());
        }

        public VectorND MultyPly(VectorND vector)
        {
            var b = MathNet.Numerics.LinearAlgebra.Vector<float>.Build.DenseOfArray(vector.ToArray());

            var x = _values.Multiply(b);

            return new VectorND(x.ToArray());
        }

        public Vector3D MultyPly(Vector3D vector)
        {
            var b = MathNet.Numerics.LinearAlgebra.Vector<float>.Build.DenseOfArray(vector.ToArray());
            
            var x = _values.Multiply(b);
            
            return new Vector3D(x.ToArray());
        }

        public List<Vector3D> MultyPly(List<Vector3D> vectors)
        {
            var xValues = new float[vectors.Count];
            var yValues = new float[vectors.Count];
            var zValues = new float[vectors.Count];

            for(int i = 0; i < vectors.Count; i++)
            {
                xValues[i] = vectors[i].X;
                yValues[i] = vectors[i].Y;
                zValues[i] = vectors[i].Z;
            }

            var x = MathNet.Numerics.LinearAlgebra.Vector<float>.Build.DenseOfArray(xValues);
            var y = MathNet.Numerics.LinearAlgebra.Vector<float>.Build.DenseOfArray(yValues);
            var z = MathNet.Numerics.LinearAlgebra.Vector<float>.Build.DenseOfArray(zValues);

            x = _values.Multiply(x);
            y = _values.Multiply(y);
            z = _values.Multiply(z);

            var resultVectors = new List<Vector3D>();
            for (int i = 0; i < vectors.Count; i++)
            {
                resultVectors.Add(new Vector3D(x[i], y[i], z[i]));
            }

            return resultVectors;
        }

        /// <summary>
        /// Raises this square matrix to a positive integer exponent and places the result into the result matrix
        /// </summary>
        /// <param name="exponent">Positive exponent</param>
        /// <returns>Matrix raised to a power of</returns>
        public MatrixNxM Power(int exponent)
        {
            if (exponent < 0)
                throw new System.ArgumentException("The exponent cannot be a negative number");

            MathNet.Numerics.LinearAlgebra.Matrix<float> resultValues =
                MathNet.Numerics.LinearAlgebra.Matrix<float>.Build.Dense(3, 3);
            _values.Power(exponent, resultValues);
            return new MatrixNxM(resultValues);
        }

        /// <summary>
        /// Computes the determinant of this matrix
        /// </summary>
        /// <returns>The determinant of this matrix</returns>
        public float Determinant()
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
        /// Component ZZ
        /// </summary>
        public float GetValueAt(int i, int j)
        {
            if (i < 0 || j < 0)
                throw new System.ArgumentOutOfRangeException("i or j < 0");

            if (i > _values.RowCount || j > _values.ColumnCount)
                throw new System.ArgumentOutOfRangeException("i or j > matrix size");

            return _values[i, j];
        }

        /// <summary>
        /// Returns the matrix type
        /// </summary>
        /// <returns>Matrix type</returns>
        public override MatrixType GetMatrixType()
        {
            return MatrixType.MatrixNxM;
        }

        /// <summary>
        /// The method returns representations of the specific matrix as a string
        /// </summary>
        /// <returns>A string that represents the current object</returns>
        public override string ToString()
        {
            return GetMatrixType().ToString();
        }

        public static MatrixNxM ConvertToMatrix(List<Vector3D> vectors)
        {
            var m = new float[vectors.Count, 3];
            for(int i = 0; i < vectors.Count; i++)
            {
                m[i, 0] = vectors[i].X;
                m[i, 1] = vectors[i].Y;
                m[i, 2] = vectors[i].Z;
            }
            return new MatrixNxM(m);
        }

        /// <summary>
        /// Dot multiplication operator.
        /// </summary>
        /// <param name="a">Value</param>
        /// <param name="b">Matrix</param>
        /// <returns>Resulting matrix after dot multiplication.</returns>
        public static MatrixNxM operator *(float a, MatrixNxM b)
        {
            var newMatrix = a * b._values;
            return new MatrixNxM(newMatrix.ToArray());
        }

        /// <summary>
        /// Size of matrix
        /// </summary>
        /// <returns>Size as array</returns>
        public override int[] GetSize()
        {
            return new int[] {_values.RowCount, _values.ColumnCount };
        }

        /// <summary>
        /// Inverse the matrix
        /// </summary>
        /// <returns>New matrix</returns>
        public override Matrix Inverse()
        {
            return new MatrixNxM(_values.Inverse());
        }
    }


    /// <summary>
    /// Reduced row Echelon form
    /// </summary>
    public class ReducedRowEchelonForm
    {

        private double[,] rref;
        private int rows;
        private int cols;

        private int[] pivot;
        private int? freeCount;

        /// <summary>
        ///   Reduces a matrix to reduced row Echelon form.
        /// </summary>
        /// 
        /// <param name="value">The matrix to be reduced.</param>
        /// <param name="inPlace">
        ///   Pass <see langword="true"/> to perform the reduction in place. The matrix
        ///   <paramref name="value"/> will be destroyed in the process, resulting in less
        ///   memory consumption.</param>
        ///   
        public ReducedRowEchelonForm(double[,] value, bool inPlace = false)
        {
            if (value == null)
                throw new System.ArgumentNullException("value");

            rref = inPlace ? value : (double[,])value.Clone();

            int lead = 0;
            rows = rref.GetLength(0);
            cols = rref.GetLength(1);

            pivot = new int[rows];
            for (int i = 0; i < pivot.Length; i++)
                pivot[i] = i;


            for (int r = 0; r < rows; r++)
            {
                if (cols <= lead)
                    break;

                int i = r;

                while (rref[i, lead] == 0)
                {
                    i++;

                    if (i >= rows)
                    {
                        i = r;

                        if (lead < cols - 1)
                            lead++;
                        else break;
                    }
                }

                if (i != r)
                {
                    // Swap rows i and r
                    for (int j = 0; j < cols; j++)
                    {
                        var temp = rref[r, j];
                        rref[r, j] = rref[i, j];
                        rref[i, j] = temp;
                    }

                    // Update indices
                    {
                        var temp = pivot[r];
                        pivot[r] = pivot[i];
                        pivot[i] = temp;
                    }
                }

                // Set to reduced row echelon form
                var div = rref[r, lead];
                if (div != 0)
                {
                    for (int j = 0; j < cols; j++)
                        rref[r, j] /= div;
                }

                for (int j = 0; j < rows; j++)
                {
                    if (j != r)
                    {
                        var sub = rref[j, lead];
                        for (int k = 0; k < cols; k++)
                            rref[j, k] -= (sub * rref[r, k]);
                    }
                }

                lead++;
            }
        }

        /// <summary>
        ///   Gets the pivot indicating the position
        ///   of the original rows before the swap.
        /// </summary>
        /// 
        public int[] Pivot { get { return pivot; } }

        /// <summary>
        ///   Gets the matrix in row reduced Echelon form.
        /// </summary>
        public double[,] Result { get { return rref; } }

        /// <summary>
        ///   Gets the number of free variables (linear
        ///   dependent rows) in the given matrix.
        /// </summary>
        public int FreeVariables
        {
            get
            {
                if (freeCount == null)
                    freeCount = count();

                return freeCount.Value;
            }
        }

        private int count()
        {
            for (int i = rows - 1; i >= 0; i--)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (rref[i, j] != 0)
                        return rows - i - 1;
                }
            }

            return 0;
        }
    }
}

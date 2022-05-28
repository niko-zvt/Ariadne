namespace Ariadne.Kernel.Math
{
    /// <summary>
    /// Abstract class for hiding the implementation of the Matrix class
    /// </summary>
    abstract class Matrix
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
    }

    /// <summary>
    /// Enumeration of all available types of matrix
    /// </summary>
    enum MatrixType
    {
        Matrix3x3,
    }

    /// <summary>
    /// Class of a matrix in three-dimensional space 3x3
    /// </summary>
    class Matrix3x3 : Matrix
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
    }
}

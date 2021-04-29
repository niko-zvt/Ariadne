namespace Ariadne.Kernel.Math
{
    /// <summary>
    /// Abstract class for hiding the implementation of the Vector class
    /// </summary>
    abstract class Vector
    {
        /// <summary>
        /// Vector specific data
        /// </summary>
        protected MathNet.Numerics.LinearAlgebra.Vector<float> _vectorData;
    }

    /// <summary>
    /// Class of a vector in three-dimensional space
    /// </summary>
    class Vector3D : Vector
    {
        /// <summary>
        /// Default constructor. Forms the vector {0, 0, 0}
        /// </summary>
        public Vector3D()
        {
            float[] values = { 0.0f, 0.0f, 0.0f };
            _vectorData = MathNet.Numerics.LinearAlgebra.Vector<float>.Build.DenseOfArray(values);
        }

        /// <summary>
        /// Constructor by float values {X, Y, Z}
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="z">Z</param>
        public Vector3D(float x, float y, float z)
        {
            float[] values = { x, y, z };
            _vectorData = MathNet.Numerics.LinearAlgebra.Vector<float>.Build.DenseOfArray(values);
        }

        /// <summary>
        /// Constructor by double values {X, Y, Z}
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="z">Z</param>
        public Vector3D(double x, double y, double z)
        {
            float[] values = { (float)x, (float)y, (float)z };
            _vectorData = MathNet.Numerics.LinearAlgebra.Vector<float>.Build.DenseOfArray(values);
        }

        /// <summary>
        /// Constructor by float array {X, Y, Z}
        /// </summary>
        /// <param name="array">Array of dimension 3</param>
        public Vector3D(float[] array)
        {
            if (array.Length != 3)
                throw new System.ArgumentException("Invalid dimension array");

            _vectorData = MathNet.Numerics.LinearAlgebra.Vector<float>.Build.DenseOfArray(array);
        }

        /// <summary>
        /// Constructor by double array {X, Y, Z}
        /// </summary>
        /// <param name="array">Array of dimension 3</param>
        public Vector3D(double[] array)
        {
            var len = array.Length;

            if (len != 3)
                throw new System.ArgumentException("Invalid dimension array");

            float[] floatArray = new float[len];
            for (int i = 0; i < len; i++)
            {
                floatArray[i] = (float)array[i];
            }

            _vectorData = MathNet.Numerics.LinearAlgebra.Vector<float>.Build.DenseOfArray(floatArray);
        }

        /// <summary>
        /// Constructor by vector
        /// </summary>
        /// <param name="vector">Vector</param>
        public Vector3D(Vector3D vector)
        {
            _vectorData = vector._vectorData;
        }

        /// <summary>
        /// Component X
        /// </summary>
        public float X { get { return _vectorData[0]; } set { _vectorData[0] = value; } }

        /// <summary>
        /// Component Y
        /// </summary>
        public float Y { get { return _vectorData[1]; } set { _vectorData[1] = value; } }

        /// <summary>
        /// Component Z
        /// </summary>
        public float Z { get { return _vectorData[2]; } set { _vectorData[2] = value; } }

    }
}

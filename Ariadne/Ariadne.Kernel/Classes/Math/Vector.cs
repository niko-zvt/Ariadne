using System.Collections.Generic;

namespace Ariadne.Kernel.Math
{
    /// <summary>
    /// Abstract class for hiding the implementation of the Vector class
    /// </summary>
    public abstract class Vector
    {
        /// <summary>
        /// Vector specific data
        /// </summary>
        protected MathNet.Numerics.LinearAlgebra.Vector<float> _vectorData;

        /// <summary>
        /// Gets the length (L2-Norm) of this vector.
        /// </summary>
        public float Length { get { return (float)_vectorData.L2Norm(); } }

        /// <summary>
        /// Gets the length or number of dimensions of this vector.
        /// </summary>
        public int Size { get { return (int)_vectorData.Count; } }

        /// <summary>
        /// Convert to array
        /// </summary>
        /// <returns>Float array</returns>
        public float[] ToArray()
        {
            if (_vectorData == null)
                throw new System.ArgumentNullException("Vector data is null!");

            return _vectorData.ToArray();
        }

        /// <summary>
        /// Check vector is valid.
        /// </summary>
        /// <returns>True if success result, otherwise - false.</returns>
        public bool IsValid()
        {
            if (_vectorData == null ||
                _vectorData.Count < 0 ||
                IsContain(float.NaN))
                return false;

            return true;
        }

        /// <summary>
        /// Check the vector for the content of a specific value.
        /// </summary>
        /// <param name="value">The value we are looking for in the vector.</param>
        /// <returns>True if success result, otherwise - false.</returns>
        public bool IsContain(float value)
        {
            if (_vectorData == null)
                return false;

            var vector = _vectorData.AsArray();

            bool exists = System.Array.Exists(vector, element => element == value);

            return exists;
        }

        /// <summary>
        /// Virtual method to return the type of the vector
        /// </summary>
        /// <returns>Type of vector</returns>
        public abstract VectorType GetVectorType();
    }

    public enum VectorType
    {
        Vector2D,
        Vector3D,
        VectorND
    }

    /// <summary>
    /// Class of a vector in three-dimensional space
    /// </summary>
    public class Vector3D : Vector
    {
        /// <summary>
        /// Private constructor
        /// </summary>
        private Vector3D(MathNet.Numerics.LinearAlgebra.Vector<float> vector)
        {
            _vectorData = MathNet.Numerics.LinearAlgebra.Vector<float>.Build.DenseOfVector(vector);
        }

        /// <summary>
        /// Default constructor. Forms the vector {0, 0, 0}
        /// </summary>
        public Vector3D()
        {
            float[] values = { 0.0f, 0.0f, 0.0f };
            _vectorData = MathNet.Numerics.LinearAlgebra.Vector<float>.Build.DenseOfArray(values);
        }

        /// <summary>
        /// Forms the vector {Value, Value, Value}
        /// </summary>
        /// <param name="value">Value</param>
        public Vector3D(float value)
        {
            float[] values = { value, value, value };
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
        /// Constructor by two points
        /// </summary>
        /// <param name="startPoint">Start point</param>
        /// <param name="endPoint">End point</param>
        public Vector3D(Vector3D startPoint, Vector3D endPoint)
        {
            _vectorData = endPoint._vectorData - startPoint._vectorData;
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

        /// <summary>
        /// Computes the dot product between this vector and another vector.
        /// </summary>
        /// <param name="other">Other vector.</param>
        /// <returns>The sum of a[i]*b[i] for all i.</returns>
        public float DotProduct(Vector3D other)
        {
            if (other == null)
                throw new System.ArgumentNullException("Other vector is null!");

            return _vectorData.DotProduct(other._vectorData);
        }

        /// <summary>
        /// Computes the cross product between this vector and another vector.
        /// </summary>
        /// <param name="other">Other vector.</param>
        /// <returns>New vector.</returns>
        public Vector3D CrossProduct(Vector3D other)
        {
            var x = this.Y * other.Z - other.Y * this.Z;
            var y = (this.X * other.Z - other.X * this.Z) * -1;
            var z = this.X * other.Y - other.X * this.Y;
            return new Vector3D(x, y, z);
        }

        /// <summary>
        /// Get this vector as unit vector.
        /// </summary>
        /// <returns>New unit vector.</returns>
        public Vector3D GetAsUnitVector()
        {
            var unit = MathNet.Spatial.Euclidean.UnitVector3D.Create(_vectorData[0], _vectorData[1], _vectorData[2]);
            return new Vector3D(unit.X, unit.Y, unit.Z);
        }

        /// <summary>
        /// Normalizes vector to a unit vector.
        /// And return new normalized vector.
        /// </summary>
        /// <returns>Normalized vector</returns>
        public Vector3D Normalize()
        {
            return Normalize(1.0f);
        }

        /// <summary>
        /// Normalizes vector to a unit vector with respect to the p-norm.
        /// And return new normalized vector.
        /// </summary>
        /// <returns>Normalized vector</returns>
        public Vector3D Normalize(float length)
        {
            var normVectorData = _vectorData.Normalize(length);
            return new Vector3D(normVectorData);
        }

        /// <summary>
        /// Normalizes this vector to a unit vector.
        /// </summary>
        public void NormalizeSelf()
        {
            NormalizeSelf(1.0f);
        }

        /// <summary>
        /// Normalizes this vector to a unit vector with respect to the p-norm.
        /// </summary>
        private void NormalizeSelf(float length)
        {
            _vectorData = _vectorData.Normalize(length);
        }

        /// <summary>
        /// Indicates whether the current vector is equal to another vector of the same type.
        /// </summary>
        /// <param name="vector3d">Another vector</param>
        /// <returns>true if the current vector is equal to the other vector; otherwise, false.</returns>
        public bool Equals(Vector3D vector3d)
        {
            return _vectorData.Equals(vector3d._vectorData);
        }

        /// <summary>
        /// Implicit type casting (Vector3D -> Vector3D) for the MathNet library.
        /// </summary>
        /// <param name="vector">Vector</param>
        public static implicit operator MathNet.Spatial.Euclidean.Vector3D(Vector3D vector)
        {
            return new MathNet.Spatial.Euclidean.Vector3D(vector.X, vector.Y, vector.Z);
        }

        /// <summary>
        /// Implicit type casting (Vector3D -> Point3D) for the MathNet library.
        /// </summary>
        /// <param name="vector">Vector</param>
        public static implicit operator MathNet.Spatial.Euclidean.Point3D(Vector3D vector)
        {
            return new MathNet.Spatial.Euclidean.Point3D(vector.X, vector.Y, vector.Z);
        }

        /// <summary>
        /// Summation operator.
        /// Adds each element of the given vector to the corresponding element of the defined vector.
        /// This operation is identical to simple vector-vector addition.
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>Resulting vector after summation.</returns>
        public static Vector3D operator +(Vector3D a, Vector3D b)
        {
            return new Vector3D(a._vectorData + b._vectorData);
        }

        /// <summary>
        /// Subtraction operator.
        /// Subtracts each element of the other vector from the corresponding element of the given vector.
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>Resulting vector after subtraction.</returns>
        public static Vector3D operator -(Vector3D a, Vector3D b)
        {
            return new Vector3D(a._vectorData - b._vectorData);
        }

        /// <summary>
        /// Dot multiplication operator.
        /// Dot (scalar) product.
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>Resulting value after dot multiplication.</returns>
        public static float operator *(Vector3D a, Vector3D b)
        {
            return a._vectorData * b._vectorData;
        }

        /// <summary>
        /// Dot multiplication operator.
        /// Multiplies each element of the vector by the given scalar.
        /// </summary>
        /// <param name="a">Vector</param>
        /// <param name="b">Number</param>
        /// <returns>Resulting vector after dot multiplication.</returns>
        public static Vector3D operator *(Vector3D a, float b)
        {
            return new Vector3D(a._vectorData * b);
        }

        /// <summary>
        /// Dot multiplication operator.
        /// Multiplies each element of the vector by the given scalar.
        /// </summary>
        /// <param name="a">Number</param>
        /// <param name="b">Vector</param>
        /// <returns>Resulting vector after dot multiplication.</returns>
        public static Vector3D operator *(float a, Vector3D b)
        {
            return new Vector3D(a * b._vectorData);
        }

        /// <summary>
        /// Pointwise division operator.
        /// Divides each element of this vector by the corresponding element of the other vector.
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>Resulting vector after division.</returns>
        public static Vector3D operator /(Vector3D a, Vector3D b)
        {
            return new Vector3D(a._vectorData / b._vectorData);
        }

        /// <summary>
        /// Pointwise division operator.
        /// Divides each element of this vector by the given scalar.
        /// </summary>
        /// <param name="a">Vector</param>
        /// <param name="b">Number</param>
        /// <returns>Resulting vector after division.</returns>
        public static Vector3D operator /(Vector3D a, float b)
        {
            return new Vector3D(a._vectorData / b);
        }

        /// <summary>
        /// Method return the type of the vector
        /// </summary>
        /// <returns>Type of vector</returns>
        public override VectorType GetVectorType()
        {
            return VectorType.Vector3D;
        }

        /// <summary>
        /// Convert vector to string.
        /// </summary>
        /// <returns>Result string.</returns>
        public override string ToString()
        {
            return base.ToString() + $":\t{X},\t{Y},\t{Z}";
        }
    }

    /// <summary>
    /// Class of a vector in three-dimensional space
    /// </summary>
    public class VectorND : Vector
    {
        /// <summary>
        /// Constructor by float values
        /// </summary>
        /// <param name="values">List of values</param>
        public VectorND(List<float> values)
        {
            _vectorData = MathNet.Numerics.LinearAlgebra.Vector<float>.Build.DenseOfArray(values.ToArray());
        }

        /// <summary>
        /// Constructor by float array
        /// </summary>
        /// <param name="array">Array of dimension N</param>
        public VectorND(float[] array)
        {
            _vectorData = MathNet.Numerics.LinearAlgebra.Vector<float>.Build.DenseOfArray(array);
        }

        /// <summary>
        /// Constructor by double array
        /// </summary>
        /// <param name="array">Array of dimension N</param>
        public VectorND(double[] array)
        {
            var len = array.Length;
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
        public VectorND(VectorND vector)
        {
            _vectorData = vector._vectorData;
        }

        /// <summary>
        /// Constructor by two vectors
        /// </summary>
        /// <param name="startPoint">Start vector</param>
        /// <param name="endPoint">End vector</param>
        public VectorND(VectorND startPoint, VectorND endPoint)
        {
            _vectorData = endPoint._vectorData - startPoint._vectorData;
        }

        /// <summary>
        /// Component by index
        /// </summary>
        public float GetValueAt(int index)
        {
            if (index < 0 || index > _vectorData.Count)
                throw new System.ArgumentOutOfRangeException("id");

            return _vectorData.At(index);
        }

        /// <summary>
        /// Indicates whether the current vector is equal to another vector of the same type.
        /// </summary>
        /// <param name="vector">Another vector</param>
        /// <returns>true if the current vector is equal to the other vector; otherwise, false.</returns>
        public bool Equals(VectorND vector)
        {
            return _vectorData.Equals(vector._vectorData);
        }

        /// <summary>
        /// Pointwise division operator.
        /// Divides each element of this vector by the given scalar.
        /// </summary>
        /// <param name="a">Vector</param>
        /// <param name="b">Number</param>
        /// <returns>Resulting vector after division.</returns>
        public static VectorND operator /(VectorND a, float b)
        {
            return new VectorND((a._vectorData / b).ToArray());
        }

        /// <summary>
        /// Dot multiplication operator.
        /// Multiplies each element of the vector by the given scalar.
        /// </summary>
        /// <param name="a">Vector</param>
        /// <param name="b">Number</param>
        /// <returns>Resulting vector after dot multiplication.</returns>
        public static VectorND operator *(VectorND a, float b)
        {
            return new VectorND((a._vectorData * b).ToArray());
        }

        /// <summary>
        /// Dot multiplication operator.
        /// Multiplies each element of the vector by the given scalar.
        /// </summary>
        /// <param name="a">Number</param>
        /// <param name="b">Vector</param>
        /// <returns>Resulting vector after dot multiplication.</returns>
        public static VectorND operator *(float a, VectorND b)
        {
            return new VectorND((a * b._vectorData).ToArray());
        }

        /// <summary>
        /// Dot multiplication operator.
        /// Dot (scalar) product.
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>Resulting value after dot multiplication.</returns>
        public static float operator *(VectorND a, VectorND b)
        {
            return a._vectorData * b._vectorData;
        }

        /// <summary>
        /// Summation operator.
        /// Adds each element of the given vector to the corresponding element of the defined vector.
        /// This operation is identical to simple vector-vector addition.
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>Resulting vector after summation.</returns>
        public static VectorND operator +(VectorND a, VectorND b)
        {
            return new VectorND((a._vectorData + b._vectorData).ToArray());
        }

        /// <summary>
        /// Subtraction operator.
        /// Subtracts each element of the other vector from the corresponding element of the given vector.
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>Resulting vector after subtraction.</returns>
        public static VectorND operator -(VectorND a, VectorND b)
        {
            return new VectorND((a._vectorData + b._vectorData).ToArray());
        }

        /// <summary>
        /// Negative operator.
        /// The operation is identical to multiplying a vector by minus one.
        /// </summary>
        /// <param name="a">Vector</param>
        /// <returns>Negative vector.</returns>
        public static VectorND operator -(VectorND a)
        {
            return new VectorND((-a._vectorData).ToArray());
        }

        /// <summary>
        /// Method return the type of the vector
        /// </summary>
        /// <returns>Type of vector</returns>
        public override VectorType GetVectorType()
        {
            return VectorType.VectorND;
        }
    }
}

using Ariadne.Kernel.Math;
using System.Collections.Generic;

namespace Ariadne.Kernel
{
    /// <summary>
    /// Delegate defining a single node shape function.
    /// </summary>
    /// <param name="u">U-coordinate.</param>
    /// <param name="v">V-coordinate.</param>
    /// <param name="w">W-coordinate.</param>
    /// <returns></returns>
    public delegate float ShapeFunctor(float u, float v, float w);

    /// <summary>
    /// Delegate defining the general form of the shape function.
    /// </summary>
    /// <param name="coords">XYZ-coords vector.</param>
    /// <param name="nodalValues">Nodal values - characteristic coefficients of the shape function.</param>
    /// <returns></returns>
    public delegate Vector3D GlobalShapeFunctor(Vector3D coords, List<Vector3D> nodalValues);

    /// <summary>
    /// Abstract class of the shape functions
    /// </summary>
    public abstract class ShapeFunction
    {
        protected List<ShapeFunctor> _localFunctors = new List<ShapeFunctor>();
        protected GlobalShapeFunctor _globalFunctor = null;

        /// <summary>
        /// Dimension of shape function
        /// </summary>
        /// <returns></returns>
        public int SpaceDimension { get { return (int)GetShapeFunctionType(); } }

        /// <summary>
        /// Nodes in shape function
        /// </summary>
        public int Size { get { return _localFunctors.Count; } }

        /// <summary>
        /// Find the UVW-coords by solving the optimization problem.
        /// </summary>
        /// <param name="xyz">Target point in XYZ-space.</param>
        /// <param name="nodalCoords">Nodal coordinates from a specific element.</param>
        /// <returns>UVW-coords.</returns>
        public Vector3D FindUVW(Vector3D xyz, List<Vector3D> nodalCoords)
        {
            if (_globalFunctor != null)
                return _globalFunctor(xyz, nodalCoords);

            return new Vector3D(float.NaN, float.NaN, float.NaN);
        }

        /// <summary>
        /// Calculate specific value by solve general form a shape function. 
        /// </summary>
        /// <param name="uvw">Target point in UVW-space.</param>
        /// <param name="nodalValues">Nodal values from a specific element.</param>
        /// <returns></returns>
        public Vector3D Calculate(Vector3D uvw, List<Vector3D> nodalValues)
        {
            if (Size != nodalValues.Count)
                throw new System.IndexOutOfRangeException("Size != number of nodes!");

            var value = new Vector3D();
            for (int i = 0; i < Size; i++)
            {
                value.X += _localFunctors[i](uvw.X, uvw.Y, uvw.Z) * nodalValues[i].X;
                value.Y += _localFunctors[i](uvw.X, uvw.Y, uvw.Z) * nodalValues[i].Y;
                value.Z += _localFunctors[i](uvw.X, uvw.Y, uvw.Z) * nodalValues[i].Z;
            }
            return value;
        }

        public Matrix3x3 Calculate(Vector3D uvw, Set<Matrix3x3> nodalValues)
        {
            if (Size != nodalValues.Count)
                throw new System.IndexOutOfRangeException("Size != number of matrixes!");

            var values = new Matrix3x3();
            for (int i = 0; i < Size; i++)
            {
                for(int x = 0; x < values.GetSize()[0]; x++)
                    for (int y = 0; y < values.GetSize()[1]; y++)
                        values[x, y] += _localFunctors[i](uvw.X, uvw.Y, uvw.Z) * nodalValues[i][x, y];
            }
            return values;
        }

        /// <summary>
        /// Virtual method to return the type of the shape function
        /// </summary>
        /// <returns>Type of shape function</returns>
        public abstract ShapeFunctionType GetShapeFunctionType();
    }

    /// <summary>
    /// Enumeration of all available types of shape functions and dimension
    /// </summary>
    public enum ShapeFunctionType
    {
        Rod_2 = 1,
        Beam_2 = 1,
        Triangle_3 = 2,
        Triangle_6 = 2,
        Triangle_10 = 2,
        Triangle_15 = 2,
        Quadrilateral_4 = 2,
        Quadrilateral_8 = 2,
        Quadrilateral_9 = 2,
        Tetrahedron_4 = 3,
        Hexahedron_8 = 3,
        Hexahedron_14 = 3,
        Hexahedron_20 = 3
    }
}

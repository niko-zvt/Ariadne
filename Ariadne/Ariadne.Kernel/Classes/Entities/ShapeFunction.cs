using Ariadne.Kernel.Math;
using System;
using System.Collections.Generic;

namespace Ariadne.Kernel
{
    public delegate float Functor(float u, float v, float w);

    public delegate Vector3D GlobalFunctor(Vector3D point, NodeSet nodes);

    /// <summary>
    /// Abstract class of the shape functions
    /// </summary>
    public abstract class ShapeFunction
    {
        protected List<Functor> _localFunctors = new List<Functor>();

        protected GlobalFunctor _globalFunctor = null;

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
        /// Virtual method to return a matrixes of shape values ​​calculated from a vector
        /// </summary>
        /// <param name="naturalCoords">Natural coordinates of point</param>
        /// <returns>Matrixes of shape values</returns>
        public Dictionary<int, Matrix3x3> GetMatrixFromNaturalPoint(Vector3D naturalCoords)
        {
            var matrixes = new Dictionary<int, Matrix3x3>();
            for (int i = 0; i < Size; i++)
            {
                var C = _localFunctors[i](naturalCoords.X, naturalCoords.Y, naturalCoords.Z);
                var matix = Matrix3x3.BuildDiagonal(C, C, C);
                matrixes.Add(i, matix);
            }
            return matrixes;
        }

        public Vector3D CalculateUV(Vector3D coords, NodeSet nodes)
        {
            if (_globalFunctor != null)
                return _globalFunctor(coords, nodes);

            return new Vector3D(float.NaN, float.NaN, float.NaN);
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

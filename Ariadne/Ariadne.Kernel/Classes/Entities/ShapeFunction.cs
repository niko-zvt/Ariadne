using Ariadne.Kernel.Math;
using System;
using System.Collections.Generic;

namespace Ariadne.Kernel
{
    public delegate float Functor(float u, float v, float w);

    public delegate Vector3D GlobalFunctor(Vector3D point, List<Vector3D> nodesCoords);

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

        public Vector3D CalculateUV(Vector3D coords, List<Vector3D> nodesCoords)
        {
            if (_globalFunctor != null)
                return _globalFunctor(coords, nodesCoords);

            return new Vector3D(float.NaN, float.NaN, float.NaN);
        }

        public Vector3D CalculateXYZ(Vector3D uv, List<Vector3D> nodesCoords)
        {
            var xyz = new Vector3D();
            for (int i = 0; i < Size; i++)
            {
                xyz.X += _localFunctors[i](uv.X, uv.Y, uv.Z) * nodesCoords[i].X;
                xyz.Y += _localFunctors[i](uv.X, uv.Y, uv.Z) * nodesCoords[i].Y;
                xyz.Z += _localFunctors[i](uv.X, uv.Y, uv.Z) * nodesCoords[i].Z;
            }
            return xyz;
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

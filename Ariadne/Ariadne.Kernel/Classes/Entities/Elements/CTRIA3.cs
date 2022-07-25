﻿using Ariadne.Kernel.Math;
using System.Collections.Generic;

namespace Ariadne.Kernel
{
    /// <summary>
    /// CTRIA3 element.
    /// This class defines a triangular, isoparametric membrane-bending or plane strain plate element
    /// </summary>
    class CTRIA3 : Element
    {
        /// <summary>
        /// Constructor by parameters
        /// </summary>
        /// <param name="parameters">Element parameters</param>
        public CTRIA3(ElementParams parameters) : base(parameters)
        {
            ShapeFunction = Triangle3.GetFunction;
        }

        /// <summary>
        /// Returns the element type
        /// </summary>
        /// <returns>Element type</returns>
        public override ElementType GetElementType()
        {
            return ElementType.CTRIA3;
        }

        /// <summary>
        /// Method for checking whether a point belongs to an element
        /// </summary>
        /// <param name="point">Point</param>
        /// <returns>true if point belong to an element; otherwise return - false</returns>
        public override bool IsPointBelong(Vector3D point)
        {
            if (GetBoundingBoxAsRef().IsPointBelong(point) == true &&
                IsPointInsideElement(point) == true)
                return true;

            return false;
        }

        /// <summary>
        /// Build local coordinate system of element.
        /// </summary>
        /// <returns>Local CS or null</returns>
        protected override LocalCSys BuildElementLCS()
        {
            // 1. Calculate origin point for LCS
            var originPoint = Coords;
            var corners = GetCornerNodes();
            var P1 = corners.GetByIndex(0).Coords;
            var P2 = corners.GetByIndex(1).Coords;
            var P3 = corners.GetByIndex(2).Coords;

            // 2. Calculate xAxis
            var xAxis = (P2 - P1).GetAsUnitVector();

            // 3. Calculate zAxis
            var zAxis = ((P2 - P1).CrossProduct(P3 - P2)).GetAsUnitVector();

            // 4. Calculate zAxis
            var yAxis = (zAxis.CrossProduct(xAxis)).GetAsUnitVector();

            return new LocalCSys(xAxis, yAxis, zAxis, originPoint);
        }

        /// <summary>
        /// Return the result of validate the element
        /// </summary>
        /// <returns>Returns true if the element is valid, otherwise - false</returns>
        public override bool IsValid()
        {
            if(NodeIDs.Count < 0 || CornerNodeIDs.Count < 0)
                return false;
            
            return true;
        }

        /// <summary>
        /// Get UV-coords by point location in 3D space.
        /// </summary>
        /// <param name="point">Target point.</param>
        /// <param name="isCalculateByLCS">true - if you need to calculate UVW-coords by local CS, false - by global CS.</param>
        /// <returns>UV-coords or NULL.</returns>
        public override Vector3D GetUVWCoordsByPoint(Vector3D point, bool isCalculateByLCS = false)
        {
            var tolerance = 1e-4f;

            // 1. Check point
            if (IsPointBelong(point) != true)
                return null;

            // 2. Calculate coords of nodes in LCS
            var coorsOfNodes = new List<Vector3D>();
            foreach (var coord in GetNodes().GetAllCoords())
            {
                coorsOfNodes.Add(new Vector3D(coord));
            }

            // 3. Points GCS -> LCS (optional)
            if (isCalculateByLCS)
            {
                var map = new AffineMap3D(GetElementLCSAsRef());
                point = map.TransformPoint(point);
                coorsOfNodes = map.TransformPoints(coorsOfNodes);
            }

            // 4. Calculate UVW-coords
            var uvw = ShapeFunction.FindUVW(point, coorsOfNodes);
            if (uvw.IsValid() == false)
                throw new System.ArgumentNullException("Natural coords is NAN!");

            // 5. Calculate prob - the difference between a real point and its approximation
            var prob = ShapeFunction.Calculate(uvw, coorsOfNodes);
            if ((point - prob).Length > tolerance)
                throw new System.ArgumentNullException("Natural coords is invalid!");

            return uvw;
        }

        /// <summary>
        /// Build a centroid coords of current element.
        /// </summary>
        /// <returns>Centroid coords.</returns>
        protected override Vector3D BuildElementCentroid()
        {
            return Coords;
        }
    }

    /// <summary>
    /// CTRIA3 element creator
    /// </summary>
    class CTRIA3Creator : ElementCreator
    {
        /// <summary>
        /// Specific element
        /// </summary>
        Element element;

        /// <summary>
        /// Constructor by parameters
        /// </summary>
        /// <param name="parameters">Element parameters</param>
        public CTRIA3Creator(ElementParams parameters) : base(parameters)
        {
            element = new CTRIA3(parameters);
        }

        /// <summary>
        /// Factory method. Builds and returns a new specific element
        /// </summary>
        /// <returns>Specific element</returns>
        public override Element BuildElement()
        {
            return element;
        }
    }
}

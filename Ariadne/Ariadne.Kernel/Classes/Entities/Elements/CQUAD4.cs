using Ariadne.Kernel.Math;
using System.Collections.Generic;

namespace Ariadne.Kernel
{
    /// <summary>
    /// CQUAD4 element.
    /// This class defines a quadrilateral, isoparametric membrane-bending or plane strain plate element
    /// </summary>
    class CQUAD4 : Element
    {
        /// <summary>
        /// Constructor by parameters
        /// </summary>
        /// <param name="parameters">Element parameters</param>
        public CQUAD4(ElementParams parameters):base(parameters)
        {
            ShapeFunction = Quadrilateral4.GetFunction;
        }

        /// <summary>
        /// Returns the element type
        /// </summary>
        /// <returns>Element type</returns>
        public override ElementType GetElementType()
        {
            return ElementType.CQUAD4;
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
        /// Get UV-coords by point location in 3D space.
        /// </summary>
        /// <param name="point">Target point.</param>
        /// <returns>UV-coords or NULL.</returns>
        public override Vector3D GetUVCoordsByPoint(Vector3D point)
        {
            // 1. Check point
            if (IsPointBelong(point) != true)
                return null;

            // 2. Calculate affine map GCS -> LCS
            var map = GetElementLCSAsRef().GetMapToLocalCS();

            // 3. Calculate target point in LCS
            var localPoint = map.TransformPoint(point);

            // 4. Calculate coords of nodes in LCS
            var coorsOfNodes = new List<Vector3D>();
            foreach(var coord in GetNodes().GetAllCoords())
            {
                coorsOfNodes.Add(new Vector3D(coord));
            }
            var localCoorsOfNodes = map.TransformPoints(coorsOfNodes);

            // 4. Calculate UV-coords
            var uvw = ShapeFunction.FindUVW(point, coorsOfNodes);

            var test = ShapeFunction.Calculate(uvw, coorsOfNodes);

            if (uvw.IsValid() == false)
                throw new System.ArgumentNullException("Natural coords is NAN!");

            return uvw;
        }

        /// <summary>
        /// Build local coordinate system of element.
        /// </summary>
        /// <returns>Local CS or null</returns>
        protected override LocalCSys BuildElementLCS()
        {
            // 1. Calculate origin point for LCS
            var originPoint = Coords;

            // 2. Calculate centroid
            var centroid = CentroidCoords;
            var corners = GetCornerNodes();
            var P1 = corners.GetByIndex(0).Coords;
            var P2 = corners.GetByIndex(1).Coords;
            var P3 = corners.GetByIndex(2).Coords;
            var P4 = corners.GetByIndex(3).Coords;

            // 3. Calculate xAxis
            var x = new Vector3D(centroid, Utils.CalculateIntersectionOfBisectorAndBase(centroid, P2, P3));
            var xAxis = (x).GetAsUnitVector();

            // 4. Calculate zAxis
            var zAxis = ((P3 - P1).CrossProduct(P4 - P2)).GetAsUnitVector();

            // 5. Calculate zAxis
            var yAxis = (zAxis.CrossProduct(xAxis)).GetAsUnitVector();
            
            return new LocalCSys(xAxis, yAxis, zAxis, originPoint);
        }

        /// <summary>
        /// Return the result of validate the element
        /// </summary>
        /// <returns>Returns true if the element is valid, otherwise - false</returns>
        public override bool IsValid()
        {
            if (NodeIDs.Count < 0 || CornerNodeIDs.Count < 0)
                return false;

            return true;
        }

        /// <summary>
        /// Build a centroid coords of current element.
        /// </summary>
        /// <returns>Centroid coords.</returns>
        protected override Vector3D BuildElementCentroid()
        {
            var centroid = new Vector3D();
            var corners = GetCornerNodes();
            var P1 = corners.GetByIndex(0).Coords;
            var P2 = corners.GetByIndex(1).Coords;
            var P3 = corners.GetByIndex(2).Coords;
            var P4 = corners.GetByIndex(3).Coords;
            var intersectionType = Utils.CalculateIntersectionOfTwoSegments(P1, P3, P2, P4, out var intersectPoints);
            if (intersectionType == Utils.IntersectionType.Point)
            {
                centroid = intersectPoints[0];
            }
            else
            {
                throw new System.ArgumentOutOfRangeException("There must be only one point of intersection!");
            }
            return centroid;
        }
    }

    /// <summary>
    /// CQUAD4 element creator
    /// </summary>
    class CQUAD4Creator : ElementCreator
    {
        /// <summary>
        /// Specific element
        /// </summary>
        Element element;

        /// <summary>
        /// Constructor by parameters
        /// </summary>
        /// <param name="parameters">Element parameters</param>
        public CQUAD4Creator(ElementParams parameters) : base(parameters)
        {
            element = new CQUAD4(parameters);
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

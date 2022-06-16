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
        public override Vector3D GetUVCoordsPoint(Vector3D point)
        {
            // Check point
            if (IsPointBelong(point) != true)
                return null;

            // 2. Calculate affine map GCS -> LCS
            var LCS = GetElementLCSAsRef();
            var GCS = GlobalCSys.Instance;
            //var transform_L2G = new AffineMap3D(GCS, LCS);

            // 3. Calculate target point and corners in LCS
            var corners = GetCornerNodes();
            var localN1 = AffineMap3D.TransformPoint(corners.GetByIndex(0).Coords, LCS, GCS);
            var localN2 = AffineMap3D.TransformPoint(corners.GetByIndex(1).Coords, LCS, GCS);
            var localN3 = AffineMap3D.TransformPoint(corners.GetByIndex(2).Coords, LCS, GCS);
            var localN4 = AffineMap3D.TransformPoint(corners.GetByIndex(3).Coords, LCS, GCS);
            var localPoint = AffineMap3D.TransformPoint(point, LCS, GCS);

            System.Console.WriteLine(localPoint.ToString());


            // TODO: LCS -> UV

            // 4. Calculate LCS -> UV

            // 5. Return uv

            throw new System.NotImplementedException();
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

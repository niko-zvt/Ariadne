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
            // 1. Check point
            if (IsPointBelong(point) != true)
                return null;

            // 2. Calculate affine map GCS -> LCS
            var LCS = GetElementLCSAsRef();
            var GCS = GlobalCSys.Instance;

            // 3. Calculate target point in LCS
            var localPoint = AffineMap3D.TransformPoint(point, LCS, GCS);

            var matrix = GetElementNaturalMatrixAsRef() as MatrixNxM;
            if (matrix == null || matrix is not MatrixNxM)
                throw new System.ArgumentNullException("Natural coords matrix is null!");

            var uv = ShapeFunction.CalculateShape(localPoint, matrix);

            if (uv == null)
                throw new System.ArgumentNullException("Natural coords is null!");

            return uv;
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
        /// Build a matrix of coefficients to determine natural coordinates
        /// </summary>
        /// <returns>Matrix of coefficients</returns>
        protected override Matrix BuildNaturalCoordMatrix()
        {
            //
            // See INFO_NATURAL_COORDS.md and        
            //
            // Sulaiman Y. Abo Diab, "Complete Pascal Interpolation Scheme
            // For Approximating The Geometry Of A Quadrilateral Element"
            // DOI: https://doi.org/10.48550/arXiv.1709.04765
            //

            // 1. Local and Global CS's
            var LCS = GetElementLCSAsRef();
            var GCS = GlobalCSys.Instance;

            // 2. Get nodes, centroid and origin point
            var corners = GetCornerNodes();
            var localN1 = AffineMap3D.TransformPoint(corners.GetByIndex(0).Coords, LCS, GCS);
            var localN2 = AffineMap3D.TransformPoint(corners.GetByIndex(1).Coords, LCS, GCS);
            var localN3 = AffineMap3D.TransformPoint(corners.GetByIndex(2).Coords, LCS, GCS);
            var localN4 = AffineMap3D.TransformPoint(corners.GetByIndex(3).Coords, LCS, GCS);
            var localOrigin = AffineMap3D.TransformPoint(Coords, LCS, GCS);
            var localCentroid = AffineMap3D.TransformPoint(CentroidCoords, LCS, GCS);

            // 3. Calculate P5 and P6 points
            Vector3D P5 = null;
            Vector3D P6 = null;
            if (Utils.CalculateIntersectionOfTwoLines(localN1, localN2, localN4, localN3, out var firstPoint) == Utils.IntersectionType.Point)
                P5 = firstPoint[0];
            if (Utils.CalculateIntersectionOfTwoLines(localN2, localN3, localN1, localN4, out var secondPoint) == Utils.IntersectionType.Point)
                P6 = secondPoint[0];

            if (P5 == null || P6 == null)
                throw new System.ArgumentException("CQUAD4 is a cquare!");

            // 4. Calculate mid-points
            var RG = localOrigin;
            var R7 = (localN1 + localN2) / 2;
            var R8 = (localN2 + localN3) / 2;
            var R9 = (localN3 + localN4) / 2;
            var R10 = (localN4 + localN1) / 2;

            System.Console.WriteLine(localN1);
            System.Console.WriteLine(localN2);
            System.Console.WriteLine(localN3);
            System.Console.WriteLine(localN4);
            System.Console.WriteLine(localOrigin);
            System.Console.WriteLine(localCentroid);
            System.Console.WriteLine(P5);
            System.Console.WriteLine(P6);
            System.Console.WriteLine(R7);
            System.Console.WriteLine(R8);
            System.Console.WriteLine(R9);
            System.Console.WriteLine(R10);

            // 5. Construct a matrix of natural coordinates of nodes
            var t1_1 = (P5 - RG).Length / (R8 - RG).Length;
            var t1_2 = -1 * ((P5 - RG).Length / (R10 - RG).Length);

            var t2_1 = (P6 - RG).Length / (R9 - RG).Length;
            var t2_2 = -1 * ((P6 - RG).Length / (R7 - RG).Length);

            float t1 = t1_1;
            float t2 = t2_1;

            var A = new MatrixNxM(new float[,] { {  1, -1, -1,   1,    1,   1   },
                                                 {  1,  1, -1,   1,   -1,   1   },
                                                 {  1,  1,  1,   1,    1,   1   },
                                                 {  1, -1,  1,   1,   -1,   1   },
                                                 {  1, t1,  0, t1*t1,  0,   0   },
                                                 {  1,  0, t2,   0,    0, t2*t2 }  });

            var detA = A.Determinant();
            if(detA <= 0.0f)
                throw new System.ArgumentException("det(A) < 0!");

            var B = A.Inverse() as MatrixNxM;

            var coordsNodes = new List<Vector3D>() { localN1,
                                                     localN2,
                                                     localN3,
                                                     localN4,
                                                     P5,
                                                     P6 };
            
            var a = MatrixNxM.ConvertToMatrix(B.MultyPly(coordsNodes));

            return a;
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

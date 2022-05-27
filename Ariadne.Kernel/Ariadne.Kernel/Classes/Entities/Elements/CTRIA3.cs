using Ariadne.Kernel.Math;

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
        /// The method checks whether the point is inside the Element
        /// </summary>
        /// <param name="point">Point</param>
        /// <returns>Returns true if point inside the element, otherwise - false</returns>
        private bool IsPointInsideElement(Vector3D point)
        {
            var corners = GetCornerNodesAsRef();
            if (corners.Count == 0)
                return false;

            throw new System.NotImplementedException();

            return true;
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

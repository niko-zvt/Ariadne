namespace Ariadne.Kernel
{
    /// <summary>
    /// PSHELL property.
    /// This class defines the membrane, bending, and transverse shear properties of shell elements (CTRIA3, CTRIAR, CQUAD4, and CQUADR entries).
    /// </summary>
    class PSHELL : Property
    {
        /// <summary>
        /// Constructor by parameters
        /// </summary>
        /// <param name="parameters">Property parameters</param>
        public PSHELL(PropertyParams parameters) : base(parameters)
        {
        }

        /// <summary>
        /// Returns the property type
        /// </summary>
        /// <returns>Property type</returns>
        public override PropertyType GetPropertyType()
        {
            return PropertyType.PSHELL;
        }
    }

    /// <summary>
    /// PSHELL property creator
    /// </summary>
    class PSHELLCreator : PropertyCreator
    {
        /// <value>
        /// Specific property
        /// </value>
        Property property;

        /// <summary>
        /// Constructor by parameters
        /// </summary>
        /// <param name="parameters">Property parameters</param>
        public PSHELLCreator(PropertyParams parameters) : base(parameters)
        {
            property = new PSHELL(parameters);
        }

        /// <summary>
        /// Factory method. Builds and returns a new specific property
        /// </summary>
        /// <returns>Specific property</returns>
        public override Property BuildProperty()
        {
            return property;
        }
    }
}

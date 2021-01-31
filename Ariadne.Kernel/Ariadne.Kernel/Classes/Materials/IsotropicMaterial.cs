namespace Ariadne.Kernel
{
    /// <summary>
    /// Isotropic material.
    /// This class defines an isotropic material. He is characterized by two elastic parameters: Young’s modulus and Poisson’s ratio
    /// </summary>
    class IsotropicMaterial : Material
    {
        /// <summary>
        /// Constructor by parameters
        /// </summary>
        /// <param name="parameters">Material parameters</param>
        public IsotropicMaterial(MaterialParams parameters) : base(parameters)
        {
            // TODO: Params
        }

        /// <summary>
        /// Returns the material type
        /// </summary>
        /// <returns>Material type</returns>
        public override MaterialType GetMaterialType()
        {
            return MaterialType.Isotropic;
        }
    }

    /// <summary>
    /// Isotropic material creator
    /// </summary>
    class IsoMaterialCreator : MaterialCreator
    {
        /// <value>
        /// Specific material
        /// </value>
        Material material;

        /// <summary>
        /// Constructor by parameters
        /// </summary>
        /// <param name="parameters">Material parameters</param>
        public IsoMaterialCreator(MaterialParams parameters) : base(parameters)
        {
            material = new IsotropicMaterial(parameters);
        }

        /// <summary>
        /// Factory method. Builds and returns a new specific material
        /// </summary>
        /// <returns>Specific material</returns>
        public override Material BuildMaterial()
        {
            return material;
        }
    }
}

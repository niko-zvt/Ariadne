namespace Ariadne.Kernel
{
    /// <summary>
    /// Orthotropic material.
    /// This class defines an orthotropic material. 
    /// He has at least 2 orthogonal planes of symmetry, where material properties are independent of direction within each plane. 
    /// Such materials require 9 independent variables (i.e. elastic constants) in their constitutive matrices.
    /// </summary>
    class OrthotropicMaterial : Material
    {
        /// <summary>
        /// Constructor by parameters
        /// </summary>
        /// <param name="parameters">Material parameters</param>
        public OrthotropicMaterial(MaterialParams parameters) : base(parameters)
        {
            // TODO: Params
        }

        /// <summary>
        /// Returns the material type
        /// </summary>
        /// <returns>Material type</returns>
        public override MaterialType GetMaterialType()
        {
            return MaterialType.Orthotropic;
        }
    }

    /// <summary>
    /// Orthotropic material creator
    /// </summary>
    class OrthoMaterialCreator : MaterialCreator
    {
        /// <value>
        /// Specific material
        /// </value>
        Material material;

        /// <summary>
        /// Constructor by parameters
        /// </summary>
        /// <param name="parameters">Material parameters</param>
        public OrthoMaterialCreator(MaterialParams parameters) : base(parameters)
        {
            material = new OrthotropicMaterial(parameters);
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

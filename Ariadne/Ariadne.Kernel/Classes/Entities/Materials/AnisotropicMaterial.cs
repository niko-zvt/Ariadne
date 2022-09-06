// Copyright 2022 Nikolay V. Zhivotenko
// Licensed under the Apache License, Version 2.0
// E-mail: niko.zvt@gmail.com

namespace Ariadne.Kernel
{
    /// <summary>
    /// Anisotropic material.
    /// This class defines an anisotropic material. 
    /// The properties of the material depend on the direction.
    /// Such materials require 21 independent variables (i.e. elastic constants) in their constitutive matrices.
    /// </summary>
    class AnisotropicMaterial : Material
    {
        /// <summary>
        /// Constructor by parameters
        /// </summary>
        /// <param name="parameters">Material parameters</param>
        public AnisotropicMaterial(MaterialParams parameters) : base(parameters)
        {
            // TODO: Params
        }

        /// <summary>
        /// Returns the material type
        /// </summary>
        /// <returns>Material type</returns>
        public override MaterialType GetMaterialType()
        {
            return MaterialType.Anisotropic;
        }
    }

    /// <summary>
    /// Orthotropic material creator
    /// </summary>
    class AnisoMaterialCreator : MaterialCreator
    {
        /// <summary>
        /// Specific material
        /// </summary>
        Material material;

        /// <summary>
        /// Constructor by parameters
        /// </summary>
        /// <param name="parameters">Material parameters</param>
        public AnisoMaterialCreator(MaterialParams parameters) : base(parameters)
        {
            material = new AnisotropicMaterial(parameters);
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

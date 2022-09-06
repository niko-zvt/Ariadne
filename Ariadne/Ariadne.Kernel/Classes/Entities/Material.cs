// Copyright 2022 Nikolay V. Zhivotenko
// Licensed under the Apache License, Version 2.0
// E-mail: niko.zvt@gmail.com

using System;

namespace Ariadne.Kernel
{
    /// <summary>
    /// Abstract class of the material
    /// </summary>
    public abstract class Material
    {
        /// <summary>
        /// Material data
        /// </summary>
        // TODO: Get rid of hiding the given object
        public object TEMPMaterialData { get; protected set; } 

        /// <summary>
        /// Protected constructor with parameters
        /// </summary>
        /// <param name="parameters">Material parameters</param>
        protected Material(MaterialParams parameters)
        {
            ID = parameters.ID;
            if (parameters.Data != null && parameters.Data is object[])
            {
                TEMPMaterialData = parameters.Data;
            }
        }

        /// <summary>
        /// Material identifier
        /// </summary>
        public int ID { get; protected set; }

        /// <summary>
        /// Virtual method to return the type of the material
        /// </summary>
        /// <returns>Type of material</returns>
        public abstract MaterialType GetMaterialType();
    }

    /// <summary>
    /// Abstract class of the material creator
    /// </summary>
    public abstract class MaterialCreator
    {
        /// <summary>
        /// Protected base constructor for implementing abstract parameter passing when creating a specific material class.
        /// </summary>
        /// <param name="parameters">Material parameters</param>
        protected MaterialCreator(MaterialParams parameters) { }

        /// <summary>
        /// A virtual function for building materials. Must be implemented in each specific class and returns the constructed material
        /// The same factory method
        /// </summary>
        /// <returns>Some constructed material</returns>
        public abstract Material BuildMaterial();

        /// <summary>
        /// Returns the specific creator of the material using the data contained in the passed parameters
        /// </summary>
        /// <param name="parameters">Material parameters</param>
        /// <returns>A specific instance of the material creator</returns>
        public static MaterialCreator GetMaterialCreatorByParams(MaterialParams parameters)
        {
            MaterialCreator creator = null;
            var type = parameters.TypeName;

            // TODO: Hide the implementation of selecting a specific creator, so that the registration of a specific type occurs somewhere in a separate place
            if (type == "isotropic")
            {
                creator = new IsoMaterialCreator(parameters);
            }
            else if (type == "orthotropic")
            {
                creator = new OrthoMaterialCreator(parameters);
            }
            else if (type == "anisotropic")
            {
                creator = new AnisoMaterialCreator(parameters);
            }
            else
            {
                throw new ArgumentOutOfRangeException("Unknown type of material");
            }

            // TODO: Insert new type of materials

            return creator;
        }

        /// <summary>
        /// Returns the name of the material type by the name of its subtype
        /// </summary>
        /// <param name="subtypeName">Subtype name</param>
        /// <returns>Material type name or NULL</returns>
        public static string GetMaterialTypeNameBySubtype(string subtypeName)
        {
            if (subtypeName == "MAT1" || 
                subtypeName == "MAT4")
            {
                return "isotropic";
            }
            else if (subtypeName == "MAT2" ||
                     subtypeName == "MAT5" ||
                     subtypeName == "MAT9")
            {
                return "anisotropic";
            }
            else if (subtypeName == "MAT3" ||
                     subtypeName == "MAT8")
            {
                return "orthotropic";
            }

            // TODO: Registration of new subtypes of materials takes place here

            return null;
        }
    }

    /// <summary>
    /// The basic structure of the generalized parameters of the material
    /// </summary>
    public struct MaterialParams
    {
        /// <summary>
        /// Material identifier
        /// </summary>
        public int ID;

        /// <summary>
        /// Name of the material type
        /// </summary>
        public string TypeName;

        /// <summary>
        /// Name of the material subtype
        /// </summary>
        public string SubtypeName;

        /// <summary>
        /// Complete material object
        /// </summary>
        public object Data;
    }

    /// <summary>
    /// Enumeration of all available types of materials
    /// </summary>
    public enum MaterialType
    {
        Undefine,
        Isotropic,
        Orthotropic,
        Anisotropic,
    }

    /// <summary>
    /// Enumeration of all available subtypes of materials
    /// </summary>
    public enum MaterialSubtype
    {
        MAT1,
        MAT2,
        MAT3,
        MAT4,
        MAT5,
        MAT8,
        MAT9,
        MAT10,
        MAT11,
        MATFT,
        MATG,
        MATHE,
        MATHP,
        MATS1,
        MATSMA,
        MATT1,
        MATT2,
        MATT3,
        MATT4,
        MATT5,
        MATT8,
        MATT9,
        MATT11,
        MATTC,
        MFLUID,
        CREEP,
    }
}

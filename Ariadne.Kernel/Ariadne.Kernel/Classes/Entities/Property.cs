using System;

namespace Ariadne.Kernel
{
    /// <summary>
    /// Abstract class of the property
    /// </summary>
    abstract class Property
    {
        /// <summary>
        /// Property data
        /// </summary>
        // TODO: Get rid of hiding the given object
        public object TEMPPropertyData { get; protected set; } 

        /// <summary>
        /// Protected constructor with parameters
        /// </summary>
        /// <param name="parameters">Property parameters</param>
        protected Property(PropertyParams parameters)
        {
            ID = parameters.ID;
            if (parameters.Data != null && parameters.Data is object[])
            {
                TEMPPropertyData = parameters.Data;
            }
        }

        /// <summary>
        /// Property identifier
        /// </summary>
        public int ID { get; protected set; }

        /// <summary>
        /// Virtual method to return the type of the property
        /// </summary>
        /// <returns>Type of property</returns>
        public abstract PropertyType GetPropertyType();
    }

    /// <summary>
    /// Abstract class of the property creator
    /// </summary>
    abstract class PropertyCreator
    {
        /// <summary>
        /// Protected base constructor for implementing abstract parameter passing when creating a specific property class.
        /// </summary>
        /// <param name="parameters">Property parameters</param>
        protected PropertyCreator(PropertyParams parameters) { }

        /// <summary>
        /// A virtual function for building properties. Must be implemented in each specific class and returns the constructed property
        /// The same factory method
        /// </summary>
        /// <returns>Some constructed property</returns>
        public abstract Property BuildProperty();

        /// <summary>
        /// Returns the specific creator of the property using the data contained in the passed parameters
        /// </summary>
        /// <param name="parameters">Property parameters</param>
        /// <returns>A specific instance of the property creator</returns>
        public static PropertyCreator GetPropertyCreatorByParams(PropertyParams parameters)
        {
            PropertyCreator creator = null;
            var type = parameters.TypeName;

            // TODO: Hide the implementation of selecting a specific creator, so that the registration of a specific type occurs somewhere in a separate place
            if (type == "PSHELL")
            {
                creator = new PSHELLCreator(parameters);
            }
            else
            {
                throw new ArgumentOutOfRangeException("Unknown type of property");
            }

            // TODO: Insert new type of properties

            return creator;
        }
    }

    /// <summary>
    /// The basic structure of the generalized parameters of the property
    /// </summary>
    struct PropertyParams
    {
        /// <summary>
        /// Property identifier
        /// </summary>
        public int ID;

        /// <summary>
        /// Name of the property type
        /// </summary>
        public string TypeName;

        /// <summary>
        /// Complete property object
        /// </summary>
        public object Data;
    }

    /// <summary>
    /// Enumeration of all available types of properties
    /// </summary>
    enum PropertyType
    {
        PBAR,
        PBARL,
        PBEAM,
        PBEAML,
        PBCOMP,
        PBEND,
        PROD,
        PTUBE,
        PPLANE,
        PLPLANE,
        PSHELL,
        PCOMP,
        PCOMPG,
        PRAC2D,
        PCONEAX,
        PSOLID,
        PLSOLID,
        PCOMPS,
        PRAC3D,
        PAABSF,
        PAERO1,
        PAERO2,
        PAERO3,
        PAERO4,
        PAERO5,
        PBUSH,
        PBUSH1D,
        PDAMP,
        PDAMP5,
        PDUM1,
        PDUM2,
        PDUM3,
        PDUM4,
        PDUM5,
        PDUM6,
        PDUM7,
        PDUM8,
        PDUM9,
        PELAS,
        PFAST,
        PGAP,
        PACABS,
        PACBAR,
        PHBDY,
        PMASS,
        PCONV,
        PCONVM,
        PVISC,
        PWELD,
        PBUSHT,
    }
}

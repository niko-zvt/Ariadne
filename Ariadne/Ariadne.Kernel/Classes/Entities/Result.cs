// Copyright 2022 Nikolay V. Zhivotenko
// Licensed under the Apache License, Version 2.0
// E-mail: niko.zvt@gmail.com

using System;

namespace Ariadne.Kernel
{
    /// <summary>
    /// Abstract class of the result
    /// </summary>
    public abstract class Result
    {
        /// <summary>
        /// Protected constructor with parameters
        /// </summary>
        /// <param name="parameters">Result parameters</param>
        protected Result(ResultParams parameters)
        {
            ID = parameters.ID;
        }

        /// <summary>
        /// ID of the specific result
        /// </summary>
        public ResultID ID { get; protected set; }

        /// <summary>
        /// Virtual method to return the result data
        /// </summary>
        /// <returns>Result data</returns>
        public abstract object GetData();

        /// <summary>
        /// Virtual method to return the type of the result
        /// </summary>
        /// <returns>Type of result</returns>
        public abstract ResultType GetResultType();

        /// <summary>
        /// Checking that the result is valid
        /// </summary>
        /// <returns>
        /// - true in the case, when the result is valid
        /// </returns>
        public abstract bool IsValid();
    }

    /// <summary>
    /// Abstract class of the result creator
    /// </summary>
    public abstract class ResultCreator
    {
        /// <summary>
        /// Protected base constructor for implementing abstract parameter passing when creating a specific result class.
        /// </summary>
        /// <param name="parameters">Result parameters</param>
        protected ResultCreator(ResultParams parameters) { }

        /// <summary>
        /// A virtual function for building results. Must be implemented in each specific class and returns the constructed result
        /// The same factory method
        /// </summary>
        /// <returns>Some constructed result</returns>
        public abstract Result BuildResult();

        /// <summary>
        /// Returns the specific creator of the result using the data contained in the passed parameters
        /// </summary>
        /// <param name="parameters">Result parameters</param>
        /// <returns>A specific instance of the result creator</returns>
        public static ResultCreator GetResultCreatorByParams(ResultParams parameters)
        {
            ResultCreator creator = null;
            var type = parameters.TypeName;

            // TODO: Hide the implementation of selecting a specific creator, so that the registration of a specific type occurs somewhere in a separate place
            if (type == "ExternalResult")
            {
                creator = new ExternalResultCreator(parameters);
            }
            else
            {
                throw new ArgumentOutOfRangeException("Unknown type of result");
            }

            // TODO: Insert new type of results

            return creator;
        }
    }

    /// <summary>
    /// Class of result identifier
    /// </summary>
    public class ResultID : ID
    {
        /// <summary>
        /// Result name separator
        /// </summary>
        private static readonly string separator = "->";

        /// <summary>
        /// Identifier
        /// </summary>
        private string id;

        /// <summary>
        /// Private constructor
        /// </summary>
        /// <param name="ID">Identifier</param>
        private ResultID(string ID)
        {
            id = ID;
        }

        /// <summary>
        /// The method returns the ID
        /// </summary>
        /// <returns>Identifier</returns>
        public string Get()
        {
            return id;
        }

        /// <summary>
        /// The method divides the identifier into its component parts
        /// </summary>
        /// <returns>Array of ID parts</returns>
        public string[] Separate()
        {
            return id.Split(separator);
        }

        /// <summary>
        /// Method for creating a specific result ID by names
        /// </summary>
        /// <param name="loadCaseName">Loadcase name</param>
        /// <param name="subCaseName">Subcase name</param>
        /// <param name="resultName">Result name</param>
        /// <returns>Specific result ID</returns>
        public static ResultID CreateByNames(string loadCaseName, string subCaseName, string resultName)
        {
            var ID = loadCaseName + separator + subCaseName + separator + resultName;
            return new ResultID(ID);
        }

        /// <summary>
        /// Method for creating a specific result ID by specific identifier
        /// </summary>
        /// <param name="ID">Identifier</param>
        /// <returns>Specific result ID</returns>
        public static ResultID CreateByID(string ID)
        {
            return new ResultID(ID);
        }

        /// <summary>
        /// Method for implicit conversion result ID to a string
        /// </summary>
        /// <param name="result">Result ID as string</param>
        public static implicit operator string(ResultID result)
        {
            return result.id;
        }

        /// <summary>
        /// Method for implicit conversion of the string to the result ID
        /// </summary>
        /// <param name="result">String as result ID</param>
        public static implicit operator ResultID(string id)
        {
            return new ResultID(id);
        }

        /// <summary>
        /// The method returns representations of the specific result ID as a string
        /// </summary>
        /// <returns>A string that represents the current object</returns>
        public override string ToString()
        {
            return id;
        }
    }

    /// <summary>
    /// The basic structure of the generalized parameters of the result
    /// </summary>
    public struct ResultParams
    {
        /// <summary>
        /// Load case name
        /// </summary>
        public string ID;

        /// <summary>
        /// Result data
        /// </summary>
        public object Data;

        /// <summary>
        /// Type Name
        /// </summary>
        public string TypeName;
    }

    /// <summary>
    /// Enumeration of all available types of results
    /// </summary>
    public enum ResultType
    {
        FeResPostResult,
    }
}

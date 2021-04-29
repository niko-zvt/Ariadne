using FeResPost;

namespace Ariadne.Kernel
{
    /// <summary>
    /// FeResPost result.
    /// This class defines a FeResPost result
    /// </summary>
    class ExternalResult : Result
    {
        /// <summary>
        /// Data collection
        /// </summary>
        private FeResPost.Result data;

        /// <summary>
        /// Constructor by parameters
        /// </summary>
        /// <param name="parameters">Result parameters</param>
        public ExternalResult(ResultParams parameters) : base(parameters)
        {
            var resData = parameters.Data;
            if (resData != null)
            {
                data = (FeResPost.Result)resData;
            }
            else
            {
                data = null;
            }
        }

        /// <summary>
        /// Returns the result data
        /// </summary>
        /// <returns>Result data</returns>
        public override object GetData()
        {
            return data;
        }

        /// <summary>
        /// Returns the result type
        /// </summary>
        /// <returns>Result type</returns>
        public override ResultType GetResultType()
        {
            return ResultType.FeResPostResult;
        }

        /// <summary>
        /// Checking that the result is valid
        /// </summary>
        /// <returns>
        /// - true in the case, when the result is valid
        /// </returns>
        public override bool IsValid()
        {
            return data != null;
        }
    }

    /// <summary>
    /// FeResPost result creator
    /// </summary>
    class ExternalResultCreator : ResultCreator
    {
        /// <summary>
        /// Specific result
        /// </summary>
        ExternalResult result;

        /// <summary>
        /// Constructor by parameters
        /// </summary>
        /// <param name="parameters">Result parameters</param>
        public ExternalResultCreator(ResultParams parameters) : base(parameters)
        {
            result = new ExternalResult(parameters);
        }

        /// <summary>
        /// Factory method. Builds and returns a new specific result
        /// </summary>
        /// <returns>Specific result</returns>
        public override Result BuildResult()
        {
            return result;
        }
    }
}

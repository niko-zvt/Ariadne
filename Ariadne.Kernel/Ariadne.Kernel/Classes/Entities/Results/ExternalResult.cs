namespace Ariadne.Kernel
{
    /// <summary>
    /// FeResPost result.
    /// This class defines a FeResPost result
    /// </summary>
    // TODO: Change the implementation
    class ExternalResult : Result
    {
        /// <summary>
        /// Data collection
        /// </summary>
        private object[,] _data;

        /// <summary>
        /// Constructor by parameters
        /// </summary>
        /// <param name="parameters">Result parameters</param>
        public ExternalResult(ResultParams parameters) : base(parameters)
        {
            var resData = parameters.Data;
            if (resData != null)
            {
                _data = (object[,])resData;
            }
            else
            {
                _data = null;
            }
        }

        /// <summary>
        /// Returns the result data
        /// </summary>
        /// <returns>Result data</returns>
        public override object GetData()
        {
            return _data;
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
            return _data != null;
        }

        /// <summary>
        /// Overriding the convert method to a string
        /// </summary>
        /// <returns>Returns a string representation of the result</returns>
        public override string ToString()
        {
            var str = $"{this.GetType()}";
            if(_data != null)
            {
                str += $"\t{_data}";
            }
            return str;
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

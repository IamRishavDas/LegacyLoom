namespace ServiceResponseShared
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public int StatusCode { get; set; }
        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }
        public string? ErrorCode { get; set; }
        public List<string> Errors { get; set; }

        protected ServiceResponse()
        {
            Errors = new List<string>();
        }

        public static ServiceResponse<T> SuccessResult(T data, int statusCode)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            return new ServiceResponse<T>
            {
                Success = true,
                Data = data,
                StatusCode = statusCode,
                Errors = new List<string>()
            };
        }
        public static ServiceResponse<T> SuccessResult(T data, int statusCode, string successMessage)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            return new ServiceResponse<T>
            {
                Success = true,
                Data = data,
                StatusCode = statusCode,
                SuccessMessage = successMessage,
                Errors = new List<string>()
            };
        }
        public static ServiceResponse<T> Failure(string errorMessage, int statusCode)
        {
            if (string.IsNullOrEmpty(errorMessage))
                throw new ArgumentNullException(nameof(errorMessage));
            return new ServiceResponse<T>
            {
                Success = false,
                ErrorMessage = errorMessage,
                StatusCode = statusCode,
                Errors = new List<string> { errorMessage }
            };
        }
        public static ServiceResponse<T> Failure(string errorMessage, string errorCode, int statusCode)
        {
            if (string.IsNullOrEmpty(errorMessage))
                throw new ArgumentNullException(nameof(errorMessage));
            return new ServiceResponse<T>
            {
                Success = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode,
                StatusCode = statusCode,
                Errors = new List<string> { errorMessage }
            };
        }
        public static ServiceResponse<T> Failure(List<string> errors, int statusCode)
        {
            errors = errors?.Where(e => !string.IsNullOrEmpty(e)).ToList() ?? new List<string>();
            return new ServiceResponse<T>
            {
                Success = false,
                ErrorMessage = errors.Any() ? "Multiple errors occurred" : null,
                Errors = errors,
                StatusCode = statusCode
            };
        }
        public static ServiceResponse<T> Failure(string errorMessage, List<string> errors, int statusCode)
        {
            if (string.IsNullOrEmpty(errorMessage))
                throw new ArgumentNullException(nameof(errorMessage));
            errors = errors?.Where(e => !string.IsNullOrEmpty(e)).ToList() ?? new List<string>();
            return new ServiceResponse<T>
            {
                Success = false,
                ErrorMessage = errorMessage,
                Errors = errors,
                StatusCode = statusCode
            };
        }
    }

    public class ServiceResponse : ServiceResponse<object>
    {
        public static ServiceResponse SuccessResult(int statusCode)
        {
            return new ServiceResponse
            {
                Success = true,
                StatusCode = statusCode,
                Data = null
            };
        }
        public static ServiceResponse SuccessResult(int statusCode, string successMessage)
        {
            return new ServiceResponse
            {
                Success = true,
                StatusCode = statusCode,
                SuccessMessage = successMessage,
                Data = null
            };
        }
        public static new ServiceResponse Failure(string errorMessage, int statusCode)
        {
            if (string.IsNullOrEmpty(errorMessage))
                throw new ArgumentNullException(nameof(errorMessage));
            return new ServiceResponse
            {
                Success = false,
                StatusCode = statusCode,
                ErrorMessage = errorMessage,
                Errors = new List<string> { errorMessage }
            };
        }
        public static new ServiceResponse Failure(string errorMessage, string errorCode, int statusCode)
        {
            if (string.IsNullOrEmpty(errorMessage))
                throw new ArgumentNullException(nameof(errorMessage));
            return new ServiceResponse
            {
                Success = false,
                StatusCode = statusCode,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode,
                Errors = new List<string> { errorMessage }
            };
        }
        public static new ServiceResponse Failure(List<string> errors, int statusCode)
        {
            errors = errors?.Where(e => !string.IsNullOrEmpty(e)).ToList() ?? new List<string>();
            return new ServiceResponse
            {
                Success = false,
                StatusCode = statusCode,
                ErrorMessage = errors.Any() ? "Multiple errors occurred" : null,
                Errors = errors
            };
        }
        public static new ServiceResponse Failure(string errorMessage, List<string> errors, int statusCode)
        {
            if (string.IsNullOrEmpty(errorMessage))
                throw new ArgumentNullException(nameof(errorMessage));
            errors = errors?.Where(e => !string.IsNullOrEmpty(e)).ToList() ?? new List<string>();
            return new ServiceResponse
            {
                Success = false,
                StatusCode = statusCode,
                ErrorMessage = errorMessage,
                Errors = errors
            };
        }
    }
}

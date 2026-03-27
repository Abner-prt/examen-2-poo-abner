namespace Examen2Api.Dtos
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        
        public static ApiResponse<T> SuccessResponse(T data, string message = "Operación exitosa")
        {
            return new ApiResponse<T> { Success = true, Message = message, Data = data };
        }

        public static ApiResponse<T> FailResponse(string message)
        {
            return new ApiResponse<T> { Success = false, Message = message, Data = default };
        }
    }
}

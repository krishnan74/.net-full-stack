namespace ChienVHShopOnline.Models.DTOs.Response
{
    public class PaginationInfo
    {
        public int TotalRecords { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }

    public class PaginatedResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public PaginationInfo? Pagination { get; set; }
        public object? Errors { get; set; }

        public static PaginatedResponse<T> SuccessResponse(T data, PaginationInfo pagination, string message = "Items fetched successfully")
        {
            return new PaginatedResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                Pagination = pagination,
                Errors = null
            };
        }

        public static PaginatedResponse<T> ErrorResponse(string message, object? errors = null)
        {
            return new PaginatedResponse<T>
            {
                Success = false,
                Message = message,
                Data = default,
                Pagination = null,
                Errors = errors
            };
        }
    }
} 
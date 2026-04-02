namespace Gym.Application.DTOs.Common;

/// <summary>
/// Standard API response wrapper
/// </summary>
public class ResponseDto<T>
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public T? Data { get; set; }

    public List<string>? Errors { get; set; }

    public static ResponseDto<T> SuccessResult(T data, string message = "Success")
    {
        return new ResponseDto<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    public static ResponseDto<T> FailureResult(string message, List<string>? errors = null)
    {
        return new ResponseDto<T>
        {
            Success = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }
}

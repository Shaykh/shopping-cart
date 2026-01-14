namespace Basket.API.Models;

/// <summary>
/// Standard error response model
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Error type or category
    /// </summary>
    /// <example>Validation failed</example>
    public string Error { get; set; } = string.Empty;

    /// <summary>
    /// Error message
    /// </summary>
    /// <example>One or more validation errors occurred</example>
    public string Message { get; set; } = string.Empty;
}

/// <summary>
/// Validation error response model
/// </summary>
public class ValidationErrorResponse : ErrorResponse
{
    /// <summary>
    /// List of validation errors
    /// </summary>
    public List<ValidationError> Errors { get; set; } = [];
}

/// <summary>
/// Individual validation error
/// </summary>
public class ValidationError
{
    /// <summary>
    /// Property name that failed validation
    /// </summary>
    /// <example>UserName</example>
    public string Property { get; set; } = string.Empty;

    /// <summary>
    /// Validation error message
    /// </summary>
    /// <example>UserName is required</example>
    public string Message { get; set; } = string.Empty;
}

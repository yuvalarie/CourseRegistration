using System;
using _CA.GamePlay.Errors.Error_Mono;

namespace _CA.GamePlay
{
    public enum ErrorType
    {
        RejectLastError,
        ResetAllError,
        ResetLastError
    }
    
    public class ErrorTypeFactory
    {
        public static ErrorType StringToErrorType(string errorType)
        {
            return errorType switch
            {
                "RejectLastError" => ErrorType.RejectLastError,
                "ResetAllErrors" => ErrorType.ResetAllError,
                "ResetLastError" => ErrorType.ResetLastError,
                _ => throw new Exception($"Unknown error type: {errorType}"),
            };
        }
        
        public static ICAError EnumToErrorType(ErrorType errorType)
        {
            return errorType switch
            {
                ErrorType.RejectLastError => new CARejectLastError(),
                ErrorType.ResetAllError => new CAResetAllError(),
                ErrorType.ResetLastError => new CAResetLastError(),
                _ => throw new Exception($"Unknown error type: {errorType}"),
            };
        }
    }
}
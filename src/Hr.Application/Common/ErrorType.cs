namespace Hr.Application.Common;
public enum ErrorType
{
    None,         
    Validation,   // 400  
    NotFound,     // 404  
    Conflict,     // 409 
    Upstream      // 502  
}

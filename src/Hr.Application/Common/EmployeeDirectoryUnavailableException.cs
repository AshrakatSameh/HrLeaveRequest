namespace Hr.Application.Common;

/// The employee directory could not be reached, answered with an error, or did
/// not respond in time.
public sealed class EmployeeDirectoryUnavailableException : Exception
{
    public EmployeeDirectoryUnavailableException(string message)
        : base(message) { }

    public EmployeeDirectoryUnavailableException(string message, Exception innerException)
        : base(message, innerException) { }
}

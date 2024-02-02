using System.Collections.ObjectModel;
using System.Management.Automation;

namespace RDSServiceLibrary;

public class RdsServiceException : Exception
{
    public Collection<ErrorRecord>? PowerShellErrors { get; }
    public string? Command { get; }
    public object? Parameters { get; }

    public RdsServiceException(string message)
        : base(message)
    {
    }

    public RdsServiceException(string message, Exception inner)
        : base(message, inner)
    {
    }
    
    public RdsServiceException(string message, Collection<ErrorRecord>? powerShellErrors, string? command, object? parameters)
        : base(message)
    {
        PowerShellErrors = powerShellErrors;
        Command = command;
        Parameters = parameters;
    }
    
    public RdsServiceException(string message, Collection<ErrorRecord>? powerShellErrors, string? command, object? parameters, Exception inner)
        : base(message, inner)
    {
        PowerShellErrors = powerShellErrors;
        Command = command;
        Parameters = parameters;
    }
}
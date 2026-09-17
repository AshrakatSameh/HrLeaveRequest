using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Application.DTO;
public record EmployeeResponse(
    int Id,
    string FirstName,
    string LastName,
    string? Email,
    string? Department,
    string? Title
    )
{
    public string FullName => $"{FirstName} {LastName}".Trim();
}


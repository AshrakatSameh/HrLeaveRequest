using Hr.Api.Contracts;
using Hr.Api.Extensions;
using Hr.Application.DTO;
using Hr.Application.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace Hr.Api.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeesController : ControllerBase
{
    private const int DefaultLimit = 50;
    private const int MaxLimit = 200;

    private readonly IEmployeeService _service;

    public EmployeesController(IEmployeeService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<EmployeeResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> Get(
        [FromQuery] string? search, [FromQuery] int? limit, CancellationToken ct)
    {
        var effectiveLimit = Math.Clamp(limit ?? DefaultLimit, 1, MaxLimit);

        var result = await _service.SearchAsync(search, effectiveLimit, ct);

        return result.IsSuccess ? Ok(result.Value) : result.ToErrorResult();
    }
}

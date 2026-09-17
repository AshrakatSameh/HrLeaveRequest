# HR — Employee Leave Requests

This is a small HR module for managing employee leave requests.

Employees can:

* View leave requests
* Submit a new request
* Delete a pending request

HR users can:

* Approve requests
* Reject requests

Employee information is not stored in this system. It comes from the external HR API (`dummyjson.com/users`).

The application only stores one table: `LeaveRequests`.

## Tech Stack

* .NET 8 / ASP.NET Core Web API
* SQL Server
* EF Core 8
* Blazor WebAssembly
* FluentValidation
* JWT authentication
* xUnit for testing

## Project Structure

* `Hr.Domain` → entities, enums and business rules
* `Hr.Application` → DTOs, interfaces, services and validators
* `Hr.Infrastructure` → EF Core, repository, external API client and caching
* `Hr.Api` → controllers, authentication and middleware
* `Hr.BlazorClient` → UI
* `Hr.Domain.Tests` → unit tests

The main dependency direction is:

`Api → Application → Domain`

`Api → Infrastructure`

The Domain project has no external dependencies, so the business rules can be tested without a database or HTTP calls.

## Running the Project

### Requirements

* .NET 8 SDK
* SQL Server

First, update the connection string in `appsettings.json`.

Then create/update the database:

```bash
dotnet ef migrations add InitialCreate --project src/Hr.Infrastructure --startup-project src/Hr.Api
dotnet ef database update --project src/Hr.Infrastructure --startup-project src/Hr.Api
```

Run the API and Blazor projects.

* API + Swagger → `https://localhost:7224/swagger`
* Blazor → `https://localhost:7180`

Run tests with:

```bash
dotnet test
```

## Main APIs

* `GET /api/leave-requests`

  * Gets leave requests
  * Supports filtering by status and employee
  * Supports pagination
  * Adds employee information to each row

* `GET /api/leave-requests/{id}`

  * Gets one leave request

* `POST /api/leave-requests`

  * Checks that the employee exists in the external HR system
  * Creates the leave request

* `PUT /api/leave-requests/{id}/status`

  * Approves or rejects a request
  * Only HR users can use it

* `DELETE /api/leave-requests/{id}`

  * Deletes a request only when it is Pending

* `GET /api/employees`

  * Gets/searches employees from the external API

* `POST /api/auth/login`

  * Returns a JWT token

## Important Business Rules

### Employee ID

There is no foreign key from `LeaveRequests.EmployeeId` to an Employee table because employees are owned by the external HR system.

When creating a leave request:

* The API checks the employee in the external system.
* If the employee does not exist → `404`.
* If the external system is unavailable → `502`.

This keeps employee data outside our database.

### Leave Status

Only these transitions are allowed:

* `Pending → Approved`
* `Pending → Rejected`

Once a request is Approved or Rejected, it cannot be reviewed again.

The status rule is implemented in the Domain layer as a simple function, so it is easy to test.

### Validation

The database also protects the data:

* Employee ID must be greater than 0
* End date cannot be before start date
* Status must be Pending, Approved or Rejected
* Type must contain a valid value

There is also a unique constraint to prevent the same employee from having the exact same pending leave request twice.

True overlapping leave dates are not handled yet. Only exact duplicates are blocked.

## External Employee API

The employee data comes from the third-party API.

I used a typed `HttpClient` with `IHttpClientFactory`.

Main points:

* 5-second timeout
* Employee data is cached for 10 minutes
* The whole employee list is fetched once instead of calling the API for every leave request
* This avoids the N+1 problem
* Searching is done against the cached employee list

So for a page containing N leave requests:

* Cache available → `0` external calls
* Cache expired → `1` external call

A `SemaphoreSlim` also prevents multiple requests from filling the cache at the same time.

## If the Employee API Goes Down

For reading leave requests:

* The API still returns the local leave requests
* Employee name may be `null`
* `employeeDirectoryAvailable` becomes `false`
* The UI shows a warning

For creating a leave request:

* The API returns `502`
* The request is not created because the employee cannot be verified

This means existing local data is still available even if the external system is down.

## Authentication

JWT is used for the review endpoint.

Demo users:

| Username | Password   | Role     |
| -------- | ---------- | -------- |
| `hr`     | `hr123`    | HR       |
| `staff`  | `staff123` | Employee |

Only the `HR` role can approve or reject.

The UI hides the Approve/Reject buttons for normal employees, but the important security check is on the API itself using:

`[Authorize(Roles = "HR")]`

So a user cannot bypass the UI and call the API directly.

## Error Handling

I use one simple error format:

```json
{
  "error": "message"
}
```

Some examples:

* `400` → invalid request
* `401` → no authentication
* `403` → user is not HR
* `404` → employee/request not found
* `409` → conflicting operation
* `502` → external HR API problem
* `500` → unexpected server error

I disabled the default ASP.NET `ProblemDetails` response so all errors have the same format.

## Security

* No raw SQL
* Server-side validation
* No stack traces returned to clients
* Only the required employee fields are read from the external API
* Sensitive fields from the external response such as password, SSN and bank information are not exposed
* CORS only allows configured client origins

## Testing

The Domain tests cover:

* Valid status transitions
* Invalid status transitions
* `400` vs `409` behavior
* Making sure a failed review does not change the request
* Invalid/undefined enum values

Run tests with:

```bash
dotnet test
```

## Current Assumptions

* The external employee API is read-only.
* Users are stored in configuration because this project only owns one database table.
* Demo passwords and JWT key are currently in `appsettings.json` for simplicity.
* In production, these should be moved to a secure secret/key vault and passwords should be hashed.
* Historical leave requests are allowed.

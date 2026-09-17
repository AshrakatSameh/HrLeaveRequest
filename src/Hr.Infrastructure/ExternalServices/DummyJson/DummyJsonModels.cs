namespace Hr.Infrastructure.ExternalServices.DummyJson;

internal sealed class DummyJsonUser
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public DummyJsonCompany? Company { get; set; }
}

internal sealed class DummyJsonCompany
{
    public string? Department { get; set; }
    public string? Title { get; set; }
}

internal sealed class DummyJsonUserListResponse
{
    public List<DummyJsonUser> Users { get; set; } = [];
    public int Total { get; set; }
}

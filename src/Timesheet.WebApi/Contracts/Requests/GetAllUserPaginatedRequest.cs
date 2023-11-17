using Timesheet.Shared.Abstractions.Queries;

namespace Timesheet.WebApi.Contracts.Requests;

public class GetAllUserPaginatedRequest : BasePaginationCalculation
{
    public string? Username { get; set; }
    public string? Fullname { get; set; }
}
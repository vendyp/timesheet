using TimesheetService.Shared.Abstractions.Queries;

namespace TimesheetService.WebApi.Contracts.Requests;

public class GetAllUserPaginatedRequest : BasePaginationCalculation
{
    public string? Username { get; set; }
    public string? Fullname { get; set; }
}
﻿namespace TimesheetService.WebApi.Client.Contracts.Responses;

public class EditOptionResponse
{
    public string Value { get; set; } = null!;
    public string? Description { get; set; }
}
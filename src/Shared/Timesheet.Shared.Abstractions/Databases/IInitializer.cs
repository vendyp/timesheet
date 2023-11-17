﻿namespace Timesheet.Shared.Abstractions.Databases;

public interface IInitializer
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}
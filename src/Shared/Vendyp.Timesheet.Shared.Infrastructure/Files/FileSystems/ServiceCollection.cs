﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Vendyp.Timesheet.Shared.Abstractions.Files;

namespace Vendyp.Timesheet.Shared.Infrastructure.Files.FileSystems;

public static class ServiceCollection
{
    public static void AddFileSystemService(this IServiceCollection services, string name = "fileOptions")
    {
        var options = services.GetOptions<FileSystemOptions>(name);
        services.TryAddSingleton(options);
        services.TryAddSingleton<IFileService, FileSystemService>();
    }
}
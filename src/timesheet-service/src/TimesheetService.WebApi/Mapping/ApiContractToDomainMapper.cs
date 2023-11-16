using TimesheetService.Domain.Entities;
using TimesheetService.Domain.Enums;
using TimesheetService.Domain.Extensions;
using TimesheetService.Shared.Abstractions.Encryption;
using TimesheetService.Shared.Abstractions.Files;
using TimesheetService.WebApi.Contracts.Requests;

namespace TimesheetService.WebApi.Mapping;

public static class ApiContractToDomainMapper
{
    public static FileRepository ToFileRepository(this UploadFileRequest request, FileResponse fileResponse)
    {
        var fileRepository = new FileRepository
        {
            FileName = request.File.FileName,
            UniqueFileName = fileResponse.NewFileName,
            Source = request.Source,
            Size = request.File.Length,
            FileExtension = Path.GetExtension(request.File.FileName).ToUpper(),
        };

        if (FileRepositoryExtensions.ListOfFileTypeImages.Any(e => e == fileRepository.FileExtension))
            fileRepository.FileType = FileType.Images;

        if (FileRepositoryExtensions.ListOfFileTypeDocuments.Any(e => e == fileRepository.FileExtension))
            fileRepository.FileType = FileType.Document;

        return fileRepository;
    }

    public static Role ToRole(this CreateRoleRequest request)
    {
        var role = new Role
        {
            Name = request.Name!,
            Description = request.Description,
        };

        role.Code = RoleExtensions.Slug(role.RoleId, role.Name);

        if (request.Scopes.Any())
            foreach (var item in request.Scopes)
                role.RoleScopes.Add(new RoleScope
                {
                    RoleId = role.RoleId,
                    Name = item
                });

        return role;
    }

    public static User ToUser(this CreateUserRequest request, string salt, ISalter salter)
    {
        var user = new User
        {
            Username = request.Username!.Trim(),
            NormalizedUsername = request.Username!.Trim().ToUpper(),
            Salt = salt,
            Password = salter.Hash(salt, request.Password!),
            FullName = request.Fullname?.Trim(),
            Email = request.EmailAddress?.Trim()
        };

        user.UserRoles.Add(
            new UserRole
            {
                RoleId = request.RoleId!.Value
            });

        return user;
    }
}
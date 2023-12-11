﻿using System.Security.Cryptography;
using System.Text;
using Vendyp.Timesheet.Shared.Abstractions.Encryption;

namespace Vendyp.Timesheet.Shared.Infrastructure.Encryption;

internal class Sha256 : ISha256
{
    public string Hash(string data)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(data));
        var builder = new StringBuilder();
        foreach (var @byte in bytes)
        {
            builder.Append(@byte.ToString("x2"));
        }

        return builder.ToString();
    }
}
// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.Common.Exceptions;

public class FileFoundException : IOException
{
    private readonly string _fileName;
    private readonly string _message;

    public FileFoundException(string message, string fileName)
    {
        _message = message;
        _fileName = fileName;
    }

    public override string Message
    {
        get
        {
            var message = $"{_message}\nFileExist {_fileName}";
            return message;
        }
    }
}

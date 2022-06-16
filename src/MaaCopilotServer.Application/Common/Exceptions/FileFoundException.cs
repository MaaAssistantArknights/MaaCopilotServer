// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.Common.Exceptions;

/// <summary>
/// The exception that is thrown when a file is found.
/// </summary>
public class FileFoundException : IOException
{
    /// <summary>
    /// The filename.
    /// </summary>
    private readonly string _fileName;

    /// <summary>
    /// The message.
    /// </summary>
    private readonly string _message;

    /// <summary>
    /// The constructor of <see cref="FileNotFoundException"/>.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="fileName">The filename.</param>
    public FileFoundException(string message, string fileName)
    {
        _message = message;
        _fileName = fileName;
    }

    /// <summary>
    /// The message of the exception.
    /// </summary>
    public override string Message
    {
        get
        {
            var message = $"{_message}\nFileExist {_fileName}";
            return message;
        }
    }
}

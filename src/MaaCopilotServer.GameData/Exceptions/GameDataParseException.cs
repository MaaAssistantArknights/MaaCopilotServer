// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.GameData.Model;

namespace MaaCopilotServer.GameData.Exceptions;

public class GameDataParseException : Exception
{
    public GameDataParseException(string type, string local, string find, string level)
        : base($"Failed to parse game data. Type: {type}, Local: {local}, Find: {find}, Level: {level}")
    { }
}

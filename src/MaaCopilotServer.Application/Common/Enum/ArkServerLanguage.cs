// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using MaaCopilotServer.Application.Common.Exceptions;

// ReSharper disable MemberCanBePrivate.Global

namespace MaaCopilotServer.Application.Common.Enum;

/// <summary>
///     Arknights server languages.
/// </summary>
[ExcludeFromCodeCoverage]
public readonly struct ArkServerLanguage
{
    /// <summary>
    ///     Language full name.
    /// </summary>
    private readonly string _fullName;
    /// <summary>
    ///     Language short name.
    /// </summary>
    private readonly string _abbr;

    /// <summary>
    ///     Check if the input string is this language or not.
    /// </summary>
    /// <param name="lang">Input language string.</param>
    /// <returns></returns>
    private bool Is(string lang) => _fullName == lang || _abbr == lang;

    /// <summary>
    ///     Constructor of <see cref="ArkServerLanguage"/>.
    /// </summary>
    /// <param name="fullName">Language full name.</param>
    /// <param name="abbr">Language short name.</param>
    private ArkServerLanguage(string fullName, string abbr)
    {
        _fullName = fullName;
        _abbr = abbr;
    }

    public static readonly ArkServerLanguage Chinese = new("chinese", "cn");
    public static readonly ArkServerLanguage English = new("english", "en");
    public static readonly ArkServerLanguage Japanese = new("japanese", "ja");
    public static readonly ArkServerLanguage Korean = new("korean", "ko");

    public static readonly ArkServerLanguage Unknown = new("unknown", "???");

    public static readonly ArkServerLanguage[] SupportedLanguages =
    {
        Chinese,
        English,
        Japanese,
        Korean
    };

    /// <summary>
    ///     Get language specific actions.
    /// </summary>
    /// <param name="cn">Operator to do when language is <see cref="Chinese"/>.</param>
    /// <param name="en">Operator to do when language is <see cref="English"/>.</param>
    /// <param name="ja">Operator to do when language is <see cref="Japanese"/>.</param>
    /// <param name="ko">Operator to do when language is <see cref="Korean"/>.</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public T GetArkServerLanguageSpecificAction<T>(T cn, T en, T ja, T ko) => _abbr switch
    {
        "cn" => cn,
        "en" => en,
        "ja" => ja,
        "ko" => ko,
        _ => throw new UnknownServerLanguageException()
    };

    /// <summary>
    ///     Parse the input string to <see cref="ArkServerLanguage"/>.
    /// </summary>
    /// <param name="language">Input language string.</param>
    /// <returns>
    ///     If the input string is one of the supported languages, return the corresponding <see cref="ArkServerLanguage"/>.
    /// If the input string is null, return <see cref="Chinese"/>. Otherwise, return <see cref="Unknown"/>.
    /// </returns>
    public static ArkServerLanguage Parse(string? language)
    {
        if (string.IsNullOrEmpty(language))
        {
            return Chinese;
        }

        var lang = language.ToLower(CultureInfo.InvariantCulture);

        foreach (var supportedLanguage in SupportedLanguages)
        {
            if (supportedLanguage.Is(lang))
            {
                return supportedLanguage;
            }
        }

        return Unknown;
    }

    public bool Equals(ArkServerLanguage other)
    {
        return _fullName == other._fullName && _abbr == other._abbr;
    }

    public override bool Equals(object? obj)
    {
        return obj is ArkServerLanguage other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_fullName, _abbr);
    }

    public static bool operator==(ArkServerLanguage left, ArkServerLanguage right) => left.Equals(right);
    public static bool operator!=(ArkServerLanguage left, ArkServerLanguage right) => !left.Equals(right);
}

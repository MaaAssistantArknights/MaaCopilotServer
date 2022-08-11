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
    ///     Language names.
    /// </summary>
    private readonly string[] _names;

    /// <summary>
    ///     Language identifier.
    /// </summary>
    private readonly string _identifiers;


    /// <summary>
    ///     Check if the input string is this language or not.
    /// </summary>
    /// <param name="lang">Input language string.</param>
    /// <returns></returns>
    private bool Is(string lang) => _names.Contains(lang.ToLowerInvariant());

    /// <summary>
    ///     Constructor of <see cref="ArkServerLanguage"/>.
    /// </summary>
    /// <param name="fullName">Language full name.</param>
    /// <param name="names">Language other names</param>
    private ArkServerLanguage(string fullName, params string[] names)
    {
        var arr = new[] { fullName.ToLowerInvariant() }.Concat(names.Select(x => x.ToLowerInvariant()));
        _names = arr.ToArray();

        _identifiers = string.Join("|", _names);
    }

    public static readonly ArkServerLanguage ChineseSimplified = new("zh_cn", "cn");
    public static readonly ArkServerLanguage ChineseTraditional = new("zh_tw", "tw");
    public static readonly ArkServerLanguage English = new("en_us", "en");
    public static readonly ArkServerLanguage Japanese = new("ja_jp", "ja");
    public static readonly ArkServerLanguage Korean = new("ko_kr", "ko");

    public static readonly ArkServerLanguage Unknown = new("unknown", "???");

    public static readonly ArkServerLanguage[] SupportedLanguages =
    {
        ChineseSimplified,
        ChineseTraditional,
        English,
        Japanese,
        Korean
    };

    /// <summary>
    ///     Get language specific actions.
    /// </summary>
    /// <param name="cn">Operator to do when language is <see cref="ChineseSimplified"/>.</param>
    /// <param name="tw">Operator to do when language is <see cref="ChineseTraditional"/>.</param>
    /// <param name="en">Operator to do when language is <see cref="English"/>.</param>
    /// <param name="ja">Operator to do when language is <see cref="Japanese"/>.</param>
    /// <param name="ko">Operator to do when language is <see cref="Korean"/>.</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public T GetArkServerLanguageSpecificAction<T>(T cn, T tw, T en, T ja, T ko)
    {
        if (this == ChineseSimplified)
        {
            return cn;
        }

        if (this == ChineseTraditional)
        {
            return tw;
        }

        if (this == English)
        {
            return en;
        }

        if (this == Japanese)
        {
            return ja;
        }

        if (this == Korean)
        {
            return ko;
        }

        throw new UnknownServerLanguageException();
    }

    /// <summary>
    ///     Parse the input string to <see cref="ArkServerLanguage"/>.
    /// </summary>
    /// <param name="language">Input language string.</param>
    /// <returns>
    ///     If the input string is one of the supported languages, return the corresponding <see cref="ArkServerLanguage"/>.
    /// If the input string is null, return <see cref="ChineseSimplified"/>. Otherwise, return <see cref="Unknown"/>.
    /// </returns>
    public static ArkServerLanguage Parse(string? language)
    {
        if (string.IsNullOrEmpty(language))
        {
            return ChineseSimplified;
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
        return _identifiers == other._identifiers;
    }

    public override bool Equals(object? obj)
    {
        return obj is ArkServerLanguage other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _identifiers.GetHashCode();
    }

    public static bool operator ==(ArkServerLanguage left, ArkServerLanguage right) => left.Equals(right);
    public static bool operator !=(ArkServerLanguage left, ArkServerLanguage right) => !left.Equals(right);
}

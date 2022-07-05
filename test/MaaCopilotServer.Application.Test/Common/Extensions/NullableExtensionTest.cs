// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.Common.Extensions;

namespace MaaCopilotServer.Application.Test.Common.Extensions;

/// <summary>
///     Tests <see cref="NullableExtension" />.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class NullableExtensionTest
{
    /// <summary>
    ///     Tests <see cref="NullableExtension.IsNotNull{T}(T?, string, string)" /> with non-null object.
    /// </summary>
    [TestMethod]
    public void TestIsNotNullNotNull()
    {
        var testObject = new object();

        testObject.IsNotNull().Should().Be(testObject);
    }

    /// <summary>
    ///     Tests <see cref="NullableExtension.IsNotNull{T}(T?, string, string)" /> with null object.
    /// </summary>
    [TestMethod]
    public void TestIsNotNullNull()
    {
        object? testObject = null;

        var action = () => testObject.IsNotNull();

        action.Should().Throw<ArgumentNullException>();
    }
}

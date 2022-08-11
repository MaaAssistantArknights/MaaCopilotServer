// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Helpers;

namespace MaaCopilotServer.Application.Test.Helpers
{
    /// <summary>
    /// Tests <see cref="EntityIdHelper"/>.
    /// </summary>
    [TestClass]
    public class EntityIdHelperTest
    {
        /// <summary>
        /// Tests <see cref="EntityIdHelper.EncodeId(long)"/>.
        /// </summary>
        [TestMethod]
        public void TestEncodeId()
        {
            EntityIdHelper.EncodeId(42).Should().Be("10042");
        }

        /// <summary>
        /// Tests <see cref="EntityIdHelper.DecodeId(string)"/> with invalid ID.
        /// </summary>
        [TestMethod]
        public void TestDecodeIdInvalid()
        {
            EntityIdHelper.DecodeId("invalid").Should().BeNull();
        }

        /// <summary>
        /// Tests <see cref="EntityIdHelper.DecodeId(string)"/> with ID out of range.
        /// </summary>
        [TestMethod]
        public void TestDecodeIdOutOfRange()
        {
            EntityIdHelper.DecodeId("9999").Should().BeNull();
        }

        /// <summary>
        /// Tests <see cref="EntityIdHelper.DecodeId(string)"/>.
        /// </summary>
        [TestMethod]
        public void TestDecodeId()
        {
            EntityIdHelper.DecodeId("10042").Should().Be(42);
        }
    }
}

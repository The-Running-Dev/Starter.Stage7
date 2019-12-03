using NUnit.Framework;
using FluentAssertions;

using Starter.Framework.Extensions;

namespace Starter.Framework.Tests.Extensions
{
    /// <summary>
    /// Tests for the EnumExtensions class
    /// </summary>
    public class EnumExtensionsTests: TestsBase
    {
        [Test]
        public void GetDescription_OnEnumWithDescription_Successful()
        {
            (TestEnum.FirstItem).GetDescription().Should().Be("First Item");
        }

        [Test]
        public void GetDescription_OnEnumWithoutDescription_Successful()
        {
            (TestEnum.SecondItem).GetDescription().Should().Be("SecondItem");
        }
    }
}
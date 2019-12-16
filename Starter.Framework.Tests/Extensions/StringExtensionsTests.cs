using NUnit.Framework;
using FluentAssertions;

using Starter.Framework.Extensions;

namespace Starter.Framework.Tests.Extensions
{
    /// <summary>
    /// Tests for the StringExtensions class
    /// </summary>
    public class StringExtensionsTests
    {
        [Test]
        public void IsEqualTo_ForDifferentStrings_Failure()
        {
            "Ben".IsEqualTo("Dan").Should().BeFalse();
        }

        [Test]
        public void IsEqualTo_ForSameStrings_Successful()
        {
            "Ben".IsEqualTo("Ben").Should().BeTrue();
        }

        [Test]
        public void IsEqualTo_ForStringsWithDifferentCase_Successful()
        {
            "wolverine".IsEqualTo("WOLVERINE").Should().BeTrue();
        }

        [Test]
        public void IsEqualTo_ForStringAndNull_Fails()
        {
            "Dan".IsEqualTo(null).Should().BeFalse();
        }

        [Test]
        public void IsEmpty_ForNonEmptyString_Fails()
        {
            "Ben".IsEmpty().Should().BeFalse();
        }

        [Test]
        public void IsEmpty_ForEmptyString_Successful()
        {
            "".IsEmpty().Should().BeTrue();
        }

        [Test]
        public void IsEmpty_ForWhiteSpaceIsString_Successful()
        {
            "     ".IsEmpty().Should().BeTrue();
        }

        [Test]
        public void IsNotEmpty_IsTrueForNotEmptyString_Successful()
        {
            "Ben".IsNotEmpty().Should().BeTrue();
        }

        [Test]
        public void IsNotEmpty_IsFalseForEmptyString_Successful()
        {
            "".IsNotEmpty().Should().BeFalse();
        }

        [Test]
        public void IsNotEmpty_IsFalseForWhiteSpaceString_Successful()
        {
            "     ".IsNotEmpty().Should().BeFalse();
        }

        [Test]
        public void FromJson_ForEntity_Successful()
        {

        }
    }
}
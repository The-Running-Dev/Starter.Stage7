using System.Text;
using FluentAssertions;
using NUnit.Framework;
using Starter.Data.Entities;
using Starter.Framework.Extensions;

namespace Starter.Framework.Tests.Extensions
{
    /// <summary>
    /// Tests for the ObjectExtensions class
    /// </summary>
    [TestFixture]
    public class ByteExtensionsTests
    {
        [Test]
        public void FromJsonBytes_ForBytes_Successful()
        {
            var cat = new Cat();
            var catBytes = Encoding.UTF8.GetBytes(cat.ToJson());

            catBytes.FromJsonBytes<Cat>().Should().BeEquivalentTo(cat);
        }

        [Test]
        public void FromJsonBytes_ForBytesOfDifferentObject_Fails()
        {
            var cat = new Cat();
            var otherCatBytes = Encoding.UTF8.GetBytes((new Cat() {Name = "Cat"}).ToJson());

            otherCatBytes.FromJsonBytes<Cat>().Should().NotBeEquivalentTo(cat);
        }
    }
}
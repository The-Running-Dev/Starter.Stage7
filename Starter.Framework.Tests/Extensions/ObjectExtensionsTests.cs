using System;
using System.Text;
using System.Collections.Generic;

using NUnit.Framework;
using FluentAssertions;

using Starter.Data.Entities;
using Starter.Framework.Extensions;

namespace Starter.Framework.Tests.Extensions
{
    /// <summary>
    /// Tests for the ObjectExtensions class
    /// </summary>
    public class ObjectExtensionsTests
    {
        [Test]
        public void IsEqualTo_ForTwoSameObjects_Successful()
        {
            var firstObject = new Cat();
            var secondObject = firstObject;

            firstObject.IsEqualTo(secondObject).Should().BeTrue();
        }

        [Test]
        public void IsEqualTo_ForTwoDifferentObjects_Fails()
        {
            var firstObject = new Cat();
            var secondObject = new Cat();

            firstObject.IsEqualTo(secondObject).Should().BeFalse();
        }

        [Test]
        public void ToJson_ForObject_Successful()
        {
            dynamic entity = new { Id = 1, Name = "Name" };

            ((object) entity).ToJson().Should().Be("{\"Id\":1,\"Name\":\"Name\"}");
        }

        [Test]
        public void ToJson_ForObjectWithFormatting_Successful()
        {
            dynamic entity = new { Id = 1, Name = "Name" };

            var json = ((object) entity).ToJson(true);
            
            json.Should().NotBeEmpty();
            json.Should().Contain("\"Id\": 1");
        }

        [Test]
        public void ToJsonBytes_ForObject_Successful()
        {
            var cat = new Cat();
            var catBytes = cat.ToJsonBytes();

            catBytes.Should().BeEquivalentTo(Encoding.UTF8.GetBytes(cat.ToJson()));
        }

        [Test]
        public void ToJsonBytes_ForTwoDifferentObject_Fails()
        {
            var cat = new Cat();
            var catBytes = new Cat().ToJsonBytes();

            cat.Should().NotBeEquivalentTo(catBytes);
        }

        [Test]
        public void ToNameValueList_ForEnum_Successful()
        {
            var abilities = new List<object>(typeof(Ability).ToNameValueList());

            abilities.Count.Should().Be(Enum.GetNames(typeof(Ability)).Length);
        }
    }
}
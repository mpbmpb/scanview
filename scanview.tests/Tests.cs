using System;
using Xunit;
using scanview;
using FluentAssertions;

namespace scanview.tests
{
    public class Tests
    {
        [Fact]
        public void FindEntities_finds_Contact_in_ContactIndex()
        {
            var result = Program.FindEntities("Contact");

            result.Should().Contain("Contact");
        }

        [Fact]
        public void FindEntities_finds_Subject_and_Day_in_SubjectIndex()
        {
            var result = Program.FindEntities("Subject");

            //result.Should().Contain(x => x != null).Which.Should();
            result.Should().Contain("Day");
            result.Should().Contain("Subject");

        }

    }
}

using System;
using Xunit;
using scanview;
using FluentAssertions;

namespace scanview.tests
{
    public class Tests
    {
        [Theory]
        [InlineData("Subject", "Index", new string[] { "Day", "Subject" })]
        [InlineData("Contact", "Index", new string[] {"Contact"})]
        [InlineData("CourseDate", "Edit", new string[] {"CourseDate"})]
        [InlineData("CourseDesign", "index", new string[] {"CourseDesign", "Seminar", "Day", "Subject"})]
        public void FindEntities_finds_all_entities_in_controller_View(string controller, string view, string[] expected)
        {
            var result = Program.FindEntities(controller, view);

            result.Should().Contain(expected);
            result.Should().HaveCount(expected.Length);

        }

    }
}

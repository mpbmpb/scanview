using System;
using Xunit;
using scanview;
using FluentAssertions;

namespace scanview.tests
{
    public class Tests
    {
        //should return controller & smallest navigation property entity
        //followed by all larger than controller entities and smaller than subject entitiesß
        [Theory]
        [InlineData("Subject", "Index", new string[] { "Day", "Subject" })]
        [InlineData("Contact", "Index", new string[] {"Contact"})]
        [InlineData("CourseDate", "Edit", new string[] {"CourseDate"})]
        [InlineData("CourseDesign", "Index", new string[] {"CourseDesign", "Subject"})]
        [InlineData("CourseDesign", "Create", new string[] {"CourseDesign", "Day"})]
        public void FindEntities_returns_controller_and_smallest_subEntity_and_entities_outside_this_range_in_controller_View(string controller, string view, string[] expected)
        {
            var result = Program.FindEntities(controller, view);

            result.Should().Contain(expected);
            result.Should().HaveCount(expected.Length);

        }
    }
}

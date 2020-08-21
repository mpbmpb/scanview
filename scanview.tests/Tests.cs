using System;
using Xunit;
using scanview;
using FluentAssertions;

namespace scanview.tests
{
    public class Tests
    {
        //should return controller & smallest navigation property entity
        //followed by all larger than controller entities and smaller than subject entities

        [Theory]
        [InlineData("Subject", "Index", new string[] { "Day", "Subject" })]
        [InlineData("Contact", "Index", new string[] {"Contact"})]
        [InlineData("CourseDate", "Edit", new string[] {"Course", "CourseDate", "Venue"})]
        [InlineData("CourseDesign", "Index", new string[] {"CourseDesign", "Subject"})]
        [InlineData("CourseDesign", "Create", new string[] {"CourseDesign", "Day"})]
        public void FindEntities_returns_controller_and_smallest_subEntity_and_entities_outside_this_range_in_controller_View(string controller, string view, string[] expected)
        {
            var result = Program.FindEntities(controller, view);

            result.Should().Contain(expected);
            result.Should().HaveCount(expected.Length);
        }

        [Theory]
        [InlineData("Subject", "Index", new string[] { "Subject", "Day" })]
        [InlineData("Subject", "Edit", new string[] { "Subject"})]
        [InlineData("Day", "Index", new string[] { "Day", "Subject"})]
        [InlineData("Day", "Details", new string[] { "Day", "Subject"})]
        [InlineData("Seminar", "Index", new string[] { "Seminar", "Day" })]
        [InlineData("Seminar", "Create", new string[] { "Seminar", "Subject" })]
        [InlineData("Seminar", "Edit", new string[] { "Seminar", "Subject" })]
        [InlineData("CourseDesign", "Index", new string[] { "CourseDesign", "Subject" })]
        [InlineData("CourseDesign", "Create", new string[] { "CourseDesign", "Day" })]
        [InlineData("CourseDate", "Create", new string[] { "Course", "CourseDate", "Venue" })]
        [InlineData("CourseDate", "Edit", new string[] { "Course", "CourseDate", "Venue" })]
        [InlineData("Course", "Index", new string[] { "Course", "Subject" })]
        [InlineData("Course", "Details", new string[] { "Course", "Subject" })]
        [InlineData("Course", "Edit", new string[] { "Course", "CourseDesign" })]
        [InlineData("Course", "Create", new string[] { "Course", "CourseDesign" })]
        [InlineData("Venue", "Create", new string[] { "Venue" })]
        [InlineData("Venue", "Details", new string[] { "Venue" })]
        public void FindEntities_returns_entities_in_proper_order(string controller, string view, string[] expected)
        {
            var result = Program.FindEntities(controller, view);
          
            result.Should().Equal(expected);
        }
    }
}

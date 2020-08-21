using System;
using Xunit;
using scanview;
using FluentAssertions;
using System.Collections.Generic;

namespace scanview.tests
{
    public class Tests
    {
        [Theory]
        [InlineData("Contact/Index", new string[] { "Contact" })]
        [InlineData("Subject/Index", new string[] { "Day", "DaySubjects", "Subject" })]
        [InlineData("Subject/Edit", new string[] { "Subject"})]
        [InlineData("Day/Index", new string[] { "Day", "DaySubjects", "Subject"})]
        [InlineData("Seminar/Index", new string[] { "Seminar", "SeminarDays", "Day" })]
        [InlineData("CourseDesign/Index", new string[] { "CourseDesign", "CourseSeminars", "Seminar", "SeminarDays", "Day", "DaySubjects", "Subject" })]
        [InlineData("CourseDesign/Create", new string[] { "CourseDesign", "CourseSeminars", "Seminar", "SeminarDays", "Day" })]
        [InlineData("CourseDate/Edit", new string[] { "Course", "CourseDates", "CourseDate", "Venue" })]
        [InlineData("Course/Index", new string[] { "Course", "CourseDesign", "CourseSeminars", "Seminar", "SeminarDays", "Day", "DaySubjects", "Subject" })]
        [InlineData("Venue/Create", new string[] { "Venue" })]
        public void FindEntities_returns_entities_in_proper_order(string controllerview, string[] output)
        {
            var result = Extensions.GetRelevantEntities(controllerview);

            var list = new List<string>();
            foreach (var item in output) list.Add(item);
            var expected = new List<List<string>> { list };

            result[0].Should().Equal(expected[0]);
        }

        [Fact]
        public void FindEntities_returns_entitylists_grouped_by_lineage()
        {
            var result = Extensions.GetRelevantEntities("Course/Details");

            var expected = new List<List<string>>{
                new List<string> { "Course", "CourseDesign", "CourseSeminars", "Seminar", "SeminarDays", "Day", "DaySubjects", "Subject" },
                new List<string> { "Course", "CourseDates", "CourseDate", "Venue" }
            };

            result[0].Should().Equal(expected[0]);
            result[1].Should().Equal(expected[1]);

        }
    }
}

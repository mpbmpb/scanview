using System;
using Xunit;
using scanview;
using FluentAssertions;
using System.Collections.Generic;
using scanview.Models;

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
            var dataModel = new DataModel();
            var viewScanner = new ViewScanner(dataModel);
            var list = new List<string>();
            foreach (var item in output) list.Add(item);
            var expected = new List<List<string>> { list };

            var result = viewScanner.GetRelatedEntities(controllerview);

            result[0].Should().Equal(expected[0]);
            result.Should().HaveCount(1);
        }

        [Fact]
        public void FindEntities_returns_entitylists_grouped_by_lineage()
        {
            var dataModel = new DataModel();
            var viewScanner = new ViewScanner(dataModel);
            var expected = new List<List<string>>{
                new List<string> { "Course", "CourseDesign", "CourseSeminars", "Seminar",
                    "SeminarDays", "Day", "DaySubjects", "Subject" },
                new List<string> { "Course", "CourseDates", "CourseDate", "Venue" }
            };

            var result = viewScanner.GetRelatedEntities("Course/Details");

            result[0].Should().Equal(expected[0]);
            result[1].Should().Equal(expected[1]);
            result.Should().HaveCount(2);

        }

        [Fact]
        public void DataModel_Build_Builds_Model_With_All_Entities_In_SchoolContext()
        {
            var dataModel = new DataModel();
            var expected = new List<Type> { typeof(Course), typeof(CourseDesign), typeof(CourseSeminar),
                typeof(Seminar), typeof(SeminarDay), typeof(Day), typeof(DaySubject), typeof(Subject),
                typeof(CourseDate), typeof(Venue), typeof(Contact)};

            dataModel.Build<SchoolContext>();
            var result = dataModel.Entities;

            result.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(typeof(Subject), new Type[] { typeof(DaySubject) })]
        [InlineData(typeof(DaySubject), new Type[] { typeof(Day), typeof(Subject) })]
        [InlineData(typeof(Day), new Type[] { typeof(DaySubject), typeof(SeminarDay) })]
        [InlineData(typeof(SeminarDay), new Type[] { typeof(Day), typeof(Seminar) })]
        [InlineData(typeof(Seminar), new Type[] { typeof(CourseDesign), typeof(SeminarDay) })]
        [InlineData(typeof(CourseSeminar), new Type[] { typeof(CourseDesign), typeof(Seminar) })]
        [InlineData(typeof(CourseDesign), new Type[] { typeof(CourseSeminar) })]
        [InlineData(typeof(Course), new Type[] { typeof(CourseDesign), typeof(CourseDate) })]
        [InlineData(typeof(CourseDate), new Type[] { typeof(Course), typeof(Venue) })]
        [InlineData(typeof(Venue), new Type[] { typeof(Contact), typeof(Contact) })]
        [InlineData(typeof(Contact), new Type[] { })]
        public void DataModel_Build_Builds_Model_With_correct_relations_mapped(Type entity, Type[] expected)
        {
            var dataModel = new DataModel();

            dataModel.Build<SchoolContext>();
            int entityIndex = dataModel.Entities.IndexOf(entity);
            var result = dataModel.NavigationRelations[entityIndex];
  
            result.Should().BeEquivalentTo(expected);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace scanview
{
    public class DataModel
    {
        internal readonly string[][] relations = new string[][]
        {
            new string [] { "Course", "CourseDesign", "CourseSeminars", "Seminar",
                            "SeminarDays", "Day", "DaySubjects", "Subject" },
            new string [] { "Course", "CourseDates", "CourseDate", "Venue", "Contact" }
        };

        internal readonly string path = "../../../../scanview/Views/";

        public List<Type> Entities = new List<Type>();
        public List<List<Type>> NavigationRelations = new List<List<Type>>();
        public List<List<Type>> NavigationPaths = new List<List<Type>>();


        public void Build<T>() where T : class
        {
            BuildTypeList<T>();
            BuildNavigationRelations();
            BuildNavigationPaths();
        }

        private void BuildTypeList<T>()
        {
            var contextProperties = typeof(T).GetProperties()
            .Where(x => x.PropertyType.IsGenericType && x.PropertyType.Name.Contains("DbSet"))
            .ToArray();
            foreach (var type in contextProperties)
            {
                var gen = type.PropertyType.GetGenericArguments()[0];
                Entities.Add(gen);
            }
        }

        private void BuildNavigationRelations()
        {
            for (int type = 0; type < Entities.Count; type++)
            {
                var propertiesRow = new List<Type>();
                var Properties = Entities[type].GetProperties().ToList();
                foreach (var property in Properties)
                {
                    if (property.PropertyType.IsGenericType)
                        propertiesRow.Add(property.PropertyType.GetGenericArguments()[0]);
                    else
                        propertiesRow.Add(property.PropertyType);
                }
                NavigationRelations.Add(NavigationPropertiesFrom(propertiesRow));
            }
        }

        private void BuildNavigationPaths()
        {
            var path = new List<List<Type>>();
            for (int entity = 0; entity < Entities.Count; entity++)
            {
                var row = new List<Type>();
                row.Add(Entities[entity]);
                var referrals = NavigationRelations[entity].Where(e => !row.Contains(e)).ToList();
                foreach (var referral in referrals)
                {
                    row.Add(referral);
                    path.Absorb(row);
                    row.Remove(referral);
                }

            }
        }

        private List<Type> NavigationPropertiesFrom(List<Type> types)
        {
            return types.Where(e => Entities.Contains(e)).ToList();
        }
    }
}
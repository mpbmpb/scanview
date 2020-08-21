using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace scanview
{
    public class Program
    {
        static void Main(string[] args)
        {
            var result = Extensions.GetRelevantEntities("Course/Details");
            int count = 1;
            foreach (var list in result)
            {
                Console.WriteLine($"List{count}");
                list.ForEach(Console.WriteLine);
                count++;
            }
        }       
    }

    public static class Extensions
    {
        private static readonly string[] entities = new string[]
        {
            "Course", "CourseDates", "CourseDesign", "CourseSeminars", "Seminar", "SeminarDays", "Day", "DaySubjects", "Subject", "CourseDate", "Venue", "Contact"
        };

        private static readonly string[][] navigationRelations = new string[][]
        {
            new string [] { "Course", "CourseDesign", "CourseSeminars", "Seminar", "SeminarDays", "Day", "DaySubjects", "Subject" },
            new string [] { "Course", "CourseDates", "CourseDate", "Venue", "Contact" }
        };

        public static List<List<string>> GetRelevantEntities(string controllerViewName)
        {
            var result = FindEntities(controllerViewName)
                .ReturnGrouped();

            result.RemoveUnnessesaryDoubles();
            result.RemoveAll(item => item.Count == 0);
            return result;
        }

        public static List<string> FindEntities(string controllerViewName)
        {
            List<string> result = new List<string>();
            string view = File.ReadAllText($"../../../../scanview/Views/{controllerViewName}.cshtml");

            foreach (var entity in entities)
            {
                if (view.ContainsAny(VariationsOf(entity)))
                    result.Add(entity);
            }
            return result;
        }

        private static List<string> VariationsOf(string entity)
        {
            return new List<string> {$".{entity}.", $".{entity} ", $".{entity})", $".{entity}\r\n",
                $".{entity}\r", $".{entity}\n", $".{entity}\"", $".{entity}>", $"<{entity}>",
                $"ViewBag.{entity}Id", $"ViewData.{entity}Id", $"@model {entity} ",
                $"@model {entity}\r\n", $"@model {entity}\r", $"@model {entity}\n",
                $"type=\"hidden\" asp-for=\"{entity}Id\"", $"type=\"hidden\" asp-for=\"{entity}.",
                $"type=\"hidden\" asp-for=\"{entity}\""}; //should regex this
        }
        public static bool ContainsAny(this string input, List<string> searchStrings)
        {
            foreach (string searchTerm in searchStrings)
                if (input.Contains(searchTerm)) return true;
            return false;
        }

        public static int[] GetRowNumbersFor(this string[][] input, string searchTerm)
        {
            int[] result = new int[input.Length];
            for (int row = 0; row < input.Length; row++)
                if (input[row].Contains(searchTerm)) result[row] = row;
            return result;
        }

        public static List<string> AddPathTo(this List<string> list, int row, string entity)
        {
            string lastEntityInResult = list[list.Count - 1];
            int entityIndex = Array.IndexOf(navigationRelations[row], lastEntityInResult);
            int startIndex = entityIndex + 1;
            int endIndex = Array.IndexOf(navigationRelations[row], entity);
            for (int index = startIndex; index <= endIndex; index++)
            {
                list.Add(navigationRelations[row][index]);
            }
            return list;
        }

        public static void RemoveUnnessesaryDoubles(this List<List<string>> list)
        {
            for (int row = list.Count - 1; row >= 0; row--)
            {
                if (list[row].Count == 1)
                {
                    string entity = list[row][0];
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (i != row && list[i].Contains(entity)) list[row].Remove(entity);
                    }
                }
            }
        }

        public static List<List<string>> ReturnGrouped(this List<string> list)
        {
            List<List<string>> result = new List<List<string>>();
            foreach (var row in navigationRelations)
            {
                result.Add(new List<string>());
            }
            foreach (string entity in list)
            {
                var rows = navigationRelations.GetRowNumbersFor(entity);
                foreach (int row in rows)
                {
                    if (result[row].Count == 0) result[row].Add(entity);
                    else result[row].AddPathTo(row, entity);
                }
            }
            return result;
        }
    }
}
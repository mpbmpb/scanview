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

        private static readonly string[][] dataModel = new string[][]
        {
            new string [] { "Course", "CourseDesign", "CourseSeminars", "Seminar", "SeminarDays", "Day", "DaySubjects", "Subject" },
            new string [] { "Course", "CourseDates", "CourseDate", "Venue", "Contact" }
        };

        private static List<string> VariationsOf(string entity)
        {
            return new List<string> {$".{entity}.", $".{entity} ", $".{entity})", $".{entity}\r\n",
                $".{entity}\r", $".{entity}\n", $".{entity}\"", $".{entity}>", $"<{entity}>",
                $"ViewBag.{entity}Id", $"ViewData.{entity}Id", $"@model {entity} ",
                $"@model {entity}\r\n", $"@model {entity}\r", $"@model {entity}\n",
                $"type=\"hidden\" asp-for=\"{entity}Id\"", $"type=\"hidden\" asp-for=\"{entity}.",
                $"type=\"hidden\" asp-for=\"{entity}\""};
        }

        public static List<List<string>> GetRelevantEntities(string controllerViewName)
        {
            var result = FindEntities(controllerViewName)
                .OrderByDataModel();

            result.RemoveUnnessesaryDoubles();
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

        public static bool ContainsAny(this string input, List<string> searchStrings)
        {
            foreach (string searchTerm in searchStrings)
                if (input.Contains(searchTerm)) return true;
            return false;
        }

        public static List<List<string>> OrderByDataModel(this List<string> list)
        {
            List<List<string>> orderedResult = GetListOfRightDimensions();

            foreach (string entity in list)
            {
                var rows = dataModel.RowNumbersWhereAppears(entity);
                foreach (int row in rows)
                {
                    if (orderedResult[row].Count == 0) orderedResult[row].Add(entity);
                    else orderedResult[row].ConnectPathTo(row, entity);
                }
            }
            return orderedResult;
        }

        public static List<List<string>> GetListOfRightDimensions()
        {
            List<List<string>> list = new List<List<string>>();
            foreach (var row in dataModel)
                list.Add(new List<string>());
            return list;
        }

        public static int[] RowNumbersWhereAppears(this string[][] input, string searchTerm)
        {
            int[] result = new int[input.Length];
            for (int row = 0; row < input.Length; row++)
                if (input[row].Contains(searchTerm)) result[row] = row;
            return result;
        }

        public static List<string> ConnectPathTo(this List<string> list, int row, string lastEntity)
        {
            string startAtEntity = list[list.Count - 1];
            int startIndex = Array.IndexOf(dataModel[row], startAtEntity) + 1;
            int lastIndex = Array.IndexOf(dataModel[row], lastEntity);
            for (int index = startIndex; index <= lastIndex; index++)
            {
                list.Add(dataModel[row][index]);
            }
            return list;
        }

        public static void RemoveUnnessesaryDoubles(this List<List<string>> list)
        {
            for (int row = 0; row < list.Count; row++)
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
            list.RemoveAll(item => item.Count == 0);
        }
    }
}
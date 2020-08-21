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
            var result = Extensions.GetRelevantEntities("Course/Index");
            result.ForEach(Console.WriteLine);
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

        //TODO  violates srp, should just find
        //      create sort function & returnEntities should be main function that calls the others
        //                                          should be One param controllerViewName i.e. "Day/Index"

        public static List<string> GetRelevantEntities(string controllerViewName)
        {
            return FindEntities(controllerViewName);

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

        public static int[] GetRowNumberFor(this string[][] input, string searchTerm)
        {
            int[] result = new int[input.Length];
            for (int row = 0; row < input.Length; row++)
                if (input[row].Contains(searchTerm)) result[row] = 1;
            return result;
        }

        // if entity has pluralform substring then belongs to entity to the left of said plural
        public static List<string> ReturnGrouped(this List<string> list)
        {
            List<string> result = new List<string>();
            foreach (string entity in list)
            {
                //find index of your row(s), if string with corresponding index in result is empty create new string
                //else check previous entry in string and complete chain (add yourself and any missing intermediates)
                navigationRelations.GetRowNumberFor(entity);
            }
            //check if string in result consists of 1 entity that entity is unique else remove that string
            //substring(.entity or entity.) or exact match
        }
    }
}

//TODO code smell too many conditionals..polymorphism?

//            string[] hits = new string[entities.Length];

//int smallestEntityInChain = Array.IndexOf(entities, "Subject");
//int controllerIndex = Array.IndexOf(entities, controllerName);
//if (controllerIndex <= smallestEntityInChain)
//{
//    bool smallerEntityFound = false;
//    result.Add(hits[controllerIndex]);
//    hits[controllerIndex] = null;
//    for (int index = smallestEntityInChain; index > controllerIndex; index--)
//    {
//        if (smallerEntityFound) { hits[index] = null; }
//        if (hits[index] != null && !smallerEntityFound)
//        {
//            result.Add(hits[index]);
//            hits[index] = null;
//            smallerEntityFound = true;
//        }
//    }
//}

//foreach (var hit in hits) { if (hit != null) { result.Add(hit); } }
//return result;
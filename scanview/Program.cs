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
            var result = FindEntities("Venue", "Index");
            result.ForEach(Console.WriteLine);
        }
        public static void printClassName(Type T)
        {

        }


        public static List<string> FindEntities(string controllerName, string viewName)
        {
            List<string> result = new List<string>();
            string[] hits = new string[entity.Length];
            string[] view = File.ReadAllLines($"../../../../scanview/Views/{controllerName}/{viewName}.cshtml");
            for (int line = 0; line < view.Length; line++)
            {
                for (int current = 0; current < entity.Length; current++)
                {
                    if (VariationsOf(entity[current]).Any(view[line].Contains)) { hits[current] = entity[current]; }
                }
            }
            foreach (var hit in hits) { if (hit != null) { result.Add(hit); } }
            return result;
        }

        private static List<string> VariationsOf(string entity)
        {
            return new List<string> {$".{entity}.", $".{entity} ", $".{entity}\"", $".{entity}>", $"<{entity}>", $"type = \"hidden\" asp-for= \"{entity}Id\"" };
        }

        private static readonly string[] entity = new string[]
        {
            "Course", "CourseDesign", "Seminar", "Day", "Subject", "CourseDate", "Venue", "Contact"
        };
    }
}

//static void printArray(int[][] s)
//{
//    foreach(int[] arr in s)
//    {
//        foreach (int n in arr)
//        {
//            Console.Write(n + ", ");
//        }
//        Console.WriteLine();
//    }
//}
//int[][] s = new int[3][];

//            for (int i = 0; i< 3; i++)
//            {
//                s[i] = Array.ConvertAll(Console.ReadLine().Split(' '), sTemp => Convert.ToInt32(sTemp));
//            }

//public static string ReverseString(string numberStr)
//{
//    return new string(numberStr.Reverse().ToArray());
//}
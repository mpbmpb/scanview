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
            string[] hits = new string[relatedEntities.Length];
            string[] view = File.ReadAllLines($"../../../../scanview/Views/{controllerName}/{viewName}.cshtml");
            for (int line = 0; line < view.Length; line++)
            {
                for (int current = 0; current < relatedEntities.Length; current++)
                {
                    if (VariationsOf(relatedEntities[current]).Any(view[line].Contains)) { hits[current] = relatedEntities[current]; }
                }
            }
            int smallestEntityInChain = Array.IndexOf(relatedEntities, "Subject");
            int controllerIndex = Array.IndexOf(relatedEntities, controllerName);
            // if controller is in range course -> subject should return only controller & smallest entity
            // all entities outside the range or bigger than controller should be added to list.
            if (controllerIndex <= smallestEntityInChain)
            {
                bool smallerEntityFound = false;
                result.Add(hits[controllerIndex]);
                hits[controllerIndex] = null;
                for (int index = smallestEntityInChain; index > controllerIndex; index--)
                {
                    if (smallerEntityFound) { hits[index] = null; }
                    if (hits[index] != null && !smallerEntityFound)
                    {
                        result.Add(hits[index]);
                        hits[index] = null;
                        smallerEntityFound = true;
                    }
                }
            }
            
            foreach (var hit in hits) { if (hit != null) { result.Add(hit); } }
            return result;
        }

        private static List<string> VariationsOf(string entity)
        {
            return new List<string> {$".{entity}.", $".{entity} ", $".{entity}\"", $".{entity}>", $"<{entity}>",
                $"type=\"hidden\" asp-for=\"{entity}Id\"" };
        }

        private static readonly string[] relatedEntities = new string[]
        {
            "Course", "CourseDesign", "Seminar", "Day", "Subject", "CourseDate", "Venue", "Contact"
        };
    }
}

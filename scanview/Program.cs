﻿using System;
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


        private static readonly string[] entities = new string[]
        {
            "Course", "CourseDesign", "Seminar", "Day", "Subject", "CourseDate", "Venue", "Contact"
        };

        //TODO violates srp, should just find -> create sort function & returnEntities should be main function that calls the others
        public static List<string> FindEntities(string controllerName, string viewName)
        {
            List<string> result = new List<string>();
            string[] hits = new string[entities.Length];
            string view = File.ReadAllText($"../../../../scanview/Views/{controllerName}/{viewName}.cshtml");
         
            for (int current = 0; current < entities.Length; current++)
            {
                if (VariationsOf(entities[current]).Any(view.Contains)) { hits[current] = entities[current]; }
            }                                               // TODO make ContainsAny ext method
            
            int smallestEntityInChain = Array.IndexOf(entities, "Subject");
            int controllerIndex = Array.IndexOf(entities, controllerName);
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
            return new List<string> {$".{entity}.", $".{entity} ", $".{entity}\r\n",
                $".{entity}\r", $".{entity}\n", $".{entity}\"", $".{entity}>", $"<{entity}>",
                $"ViewBag.{entity}Id", $"ViewData.{entity}Id", $"@model {entity} ",
                $"@model {entity}\r\n", $"@model {entity}\r", $"@model {entity}\n",
                $"type=\"hidden\" asp-for=\"{entity}Id\"", $"type=\"hidden\" asp-for=\"{entity}.",
                $"type=\"hidden\" asp-for=\"{entity}\""};
        }

    }
}

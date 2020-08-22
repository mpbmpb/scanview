using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace scanview
{
    using scanview.Models;

    public class Program
    {
        static void Main(string[] args)
        {
            var dataModel = new DataModel();
            var viewScanner = new ViewScanner(dataModel);

            var result = viewScanner.GetRelatedEntities("Course/Details");
            int count = 1;
            foreach (var list in result)
            {
                Console.WriteLine($"List{count}");
                list.ForEach(Console.WriteLine);
                count++;
            }

            dataModel.Build<SchoolContext>();
        }
    }


    public class DataModel
    {
        internal readonly string[][] relations = new string[][]
        {
            new string [] { "Course", "CourseDesign", "CourseSeminars", "Seminar",
                            "SeminarDays", "Day", "DaySubjects", "Subject" },
            new string [] { "Course", "CourseDates", "CourseDate", "Venue", "Contact" }
        };

        internal readonly string path = "../../../../scanview/Views/";

        public void Build<T>() where T : class//srp should only build, ask for blocks with methods
        {
            var contextProperties = typeof(T).GetProperties()
                .Where(x => x.PropertyType.IsGenericType && x.PropertyType.Name
                .Contains("DbSet")).ToArray();
            List<Type> TypeList = new List<Type>();
            foreach (var type in contextProperties)
            {
                var gen = type.PropertyType.GetGenericArguments()[0];
                TypeList.Add(gen);
            }

            int[][] relationModel = new int[TypeList.Count][];
            for (int i = 0; i < TypeList.Count; i++)
            {  
                var genericProperties = TypeList[i].GetProperties()
                    .Where(x => x.PropertyType.IsGenericType
                    && TypeList.Contains(x.PropertyType.GetGenericArguments()[0])).ToArray();
                var nonGenericProperties = TypeList[i].GetProperties()
                    .Where(x => TypeList.Contains(x.PropertyType)).ToArray();

                int[] navigationPropertyPointers = new int[genericProperties.Length + nonGenericProperties.Length];
                for (int index = 0; index < genericProperties.Length; index++)
                {
                    navigationPropertyPointers[index] = TypeList.IndexOf(genericProperties[index]
                                                        .PropertyType.GetGenericArguments()[0]);
                }
                for (int index = 0; index < nonGenericProperties.Length; index++)
                {
                    navigationPropertyPointers[index + genericProperties.Length] = TypeList
                        .IndexOf(nonGenericProperties[index].PropertyType);
                }

                relationModel[i] = navigationPropertyPointers;
            }
        }
    }

    public class ViewScanner
    {
        private List<string> VariationsOf(string entity)
        {
            return new List<string> {$".{entity}.", $".{entity} ", $".{entity})", $".{entity}\r\n",
                $".{entity}\r", $".{entity}\n", $".{entity}\"", $".{entity}>", $"<{entity}>",
                $"ViewBag.{entity}Id", $"ViewData.{entity}Id", $"@model {entity} ",
                $"@model {entity}\r\n", $"@model {entity}\r", $"@model {entity}\n",
                $"type=\"hidden\" asp-for=\"{entity}Id\"", $"type=\"hidden\" asp-for=\"{entity}.",
                $"type=\"hidden\" asp-for=\"{entity}\""};
        }

        private DataModel Model { get; set; }

        public ViewScanner(DataModel model)
        {
            Model = model;
        }
 
        public  List<List<string>> GetRelatedEntities(string controllerViewName)
        {
            var entities = FindEntities(controllerViewName);
            var result = OrderByDataModel(entities)
                .CleanUp();
            return result;
        }

        public  List<string> FindEntities(string controllerViewName)
        {
            List<string> result = new List<string>();
            string view = File.ReadAllText($"{Model.path}{controllerViewName}.cshtml");

            foreach (var row in Model.relations)
                foreach (var entity in row)
            {
                if (view.ContainsAny(VariationsOf(entity)))
                    result.Add(entity);
            }
            return result;
        }

        public List<List<string>> OrderByDataModel(List<string> list)
        {
            List<List<string>> orderedResult = GetListOfRightDimensions();
            var model = Model.relations;

            foreach (string entity in list)
            {
                var rows = model.RowNumbersWhereAppears(entity);
                foreach (int row in rows)
                {
                    if (orderedResult[row].Count == 0) orderedResult[row].Add(entity);
                    else orderedResult[row].ConnectPathTo(model[row], entity);
                }
            }
            return orderedResult;
        }

        public  List<List<string>> GetListOfRightDimensions()
        {
            List<List<string>> list = new List<List<string>>();
            foreach (var row in Model.relations)
                list.Add(new List<string>());
            return list;
        }
    }

    public static class Extensions
    {

        public static bool ContainsAny(this string input, List<string> searchStrings)
        {
            foreach (string searchTerm in searchStrings)
                if (input.Contains(searchTerm)) return true;
            return false;
        }

        public static int[] RowNumbersWhereAppears(this string[][] input, string searchTerm)
        {
            int[] result = new int[input.Length];
            for (int row = 0; row < input.Length; row++)
                if (input[row].Contains(searchTerm)) result[row] = row;
            return result;
        }

        public static List<string> ConnectPathTo(this List<string> list,
                                                string[] modelRow, string lastEntity)
        {
            string startAtEntity = list[list.Count - 1];
            int startIndex = Array.IndexOf(modelRow, startAtEntity) + 1;
            int lastIndex = Array.IndexOf(modelRow, lastEntity);
            for (int index = startIndex; index <= lastIndex; index++)
            {
                list.Add(modelRow[index]);
            }
            return list;
        }

        public static List<List<string>> CleanUp(this List<List<string>> list)
        {
            for (int row = 0; row < list.Count; row++)
            {
                if (list[row].Count == 1)
                {
                    string entity = list[row][0];
                    for (int i = 0; i < list.Count; i++)
                        if (i != row && list[i].Contains(entity)) list[row].Remove(entity);
                }
            }
            list.RemoveAll(item => item.Count == 0);
            return list;
        }
    }
}
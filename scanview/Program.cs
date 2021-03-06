﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using scanview.Models;

namespace scanview
{

    public class Program
    {
        static void Main(string[] args)
        {
            var dataModel = new DataModel();

            dataModel.Build<SchoolContext>();
            for (int index = 0; index < dataModel.Entities.Count; index++)
            {
                string fill;
                if (dataModel.Entities[index].Name.Length < 8) fill = "\t";
                else fill = "";
                Console.Write($"{dataModel.Entities[index].Name}{fill}\t " + " { ");
                dataModel.NavigationRelations[index].ForEach(x => Console.Write($"{x.Name}, "));
                Console.WriteLine(" }");
            }
            var matrix = new List<List<string>>
            {
                new List<string> { "one", "two", "three" },
                new List<string> { "five", "six", "seven"},
                new List<string> { "seven", "six", "five"},
                new List<string> { "two", "three" },
                new List<string> { "three", "four", "five" },
                new List<string> { "two", "three", "four", "five" },
                new List<string> { "three", "two" },
                new List<string> { "four", "three", "two" },
                new List<string> { "four", "three", "one" },
                new List<string> { "five", "four", "three"},
                new List<string> { "five", "four", "three", "one" },
            };
            var List = new List<string> { "two", "three", "four", "five", "six" };

            int counter = 0;
            foreach (var row in matrix)
            {
                if (row.Count <= List.Count)
                {
                    row.Print();
                    int matchIndex = 0;
                    bool Match = row.IsSubsetOf(List);
                    bool PartialMatch = false;
                    if (!Match)
                    {
                        for (int i = 1; i < row.Count; i++)
                        {
                            if (List.StartsOrEndsWith(row.GetRange(i, row.Count - i)))
                            {
                                matchIndex = i;
                                row.Print();
                                PartialMatch = true;
                                break;
                            }
                            row.Reverse();
                            if (List.StartsOrEndsWith(row.GetRange(i, row.Count - i)))
                            {
                                matchIndex = i;
                                row.Print();
                                PartialMatch = true;
                                break;
                            }

                        }
                    }
                    if (Match) Console.WriteLine($"row {counter} matches.");
                    if (PartialMatch) Console.WriteLine($"row {counter} has partial match at index {matchIndex}");
                }
                // TODO do thesame as section above but switch row and List
                Console.WriteLine("--------------------------------------");
                counter++;
            }
        }
    }
    public static class Extra
    {
        public static bool IsSubsetOf(this List<string> row, List<string> List)
        {
            for (int i = 0; i < List.Count - row.Count + 1; i++)
            {
                if (List.GetRange(i, row.Count).SequenceEqual(row)) return true;
                row.Reverse();
                if (List.GetRange(i, row.Count).SequenceEqual(row)) return true;
            }
            return false;
        }

        public static bool StartsOrEndsWith(this List<string> List, List<string> row)
        {
            int end = List.Count;
            row.Print();
            if (List.GetRange(0, row.Count).SequenceEqual(row)) return true;
            row.Reverse();
            row.Print();
            if (List.GetRange(end - row.Count, row.Count).SequenceEqual(row)) return true;
            return false;
        }

        public static void Print(this List<string> List)
        {
            foreach (var x in List) Console.Write(x + " ");
            Console.WriteLine();
        }
    }
}

//var viewScanner = new ViewScanner(dataModel);
//var result = viewScanner.GetRelatedEntities("Course/Details");
//int count = 1;
//foreach (var list in result)
//{
//    Console.WriteLine($"List{count}");
//    list.ForEach(Console.WriteLine);
//    count++;
//}
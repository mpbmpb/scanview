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
            var result = FindEntities("Subject");
            Array.ForEach(result, Console.WriteLine);
            Console.ReadLine();

        }
        public static void printClassName(Type T)
        {

        }


        public static string[] FindEntities(string viewName)
        {
            List<string> result = new List<string>();
            string[] hits = new string[entity.Length];
            string[] view = File.ReadAllLines($"Views/{viewName}/Index.cshtml");
            for (int line = 0; line < view.Length; line++)
            {
                for (int i = 0; i < entity.Length; i++)
                {
                    if (view[line].Contains(entity[i])) { hits[i] = entity[i]; }

                }
            }
            return hits;
        }

        private static readonly string[] entity = new string[]
        {
            "Subject", "Day", "Contact"
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace scanview
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] testing = new int[3];
            static int saveThePrisoner(int n, int m, int s)
            {
                int result = ((m - 1) % n) + s;
                return (result - 1) % n + 1;
            }
            int[] a = new int[3];
            a[0] = 1;
            
            
            string[] input = File.ReadAllLines("input00.txt");
            string[] answers = File.ReadAllLines("output00.txt");
            int count = Convert.ToInt32(input[0]);

            for (int line = 1; line <= count; line++)
            {
                string[] values = input[line].Split(' ');
                int n = Convert.ToInt32(values[0]);
                int m = Convert.ToInt32(values[1]);
                int s = Convert.ToInt32(values[2]);

                int result = saveThePrisoner(n, m, s);
                int answer = Convert.ToInt32(answers[line - 1]);
                if (result != answer)
                {
                    Console.WriteLine($"Wrong answer to case at line {line}");
                    Console.WriteLine($"Values: {input[line]} , expected: {answer} doch instead got: {result}");
                }
            }

            Console.ReadLine();
            
        }


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
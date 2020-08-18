using System;
using System.Collections.Generic;
using System.Linq;

namespace scanview
{
    class Program
    {
        static void Main(string[] args)
        {
            static int formingMagicSquare(int[][] s)
            {
                int cost = 0;
                int[][] copy = copyArray(s);
                copy[1][1] = 5;
                cost += Math.Abs(s[1][1] - 5);
                if (isMagicSquare(copy)) { return cost; }

                List<int[]> middleRows = getMiddleRows();
                List<int[]> topBottomRows = getTopBottomRows();
                cost = 81;

                for (int middleRowGuess = 0; middleRowGuess < 8; middleRowGuess++)
                {
                    for (int topRowGuess = 0; topRowGuess < 24; topRowGuess++)
                    {
                        for (int bottomRowGuess = 0; bottomRowGuess < 24; bottomRowGuess++)
                        {
                            int currentCost = 0;
                            if (bottomRowGuess == topRowGuess
                                && bottomRowGuess != 23) { bottomRowGuess++; }
                            if (isMagicSquare(
                                new int[3][]
                                {
                                    topBottomRows[topRowGuess],
                                    middleRows[middleRowGuess],
                                    topBottomRows[bottomRowGuess]

                                }))
                            {
                                currentCost += costPerRow(s[0], topBottomRows[topRowGuess]);
                                currentCost += costPerRow(s[1], middleRows[middleRowGuess]);
                                currentCost += costPerRow(s[2], topBottomRows[bottomRowGuess]);
                                if (currentCost < cost) { cost = currentCost; }
                            }
                        }
                    }
                }
                return cost;
            }
            static bool isMagicSquare(int[][] s)
            {
                List<int> check = new List<int>();
                for ( int i = 0; i < 3; i++)
                {
                    check.Add(s[i].Sum());
                    int som = 0;
                    for ( int j = 0; j < 3; j++)
                    {
                        som += s[j][i];
                    }
                    check.Add(som);
                }
                check.Add(s[0][0] + s[1][1] + s[2][2]);
                check.Add(s[0][2] + s[1][1] + s[2][0]);

                return (check.Max() == check.Min() && check.Max() == 15
                        && containsAllValues(s));
            }

            static bool containsAllValues(int[][] s)
            {
                int check = 0;
                for ( int i = 1; i < 10; i++)
                {
                    if (s[0].Contains(i) || s[1].Contains(i) || s[2].Contains(i)) { check++; }
                }
                return check == 9;
            }

            static int[][] copyArray (int[][] s)
            {
                int[][] copy = new int[3][];
                for (int i = 0; i < 3; i++)
                {
                    copy[i] = s[i].Select(a => a).ToArray();

                }
                return copy;
            }

            static List<int[]> getMiddleRows()
            {
                List<int[]> middleRows = new List<int[]>
                {
                    new int[]{ 1, 5, 9 },
                    new int[]{ 9, 5, 1 },
                    new int[]{ 2, 5, 8 },
                    new int[]{ 8, 5, 2 },
                    new int[]{ 3, 5, 7 },
                    new int[]{ 7, 5, 3 },
                    new int[]{ 4, 5, 6 },
                    new int[]{ 6, 5, 4 },
                };
                return middleRows;
            }

            static List<int[]> getTopBottomRows()
            {
                List<int[]> rows = new List<int[]>();
                for (int x = 1; x < 10; x++)
                {
                    if (x == 5) { x++; }
                    for (int y = 1; y < 10; y++)
                    {
                        if (y != 5 && y != x)
                        {
                            for (int z = 1; z < 10; z++)
                            {
                                if (z != 5 && z != x && z != y)
                                {
                                    if (x + y + z == 15) { rows.Add(new int[] { x, y, z }); }
                                }
                            }
                        }
                    }
                }
                return rows;
            }

            static int costPerRow(int[] s, int[] copy)
            {
                int costRow = 0;
                for (int i = 0; i < 3; i++)
                {
                    costRow += Math.Abs(s[i] - copy[i]);
                }
                return costRow;
            }

            int[][] s = new int[3][];

            for (int i = 0; i < 3; i++)
            {
                s[i] = Array.ConvertAll(Console.ReadLine().Split(' '), sTemp => Convert.ToInt32(sTemp));
            }

           

            Console.WriteLine(isMagicSquare(s));
            var result = formingMagicSquare(s);
            Console.WriteLine("result: " + result);
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
using System;
using System.Collections.Generic;
using System.Linq;

namespace scanview
{
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
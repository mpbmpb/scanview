using System;
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
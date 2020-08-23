using System.Collections.Generic;
using System.IO;

namespace scanview
{
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

        public List<List<string>> GetRelatedEntities(string controllerViewName)
        {
            var entities = FindEntities(controllerViewName);
            var result = OrderByDataModel(entities)
                .CleanUp();
            return result;
        }

        public List<string> FindEntities(string controllerViewName)
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

        public List<List<string>> GetListOfRightDimensions()
        {
            List<List<string>> list = new List<List<string>>();
            foreach (var row in Model.relations)
                list.Add(new List<string>());
            return list;
        }
    }
}
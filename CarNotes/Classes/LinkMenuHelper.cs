using CarNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.Classes
{
    public class LinkMenuHelper
    {
        private List<LinkMenuModel> Menu = new List<LinkMenuModel> { new LinkMenuModel { NameLink="Общая таблица", MethodLink="Index", Color ="#EB86E5"},
        new LinkMenuModel { NameLink="Заправка", MethodLink="GoToRefuelEvents", Color="#84EBDF" }, new LinkMenuModel{ NameLink="Ремонт",
            MethodLink="GoToRepairEvents", Color="#EB7C7A"}, new LinkMenuModel{ NameLink="Новая заправка", MethodLink="CreateNewEvent", 
            Color="#A8EB8A"}, new LinkMenuModel{ NameLink="Новый ремонт", MethodLink="CreateNewRepairEvent", Color="#EBA281"} };

        public List<LinkMenuModel> get(string name) 
        {
            var answer = Menu.Where(x => x.NameLink != name).ToList();
            return answer;
        }

        public string getColor(string name)
        {
            var answer = Menu.Find(x => x.NameLink == name);
            return answer.Color;
        }
    }
}
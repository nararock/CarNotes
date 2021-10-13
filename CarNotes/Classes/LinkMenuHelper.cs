using CarNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.Classes
{
    public class LinkMenuHelper
    {
        private List<LinkMenuModel> Menu;

        private List<SubMenuModel> SubMenus = new List<SubMenuModel> { new SubMenuModel { Name = "Новая заправка" }, new SubMenuModel { Name = "Новый ремонт" } };

        public LinkMenuHelper()
        {
            Menu = new List<LinkMenuModel> {
                new LinkMenuModel { NameLink="Общая таблица", MethodLink="~/Home/Index", Color ="#EB86E5", Buttons=SubMenus },
                new LinkMenuModel { NameLink="Заправка", MethodLink="~/Refuel/Index", Color="#84EBDF", Buttons=new List<SubMenuModel>{ SubMenus[0]} }, 
                new LinkMenuModel { NameLink="Ремонт", MethodLink="~/Repair/Index", Color="#EB7C7A", Buttons=new List<SubMenuModel>{ SubMenus[1]} }
            };
        }

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

        public List<SubMenuModel> getButtons(string namePage)
        {
            var answer = Menu.First(x => x.NameLink == namePage).Buttons;
            return answer;
        }
    }
}
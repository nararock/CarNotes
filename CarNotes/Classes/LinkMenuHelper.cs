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

        private List<SubMenuModel> SubMenus = new List<SubMenuModel> { new SubMenuModel { Name = "Новая заправка" }, new SubMenuModel { Name = "Новый ремонт" },
            new SubMenuModel { Name = "Добавить новое транспортное средство"}, new SubMenuModel { Name= "Новый расход"} };

        public LinkMenuHelper()
        {
            Menu = new List<LinkMenuModel> {
                new LinkMenuModel { NameLink="Общая таблица", MethodLink="~/Home/Index", Color ="violet", Buttons=new List<SubMenuModel> { SubMenus[0], SubMenus[1], SubMenus[3]} },
                new LinkMenuModel { NameLink="Заправки", MethodLink="~/Refuel/Index", Color="purple", Buttons=new List<SubMenuModel>{ SubMenus[0]} }, 
                new LinkMenuModel { NameLink="Ремонты", MethodLink="~/Repair/Index", Color="pink", Buttons=new List<SubMenuModel>{ SubMenus[1]} },
                new LinkMenuModel {NameLink="Гараж" , MethodLink="~/Vehicle/Index" , Color="brown" , Buttons=new List<SubMenuModel> { SubMenus[2]}},
                new LinkMenuModel {NameLink="Расходы", MethodLink="~/Expense/Index", Color="blue", Buttons=new List<SubMenuModel> {SubMenus[3]}}
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
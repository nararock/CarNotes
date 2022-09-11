using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarNotes.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CarNotes.Test.Classes.StatisticHelper
{
    [TestClass]
    public class GetTimeSpentOnSite
    {
        [TestMethod]
        public void Yesterday()
        {
            var statisticHelper = new CarNotes.Classes.StatisticHelper();
            var start = new DateTime(2000, 01, 01, 0, 0, 0);
            var end = new DateTime(2000, 01, 02, 0, 0, 0);
            var reference = new CommonTimeOnSite
            {
                Day = 1,
                formDay = "день",
                formMonth = "",
                formYear = ""
            };
            var result = statisticHelper.GetTimeSpentOnSite(start, end);
            Assert.IsNotNull(result);
            Assert.AreEqual(reference.Day, result.Day);
            Assert.AreEqual(reference.Month, result.Month);
            Assert.AreEqual(reference.Year, result.Year);
            Assert.AreEqual(reference.formDay, result.formDay);
            Assert.AreEqual(reference.formMonth, result.formMonth);
            Assert.AreEqual(reference.formYear, result.formYear);
        }

        [TestMethod]
        public void MonthWithoutDay()
        {
            var statisticHelper = new CarNotes.Classes.StatisticHelper();
            var start = new DateTime(2000, 01, 03, 0, 0, 0);
            var end = new DateTime(2000, 02, 02, 0, 0, 0);
            var reference = new CommonTimeOnSite
            {
                Day = 30,
                formDay = "дней",
                formMonth = "",
                formYear = ""
            };
            var result = statisticHelper.GetTimeSpentOnSite(start, end);
            Assert.IsNotNull(result);
            Assert.AreEqual(reference.Day, result.Day);
            Assert.AreEqual(reference.Month, result.Month);
            Assert.AreEqual(reference.Year, result.Year);
            Assert.AreEqual(reference.formDay, result.formDay);
            Assert.AreEqual(reference.formMonth, result.formMonth);
            Assert.AreEqual(reference.formYear, result.formYear);
        }

        [TestMethod]
        public void NumberMonthsIsZero()
        {
            var statisticHelper = new CarNotes.Classes.StatisticHelper();
            var start = new DateTime(2001, 01, 05, 0, 0, 0);
            var end = new DateTime(2002, 01, 03, 0, 0, 0);
            var reference = new CommonTimeOnSite()
            {
                Day = 28,
                Month = 11,
                formDay = "дней",
                formMonth = "месяцев",
                formYear = ""
            };
            var result = statisticHelper.GetTimeSpentOnSite(start, end);
            Assert.IsNotNull(result);
            Assert.AreEqual(reference.Day, result.Day);
            Assert.AreEqual(reference.Month, result.Month);
            Assert.AreEqual(reference.Year, result.Year);
            Assert.AreEqual(reference.formDay, result.formDay);
            Assert.AreEqual(reference.formMonth, result.formMonth);
            Assert.AreEqual(reference.formYear, result.formYear);
        }
    }
}

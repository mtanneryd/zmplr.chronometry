using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zmplr.Chronometry.Tests
{
    [TestClass]
    public class DateTimeExtensionsFixture
    {
        [TestMethod]
        public void TimePeriodShouldBeSlicedInDecades()
        {
            var from = new DateTime(1968, 10, 4);
            var to = new DateTime(2018, 10, 4);
            var interval = new Interval(from, to);
            var decades = interval.Slice(CalendarUnit.Decade).ToArray();
            Assert.AreEqual(6, decades.Length);
        }

        [TestMethod]
        public void NextDay()
        {
            var today = new DateTime(2016, 10, 04);
            var nextDay = today.NextMonday();
            Assert.AreEqual(new DateTime(2016, 10, 10), nextDay);

            nextDay = today.NextTuesday(2);
            Assert.AreEqual(new DateTime(2016, 10, 18), nextDay);

            nextDay = today.NextWednesday();
            Assert.AreEqual(new DateTime(2016, 10, 5), nextDay);

            nextDay = today.NextThursday(3);
            Assert.AreEqual(new DateTime(2016, 10, 20), nextDay);

            nextDay = today.NextFriday();
            Assert.AreEqual(new DateTime(2016, 10, 7), nextDay);

            nextDay = today.NextSaturday(2);
            Assert.AreEqual(new DateTime(2016, 10, 15), nextDay);

            nextDay = today.NextSunday();
            Assert.AreEqual(new DateTime(2016, 10, 9), nextDay);
        }

        [TestMethod]
        public void PrevDay()
        {
            var today = new DateTime(2016, 10, 04);
            var prevDay = today.PrevMonday();
            Assert.AreEqual(new DateTime(2016, 10, 3), prevDay);

            prevDay = today.PrevTuesday(2);
            Assert.AreEqual(new DateTime(2016, 9, 20), prevDay);

            prevDay = today.PrevWednesday();
            Assert.AreEqual(new DateTime(2016, 9, 28), prevDay);

            prevDay = today.PrevThursday(3);
            Assert.AreEqual(new DateTime(2016, 9, 15), prevDay);

            prevDay = today.PrevFriday();
            Assert.AreEqual(new DateTime(2016, 9, 30), prevDay);

            prevDay = today.PrevSaturday(2);
            Assert.AreEqual(new DateTime(2016, 9, 24), prevDay);

            prevDay = today.PrevSunday();
            Assert.AreEqual(new DateTime(2016, 10, 2), prevDay);
        }

        [TestMethod]
        public void CalculateQuarter()
        {
            var d = new DateTime(2016, 1, 1);
            Assert.AreEqual(1, d.QuarterOrdinal());

            d = new DateTime(2016, 2, 1);
            Assert.AreEqual(1, d.QuarterOrdinal());

            d = new DateTime(2016, 3, 1);
            Assert.AreEqual(1, d.QuarterOrdinal());

            d = new DateTime(2016, 4, 1);
            Assert.AreEqual(2, d.QuarterOrdinal());

            d = new DateTime(2016, 5, 1);
            Assert.AreEqual(2, d.QuarterOrdinal());

            d = new DateTime(2016, 6, 1);
            Assert.AreEqual(2, d.QuarterOrdinal());

            d = new DateTime(2016, 7, 1);
            Assert.AreEqual(3, d.QuarterOrdinal());

            d = new DateTime(2016, 8, 1);
            Assert.AreEqual(3, d.QuarterOrdinal());

            d = new DateTime(2016, 9, 1);
            Assert.AreEqual(3, d.QuarterOrdinal());

            d = new DateTime(2016, 10, 1);
            Assert.AreEqual(4, d.QuarterOrdinal());

            d = new DateTime(2016, 11, 1);
            Assert.AreEqual(4, d.QuarterOrdinal());

            d = new DateTime(2016, 12, 1);
            Assert.AreEqual(4, d.QuarterOrdinal());
        }

        [TestMethod]
        public void LastDayOfWeekFromMonday()
        {
            var monday = new DateTime(2019,3, 4);
            var expected = new DateTime(2019, 3, 10);
            var actual = monday.LastDayOfWeek();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LastDayOfWeekFromTuesday()
        {
            var tuesday = new DateTime(2019,3, 5);
            var expected = new DateTime(2019, 3, 10);
            var actual = tuesday.LastDayOfWeek();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LastDayOfWeekFromWednesday()
        {
            var wednesday = new DateTime(2019,3, 6);
            var expected = new DateTime(2019, 3, 10);
            var actual = wednesday.LastDayOfWeek();
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void LastDayOfWeekFromThursday()
        {
            var thursday = new DateTime(2019,3, 7);
            var expected = new DateTime(2019, 3, 10);
            var actual = thursday.LastDayOfWeek();
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void LastDayOfWeekFromFriday()
        {
            var friday = new DateTime(2019,3, 8);
            var expected = new DateTime(2019, 3, 10);
            var actual = friday.LastDayOfWeek();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LastDayOfWeekFromSaturday()
        {
            var saturday = new DateTime(2019,3, 9);
            var expected = new DateTime(2019, 3, 10);
            var actual = saturday.LastDayOfWeek();
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void LastDayOfWeekFromSunday()
        {
            var sunday = new DateTime(2019,3, 10);
            var expected = new DateTime(2019, 3, 10);
            var actual = sunday.LastDayOfWeek();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FirstDayOfWeekFromTuesday()
        {
            var tuesday = new DateTime(2019,3, 5);
            var expected = new DateTime(2019, 3, 4);
            var actual = tuesday.FirstDayOfWeek();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FirstDayOfWeekFromWednesday()
        {
            var tuesday = new DateTime(2019,3, 6);
            var expected = new DateTime(2019, 3, 4);
            var actual = tuesday.FirstDayOfWeek();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FirstDayOfWeekFromThursday()
        {
            var tuesday = new DateTime(2019,3, 7);
            var expected = new DateTime(2019, 3, 4);
            var actual = tuesday.FirstDayOfWeek();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FirstDayOfWeekFromFriday()
        {
            var tuesday = new DateTime(2019,3, 8);
            var expected = new DateTime(2019, 3, 4);
            var actual = tuesday.FirstDayOfWeek();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FirstDayOfWeekFromSaturday()
        {
            var tuesday = new DateTime(2019,3, 9);
            var expected = new DateTime(2019, 3, 4);
            var actual = tuesday.FirstDayOfWeek();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FirstDayOfWeekFromSunday()
        {
            var tuesday = new DateTime(2019,3, 10);
            var expected = new DateTime(2019, 3, 4);
            var actual = tuesday.FirstDayOfWeek();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FirstDayOfWeekFromMonday()
        {
            var tuesday = new DateTime(2019, 3, 11);
            var expected = new DateTime(2019, 3, 11);
            var actual = tuesday.FirstDayOfWeek();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateNumberOfDaysLeftInMonthInJanuary()
        {
            var d = new DateTime(2010, 1, 15);
            var days = d.NumberOfDaysLeftInMonth();
            Assert.AreEqual(16, days);
        }

        [TestMethod]
        public void CalculateWeekDaysUntil()
        {
            var from = new DateTime(2010, 2, 15);
            var to = new DateTime(2010, 2, 28);
            var days = from.WeekDaysUntil(to).Count();
            Assert.AreEqual(10, days);
        }

        [TestMethod]
        public void CalculateWeekendDaysUntil()
        {
            var from = new DateTime(2010, 2, 11);
            var to = new DateTime(2010, 2, 28);
            var days = from.WeekendDaysUntil(to).Count();
            Assert.AreEqual(6, days);
        }

        [TestMethod]
        public void CalculateNumberOfDaysLeftInMonthInFebruari()
        {
            var d = new DateTime(2010, 2, 15);
            var days = d.NumberOfDaysLeftInMonth();
            Assert.AreEqual(13, days);
        }

        [TestMethod]
        public void CalculateNumberOfDaysLeftInMonthInFebruariOnLeapYear()
        {
            var d = new DateTime(2000, 2, 15);
            var days = d.NumberOfDaysLeftInMonth();
            Assert.AreEqual(14, days);
        }

        [TestMethod]
        public void CalculateNumberOfDaysLeftInOrdinaryYear()
        {
            var d = new DateTime(2010, 2, 15);
            var days = d.NumberOfDaysLeftInYear();
            Assert.AreEqual(319, days);
        }

        [TestMethod]
        public void CalculateNumberOfDaysLeftInLeapYear()
        {
            var d = new DateTime(2008, 2, 15);
            var days = d.NumberOfDaysLeftInYear();
            Assert.AreEqual(320, days);
        }
        [TestMethod]
        public void CalculateNumberOfHoursInOrdinaryYear()
        {
            var from = new DateTime(2003, 1, 1);
            var to = new DateTime(2003, 12, 31);
            var days = from.HoursUntil(to);
            Assert.AreEqual(8760, days);
        }

        [TestMethod]
        public void CalculateNumberOfHoursInLeapYear()
        {
            var from = new DateTime(2004, 1, 1);
            var to = new DateTime(2004, 12, 31);
            var days = from.HoursUntil(to);
            Assert.AreEqual(8784, days);
        }

        [TestMethod]
        public void CalculateNumberOfHoursInJanuary()
        {
            var from = new DateTime(2003, 1, 1);
            var to = new DateTime(2003, 1, 31);
            var days = from.HoursUntil(to);
            Assert.AreEqual(744, days);
        }

        [TestMethod]
        public void CalculateNumberOfHoursDuringSummerTimeTransitionInEurope()
        {
            var from = new DateTime(2003, 3, 1);
            var to = new DateTime(2003, 3, 31);
            var days = from.HoursUntil(to);
            Assert.AreEqual(743, days);
        }

        [TestMethod]
        public void CalculateNumberOfHoursDuringWinterTimeTransitionInEurope()
        {
            var from = new DateTime(2003, 10, 1);
            var to = new DateTime(2003, 10, 31);
            var days = from.HoursUntil(to);
            Assert.AreEqual(745, days);
        }

        [TestMethod]
        public void ThereShouldBeFiveYearsBetween20080201And20120131()
        {
            var from = new DateTime(2008,02,01);
            var to = new DateTime(2012,01,31);
            var numberOfYears = from.NumberOfYearsUntil(to);
            Assert.AreEqual(5, numberOfYears);
        }

        [TestMethod]
        public void ThereShouldBeFiveYearIntervalsBetween20080201And20120131()
        {
            var from = new DateTime(2008,02,01);
            var to = new DateTime(2012,01,31);
            var numberOfYears = from.YearsUntil(to).Count();
            Assert.AreEqual(5, numberOfYears);
        }
    }
}

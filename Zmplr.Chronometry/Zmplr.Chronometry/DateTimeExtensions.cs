/*
 * Copyright ©  2017-2019 Tånneryd IT AB
 * 
 * This file is part of the nuget package Zmplr.Chronometry.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *   http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace Zmplr.Chronometry
{
    public static class DateTimeExtensions
    {
        public static int[] NumberOfDaysInMonth = { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        public static bool IsDayLightSavingBegin(this DateTime selectedDay)
        {
            var dslBegin = new Interval(new DateTime(selectedDay.Year, 3, 1), new DateTime(selectedDay.Year, 3, 30))
                .FilterOnDayOfWeek(DayOfWeek.Sunday)
                .OrderBy(d=>d)
                .Last();
            return (selectedDay == dslBegin);
        }

        public static bool IsDayLightSavingEnd(this DateTime selectedDay)
        {
            var dslEnd = new Interval(new DateTime(selectedDay.Year, 10, 1), new DateTime(selectedDay.Year, 10, 31))
                .FilterOnDayOfWeek(DayOfWeek.Sunday)
                .OrderBy(d => d)
                .Last();
            return (selectedDay == dslEnd);
        }

        public static int QuarterOrdinal(this DateTime self)
        {
            var quarter = ((self.Month - 1) / 3) + 1;
            return quarter;
        }

        public static int NumberOfDaysLeftInMonth(this DateTime from)
        {
            var nextMonth = from.AddMonths(1);
            nextMonth = new DateTime(nextMonth.Year, nextMonth.Month, 1);
            int daysLeftInMonth = (nextMonth - from).Days - 1;
            return daysLeftInMonth;
        }

        public static int NumberOfDaysLeftInYear(this DateTime from)
        {
            var nextYear = new DateTime(from.Year, 12, 31);
            int daysLeftInYear = (nextYear - from).Days;
            return daysLeftInYear;
        }


        public static int NumberOfDaysUntil(this DateTime from, DateTime to)
        {
            var diff = to.Date - from.Date;
            return (int)diff.TotalDays + 1;
        }

        public static int NumberOfWeeksUntil(this DateTime from, DateTime to)
        {
            @from = from.FirstDayOfWeek();
            to = to.LastDayOfWeek();

            return NumberOfDaysUntil(from, to) / 7;
        }

        public static int NumberOfMonthsUntil(this DateTime from, DateTime to)
        {
            return from.MonthsUntil(to).Count();
        }

        public static int NumberOfQuartersUntil(this DateTime from, DateTime to)
        {
            // Find the first day of the quarter containing from
            from = from.FirstDayOfQuarter();
            var quarters = 0;
            foreach (var day in from.DaysUntil(to))
            {
                if ((day.Month == 1 && day.Day == 1) ||
                    (day.Month == 4 && day.Day == 1) ||
                    (day.Month == 7 && day.Day == 1) ||
                    (day.Month == 10 && day.Day == 1))
                    quarters++;
            }

            return quarters;
        }

        public static int NumberOfHalfYearsUntil(this DateTime from, DateTime to)
        {
            // Find the first day of the half year containing from
            from = from.FirstDayOfHalfYear();
            var halfYears = 0;
            foreach (var day in from.DaysUntil(to))
            {
                if ((day.Month == 1 && day.Day == 1) ||
                    (day.Month == 7 && day.Day == 1))
                    halfYears++;
            }

            return halfYears;
        }

        public static int NumberOfSeasonsUntil(this DateTime from, DateTime to)
        {
            // Find the first day of the half year containing from
            from = from.FirstDayOfSeason();
            var seasons = 0;
            foreach (var day in from.DaysUntil(to))
            {
                if ((day.Month == 4 && day.Day == 1) ||
                    (day.Month == 10 && day.Day == 1))
                    seasons++;
            }

            return seasons;
        }

        public static int NumberOfYearsUntil(this DateTime from, DateTime to)
        {
            var interval = new Interval(from, to);
            return interval.Slice(CalendarUnit.Year).Count();
        }

        public static IEnumerable<DateTime> DaysUntil(this DateTime from, DateTime to)
        {
            while (from <= to)
            {
                yield return new DateTime(from.Year, from.Month, from.Day);
                from = from.AddDays(1);
            }
        }


        public static IEnumerable<DateTime> WeekDaysUntil(this DateTime from, DateTime to)
        {
            while (from <= to)
            {
                var d = new DateTime(from.Year, from.Month, from.Day);
                from = from.AddDays(1);

                if (d.DayOfWeek == DayOfWeek.Saturday) continue;
                if (d.DayOfWeek == DayOfWeek.Sunday) continue;

                yield return d;
            }
        }

        public static IEnumerable<DateTime> WeekendDaysUntil(this DateTime from, DateTime to)
        {
            while (from <= to)
            {
                var d = new DateTime(from.Year, from.Month, from.Day);
                from = from.AddDays(1);

                switch (d.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                    case DayOfWeek.Tuesday:
                    case DayOfWeek.Wednesday:
                    case DayOfWeek.Thursday:
                    case DayOfWeek.Friday:
                        continue;
                    default:
                        yield return d;
                        break;
                }
            }
        }

        /// <summary>
        /// This method relies on microsoft handling the effects of
        /// daylight saving transitions. Mostly, it works ...
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static int HoursUntil(this DateTime from, DateTime to)
        {
            // move the to day one day ahead so that hours
            // from the original to-day is included
            to = to.AddDays(1);

            var utcTo = to.Date;
            if (utcTo.Kind != DateTimeKind.Utc) utcTo = utcTo.ToUniversalTime();
            var utcFrom = from.Date;
            if (utcFrom.Kind != DateTimeKind.Utc) utcFrom = utcFrom.ToUniversalTime();

            var ts = utcTo - utcFrom;
            return (int)ts.TotalHours;
        }

        public static IEnumerable<DateTime> MonthsUntil(this DateTime from, DateTime to)
        {
            var fromMonth = new DateTime(from.Year, from.Month, 1);
            var toMonth = new DateTime(to.Year, to.Month, 1);

            while (fromMonth.Date <= toMonth.Date)
            {
                yield return fromMonth;
                fromMonth = fromMonth.AddMonths(1);
            }
        }

        public static IEnumerable<DateTime> YearsUntil(this DateTime from, DateTime to)
        {
            while (from.Year <= to.Year)
            {
                yield return new DateTime(from.Year, 1, 1);
                from = from.AddYears(1);
            }
        }

        public static bool IsSameMonth(this DateTime self, DateTime other)
        {
            return self.Year == other.Year && self.Month == other.Month;
        }

        public static bool IsLeapYear(this DateTime self)
        {
            var year = self.Year;
            bool isLeapYear = ((year % 4 == 0) && (year % 100 != 0) || (year % 400 == 0));
            return isLeapYear;
        }

        public static DateTime FirstDayOfWeek(this DateTime self)
        {
            if (self.DayOfWeek == DayOfWeek.Sunday)
            {
                return self.AddDays(-6);
            }
            else
            {
                var dayOfWeek = (int) self.DayOfWeek;
                return self.AddDays(-(dayOfWeek-1));
            }
        }

        public static DateTime FirstDayOfMonth(this DateTime self)
        {
            return new DateTime(self.Year, self.Month, 1);
        }

        public static DateTime FirstDayOfQuarter(this DateTime self)
        {
            return self.Quarter().Start;
        }

        public static DateTime FirstDayOfHalfYear(this DateTime self)
        {
            return self.HalfYear().Start;
        }

        public static DateTime FirstDayOfSeason(this DateTime self)
        {
            return self.Season().Start;
        }

        public static Interval Quarter(this DateTime self)
        {
            if (self.Month>=1 && self.Month<=3)
                return new Interval(new DateTime(self.Year, 1, 1), new DateTime(self.Year, 3, 31));
            else if (self.Month>=4 && self.Month<=6)
                return new Interval(new DateTime(self.Year, 4, 1), new DateTime(self.Year, 6, 30));
            else if (self.Month>=7 && self.Month<=9)
                return new Interval(new DateTime(self.Year, 7, 1), new DateTime(self.Year, 9, 30));
            
            return new Interval(new DateTime(self.Year, 10, 1), new DateTime(self.Year, 12, 31));
        }

        public static Interval HalfYear(this DateTime self)
        {
            if (self.Month>=1 && self.Month<=6)
                return new Interval(new DateTime(self.Year, 1, 1), new DateTime(self.Year, 6, 30));
            
            return new Interval(new DateTime(self.Year, 7, 1), new DateTime(self.Year, 12, 31));
        }

        public static Interval Season(this DateTime self)
        {
            if (self.Month>=4 && self.Month<=9)
                return new Interval(new DateTime(self.Year, 4, 1), new DateTime(self.Year, 9, 30));
            if (self.Month < 4)
                return new Interval(new DateTime(self.Year-1, 10, 1), new DateTime(self.Year, 3, 31));

            return new Interval(new DateTime(self.Year, 10, 1), new DateTime(self.Year+1, 3, 31));
        }
        public static DateTime FirstDayOfYear(this DateTime self)
        {
            return new DateTime(self.Year, 1, 1);
        }

        public static DateTime LastDayOfWeek(this DateTime self)
        {
            if (self.DayOfWeek == DayOfWeek.Sunday)
            {
                return self;
            }
            else
            {
                var dayOfWeek = (int) self.DayOfWeek;
                return self.AddDays(7-dayOfWeek);
            }
        }

        public static DateTime LastDayOfMonth(this DateTime self)
        {
            var day = NumberOfDaysInMonth[self.Month];
            if (self.Month == 2 && IsLeapYear(self)) day++;
            return new DateTime(self.Year, self.Month, day);
        }

        public static DateTime NextMonday(this DateTime self, int count = 1)
        {
            return NextDay(self, DayOfWeek.Monday, count);
        }
        public static DateTime NextTuesday(this DateTime self, int count = 1)
        {
            return NextDay(self, DayOfWeek.Tuesday, count);
        }
        public static DateTime NextWednesday(this DateTime self, int count = 1)
        {
            return NextDay(self, DayOfWeek.Wednesday, count);
        }
        public static DateTime NextThursday(this DateTime self, int count = 1)
        {
            return NextDay(self, DayOfWeek.Thursday, count);
        }
        public static DateTime NextFriday(this DateTime self, int count = 1)
        {
            return NextDay(self, DayOfWeek.Friday, count);
        }
        public static DateTime NextSaturday(this DateTime self, int count = 1)
        {
            return NextDay(self, DayOfWeek.Saturday, count);
        }
        public static DateTime NextSunday(this DateTime self, int count = 1)
        {
            return NextDay(self, DayOfWeek.Sunday, count);
        }
        private static DateTime NextDay(DateTime self, DayOfWeek dayOfWeek, int count)
        {
            if (count < 1) throw new ArgumentException("count must be > 0");

            var interval = new Interval { Start = self.AddDays(1), Stop = self.AddDays(7 * count) };
            var next = interval.FilterOnDayOfWeek(dayOfWeek).Skip(count - 1).First();

            return next;
        }

        public static DateTime PrevMonday(this DateTime self, int count = 1)
        {
            return PrevDay(self, DayOfWeek.Monday, count);
        }
        public static DateTime PrevTuesday(this DateTime self, int count = 1)
        {
            return PrevDay(self, DayOfWeek.Tuesday, count);
        }
        public static DateTime PrevWednesday(this DateTime self, int count = 1)
        {
            return PrevDay(self, DayOfWeek.Wednesday, count);
        }
        public static DateTime PrevThursday(this DateTime self, int count = 1)
        {
            return PrevDay(self, DayOfWeek.Thursday, count);
        }
        public static DateTime PrevFriday(this DateTime self, int count = 1)
        {
            return PrevDay(self, DayOfWeek.Friday, count);
        }
        public static DateTime PrevSaturday(this DateTime self, int count = 1)
        {
            return PrevDay(self, DayOfWeek.Saturday, count);
        }
        public static DateTime PrevSunday(this DateTime self, int count = 1)
        {
            return PrevDay(self, DayOfWeek.Sunday, count);
        }
        private static DateTime PrevDay(DateTime self, DayOfWeek dayOfWeek, int count)
        {
            if (count < 1) throw new ArgumentException("count must be > 0");

            var interval = new Interval { Start = self.AddDays(-7 * count), Stop = self.AddDays(-1) };
            var next = Enumerable.Reverse(interval.FilterOnDayOfWeek(dayOfWeek))
                .Skip(count - 1)
                .First();

            return next;
        }

    }
}

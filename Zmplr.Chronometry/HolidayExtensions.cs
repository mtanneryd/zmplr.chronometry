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

namespace Zmplr.Chronometry
{
    public static class HolidayExtensions
    {
        public static DateTime EasterSunday(int year)
        {
            int day = 0;
            int month = 0;

            int g = year % 19;
            int c = year / 100;
            int h = (c - (int)(c / 4) - (int)((8 * c + 13) / 25) + 19 * g + 15) % 30;
            int i = h - (int)(h / 28) * (1 - (int)(h / 28) * (int)(29 / (h + 1)) * (int)((21 - g) / 11));

            day = i - ((year + (int)(year / 4) + i + 2 - c + (int)(c / 4)) % 7) + 28;
            month = 3;

            if (day > 31)
            {
                month++;
                day -= 31;
            }

            return new DateTime(year, month, day);
        }

        public static DateTime MaundyThursday(int year)
        {
            return EasterSunday(year).AddDays(-3);
        }

        public static DateTime GoodFriday(int year)
        {
            return EasterSunday(year).AddDays(-2);
        }

        public static DateTime EasterMonday(int year)
        {
            return EasterSunday(year).AddDays(1);
        }

        public static DateTime PalmSunday(int year)
        {
            return EasterSunday(year).AddDays(-7);
        }

        public static DateTime WhitSunday(int year)
        {
            return EasterSunday(year).AddDays(7 * 7);
        }

        public static DateTime WhitMonday(int year)
        {
            return WhitSunday(year).AddDays(1);
        }

        public static DateTime AscensionDay(int year)
        {
            return WhitSunday(year).AddDays(-10);
        }

        public static DateTime AshWednesday(int year)
        {
            return EasterSunday(year).AddDays(-47);
        }

        public static DateTime FirstSundayOfAdvent(int year)
        {
            int weeks = 4;
            int correction = 0;
            DateTime christmas = new DateTime(year, 12, 25);

            if (christmas.DayOfWeek != DayOfWeek.Sunday)
            {
                weeks--;
                correction = ((int)christmas.DayOfWeek - (int)DayOfWeek.Sunday);
            }
            return christmas.AddDays(-1 * ((weeks * 7) + correction));
        }

    }
}
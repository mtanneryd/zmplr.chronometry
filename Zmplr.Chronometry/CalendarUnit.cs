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
namespace Zmplr.Chronometry
{
    public enum CalendarUnit
    {
        Day,
        Week,
        Month,
        Quarter,
        HalfYear,
        Season, // april 1 - september 30 | october 1 to march 31
        Year,
        Decade,
        Century,
        Millenium,
        Aeon
    }
}
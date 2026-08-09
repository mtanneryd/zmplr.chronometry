# Zmplr.Chronometry

**Zmplr.Chronometry** is a small .NET library of `DateTime` helpers, inclusive date intervals, calendar slicing, and Christian movable-feast dates. Use it when you need weekday navigation, period boundaries (week / month / quarter / season), day enumeration between two dates, or interval set operations without pulling in a heavy calendar framework.

It is deliberately focused on calendar dates (day granularity). It is not a time-zone database, NodaTime replacement, or business-holiday calendar for every country.

---

## What it does

- **Date navigation** — next/previous Monday–Sunday (nth occurrence), first/last day of week/month/year
- **Period boundaries** — calendar quarter, half-year, and season as `Interval` values
- **Inclusive ranges** — enumerate days, weekdays, weekends, months, and years between two dates
- **Duration counts** — days/weeks/months/quarters/years left or until a date; hour spans with DST-aware UTC conversion
- **Intervals** — contain, intersect, except, and slice a range into calendar units
- **Movable feasts** — Easter and related Western Christian dates for a given year
- **EU daylight-saving markers** — last Sunday of March / October helpers

### Mental model

| Concept | Meaning |
|---------|---------|
| **`DateTime` extensions** | Operate on a single instant/date; many methods use `.Date` semantics (calendar day) |
| **`Interval`** | Inclusive `[Start, Stop]` range of calendar days |
| **`CalendarUnit`** | Unit used when slicing an interval (day, week, month, …) |
| **Week** | Monday–Sunday (ISO-style week start) |

---

## Requirements

Multi-targets:

| TFM | Supported |
|-----|-----------|
| `net8.0` | Yes |
| `net48` | Yes |
| `netstandard2.0` | Yes |
| `netstandard2.1` | Yes |

No extra runtime packages are required.

---

## Install

```bash
dotnet add package Zmplr.Chronometry
```

```csharp
using Zmplr.Chronometry;
```

---

## Quick start

```csharp
using Zmplr.Chronometry;

var today = new DateTime(2026, 8, 9); // Sunday

// Week boundaries (Monday–Sunday)
var weekStart = today.FirstDayOfWeek();  // 2026-08-03
var weekEnd   = today.LastDayOfWeek();   // 2026-08-09

// Next / previous weekday
var nextMonday = today.NextMonday();     // 2026-08-10
var prevFriday = today.PrevFriday(2);    // second Friday before today

// Period containing a date
Interval q3 = today.Quarter();           // 2026-07-01 .. 2026-09-30
int q       = today.QuarterOrdinal();    // 3

// Inclusive day walk and weekday filter
var days = new DateTime(2026, 2, 15).WeekDaysUntil(new DateTime(2026, 2, 28));
// 10 weekdays

// Slice a multi-year span into decades
var span = new Interval(new DateTime(1968, 10, 4), new DateTime(2018, 10, 4));
Interval[] decades = span.Slice(CalendarUnit.Decade).ToArray();

// Easter family for a year
DateTime easter = HolidayExtensions.EasterSunday(2026);
DateTime goodFriday = HolidayExtensions.GoodFriday(2026);
```

---

## DateTime extensions

All methods are in `DateTimeExtensions` under `Zmplr.Chronometry`.

### Week, month, and year boundaries

| Method | Returns |
|--------|---------|
| `FirstDayOfWeek()` / `LastDayOfWeek()` | Monday / Sunday of the containing week |
| `FirstDayOfMonth()` / `LastDayOfMonth()` | 1st / last calendar day of the month (leap-year aware for February) |
| `FirstDayOfYear()` | 1 January of `self.Year` |
| `FirstDayOfQuarter()` / `FirstDayOfHalfYear()` / `FirstDayOfSeason()` | Start of the period containing `self` |

```csharp
new DateTime(2019, 3, 5).FirstDayOfWeek(); // 2019-03-04 (Monday)
new DateTime(2019, 3, 5).LastDayOfWeek();  // 2019-03-10 (Sunday)
new DateTime(2000, 2, 15).LastDayOfMonth(); // 2000-02-29
```

### Quarters, half-years, and seasons

| Method | Meaning |
|--------|---------|
| `QuarterOrdinal()` | `1`–`4` for calendar quarters |
| `Quarter()` | `Interval` for Q1–Q4 of the year |
| `HalfYear()` | Jan–Jun or Jul–Dec as an `Interval` |
| `Season()` | Summer `Apr 1 – Sep 30`, winter `Oct 1 – Mar 31` (winter may cross the year boundary) |

```csharp
var d = new DateTime(2026, 11, 15);
d.Quarter();   // 2026-10-01 .. 2026-12-31
d.HalfYear();  // 2026-07-01 .. 2026-12-31
d.Season();    // 2026-10-01 .. 2027-03-31
```

### Next and previous weekdays

`NextMonday` … `NextSunday` and `PrevMonday` … `PrevSunday` take an optional `count` (default `1`) for the *n*th occurrence **strictly after** or **strictly before** `self`. `count` must be `> 0`.

```csharp
var today = new DateTime(2016, 10, 4); // Tuesday
today.NextMonday();    // 2016-10-10
today.NextTuesday(2);  // 2016-10-18
today.PrevFriday();    // 2016-09-30
```

### Enumerate and count until a date

Ranges are **inclusive** of both endpoints unless noted.

| Method | Result |
|--------|--------|
| `DaysUntil(to)` | Every calendar day from `from` through `to` |
| `WeekDaysUntil(to)` | Monday–Friday only |
| `WeekendDaysUntil(to)` | Saturday–Sunday only |
| `MonthsUntil(to)` | First of each month from `from`’s month through `to`’s month |
| `YearsUntil(to)` | 1 January for each year from `from.Year` through `to.Year` |
| `NumberOfDaysUntil(to)` | Inclusive day count (`(to.Date - from.Date).Days + 1`) |
| `NumberOfWeeksUntil(to)` | Whole weeks spanning Monday–Sunday alignment of the range |
| `NumberOfMonthsUntil(to)` | Count of months from `MonthsUntil` |
| `NumberOfQuartersUntil` / `NumberOfHalfYearsUntil` / `NumberOfSeasonsUntil` / `NumberOfYearsUntil` | Period counts over the span |
| `NumberOfDaysLeftInMonth()` / `NumberOfDaysLeftInYear()` | Remaining days after `from` in that month/year |
| `HoursUntil(to)` | Hours from start of `from`’s date through end of `to`’s date, via UTC (accounts for DST transitions when `DateTime` conversion does) |

```csharp
var from = new DateTime(2010, 2, 15);
var to   = new DateTime(2010, 2, 28);

from.NumberOfDaysLeftInMonth();           // 13
from.WeekDaysUntil(to).Count();           // 10
from.HoursUntil(new DateTime(2003, 3, 31)); // 743 in a European spring-forward month
```

### Daylight saving (EU-style)

| Method | Meaning |
|--------|---------|
| `IsDayLightSavingBegin(day)` | `true` if `day` is the **last Sunday of March** in that year |
| `IsDayLightSavingEnd(day)` | `true` if `day` is the **last Sunday of October** in that year |

These match common EU transition *dates*, not arbitrary IANA zones. Use a proper time-zone library when you need zone rules.

### Misc

| Method / field | Meaning |
|----------------|---------|
| `IsLeapYear()` | Gregorian leap-year test for `self.Year` |
| `IsSameMonth(other)` | Same year and month |
| `NumberOfDaysInMonth` | Static lookup array (index `1`–`12`; February is `28` — use `LastDayOfMonth` for leap years) |

---

## Interval

`Interval` is an inclusive calendar-day range with `Start`, `Stop`, and `TimeSpan` (`Stop - Start`).

```csharp
var interval = new Interval(new DateTime(2026, 1, 1), new DateTime(2026, 1, 31));
interval.Contains(new DateTime(2026, 1, 15)); // true
interval.TimeSpan; // 30 days in ticks
```

### Containment and set operations

| Method | Behaviour |
|--------|-----------|
| `Contains(DateTime)` | `Start <= day <= Stop` |
| `Contains(from, to, day)` | Static helper over a temporary interval |
| `HasIntersectionWith(other)` | Whether `Intersect` is non-null |
| `Intersect(other)` | Inclusive overlapping day range, or `null` |
| `Except(other)` | Remaining day runs after removing `other`’s days (may yield multiple intervals) |
| `IsEnclosedBy(other)` | Overlap check against `other` (shared days) |

```csharp
var a = new Interval(new DateTime(2026, 1, 1), new DateTime(2026, 1, 20));
var b = new Interval(new DateTime(2026, 1, 10), new DateTime(2026, 1, 31));

Interval overlap = a.Intersect(b);
// Start = 2026-01-10, Stop = 2026-01-20
```

### Slice by calendar unit

```csharp
IEnumerable<Interval> Slice(CalendarUnit calendarUnit);
static IEnumerable<Interval> Slice(DateTime from, DateTime to, CalendarUnit calendarUnit);
```

Splits the interval into sub-intervals of the given unit. The **first and last** pieces may be partial (e.g. a week that does not start on Monday).

Supported by `Slice`:

| `CalendarUnit` | Grouping |
|----------------|----------|
| `Day` | One interval per day |
| `Week` | Monday-aligned weeks where possible |
| `Month` | Calendar months |
| `Quarter` | Calendar quarters |
| `Year` | Calendar years |
| `Decade` | Ten-year bands (`year / 10`) |
| `Century` | Hundred-year bands |
| `Millenium` | Thousand-year bands |
| `Aeon` | Single interval covering the whole span |

`HalfYear` and `Season` exist on the enum for use with `DateTime` helpers (`HalfYear()`, `Season()`); they are **not** implemented in `Interval.Slice`.

```csharp
var from = new DateTime(1968, 10, 4);
var to   = new DateTime(2018, 10, 4);
var decades = new Interval(from, to).Slice(CalendarUnit.Decade).ToArray();
// 6 decade slices across the span
```

### Weekdays inside an interval

```csharp
interval.Mondays();
interval.Tuesdays();
// ...
interval.Sundays();
interval.FilterOnDayOfWeek(DayOfWeek.Wednesday);
```

Returns ordered `DateTime[]` of matching calendar days in `[Start, Stop]`.

---

## Holidays (movable feasts)

`HolidayExtensions` computes Western Christian movable dates from Easter for a given year (Computus). These are **not** a full public-holiday calendar (no fixed national holidays, no Orthodox Easter).

| Method | Relative to Easter Sunday |
|--------|---------------------------|
| `EasterSunday(year)` | — |
| `PalmSunday(year)` | −7 days |
| `MaundyThursday(year)` | −3 days |
| `GoodFriday(year)` | −2 days |
| `EasterMonday(year)` | +1 day |
| `AscensionDay(year)` | Whit Sunday − 10 days |
| `WhitSunday(year)` | +7 weeks |
| `WhitMonday(year)` | Whit Sunday + 1 day |
| `AshWednesday(year)` | −47 days |
| `FirstSundayOfAdvent(year)` | Four Sundays before Christmas (25 Dec) |

```csharp
int year = 2026;
var easter = HolidayExtensions.EasterSunday(year);
var advent = HolidayExtensions.FirstSundayOfAdvent(year);
```

---

## Design notes and caveats

1. **Calendar days, not instants** — Most helpers ignore time-of-day or normalize to dates. Prefer `DateTime.Date` inputs when mixing with timestamps.
2. **Inclusive intervals** — `Interval` and `*Until` enumerations include both ends.
3. **Monday-based weeks** — Consistent with ISO week start; differs from `CultureInfo` calendars that start on Sunday.
4. **`HoursUntil` and DST** — Converts date bounds to UTC so spring-forward / fall-back days can yield 23 or 25 hours. Behaviour depends on the local system’s time-zone conversion rules.
5. **No IANA / NodaTime** — For zone-safe scheduling across regions, compose this library with `TimeZoneInfo` or NodaTime; do not treat EU DST helpers as global truth.
6. **Holiday scope** — Easter-derived dates only; build country-specific holiday sets yourself on top if needed.
7. **`CalendarUnit.HalfYear` / `Season` in `Slice`** — Use `DateTime.HalfYear()` / `Season()` (or custom grouping); `Slice` does not expand those units today.

---

## Package contents

| Type | Role |
|------|------|
| `DateTimeExtensions` | Navigation, periods, enumeration, counts, DST markers |
| `Interval` | Inclusive range, set ops, slicing, weekday filters |
| `CalendarUnit` | Units for `Interval.Slice` |
| `HolidayExtensions` | Easter and related movable feasts |

Source: [github.com/mtanneryd/zmplr.chronometry](https://github.com/mtanneryd/zmplr.chronometry)

---

## Versioning

Package versions are produced with [GitVersion](https://gitversion.net/) (same scheme as other Zmplr libraries):

| Branch | Example |
|--------|---------|
| `master` / `main` | `2026.1.0` |
| `develop` | `2026.1.0-beta.20260809.43371` |
| `feature/*` | `2026.1.0-alpha.…` |
| `release/*` | `2026.1.0-rc.N` |

---

## License

Licensed under the [Apache License, Version 2.0](https://www.apache.org/licenses/LICENSE-2.0).

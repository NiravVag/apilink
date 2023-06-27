using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Common
{
    public class DateObject
    {
        public DateObject() { }

        public DateObject(int year, int month, int day)
        {
            this.Year = year;
            this.Month = month;
            this.Day = day;
        }

        public int Year { get; set; }

        public int Month { get; set; }

        public int Day { get; set; }

        public DateTime ToDateTime()
        {
            return new DateTime(Year, Month, Day);
        }

        public DateTime? ToNullableDateTime()
        {
            if (Year == 0 || Month == 0 || Day == 0)
                return null; 

            return new DateTime(Year, Month, Day);
        }

        public override bool Equals(object obj)
        {
            var item = obj as DateObject;

            if (item == null)
                return false;

            return item.Year == this.Year && item.Month == this.Month && item.Day == this.Day;
        }
    }
}

using System;
using System.Runtime.InteropServices;

namespace GovUk.Frontend.AspNetCore
{
    [StructLayout(LayoutKind.Auto)]
    public readonly struct Date : IComparable, IComparable<Date>, IEquatable<Date>
    {
        private readonly DateTime _dt;

        public Date(int year, int month, int day)
            : this(new DateTime(year, month, day))
        {
        }

        private Date(DateTime dateTime)
        {
            _dt = dateTime;
        }

        public int Day => _dt.Day;

        public int Month => _dt.Month;

        public int Year => _dt.Year;

        public static bool operator ==(Date left, Date right) => left.Equals(right);

        public static bool operator !=(Date left, Date right) => !(left == right);

        public static bool operator <(Date left, Date right) => left._dt < right._dt;

        public static bool operator <=(Date left, Date right) => left._dt <= right._dt;

        public static bool operator >(Date left, Date right) => left._dt > right._dt;

        public static bool operator >=(Date left, Date right) => left._dt >= right._dt;

        public static explicit operator Date(DateTime dt) => new Date(dt);

        public static int Compare(Date t1, Date t2) => DateTime.Compare(t1._dt, t2._dt);

        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            if (!(obj is Date))
            {
                throw new ArgumentException($"Object must be of type {nameof(Date)}.");
            }

            return Compare(this, (Date)obj);
        }

        public int CompareTo(Date value) => Compare(this, value);

        public void Deconstruct(out int year, out int month, out int day)
        {
            year = Year;
            month = Month;
            day = Day;
        }

        public override bool Equals(object obj) => obj is Date date && Equals(date);

        public bool Equals(Date other) => other.Day == Day && other.Month == Month && other.Year == Year;

        public override int GetHashCode() => HashCode.Combine(Day, Month, Year);

        public DateTime ToDateTime(DateTimeKind kind = DateTimeKind.Unspecified) =>
            new DateTime(Year, Month, Day, 0, 0, 0, kind);

        public override string ToString() => $"{Day:D2}/{Month:D2}/{Year:D4}";
    }
}

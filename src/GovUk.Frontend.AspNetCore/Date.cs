using System;
using System.Runtime.InteropServices;

namespace GovUk.Frontend.AspNetCore
{
    /// <summary>
    /// Represents a date.
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public readonly struct Date : IComparable, IComparable<Date>, IEquatable<Date>
    {
        private readonly DateTime _dt;

        /// <summary>
        /// Creates a new instance of the <see cref="Date"/> structure to the specified year, month and day.
        /// </summary>
        /// <param name="year">The year (1 through 9999).</param>
        /// <param name="month">The month (1 through 12).</param>
        /// <param name="day">The day (1 through the number of days in <paramref name="month" />).</param>
        public Date(int year, int month, int day)
            : this(new DateTime(year, month, day))
        {
        }

        private Date(DateTime dateTime)
        {
            _dt = dateTime;
        }

        /// <summary>
        /// Gets the current date.
        /// </summary>
        public static Date Today => new(DateTime.Today);

        /// <summary>
        /// Gets the day component of the date.
        /// </summary>
        public int Day => _dt.Day;

        /// <summary>
        /// Gets the month component of the date.
        /// </summary>
        public int Month => _dt.Month;

        /// <summary>
        /// Gets the year component of the date.
        /// </summary>
        public int Year => _dt.Year;

        /// <summary>
        /// Determines whether two specified instances of <see cref="Date"/> are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if left and right represent the same date; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(Date left, Date right) => left.Equals(right);

        /// <summary>
        /// Determines whether two specified instances of <see cref="Date"/> are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if left and right do not represent the same date; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(Date left, Date right) => !(left == right);

        /// <summary>
        /// Determines whether one specified <see cref="Date"/> is earlier than another specified <see cref="Date"/>.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if left is earlier than right; otherwise, <see langword="false"/>.</returns>
        public static bool operator <(Date left, Date right) => left._dt < right._dt;

        /// <summary>
        /// Determines whether one specified <see cref="Date"/> represents a date that is the same as or earlier than another specified <see cref="Date"/>.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if left is the same as or earlier than right; otherwise, <see langword="false"/>.</returns>
        public static bool operator <=(Date left, Date right) => left._dt <= right._dt;

        /// <summary>
        /// Determines whether one specified <see cref="Date"/> is later than another specified <see cref="Date"/>.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if left is later than right; otherwise, <see langword="false"/>.</returns>
        public static bool operator >(Date left, Date right) => left._dt > right._dt;

        /// <summary>
        /// Determines whether one specified <see cref="Date"/> represents a date that is the same as or later than another specified <see cref="Date"/>.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns><see langword="true"/> if left is the same as or later than right; otherwise, <see langword="false"/>.</returns>
        public static bool operator >=(Date left, Date right) => left._dt >= right._dt;

        /// <inheritdoc/>
        public int CompareTo(object? value)
        {
            if (value == null)
            {
                return 1;
            }

            if (value is not Date date)
            {
                throw new ArgumentException($"Object must be of type {nameof(Date)}.");
            }

            return CompareTo(date);
        }

        /// <inheritdoc/>
        public int CompareTo(Date value) => _dt.CompareTo(value);

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is Date date && Equals(date);

        /// <inheritdoc/>
        public bool Equals(Date other) => other.Day == Day && other.Month == Month && other.Year == Year;

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(Day, Month, Year);

        /// <summary>
        /// Creates a new instance of the <see cref="Date"/> structure from the date part of the specified <see cref="DateTime"/>.
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> instance.</param>
        /// <returns>The <see cref="Date"/> instance composed of the date part of the specified input time <see cref="DateTime"/> instance.</returns>
        public static Date FromDateTime(DateTime value) => new(value);

        /// <summary>
        /// Returns a <see cref="DateTime"/> that is set to the date of this <see cref="Date"/> instance.
        /// </summary>
        /// <returns>The <see cref="DateTime"/> instance composed of the date of the current <see cref="Date"/> instance.</returns>
        public DateTime ToDateTime(DateTimeKind kind = DateTimeKind.Unspecified) =>
            new(Year, Month, Day, 0, 0, 0, kind);

        /// <summary>
        /// Converts the value of the current <see cref="Date"/> object to its string representation in the dd/MM/yyyy format.
        /// </summary>
        /// <returns>A string representation of the current <see cref="Date"/> object.</returns>
        public override string ToString() => $"{Day:D2}/{Month:D2}/{Year:D4}";
    }
}

using System;
using UnityEngine;
using System.Globalization;

public static class DateUtils {

	#region Constants

	/// <summary>
	/// The epoch date as a DateTime.
	/// </summary>
	public static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

	/// <summary>
	/// The number of seconds in a minute.
	/// </summary>
	public const long UNIX_MINUTE = 60L;

	/// <summary>
	/// The number of seconds in an hour.
	/// </summary>
	public const long UNIX_HOUR = 3600L;

	/// <summary>
	/// The number of seconds in a day.
	/// </summary>
	public const long UNIX_DAY = 86400L;

	#endregion

	private static readonly string[] Iso8601Format = new string[]
	{
		@"yyyy-MM-dd\THH:mm:ss.FFFFFFF\Z",
		@"yyyy-MM-dd\THH:mm:ss\Z",
		@"yyyy-MM-dd\THH:mm:ssK"
	};

	private static readonly string WriteIso8601Format = @"yyyy-MM-dd\THH:mm:ss.FFFFFFF\Z";

	public static string IsoDateString(DateTime date)
	{
		return date.ToString(WriteIso8601Format, CultureInfo.InvariantCulture);
	}

	public static DateTime ParseIso(string dateString)
	{
		return DateTime.ParseExact(dateString, Iso8601Format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
	}

	public static bool TryParseIso(string dateString, out DateTime result)
	{
		return DateTime.TryParseExact(dateString, Iso8601Format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out result);
	}

	#region Unix Time Methods

	/// <summary>
	/// Gets the current unix timestamp as an integer.
	/// </summary>
	/// <returns>The time as a unix timestamp.</returns>
	public static int UnixTime32() {
		//return DateTime.UtcNow.ToUnixTime32();
		return (int)(System.DateTime.UtcNow - epoch).TotalSeconds;
	}

	/// <summary>
	/// Returns the current unix timestamp in a long value.
	/// </summary>
	/// <returns>The time as a unix timestamp.</returns>
	public static long UnixTime() {
		return (long)(System.DateTime.UtcNow - epoch).TotalSeconds;
	}
	//System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
	//int cur_time = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
	/// <summary>
	/// Converts a given DateTime into a long value Unix timestamp.
	/// </summary>
	/// <returns>The given DateTime as a long in Unix timestamp format.</returns>
	/// <param name="value">Timestamp to convert.</param>
	public static long ToUnixTime(this DateTime value) {
		return (value.ToUniversalTime().Ticks - epoch.Ticks) / TimeSpan.TicksPerSecond;
	}

    /// <summary>
    /// Converts a given DateTime into a long value milliseconds
    /// </summary>
    /// <returns>The given DateTime as a long in Unix timestamp format.</returns>
    /// <param name="value">Timestamp to convert.</param>
    public static long ToTimeMs(this DateTime value)
    {
        return value.Ticks / TimeSpan.TicksPerMillisecond;
    }

    /// <summary>
    /// Converts a given DateTime into an integer Unix timestamp.
    /// </summary>
    /// <returns>The given DateTime in Unix timestamp format.</returns>
    /// <param name="value">Timestamp to convert.</param>
    public static int ToUnixTime32(this DateTime value) {
		return (int)ToUnixTime(value);
	}

	#endregion

	#region DateTime Functions

	/// <summary>
	/// Converts a Unix Timestamp into a System.DateTime object.
	/// </summary>
	/// <returns> DateTime object representing a time equal to the provided Unix timestamp.</returns>
	/// <param name="value">The timestamp to convert.</param>
	public static DateTime ToDateTime(long value) {
		return new DateTime((TimeSpan.TicksPerSecond * value) + epoch.Ticks);
	}

	/// <summary>
	/// Converts an integer Unix Timestamp into a System.DateTime object.
	/// </summary>
	/// <returns>A DateTime object representing a time equal to the provided Unix timestamp.</returns>
	/// <param name="value">The timestamp to convert.</param>
	public static DateTime ToDateTime(int value) {
		return new DateTime((TimeSpan.TicksPerSecond * value) + epoch.Ticks);
	}

    public static string ToStringFormat(this DateTime dateTime) // IOS8601
    {
        return dateTime.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture);  //  ISO 8601 UTC format
    }
	/// <summary>
	/// Checks if the current time falls within two dates.
	/// </summary>
	/// <returns><c>true</c>, if falls within was dates, <c>false</c> otherwise.</returns>
	/// <param name="begin">Beginning date.</param>
	/// <param name="end">Ending date.</param>
	public static bool DateFallsWithin(DateTime beginDate, DateTime endDate) {
		long begin = beginDate.ToUnixTime();
		long end = endDate.ToUnixTime();
		long now = UnixTime();

		return (begin <= now && // current time is after begin time
		        (end >= now ||  // current time is before end time OR
		         begin > end));              // begin time is after end time, implying that there is no end time.
	}

	/// <summary>
	/// Gets a sortable string in ISO-8601 format.
	/// </summary>
	/// <param name="time">The time to get the string for.</param>
	/// <returns>A sortable string in ISO-8601 format.</returns>
	public static string GetSortableDateTimeString(DateTime time)
	{
		return time.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
	}



    public static string getDateFormat(this DateTime time)
    {
        return time.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
    }

    public static string getTimeFormat(this DateTime time)
    {
        return time.ToString("HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
    }

	public static string GetReadableTimeDuration(int duration)
	{
		float seconds = Mathf.FloorToInt(duration % 60);
		float mins = Mathf.FloorToInt(duration / 60 % 60);
		float hours = Mathf.FloorToInt(duration / 3600);
		return (hours > 0 ? hours + ":" : "").ToString() + (mins < 10 ? "0" + mins.ToString() : mins.ToString()) + ":" + (seconds < 10 ? "0" + seconds.ToString() : seconds.ToString());
	}
	
	#endregion
}

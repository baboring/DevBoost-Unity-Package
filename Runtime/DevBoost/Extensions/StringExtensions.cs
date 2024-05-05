/* ---------------------------------------------------------------------
 * Created on Mon Jan 11 2021 3:55:29 PM
 * Author : Benjamin Park
 * Description : String extensions
--------------------------------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace DevBoost.Extensions
{

    public static class StringExtensions
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (value == null)
                return null;

            return value.Substring(0, Math.Min(value.Length, maxLength));
        }
        public static bool StartsWithAny(this string s, IEnumerable<string> items)
        {
            return items.Any(i => s.StartsWith(i));
        }

        public static object Parse(this string s, Type type)
        {
            return Parse(s, type, out var error);
        }
        public static object Parse(this string s, Type type, out bool error)
        {
            error = false;

            if (type == typeof(string))
                return s;

            if (type == typeof(int))
                return ParseInt(s, out error);

            if (type == typeof(float))
                return ParseFloat(s, out error);

            if (type == typeof(bool))
                return ParseBool(s, out error);

            if (type.IsEnum)
            {
                object result;
                try
                {
                    result = Enum.Parse(type, s, true);
                }
                catch (ArgumentException)
                {
                    result = default(object);
                    error = false;
                }
                return result;
            }

            return default(object);
        }

        public static int ParseInt(this string s, out bool error)
        {
            error = !int.TryParse(s, out var result);

            if (error)
                Debug.LogWarning($"Error at parsing '{s}' to Integer");

            return error
                ? 0
                : result;
        }

        public static readonly string[] TrueOptions = new string[] { "true", "yes" };
        public static readonly string[] FalseOptions = new string[] { "false", "no" };
        public static bool ParseBool(this string s, out bool error)
        {
            s = s.ToLower();
            error = false;

            for (int i = 0; i < TrueOptions.Length; i++)
            {
                if (s == TrueOptions[i])
                    return true;
                if (s == FalseOptions[i])
                    return false;
            }

            error = true;
            return false;
        }

        public static float ParseFloat(this string s, out bool error)
        {
            error = !float.TryParse(
                s.Replace(',', '.'),
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out var result);
            return result;
        }
    }

}
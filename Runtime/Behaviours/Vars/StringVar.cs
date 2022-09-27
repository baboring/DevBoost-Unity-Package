/* *************************************************
*  Created:  2018-1-28 19:51:59
*  File:     StringVar.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/
using System;
using UnityEngine;

namespace DevBoost.ActionBehaviour
{
    // string var class
    public class StringVar : NullableVar<string> 
    {
        public string asString
        {
            get => Value;
            set => SetValue(value);
        }

        public int asInt
        {
            get { return Convert.ToInt32(asString, 0); }
            set { asString = value.ToString(); }
        }
        public long asLong
        {
            get { return Convert.ToLong(asString, 0); }
            set { asString = value.ToString(); }
        }
        public float asFloat
        {
            get { return Convert.ToFloat32(asString, 0); }
            set { asString = value.ToString(); }
        }

        public bool asBool
        {
            get {
                if (bool.TryParse(asString, out bool value))
                    return value;
                return Convert.ToInt32(asString, 0) > 0;
            }
            set { asString = value ? "1" : "0"; }
        }
    }


    public static class Convert
    {

        /// <summary>
        /// Parse string to bool
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool ToBool(string str, bool _default = false)
        {
            if (string.IsNullOrEmpty(str))
                return _default;

            str = str.Trim();
            if (bool.TryParse(str, out bool value))
                return value;
            else
                Debug.LogError("Wrong format of value for bool : " + str);
            return value;
        }

        /// <summary>
        /// Parse string to short
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static short ToShort(string str, short _default = 0)
        {
            if (string.IsNullOrEmpty(str))
                return _default;

            str = str.Trim();
            short value = _default;
            if (double.TryParse(str, out double testValue))
                value = (short)Math.Truncate(testValue);
            else if (!short.TryParse(str, out value))
                Debug.LogError("Wrong format of value for short : " + str);
            return value;
        }

        /// <summary>
        /// Parse String to Int
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt32(string str, int _default = 0)
        {
            if (string.IsNullOrEmpty(str))
                return _default;
            str = str.Trim();
            int value = _default;
            if (double.TryParse(str, out double testValue))
                value = (int)Math.Truncate(testValue);
            else if (!int.TryParse(str, out value))
                Debug.LogError("Wrong format of value for int : " + str);
            return value;
        }

        /// <summary>
        /// Parse String to Int
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int CastToInt32(double value)
        {
            return (int)Math.Truncate(value);
        }

        /// <summary>
        /// Parse String to Long
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long CastToInt64(double value)
        {
            return (long)Math.Truncate(value);
        }
        /// <summary>
        /// Parse String to Long
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long ToLong(string str, long _default = 0)
        {
            if (string.IsNullOrEmpty(str))
                return _default;
            str = str.Trim();
            long value = _default;
            if (double.TryParse(str, out double testValue))
                value = (long)Math.Truncate(testValue);
            else if (!long.TryParse(str, out value))
                Debug.LogError("Wrong format of value for long : " + str);
            return value;
        }

        /// <summary>
        /// Parse string to float
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float ToFloat32(string str, float _default = 0)
        {
            if (string.IsNullOrEmpty(str))
                return _default;

            str = str.Trim();
            if (float.TryParse(str, out float value))
                return value;
            else
                Debug.LogError("Wrong format of value for float : " + str);
            return value;
        }


        /// <summary>
        /// Parse string to double
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double ToDouble(string str, double _default = 0)
        {
            if (string.IsNullOrEmpty(str))
                return _default;
            str = str.Trim();
            if (double.TryParse(str, out double value))
                return value;
            else
                Debug.LogError("Wrong format of value for double : " + str);
            return value;
        }

        /// <summary>
        /// Parse string to Decimal
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal ToDecimal(string str, decimal _default = 0)
        {
            if (string.IsNullOrEmpty(str))
                return _default;
            str = str.Trim();
            if (decimal.TryParse(str, out decimal value))
                return value;
            else
                Debug.LogError("Wrong format of value for decimal : " + str);
            return value;
        }

        /// <summary>
        /// Convert two string parameters to Vector2
        /// </summary>
        /// <param name="szX"></param>
        /// <param name="szY"></param>
        /// <returns></returns>
        public static Vector2 ToVector2(string szX, string szY)
        {
            return new Vector2(ToFloat32(szX), ToFloat32(szY));
        }

        /// <summary>
        /// Formated currency string by number 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string CurrencyToString(uint value)
        {
            if (value >= 1000 && (value % 1000) == 0)
                return string.Format("{0}k", value / 1000f);
            else if (value >= 1000 && (value % 100) == 0)
                return string.Format("{0:#.#}k", (float)value / (float)1000f);
            else
                return string.Format("{0:0,0}", value);

        }

    }



}
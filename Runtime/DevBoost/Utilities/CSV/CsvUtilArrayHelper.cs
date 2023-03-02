/* *************************************************
*  Created:  2021-7-26 17:32:30
*  Author:   Benjamin
*  Purpose:  []
****************************************************/
using System.ComponentModel;
using System;
using System.Globalization;

namespace DevBoost.Utilities
{
    public static class CsvUtilArrayHelper
    {
        // adding example converting string array value from the string
        public static void AddCustumTypeDescriptorProvider()
        {
            TypeDescriptor.AddProvider(new CustumTypeDescriptorProvider(), typeof(string[]));
            TypeDescriptor.AddProvider(new CustumTypeDescriptorProvider(), typeof(int[]));
            TypeDescriptor.AddProvider(new CustumTypeDescriptorProvider(), typeof(float[]));
        }

        public class CustumTypeDescriptorProvider : TypeDescriptionProvider
        {
            public override ICustomTypeDescriptor GetTypeDescriptor(System.Type objectType, object instance)
            {
                if (objectType.Name == "String[]") return new StringArrayDescriptor();
                if (objectType.Name == "Int32[]") return new IntArrayDescriptor();
                if (objectType.Name == "Single[]") return new FloatArrayDescriptor();
                return base.GetTypeDescriptor(objectType, instance);
            }
        }
    }

    // -----------------------------------------------------------
    public class StringArrayDescriptor : CustomTypeDescriptor
    {
        public override TypeConverter GetConverter() { return new StringArrayConverter(); }
        public class StringArrayConverter : ArrayConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(string))
                    return true;
                return base.CanConvertFrom(context, sourceType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                string s = value as string;
                if (!string.IsNullOrEmpty(s))
                    return s.Split(new char[] { '\n', '|', ',' });

                return new string[0];
            }
        }
    }

    // -----------------------------------------------------------
    public class IntArrayDescriptor : CustomTypeDescriptor
    {
        public override TypeConverter GetConverter() { return new IntArrayConverter(); }
        public class IntArrayConverter : ArrayConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(int))
                    return true;
                return base.CanConvertFrom(context, sourceType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                string s = value as string;
                if (!string.IsNullOrEmpty(s))
                {
                    var items = s.Split(new char[] { '\n', '|',  ',' });
                    var result = new int[items.Length];
                    for (int i = 0; i < items.Length; ++i)
                        Int32.TryParse(items[i],out result[i]);
                    return result;
                }

                return new int[0];
            }
        }
    }
    
    // -----------------------------------------------------------
    public class FloatArrayDescriptor : CustomTypeDescriptor
    {
        public override TypeConverter GetConverter() { return new FloatArrayConverter(); }
        public class FloatArrayConverter : ArrayConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(float))
                    return true;
                return base.CanConvertFrom(context, sourceType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                string s = value as string;
                if (!string.IsNullOrEmpty(s))
                {
                    var items = s.Split(new char[] { '\n', '|', ',' });
                    var result = new float[items.Length];
                    for (int i = 0; i < items.Length; ++i)
                        Single.TryParse(items[i], NumberStyles.Any, CultureInfo.InvariantCulture, out result[i]);
                    return result;
                }
                return new float[0];
            }
        }
    }

    
}
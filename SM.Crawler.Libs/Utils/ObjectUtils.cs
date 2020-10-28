using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SM.Libs.Utils
{
    public static class ObjectUtils
    {
        public static void SetValue(object obj, string propertyName, object value)
        {
            SetValue(obj, propertyName, value, -1);
        }

        public static void SetValue(object obj, string propertyName, object value, int index)
        {
            if (obj == null)
            {
                throw new NullReferenceException($"Could not assign value for a null object");
            }

            var objectProperties = obj.GetType().GetProperties();
            var property = objectProperties.FirstOrDefault(x => x.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));

            if (property == null)
            {
                throw new Exception($"Property {propertyName} is not existed");
            }

            dynamic convertedValue = value;
            if (!property.PropertyType.IsClass && !(property.PropertyType == typeof(string)) && !(property.PropertyType.IsGenericType))
            {
                convertedValue = Convert.ChangeType(value, Type.GetTypeCode(property.PropertyType));
            }

            if (index >= 0)
            {
                property.SetValue(obj, convertedValue, new object[] { index });
            }
            else
            {
                property.SetValue(obj, convertedValue);
            }
        }

        public static void SetValue(object obj, string propertyName, object value, Func<object, object> convertFunc)
        {
            var v = convertFunc(obj);
            SetValue(obj, propertyName, v);
        }
        public static TType GetValue<TType>(object obj, string propertyName)
        {
            if (obj == null)
            {
                throw new NullReferenceException($"Could not get value of a null object");
            }

            var objectProperties = obj.GetType().GetProperties();
            var property = objectProperties.FirstOrDefault(x => x.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));

            if (property == null)
            {
                throw new Exception($"Property {propertyName} does not existed");
            }

            object value = property.GetValue(obj);
            try
            {
                TType convertedValue = (TType)Convert.ChangeType(value, typeof(TType));
                return convertedValue;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
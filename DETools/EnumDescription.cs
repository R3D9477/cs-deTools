using System;
using System.Linq;
using System.Globalization;
using System.ComponentModel;
using System.Collections.Generic;

namespace DETools
{
	public class EnumDescriptionTypeConverter<T> : TypeConverter where T : struct
	{
		private readonly Dictionary<T, string> enumValueToStringMap = new Dictionary<T,string>();
		private readonly Dictionary<string, T> stringToEnumValueMap = new Dictionary<string,T>(StringComparer.InvariantCultureIgnoreCase); 
		
		
		public EnumDescriptionTypeConverter()
		{
			if (!typeof(T).IsEnum)
				throw new InvalidOperationException("Type is not Enum");
	
			if (typeof(T).IsDefined(typeof(FlagsAttribute), false))
				throw new InvalidOperationException("Flags Attribute not supported");
	
			foreach (var enumValue in (T[]) Enum.GetValues(typeof(T)))
				stringToEnumValueMap[enumValue.ToString()] = enumValue;
	
			foreach (var mapping in GetEnumMappingsFromDescriptionAttribute())
			{
				enumValueToStringMap[mapping.Key] = mapping.Value;
				stringToEnumValueMap[mapping.Value] = mapping.Key;
			}
		}
		
		
		private static IEnumerable<KeyValuePair<T, string>> GetEnumMappingsFromDescriptionAttribute()
		{
			return (from value in (T[]) Enum.GetValues(typeof (T))
				let field = typeof (T).GetField(value.ToString())
				let attribute = (DescriptionAttribute[]) field.GetCustomAttributes(typeof (DescriptionAttribute), false)
				let description = attribute.Length == 1 ? attribute[0].Description : value.ToString()
				select new {value, description}).ToDictionary(t => t.value, t => t.description);
		}
		
		
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return
				sourceType == typeof(T)			||
				sourceType == typeof(String)	||
				sourceType == typeof(Int16)		||
				sourceType == typeof(Int32)		||
				sourceType == typeof(Int64);
		}
	
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return
				destinationType == typeof(T)		||
				destinationType == typeof(String)	||
				destinationType == typeof(Int16)	||
				destinationType == typeof(Int32)	||
				destinationType == typeof(Int64);
		}
		
		
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			Type valType = value.GetType();
			
			if (valType == typeof(T))
				return (T)value;
			if (valType == typeof(String))
				return GetValueFromString(value as string);
			if (valType == typeof(Int16) || valType == typeof(Int32) || valType == typeof(Int64))
				return (T)Enum.ToObject(typeof(T), Convert.ToInt32(value));
			
			return base.ConvertFrom(context, culture, value);
		}
	
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
				throw new ArgumentNullException("destinationType");
			
			if (value != null)
			{
				if (destinationType == typeof(T))
					return (T)value;
				if (destinationType == typeof(String))
					return GetDescription((T)value);
				if (destinationType == typeof(Int16))
					return Convert.ToInt16((T)value);
				if (destinationType == typeof(Int32))
					return Convert.ToInt32((T)value);
				if (destinationType == typeof(Int32))
					return Convert.ToInt64((T)value);
			}
			
			return base.ConvertTo(context, culture, value, destinationType);
		}
		
		
		public override bool IsValid(ITypeDescriptorContext context, object value)
		{
			if(value == null)
				throw new ArgumentNullException("value");
	
			Type valType = value.GetType();
			
			if (valType == typeof(T))
				return enumValueToStringMap.ContainsKey((T)value);
			if (valType == typeof(String))
				return stringToEnumValueMap.ContainsKey(value as string);
			if (valType == typeof(Int16) || valType == typeof(Int32) || valType == typeof(Int64))
				return typeof(T).IsEnumDefined(Convert.ToInt32(value));
			
			throw new InvalidOperationException("Unknown enum type.");
		}
		
		
		private T GetValueFromString (string value)
		{
			T convert;
			
			if(stringToEnumValueMap.TryGetValue(value, out convert))
			   return convert;
	
			throw new FormatException(String.Format("{0} is not a valid value for {1}.", value, typeof(T).Name));
		}
		
		private string GetDescription (T value)
		{
			string convert;
			
			if (enumValueToStringMap.TryGetValue(value, out convert))
				return convert;
	
			throw new ArgumentException(String.Format("The value '{0}' is not a valid value for the enum '{1}'.", value, typeof (T).Name));
		}
	}
}
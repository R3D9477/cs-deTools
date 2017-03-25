using System;
using System.ComponentModel;

namespace TestApp
{
	[TypeConverter(typeof(DETools.EnumDescriptionTypeConverter<TimeUnits>))]
	public enum TimeUnits
	{
		[Description("Seconds")] s,
		[Description("Minutes")] m,
		[Description("Hours")] h
	}
}

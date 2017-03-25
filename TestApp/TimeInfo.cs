using System;

namespace TestApp
{
	public class TimeInfo
	{
		public int Value { get; set; }
		public TimeUnits Units { get; set; }
		
		public TimeInfo ()
		{
			this.Value = 0;
			this.Units = TimeUnits.s;
		}
		
		public TimeInfo (int Value, TimeUnits Units)
		{
			this.Value = Value;
			this.Units = Units;
		}
	}
}

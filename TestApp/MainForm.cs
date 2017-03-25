using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace TestApp
{
	public partial class MainForm : Form
	{
		List<TimeInfo> timeList = new List<TimeInfo>();
		
		public void UpdateTotalTime ()
		{
			int totalSeconds = 0, weight;
			
			foreach (TimeInfo t in timeList)
			{
				switch (t.Units)
				{
					case TimeUnits.h:
						weight = 3600;
						break;		
					case TimeUnits.m:
						weight = 60;
						break;
					default:
						weight = 1;
						break;
				}
				
				totalSeconds += t.Value * weight;
			}
			
			int hours = totalSeconds / 3600;
			int minutes = (totalSeconds - hours * 3600) / 60;
			int seconds = totalSeconds - hours * 3600 - minutes  * 60;
			
			TotalTimeLabel.Text = String.Format("{0} hours {1} minutes {2} seconds", hours, minutes, seconds);
		}
		
		public MainForm()
		{
			InitializeComponent();
			
			timeList.Add(new TimeInfo(10, TimeUnits.h));
			timeList.Add(new TimeInfo(20, TimeUnits.m));
			timeList.Add(new TimeInfo(30, TimeUnits.s));
			
			bindingSource1.DataSource = null;
			bindingSource1.DataSource = timeList;
			bindingSource1.AllowNew = true;
			
			dataGridView1.DataSource = null;
			dataGridView1.AutoGenerateColumns = true;
			dataGridView1.DataSource = bindingSource1;
			
			DETools.DataGridViewHelper.ReplaceWithDropDownColumn(dataGridView1, "Units", "Units", typeof(TimeUnits));
		}
		
		void DataGridView1CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			UpdateTotalTime();
		}
		
		void DataGridView1RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
		{
			UpdateTotalTime();
		}
	}
}

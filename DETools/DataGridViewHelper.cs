using System;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;

namespace DETools
{
	public static class DataGridViewHelper
	{
		public static void ReplaceWithDropDownColumn (DataGridView dgv, string srcColumnaName, string dropDownColumnName, Type dropDownType = null)
		{
			dgv.Columns[srcColumnaName].Visible = false;
			
			if (dropDownType == null)
				dropDownType = dgv.Columns[srcColumnaName].ValueType;
			
			TypeConverter tc = TypeDescriptor.GetConverter(dropDownType);
			
			var combo = new DataGridViewComboBoxColumn();
			
			combo.ValueType = dropDownType;
			combo.Name = dropDownColumnName;
			combo.DataPropertyName = srcColumnaName;
			combo.DisplayMember = "Title";
			combo.ValueMember = "Value";
			combo.DataSource = Enum.GetValues(dropDownType).Cast<dynamic>().ToArray().Select(x => new { Title = tc.ConvertTo(x, typeof(String)), Value = tc.ConvertTo(x, dropDownType) }).ToList();
			
			if (dgv.Columns.Contains(dropDownColumnName))
				dgv.Columns.Remove(dgv.Columns[dropDownColumnName]);
			
			dgv.Columns.Add(combo);
		}
	}
}
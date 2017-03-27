using System;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;

namespace Sys.db
{
	public static class DbViewHelper
	{
		public static void ReplaceWithDropDownColumn (DataGridView dgv, string srcColumnName, string dropDownColumnName, Type dropDownType = null)
		{
			dgv.Columns[srcColumnName].Visible = false;
			
			if (dropDownType == null)
				dropDownType = dgv.Columns[srcColumnName].ValueType;
			
			TypeConverter tc = TypeDescriptor.GetConverter(dropDownType);
			
			var combo = new DataGridViewComboBoxColumn();
			
			combo.ValueType = dropDownType;
			combo.Name = dropDownColumnName;
			combo.DataPropertyName = srcColumnaName;
			combo.DisplayMember = "Title";
			combo.ValueMember = "Value";
			combo.DataSource = Enum.GetValues(dropDownType).Cast<dynamic>().ToArray().Select(x => new { Title = tc.ConvertTo(x, typeof(String)), Value = tc.ConvertTo(x, dgv.Columns[srcColumnName].ValueType) }).ToList();
			
			if (dgv.Columns.Contains(dropDownColumnName))
				dgv.Columns.Remove(dgv.Columns[dropDownColumnName]);
			
			dgv.Columns.Add(combo);
		}
	}
}

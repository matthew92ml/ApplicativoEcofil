using System;
using System.Linq;
using System.Windows.Forms;
using ApplicativoEcofil.Class;
using ApplicativoEcofil.Properties;

namespace ApplicativoEcofil
	{
	public partial class Option : Form
	{
		private readonly double[] ps;
		private readonly int index=0;
		private static readonly string[] set = new string[9] { "updateData", "timeSeconds", "AlarmClock", "Active", "Min", "Max", "ActivePS", "updateData2", "timeMinutes" };
		public Option(Garbage garb, string[] garbage)
		{
			InitializeComponent();
			ps = Function.PsReader();
			Icon = Resources.favicon;
			rangeSlider.ShowLabels = true;
			rangeSlider.Enabled= false;
			rangeSlider.TickFrequency = 10;
			rangeSlider.Maximum = 0;
			rangeSlider.Maximum = 200;
			checkBoxUpdate.Checked = (bool)Settings.Default[set[0]];
			checkBoxUpdate2.Checked = (bool)Settings.Default[set[7]];
			comboBoxUpdate.Items.Add(0);
			foreach (int item2 in Enumerable.Range(1, 20).Select(m => m * 200))
			{
				comboBoxUpdate.Items.Add(item2);
			}
            foreach ( int item3 in Enumerable.Range(1,20).Select(x => x * 5))
			{
				comboBoxUpdate2.Items.Add(item3);
			}
			string[] type = garb.type;
			foreach (string item in type)
			{
				comboBoxGarbage.Items.Add(item);
			}
			foreach (string item4 in Settings.Default[set[2]].ToString().Split(';').Where( x=> x != ""))
			{
				checkedListDateTime.Items.Add(item4, isChecked: true);
			}
			comboBoxUpdate.SelectedIndex = comboBoxUpdate.Items.IndexOf((int)Settings.Default[set[1]]);
			comboBoxUpdate2.SelectedIndex = comboBoxUpdate2.Items.IndexOf((int)Settings.Default[set[8]]);
			dateTimePicker.Format = DateTimePickerFormat.Custom;
			dateTimePicker.CustomFormat = "dddHH:mm:ss";
			checkActive.Checked = (bool)Settings.Default[set[3]];
			checkBoxPs.Checked = (bool)Settings.Default[set[6]];
			rangeSlider.SliderMax = ((int)Settings.Default[set[5]] < 200) ? ((int)Settings.Default[set[5]]) : 200;
			rangeSlider.SliderMin = ((int)Settings.Default[set[4]] > 0) ? ((int)Settings.Default[set[4]]) : 0;
			labelMin.Text = rangeSlider.SliderMin.ToString();
			labelMax.Text = rangeSlider.SliderMax.ToString();
			flowPS.Enabled = !checkBoxPs.Checked;
			foreach (double item in ps)
			{
				flowPS.Controls.Add(new TextBox()
				{
					Width = 40,
					Height = 20,
					Text = item.ToString("0.000"),
					Name = garbage[10 + index]
				});
				flowPS.Controls.Add(new Label()
				{
					Width = 130,
					Height = 20,
					Text = " Kg/l " + garbage[10 + index].ToUpper(),
					Name = garbage[10 + index]
				});
				index++;
			}
		}
		private void ButtonExit_Click(object sender, EventArgs e)
		{
			Close();
		}
		private void BtAdd_Click(object sender, EventArgs e)
		{
			checkedListDateTime.Items.Add(dateTimePicker.Text + "-" + comboBoxGarbage.SelectedItem, isChecked: false);
		}
		private void CheckActive_CheckedChanged(object sender, EventArgs e)
		{
			rangeSlider.Enabled = (sender as CheckBox).Checked;
		}
		private void RangeSlider_MouseMove(object sender, MouseEventArgs e)
		{
			labelMin.Text = rangeSlider.SliderMin.ToString();
			labelMax.Text = rangeSlider.SliderMax.ToString();
		}
		private void CheckedListDateTime_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (checkedListDateTime.SelectedItem != null)
			{
				checkedListDateTime.Items.Remove(checkedListDateTime.SelectedItem);
			}
		}
		private void ButtonSave_Click(object sender, EventArgs e)
		{
			Settings.Default[set[2]] = string.Empty;
			foreach (string checkedItem in checkedListDateTime.CheckedItems)
			{
				Settings @default = Settings.Default;
				string propertyName = set[2];
				@default[propertyName] = @default[propertyName]?.ToString() + checkedItem + ";";
			}
			Settings.Default[set[0]] = checkBoxUpdate.Checked;
			Settings.Default[set[1]] = comboBoxUpdate.SelectedItem;
			Settings.Default[set[7]] = checkBoxUpdate2.Checked;
			Settings.Default[set[8]] = comboBoxUpdate2.SelectedItem;
			Settings.Default[set[3]] = checkActive.Checked;
			Settings.Default[set[4]] = checkActive.Checked ? rangeSlider.SliderMin : 80;
			Settings.Default[set[5]] = checkActive.Checked ? rangeSlider.SliderMax : 120; 
			Settings.Default[set[6]] = checkBoxPs.Checked;
            if (!checkBoxPs.Checked)
			{
				Function.PsWriter(flowPS);
			}
			Settings.Default.Save();
			Settings.Default.Reload();
			Close();
		}
        private void CheckBoxPs_CheckedChanged(object sender, EventArgs e)
        {
			flowPS.Enabled= !checkBoxPs.Checked;
        }
    }
}
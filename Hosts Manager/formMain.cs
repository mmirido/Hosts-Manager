using Mirido.Helper;
using System;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Hosts_Manager
{
	public partial class formMain : Form
	{
		public formMain()
		{
			InitializeComponent();
			InitDataGridView();
		}

		private void formMain_FormClosing(object sender, FormClosingEventArgs e) => Properties.Settings.Default.Save();

		private void exitToolStripMenuItem_Click(object sender, EventArgs e) => Application.Exit();
		private void aboutToolStripMenuItem_Click(object sender, EventArgs e) => Controller.ShowAbout();

		private void addButton_Click(object sender, EventArgs e) // Unfinished
		{
			DialogResult rs = Controller.AddURL(out string newURL); // Add more textbox for redirected IP
			if (rs == DialogResult.OK)
			{
				string redirectedIP = Properties.Settings.Default.DefaultIP;
				AddToDataGridView(newURL, redirectedIP);
			}
		}

		private void addBulkButton_Click(object sender, EventArgs e) // Unfinished
		{
			DialogResult rs = Controller.AddURL(out string newURL); // Add more textbox for redirected IP
			if (rs == DialogResult.OK)
			{
				string redirectedIP = Properties.Settings.Default.DefaultIP;
				AddToDataGridView(newURL, redirectedIP);
			}
		}

		private void editToolStripButton_Click(object sender, EventArgs e)
		{

		}

		private void removeToolStripButton_Click(object sender, EventArgs e)
		{
			int selectedRowCount = dataGridView.Rows.GetRowCount(DataGridViewElementStates.Selected);
			if (selectedRowCount > 0)
			{
				if (Properties.Settings.Default.RemoveConfirmation)
				{
					DialogResult rs = Messages.ShowConfirmation("Are you sure want to delete the selected URL(s)?");
					if (rs != DialogResult.Yes)
						return;
				}

				RemoveFromDataGridView(selectedRowCount);
			}
		}

		private void buttonProcess_Click(object sender, EventArgs e) => dataGridView.DataSource = Controller.SaveBlockList(dataGridView.DataSource as DataTable);

		private void AddToDataGridView(string newURL, string redirectedIP)
		{
			DataTable dt = dataGridView.DataSource as DataTable;
			dt.Rows.Add(newURL, redirectedIP);
			dt.AcceptChanges();
		}

		private void RemoveFromDataGridView(int rowCount)
		{
			DataTable dt = dataGridView.DataSource as DataTable;
			for (int i = 0; i < rowCount; i++)
			{
				dt.Rows.RemoveAt(dataGridView.SelectedRows[i].Index);
			}
		}

		private void InitDataGridView()
		{
			dataGridView.DataSource = null;
			dataGridView.DataSource = Controller.LoadBlockList();
			dataGridView.Columns.Add(DataGridViewHelper.BlankDataGridViewColumn());
		}
	}
}
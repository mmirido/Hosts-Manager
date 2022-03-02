using Hosts_Manager.Controllers;
using Mirido.Helper;
using System;
using System.Data;
using System.Windows.Forms;

namespace Hosts_Manager
{
	public partial class formMain : Form
	{
		private DataSet dataSet;

		public formMain() => InitializeComponent();

		#region Events
		#region Form Events
		private void formMain_Load(object sender, EventArgs e)
		{
			InitController.InitSettings();
			dataSet = InitController.InitData();

			dgvLists.AutoGenerateColumns = false;
			dgvContent.AutoGenerateColumns = false;

			InitController.InitDgvLists(dgvLists);
			InitController.InitDgvContent(dgvContent);

			dgvLists.DataSource = dataSet.Tables["List"];
			dataSet.AcceptChanges();

			dgvLists.SelectionChanged += dgvLists_SelectionChanged;
			dgvLists.RowsAdded += DgvLists_RowsAdded;
			dgvLists.RowsRemoved += DgvLists_RowsRemoved;

			dgvContent.RowsAdded += DgvContent_RowsAdded;
			dgvContent.RowsRemoved += DgvContent_RowsRemoved;

			ListRowCountChanged(dgvLists);
			ContentRowCountChanged(dgvContent);

			dgvLists.ClearSelection();
		}

		private void formMain_FormClosing(object sender, FormClosingEventArgs e) => Properties.Settings.Default.Save();
		#endregion

		#region Menu Events
		private void exitToolStripMenuItem_Click(object sender, EventArgs e) => Application.Exit();
		private void aboutToolStripMenuItem_Click(object sender, EventArgs e) => UIController.ShowAbout();
		private void newToolStripMenuItem_Click(object sender, EventArgs e) => NewList();
		private void newToolStripButton_Click(object sender, EventArgs e) => NewList();
		private void saveToolStripMenuItem_Click(object sender, EventArgs e) => SaveChanges();
		private void saveToolStripButton_Click(object sender, EventArgs e) => SaveChanges();
		private void discardToolStripMenuItem_Click(object sender, EventArgs e) => DiscardChanges();

		private void NewList()
		{
			string newListName = UIController.AddNewList();
			if (dataSet.Tables.Contains(newListName))
			{
				if (MsgBox.ShowConfirmation($"There is already a list named \"{newListName}\".{Environment.NewLine}" +
					$"Do you want to try again?") == DialogResult.Yes)
					NewList();
			}
			else
			{
				DataTable dt = dataSet.Tables["List"];
				dt.Rows.Add(newListName, true);
			}
		}

		//private void addBulkButton_Click(object sender, EventArgs e)
		//{
		//	DataTable dt = dgv.DataSource as DataTable;
		//	dt = Controller.AddURLs(dt);
		//	dt.AcceptChanges();
		//}
		private void SaveChanges()
		{
			dgvLists.EndEdit();
			dgvContent.EndEdit();

			if (dataSet.HasChanges())
				dataSet.AcceptChanges();

			UIController.SaveData(dataSet);
			UIController.GenerateHostsFile(dataSet);
		}
		private void DiscardChanges()
		{
			dgvLists.EndEdit();
			dgvContent.EndEdit();

			if (dataSet.HasChanges())
				dataSet.RejectChanges();
		}
		#endregion

		#region Data Grid View Events
		#region Lists Events
		private void dgvLists_SelectionChanged(object sender, EventArgs e)
		{
			if (sender is DataGridView dgv && dgv.SelectedRows.Count > 0)
			{
				if (!dgvContent.Enabled)
					dgvContent.Enabled = true;

				dgvContent.EndEdit();

				string tableName = dgv.SelectedRows[0].Cells["name"].Value.ToString();
				if (!dataSet.Tables.Contains(tableName))
				{
					using (DataTable dt = UIController.LoadList(tableName))
					{
						dataSet.Tables.Add(dt);
					}
				}
				dgvContent.DataSource = dataSet.Tables[tableName];
			}
			else
			{
				if (dgvContent.Enabled)
					dgvContent.Enabled = false;
			}
		}

		private void DgvLists_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e) => ListRowCountChanged(sender as DataGridView);
		private void DgvLists_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e) => ListRowCountChanged(sender as DataGridView);

		private void ListRowCountChanged(DataGridView dgv) => lbLists.Text = $"List Count: {dgv.Rows.Count}";
		#endregion

		#region Content Events
		private void DgvContent_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e) => ContentRowCountChanged(sender as DataGridView);
		private void DgvContent_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e) => ContentRowCountChanged(sender as DataGridView);

		private void ContentRowCountChanged(DataGridView dgv) => lbContent.Text = $"Entry Count: {dgv.Rows.Count - 1}";
		#endregion

		#endregion

		#endregion

		//private void editToolStripButton_Click(object sender, EventArgs e)
		//{

		//}

		//private void removeToolStripButton_Click(object sender, EventArgs e)
		//{
		//	/*int selectedRowCount = dataGridView.Rows.GetRowCount(DataGridViewElementStates.Selected);
		//	if (selectedRowCount > 0)
		//	{
		//		if (Properties.Settings.Default.RemoveConfirmation)
		//		{
		//			DialogResult rs = Messages.ShowConfirmation("Are you sure want to delete the selected URL(s)?");
		//			if (rs != DialogResult.Yes)
		//				return;
		//		}

		//		RemoveFromDataGridView(selectedRowCount);
		//	}*/
		//}

		//private void RemoveFromDataGridView(int rowCount)
		//{
		//	DataTable dt = dgv.DataSource as DataTable;
		//	for (int i = rowCount - 1; i >= 0; i--)
		//	{
		//		dt.Rows.RemoveAt(dgv.SelectedRows[i].Index);
		//	}
		//}

		//private void newToolStripMenuItem_Click(object sender, EventArgs e) => AddNewList();
		//private void newToolStripButton_Click(object sender, EventArgs e) => AddNewList();
		//private void newCMSItem_Click(object sender, EventArgs e) => AddNewList();

		////private void DeleteCMSItem_Click(object sender, EventArgs e) => DeleteList();

		//private void saveToolStripButton_Click(object sender, EventArgs e) => SaveChange();
		//private void saveToolStripMenuItem_Click(object sender, EventArgs e) => SaveChange();

		//private void AddNewList()
		//{
		//	/*DataTable dt = dgvLists.DataSource as DataTable;
		//	Controller.AddNewList(dt);
		//	dt.AcceptChanges();*/
		//}

		///*private void DeleteList()
		//{
		//	DataTable dt = dgvLists.DataSource as DataTable;

		//	dt.Rows[dgvLists.SelectedRows[0].Index].Delete();
		//	dt.AcceptChanges();
		//}*/

		//private void SaveChange()
		//{

		//}

		//private void dgvLists_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
		//{
		//	/*DataTable dt = dgvLists.DataSource as DataTable;
		//	Controller.DeleteList(e.Row.Cells["path"].Value.ToString());
		//	dt.AcceptChanges();*/
		//}

		//private void dgvLists_SelectionChanged(object sender, EventArgs e)
		//{
		//	if (sender is DataGridView dgv && dgv.SelectedRows.Count > 0)
		//	{
		//		DataGridViewRow row = dgv.SelectedRows[0];
		//		if (row != null)
		//		{
		//			string filePath = row.Cells["path"].Value.ToString();

		//			DataTable dt = dgv.DataSource as DataTable;
		//			//dgv.DataSource = null;
		//			dt = Controller.LoadList(dt, filePath);
		//			//dgv.DataSource = dt;
		//		}
		//	}
		//}

		//private void dgvLists_KeyDown(object sender, KeyEventArgs e)
		//{
		//	/*if (e.KeyCode == Keys.F2)
		//	{
		//		e.Handled = true;
		//		dgvLists.BeginEdit(true);
		//	}*/
		//}

		//private void newToolStripMenuItem_Click_1(object sender, EventArgs e)
		//{

		//}

		///*
		//protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		//{
		//	if (keyData == Keys.F2)
		//	{
		//		editToolStripButton_Click(editToolStripButton, null);
		//		return true;
		//	}
		//	if (keyData == Keys.Delete)
		//	{
		//		removeToolStripButton_Click(removeToolStripButton, null);
		//		return true;
		//	}
		//	return base.ProcessCmdKey(ref msg, keyData);
		//}
		//*/

	}
}
using Hosts_Manager.Properties;
using Mirido.Helper;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Hosts_Manager.Controllers
{
	internal class InitController
	{
		internal static void InitSettings()
		{
			Settings.Default.exeFile = new Uri(Assembly.GetEntryAssembly().CodeBase).LocalPath;
			Settings.Default.exeDir = Path.GetDirectoryName(Settings.Default.exeFile);
			Settings.Default.dataDir = Path.Combine(Settings.Default.exeDir, "data");
			Settings.Default.dataFile = Path.Combine(Settings.Default.dataDir, "Data.xml");

			//if (!Directory.Exists(dataDir))
			//	CreateDataDirectory();
		}

		internal static DataSet InitData()
		{
			DataSet ds = new DataSet("Data");
			if (File.Exists(Settings.Default.dataFile))
				ds.ReadXml(Settings.Default.dataFile);
			else
				ds.Tables.Add(InitDtLists());
			return ds;
		}

		internal static void InitDgvLists(DataGridView dgv)
		{
			DataGridViewColumn col = new DataGridViewTextBoxColumn()
			{
				AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
				Name = "name",
				DataPropertyName = "name",
				HeaderText = "Name",
				Resizable = DataGridViewTriState.False,
				SortMode = DataGridViewColumnSortMode.Automatic,
				ReadOnly = true
			};
			dgv.Columns.Add(col);

			col = new DataGridViewCheckBoxColumn()
			{
				AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
				Name = "enabled",
				DataPropertyName = "enabled",
				HeaderText = "Enabled",
				Resizable = DataGridViewTriState.False,
				MinimumWidth = 78
			};
			dgv.Columns.Add(col);
		}
		internal static void InitDgvContent(DataGridView dgv)
		{
			DataGridViewColumn col = new DataGridViewCheckBoxColumn()
			{
				AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
				Name = "enabled",
				DataPropertyName = "enabled",
				HeaderText = "Enabled",
				Resizable = DataGridViewTriState.False,
				//Width = 78,
				MinimumWidth = 78
			};
			dgv.Columns.Add(col);

			col = new DataGridViewTextBoxColumn()
			{
				Name = "ip",
				DataPropertyName = "ip",
				HeaderText = "IP Address",
				MinimumWidth = 123
				//Width = 123
			};
			dgv.Columns.Add(col);

			col = new DataGridViewTextBoxColumn()
			{
				Name = "host",
				DataPropertyName = "host",
				HeaderText = "Host Name",
				MinimumWidth = 125
				//Width = 125
			};
			dgv.Columns.Add(col);

			col = new DataGridViewTextBoxColumn()
			{
				AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
				Name = "comment",
				DataPropertyName = "comment",
				HeaderText = "Comment"
				//MinimumWidth = 8
			};
			dgv.Columns.Add(col);

			//if (File.Exists(selectedListFile))
			//	dgv.DataSource = LoadList(dgv.DataSource as DataTable, selectedListFile);
			//else
			//	CreateNewFile(dgv.DataSource as DataTable, selectedListFile);
		}

		internal static DataTable InitDtLists()
		{
			DataTable dt = new DataTable("List");

			DataColumn col = new DataColumn
			{
				ColumnName = "name",
				DataType = typeof(string),
				ColumnMapping = MappingType.Attribute
			};
			dt.Columns.Add(col);

			col = new DataColumn()
			{
				ColumnName = "enabled",
				DataType = typeof(bool),
				ColumnMapping = MappingType.Attribute
			};
			dt.Columns.Add(col);

			dt.Rows.Add("Default List", true);

			return dt;
		}
		internal static DataTable InitDtContent(string tableName)
		{
			DataTable dt = new DataTable(tableName);

			DataColumn column = new DataColumn
			{
				ColumnName = "enabled",
				DataType = typeof(bool),
				ColumnMapping = MappingType.Attribute
			};
			dt.Columns.Add(column);

			column = new DataColumn
			{
				ColumnName = "ip",
				DataType = typeof(string),
				DefaultValue = Settings.Default.defaultIP,
				ColumnMapping = MappingType.Element
			};
			dt.Columns.Add(column);

			column = new DataColumn
			{
				ColumnName = "host",
				DataType = typeof(string),
				ColumnMapping = MappingType.Element
			};
			dt.Columns.Add(column);

			column = new DataColumn
			{
				ColumnName = "comment",
				DataType = typeof(string),
				ColumnMapping = MappingType.Element
			};
			dt.Columns.Add(column);

			return dt;
		}
		/*



		internal static void DeleteList(string filePath)
		{
			if (File.Exists(filePath))
				File.Delete(filePath);
		}

		internal static DataTable AddURLs(DataTable dt)
		{
			DialogResult rs = InputBox.Show("Enter new URLs to list", new (string, bool)[] { ("URL", true), ("IP", false) }, out string[] value);
			if (rs == DialogResult.OK)
			{
				string[] URLs = value[0].Split('\n');
				for (int i = 0; i < URLs.Length; i++)
				{
					if (URLs[i] != string.Empty)
						dt.Rows.Add(URLs[i], value[1]);
				}
				dt.AcceptChanges();
			}
			return dt;
		}
*/

	}
}
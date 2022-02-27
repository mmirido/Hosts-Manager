using Mirido.Helper;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Hosts_Manager
{
	internal class Controller
	{
		internal static DialogResult AddURL(out string result)
		{
			string input = string.Empty;
			DialogResult rs = Messages.ShowInputBox(ref input, "Enter URL", "Enter new URL to list");
			result = rs == DialogResult.OK ? input : string.Empty;
			return rs;
		}

		internal static DialogResult AddURLs(out string result)
		{
			result = string.Empty;
			return DialogResult.None;
		}

		internal static DataTable LoadBlockList()
		{
			try
			{
				DataTable dt = XmlHelper.LoadToDataTable(Properties.Settings.Default.DataPath, Properties.Settings.Default.ListFileName);
				dt = DataTableHelper.Sort(dt,false, ("host", true));

				return dt;
			}
			catch (DirectoryNotFoundException)
			{
				CreateDataDirectory();
				return LoadBlockList();
			}
			catch (FileNotFoundException)
			{
				CreateNewList();
				return LoadBlockList();
			}
			catch (Exception e)
			{
				Messages.ShowError($"Cannot load data, please try again!{Environment.NewLine}{e.Message}");
				return null;
			}
		}

		private static void CreateNewList()
		{
			try
			{
				DataTable table = CreateTable("Domain");
				XmlHelper.SaveFromDataTable(Properties.Settings.Default.DataPath, Properties.Settings.Default.ListFileName, table);
			}
			catch (DirectoryNotFoundException)
			{
				CreateDataDirectory();
				CreateNewList();
			}
			catch (Exception e)
			{
				Messages.ShowError($"{e.Message}");
			}
		}

		private static void CreateDataDirectory() => Directory.CreateDirectory(Properties.Settings.Default.DataPath);

		private static DataTable CreateTable(string tableName)
		{
			DataTable table = new DataTable(tableName);

			DataColumn column = new DataColumn
			{
				Unique = true,
				DataType = typeof(string),
				AllowDBNull = false,
				Caption = "host",
				ColumnName = "host",
				ColumnMapping = MappingType.Attribute
			};
			table.Columns.Add(column);

			column = new DataColumn
			{
				DataType = typeof(string),
				AllowDBNull = true,
				Caption = "ip",
				ColumnName = "ip",
				DefaultValue = Properties.Settings.Default.DefaultIP,
				ColumnMapping = MappingType.Attribute
			};
			table.Columns.Add(column);

			table.AcceptChanges();
			return table;
		}

		internal static DataTable SaveBlockList(DataTable table)
		{
			try
			{
				table = DataTableHelper.Sort(table,true, ("host", true));

				XmlHelper.SaveFromDataTable(Properties.Settings.Default.DataPath, Properties.Settings.Default.ListFileName, table);
				GenerateHostsFile(table);
				return table;
			}
			catch (DirectoryNotFoundException)
			{
				CreateDataDirectory();
				return SaveBlockList(table);
			}
			catch (Exception e)
			{
				Messages.ShowError($"{e.Message}");
				return table;
			}
		}

		private static void GenerateHostsFile(DataTable table)
		{
			string content = DataHandler.GenerateHostsHeader();
			string extContent = string.Empty;

			foreach (DataRow row in table.Rows)
			{
				content += $"{row["ip"]} {row["host"]}{Environment.NewLine}";
				extContent += $"{row["ip"]} www.{row["host"]}{Environment.NewLine}";
			}
			content += extContent;

			if (File.Exists(Properties.Settings.Default.DefaultPath + Properties.Settings.Default.Hosts))
				File.Copy(Properties.Settings.Default.DefaultPath + Properties.Settings.Default.Hosts,
					   Properties.Settings.Default.DefaultPath + Properties.Settings.Default.HostsBackup, true);
			File.WriteAllText(Properties.Settings.Default.DefaultPath + Properties.Settings.Default.Hosts, content);
		}

		internal static void ShowAbout()
		{
			string company = ((AssemblyCompanyAttribute) Attribute.GetCustomAttribute(
				Assembly.GetExecutingAssembly(), typeof(AssemblyCompanyAttribute), false))
			   .Company;

			string copyright = ((AssemblyCopyrightAttribute) Attribute.GetCustomAttribute(
				Assembly.GetExecutingAssembly(), typeof(AssemblyCopyrightAttribute), false))
			   .Copyright;

			string text =
				$"{Assembly.GetExecutingAssembly().GetName().Name}{Environment.NewLine}" +
				$"Version {Assembly.GetExecutingAssembly().GetName().Version}{Environment.NewLine}" +
				$"{copyright} {company}.{Environment.NewLine}" +
				$"All rights reserved.";
			string caption = $"About {Assembly.GetExecutingAssembly().GetName().Name}";
			MessageBoxButtons button = MessageBoxButtons.OK;
			MessageBoxIcon icon = MessageBoxIcon.Information;

			MessageBox.Show(text, caption, button, icon);
		}

		/*
        private void ExportDgvToXML()
        {
            DataTable dt = (DataTable) dataGridView1.DataSource;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "XML|*.xml";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    dt.WriteXml(sfd.FileName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
        */
	}
}
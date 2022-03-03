using Hosts_Manager.Properties;
using Mirido.Helper;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Hosts_Manager.Controllers
{
	internal class UIController
	{
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

		internal static DataTable LoadList(string tableName)
		{
			try
			{
				using (DataSet ds = new DataSet())
				{
					ds.ReadXml(Settings.Default.dataFile);
					if (ds.Tables.Contains(tableName))
					{
						using (DataTable dt = ds.Tables[tableName])
						{
							return dt;
						}
					}
					else
						throw new FileNotFoundException();
				}
			}
			catch (DirectoryNotFoundException)
			{
				CreateDataDirectory();
				return LoadList(tableName);
			}
			catch (FileNotFoundException)
			{
				using (DataTable dt = InitController.InitDtContent(tableName))
				{
					return dt;
				}
			}
			catch (Exception e)
			{
				MsgBox.ShowError($"Cannot load data, please try again!{Environment.NewLine}{e.Message}");
				return null;
			}
		}

		private static void CreateDataDirectory() => Directory.CreateDirectory(Settings.Default.dataDir);

		internal static void SaveData(DataSet dataSet)
		{
			try
			{
				dataSet.WriteXml(Settings.Default.dataFile, XmlWriteMode.WriteSchema);
			}
			catch (DirectoryNotFoundException)
			{
				CreateDataDirectory();
				SaveData(dataSet);
			}
			catch (Exception e)
			{
				MsgBox.ShowError(e.Message);
			}
		}

		internal static void GenerateHostsFile(DataSet dataSet)
		{
			string content = DataHandler.GenerateHostsHeader();
			string extContent = string.Empty;

			DataTable dtList = dataSet.Tables["List"];
			if (dtList.Rows.Count > 0)
				foreach (DataRow list in dtList.Rows)
				{
					if (list["enabled"].Equals(true))
					{
						DataTable dt = dataSet.Tables[list["name"].ToString()];
						if (dt.Rows.Count > 0)
							foreach (DataRow row in dt.Rows)
							{
								if (row["enabled"].Equals(true))
								{
									content += $"{row["ip"]} {row["host"]}" +
										(row["comment"].ToString() != string.Empty ? $" # {row["comment"]}" : "") + $"{Environment.NewLine}";
									extContent += $"{row["ip"]} www.{row["host"]}" +
										(row["comment"].ToString() != string.Empty ? $" # {row["comment"]}" : "") + $"{Environment.NewLine}";
								}
							}
					}
				}

			content += extContent;

			if (File.Exists(Settings.Default.hostsDir + Settings.Default.hostsFile))
				File.Copy(Settings.Default.hostsDir + Settings.Default.hostsFile,
					   Settings.Default.hostsDir + Settings.Default.hostsBakFile, true);
			File.WriteAllText(Settings.Default.hostsDir + Settings.Default.hostsFile, content);
		}

		internal static DialogResult EditList(string caption, out string newName)
		{
			newName = string.Empty;
			DialogResult rs = InputBox.Show(caption, new (string, bool)[] { ("Enter List Name", false) }, out string[] value);
			if (rs == DialogResult.OK)
			{
				if (value.Length > 0)
					newName = value[0];
				return rs;
			}
			return rs;
		}

		internal static void ExportToXML(DataSet dataSet)
		{
			SaveFileDialog sfd = new SaveFileDialog
			{
				Filter = "XML|*.xml"
			};
			if (sfd.ShowDialog() == DialogResult.OK)
			{
				try
				{
					dataSet.WriteXml(sfd.FileName, XmlWriteMode.WriteSchema);
				}
				catch (Exception e)
				{
					MsgBox.ShowError(e.Message);
				}
			}
		}
	}
}

using System;
using System.IO;
using System.Linq;

namespace Hosts_Manager
{
	internal class DataHandler
	{
		private const string defaultHosts = @"data\DefaultHosts";

		/// <summary>
		/// Return the header of hosts file. Generate new one if default header does not exist.
		/// </summary>
		/// <param name="GenerateDefaultHosts">@true  will generate a default hosts file if one does not exist.</param>
		/// <returns>Content of the header of hosts file.</returns>
		public static string GenerateHostsHeader(bool GenerateDefaultHosts = true)
		{
			string header;
			if (File.Exists(defaultHosts))
				header = File.ReadAllText(defaultHosts);
			else
			{
				header =
					$"# Copyright (c) 1993-2006 Microsoft Corp.{Environment.NewLine}" +
					$"#{Environment.NewLine}" +
					$"# This is a sample HOSTS file used by Microsoft TCP/IP for Windows.{Environment.NewLine}" +
					$"#{Environment.NewLine}" +
					$"# This file contains the mappings of IP addresses to host names. Each{Environment.NewLine}" +
					$"# entry should be kept on an individual line. The IP address should{Environment.NewLine}" +
					$"# be placed in the first column followed by the corresponding host name.{Environment.NewLine}" +
					$"# The IP address and the host name should be separated by at least one{Environment.NewLine}" +
					$"# space.{Environment.NewLine}" +
					$"#{Environment.NewLine}" +
					$"# Additionally, comments (such as these) may be inserted on individual{Environment.NewLine}" +
					$"# lines or following the machine name denoted by a '#' symbol.{Environment.NewLine}" +
					$"#{Environment.NewLine}" +
					$"# For example:{Environment.NewLine}" +
					$"#{Environment.NewLine}" +
					$"#      102.54.94.97     rhino.acme.com          # source server{Environment.NewLine}" +
					$"#       38.25.63.10     x.acme.com              # x client host{Environment.NewLine}" +
					$"# localhost name resolution is handle within DNS itself.{Environment.NewLine}" +
					$"#\t127.0.0.1       localhost{Environment.NewLine}" +
					$"#\t::1             localhost{Environment.NewLine}";
				if (GenerateDefaultHosts)
					File.WriteAllText(defaultHosts, header);
			}
			return header;
		}
	}
}
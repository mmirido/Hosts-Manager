using System;
using System.Drawing;
using System.Windows.Forms;

namespace Hosts_Manager
{
	public class InputBox
	{
		public static DialogResult Show(string caption, (string prompt, bool isMultiValue)[] outputs, out string[] value) => InitInputBox(caption, outputs, out value);

		private static DialogResult InitInputBox(string caption, (string prompt, bool isMultiValue)[] outputs, out string[] value)
		{
			int n = outputs.Length;

			if (n > 0)
			{
				Label[] labels = new Label[n];
				TextBox[] textBoxes = new TextBox[n];

				int labelWidth = 0;

				for (int i = 0; i < n; i++)
				{
					textBoxes[i] = new TextBox();
					if (outputs[i].isMultiValue)
					{
						textBoxes[i].Size = new Size(400, 100);
						textBoxes[i].Multiline = true;
						textBoxes[i].ScrollBars = ScrollBars.Both;
						textBoxes[i].AcceptsReturn = true;
					}
					else
						textBoxes[i].Size = new Size(400, 100);

					labels[i] = new Label
					{
						AutoSize = true,
						Height = textBoxes[i].Height,
						Text = outputs[i].prompt
					};

					labelWidth = Math.Max(labelWidth, labels[i].Width);
				}

				labels[0].Width = labelWidth;
				labels[0].Location = new Point(12, 12);

				int tbX = labels[0].Right + 12;
				textBoxes[0].Location = new Point(labels[0].Right + 12, 12);

				for (int i = 1; i < n; i++)
				{
					int newY = labels[i - 1].Bottom + 12;
					labels[i].Width = labelWidth;
					labels[i].Location = new Point(12, newY);
					textBoxes[i].Location = new Point(tbX, newY);
				}

				Button buttonCancel = new Button
				{
					AutoSize = true,
					Text = "&Cancel",
					DialogResult = DialogResult.Cancel,
					Location = new Point(textBoxes[n - 1].Right - 75, textBoxes[n - 1].Bottom + 12),
					Size = new Size(75, 20)
				};

				Button buttonOk = new Button
				{
					AutoSize = true,
					Text = "&OK",
					DialogResult = DialogResult.OK,
					Location = new Point(buttonCancel.Left - buttonCancel.Width - 12, buttonCancel.Top),
					Size = new Size(buttonCancel.Width, buttonCancel.Height)
				};

				Form form = new Form
				{
					AutoScaleMode = AutoScaleMode.Dpi,
					Text = caption,
					ClientSize = new Size(20, 20),
					FormBorderStyle = FormBorderStyle.FixedDialog,
					StartPosition = FormStartPosition.CenterParent,
					MinimizeBox = false,
					MaximizeBox = false
				};

				form.AcceptButton = buttonOk;
				form.CancelButton = buttonCancel;
				form.ClientSize = new Size(textBoxes[n - 1].Right + 12, buttonOk.Bottom + 24);

				for (int i = 0; i < n; i++)
				{
					form.Controls.AddRange(new Control[] { labels[i], textBoxes[i] });
				}
				form.Controls.AddRange(new Control[] { buttonOk, buttonCancel });

				DialogResult dialogResult = form.ShowDialog();

				value = new string[n];
				for (int i = 0; i < n; i++)
				{
					value[i] = textBoxes[i].Text;
				}
				return dialogResult;
			}
			value = new string[0] { };
			return DialogResult.None;
		}
	}
}
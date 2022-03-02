using System.Drawing;
using System.Windows.Forms;

namespace Hosts_Manager
{
	public class InputBox
	{
		public static DialogResult Show(string caption, (string prompt, bool isMultiValue)[] reqs, out string[] value) => InitInputBox(caption, reqs, out value);

		private static DialogResult InitInputBox(string caption, (string prompt, bool isMultiValue)[] reqs, out string[] value)
		{
			int n = reqs.Length;

			if (n > 0)
			{
				Label[] labels = new Label[n];
				TextBox[] textBoxes = new TextBox[n];

				labels[0] = new Label()
				{
					AutoSize = true,
					AutoEllipsis = true,
					Text = reqs[0].prompt,
					Location = new Point(12, 12),
					MaximumSize = new Size(400, 20)
				};

				textBoxes[0] = new TextBox();
				if (reqs[0].isMultiValue)
				{
					textBoxes[0].AcceptsReturn = true;
					textBoxes[0].Multiline = true;
					textBoxes[0].ScrollBars = ScrollBars.Both;
					textBoxes[0].Size = new Size(400, 100);
				}
				else
					textBoxes[0].Size = new Size(400, 20);
				textBoxes[0].Location = new Point(12, labels[0].Bottom + 12);

				for (int i = 1; i < n; i++)
				{
					labels[i] = new Label
					{
						AutoSize = true,
						AutoEllipsis = true,
						Text = reqs[i].prompt,
						Location = new Point(12, textBoxes[i - 1].Bottom + 12),
						MaximumSize = new Size(400, 20)
					};

					textBoxes[i] = new TextBox();
					if (reqs[i].isMultiValue)
					{
						textBoxes[i].Size = new Size(400, 100);
						textBoxes[i].Multiline = true;
						textBoxes[i].ScrollBars = ScrollBars.Both;
						textBoxes[i].AcceptsReturn = true;
					}
					else
						textBoxes[i].Size = new Size(400, 20);
					textBoxes[i].Location = new Point(labels[i].Bottom + 12);
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
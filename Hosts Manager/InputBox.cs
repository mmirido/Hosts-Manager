using System.Drawing;
using System.Windows.Forms;

namespace Hosts_Manager
{
	public class InputBox
	{
		public static DialogResult Show(ref string value, string prompt, string caption)
		{
			return InitInputBox(ref value, prompt, caption);
		}

		private static DialogResult InitInputBox(ref string value, string prompt, string caption)
		{
			Label label = new Label
			{
				AutoSize = true,
				Location = new Point(12, 12),
				Text = prompt
			};

			TextBox textBox = new TextBox
			{
				Location = new Point(12, label.Bottom + 12),
				Size = new Size(400, 20)
			};

			Button buttonCancel = new Button
			{
				AutoSize = true,
				Text = "&Cancel",
				DialogResult = DialogResult.Cancel,
				Location = new Point(textBox.Right - 75, textBox.Bottom + 12),
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
				ClientSize = new Size(200, 20),
				FormBorderStyle = FormBorderStyle.FixedDialog,
				StartPosition = FormStartPosition.CenterParent,
				MinimizeBox = false,
				MaximizeBox = false,
				AcceptButton = buttonOk,
				CancelButton = buttonCancel
			};
			form.ClientSize = new Size(textBox.Right + 12, buttonOk.Bottom + 24);
			form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });

			DialogResult dialogResult = form.ShowDialog();
			value = textBox.Text;
			return dialogResult;
		}
	}
}
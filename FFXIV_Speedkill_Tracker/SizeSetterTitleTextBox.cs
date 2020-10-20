using System;
using System.Windows.Forms;
using System.Drawing;

namespace FFXIV_Speedkill_Tracker
{
    public class SizeSetterTitleTextBox : TextBox
    {
        private readonly int WIDTH = 250;
        private readonly int HEIGHT = 50;

        private readonly int X = 150;
        private readonly int Y = 30;

        public SizeSetterTitleTextBox(String title)
        {
            this.Size = new Size(WIDTH, HEIGHT);
            this.Location = new Point(X, Y);

            this.Text = title;

            this.ReadOnly = true;
            this.TextAlign = HorizontalAlignment.Center;
        }
    }
}

using System;
using System.Windows.Forms;
using System.Drawing;

namespace FFXIV_Speedkill_Tracker
{
    public class WidthSetter : TextBox
    {
        private readonly int WIDTH = 100;
        private readonly int HEIGHT = 50;

        private readonly int X = 150;
        private readonly int Y = 100;

        public WidthSetter(int initialWidth)
        {
            this.Size = new Size(WIDTH, HEIGHT);
            this.Location = new Point(X, Y);

            this.Text = initialWidth.ToString();
        }
    }
}

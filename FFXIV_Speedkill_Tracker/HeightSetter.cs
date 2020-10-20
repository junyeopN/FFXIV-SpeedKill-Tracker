using System;
using System.Windows.Forms;
using System.Drawing;

namespace FFXIV_Speedkill_Tracker
{
    public class HeightSetter : TextBox
    {
        private readonly int WIDTH = 100;
        private readonly int HEIGHT = 50;

        private readonly int X = 300;
        private readonly int Y = 100;

        public HeightSetter(int initialHeight)
        {
            this.Size = new Size(WIDTH, HEIGHT);
            this.Location = new Point(X, Y);

            this.Text = initialHeight.ToString();
        }
    }
}

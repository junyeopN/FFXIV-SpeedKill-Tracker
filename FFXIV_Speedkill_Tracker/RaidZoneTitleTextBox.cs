using System.Windows.Forms;
using System.Drawing;

namespace FFXIV_Speedkill_Tracker
{
    public class RaidZoneTitleTextBox: TextBox
    {
        public static readonly string INITIAL_TITLE = "Welcome To FFXIV SpeedKill Tracker";

        public static readonly int WIDTH = 360;
        public static readonly int HEIGHT = 100; 

        public static readonly int X = 100;
        public static readonly int Y = 0;

        public RaidZoneTitleTextBox()
        {
            this.Size = new Size(WIDTH, HEIGHT);
            this.Location = new Point(X, Y);

            this.Text = INITIAL_TITLE;

            this.ReadOnly = true;
        }
    }
}

using System.Windows.Forms;
using System.Drawing;

namespace FFXIV_Speedkill_Tracker
{
    public class TrackerPage : Form
    {
        public static readonly string INITIAL_TITLE = "Welcome To FFXIV SpeedKill Tracker";


        public static readonly int X = 1300;
        public static readonly int Y = 800;

        public static readonly string EMPTY_STRING = "   ";


        public TrackerPage(int width, int height)
        {
            this.TopMost = true;
            this.ControlBox = false;
            this.Text = EMPTY_STRING;

            this.Size = new Size(width, height);
            this.Location = new Point(X, Y);
        }
    }
}

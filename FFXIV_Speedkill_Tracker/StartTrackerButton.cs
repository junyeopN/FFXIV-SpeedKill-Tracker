using System;
using System.Windows.Forms;
using System.Drawing;


namespace FFXIV_Speedkill_Tracker
{
    class StartTrackerButton: Button 
    {
        public readonly String START_TRACKER_TEXT = "Start Tracker";
        public readonly String END_TRACKER_TEXT = "Close Tracker";
        

        public static readonly int WIDTH = 200;
        public static readonly int HEIGHT = 20; 

        public static readonly int X = 475;
        public static readonly int Y = 400;


        public StartTrackerButton()
        {
            this.Text = START_TRACKER_TEXT;

            this.Size = new Size(WIDTH, HEIGHT);
            this.Location = new Point(X, Y);
        }

        public void ChangeText() {
            if(Text.Equals(START_TRACKER_TEXT)) 
            {
                Text = END_TRACKER_TEXT;
            }
            else 
            {
                Text = START_TRACKER_TEXT;
            }
        }

    }
}

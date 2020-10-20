using System;
using System.Windows.Forms;
using System.Drawing;

namespace FFXIV_Speedkill_Tracker
{
    public class SizeSetter : System.Windows.Forms.Panel
    {
        private SizeSetterTitleTextBox _sizeSetterTitleTextBox = null;

        private readonly int WIDTH = 550;
        private readonly int HEIGHT = 120;

        private TextBox _widthSetterTitle = null;
        private TextBox _heightSetterTitle = null;

        private WidthSetter _widthSetter = null;
        private HeightSetter _heightSetter = null;

        public SizeSetter(String sizeSetterTitle, Point initialSizeSetterLocation, Size initialSettingFormSize)
        {
            this.Size = new Size(WIDTH, HEIGHT);
            this.Location = initialSizeSetterLocation;

            _sizeSetterTitleTextBox = new SizeSetterTitleTextBox(sizeSetterTitle);

            InitSetterTitleBox();


            _widthSetter = new WidthSetter(initialSettingFormSize.Width);
            _heightSetter = new HeightSetter(initialSettingFormSize.Height);

            Controls.Add(_sizeSetterTitleTextBox);
            Controls.Add(_widthSetterTitle);
            Controls.Add(_widthSetter);
            Controls.Add(_heightSetterTitle);
            Controls.Add(_heightSetter);
        }

        private void InitSetterTitleBox()
        {
            _widthSetterTitle = new TextBox();
            _heightSetterTitle = new TextBox();

            _widthSetterTitle.Size = new Size(100, 30);
            _heightSetterTitle.Size = new Size(100, 30);

            _widthSetterTitle.Location = new Point(150, 60);
            _heightSetterTitle.Location = new Point(300, 60);

            _widthSetterTitle.Text = "width";
            _heightSetterTitle.Text = "height";

            _widthSetterTitle.TextAlign = HorizontalAlignment.Center;
            _heightSetterTitle.TextAlign = HorizontalAlignment.Center;

            _widthSetterTitle.ReadOnly = true;
            _heightSetterTitle.ReadOnly = true;

        }

        public int GetWidth()
        {
            return Int32.Parse(_widthSetter.Text);
        }

        public int GetHeight()
        {
            return Int32.Parse(_heightSetter.Text);
        }

    }
}

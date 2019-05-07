using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace Desktop_Clock
{
    public partial class Form1 : Form
    {
        string windowed;
        public Form1()
        {
            InitializeComponent();

            string center = ConfigurationManager.AppSettings["Center"];
            center = center.ToLower();

            windowed = ConfigurationManager.AppSettings["Windowed"];
            windowed = windowed.ToLower();

            string onTop = ConfigurationManager.AppSettings["OnTop"];
            onTop = onTop.ToLower();

            string backgroundMode = ConfigurationManager.AppSettings["BackgroundMode"];
            backgroundMode = backgroundMode.ToLower();

            int fontRed = Convert.ToInt32(ConfigurationManager.AppSettings["FontRed"]);
            int fontGreen = Convert.ToInt32(ConfigurationManager.AppSettings["FontGreen"]);
            int fontBlue = Convert.ToInt32(ConfigurationManager.AppSettings["FontBlue"]);

            if (center == "no")
            {
                string desktopX = ConfigurationManager.AppSettings["DesktopLocationX"];
                string desktopY = ConfigurationManager.AppSettings["DesktopLocationY"];
                this.SetDesktopLocation(Convert.ToInt32(desktopX), Convert.ToInt32(desktopY));
               
            }
            else if (center == "yes")
            {
                this.StartPosition = FormStartPosition.CenterScreen;
            }

            if (backgroundMode == "default")
            {
                //No transparency (standard look)
                label2.Parent = label1;
                label2.BackColor = Color.Transparent;
                label1.Parent = pictureBox1;
                label1.BackColor = Color.Transparent;
                setFontColor(fontRed, fontGreen, fontBlue);
                updateBackground();
            }
            else if (backgroundMode == "transparent")
            {
                //Completely transparent

                if (fontRed > 0)
                {
                    this.TransparencyKey = Color.FromArgb(255, fontRed - 1, 255, 255);
                    label1.BackColor = Color.FromArgb(255, fontRed - 1, 255, 255);
                    label2.BackColor = Color.FromArgb(255, fontRed - 1, 255, 255);
                    pictureBox1.BackColor = Color.FromArgb(255, fontRed - 1, 255, 255);
                    this.BackColor = Color.FromArgb(255, fontRed - 1, 255, 255);
                }
                else {
                    this.TransparencyKey = Color.FromArgb(255, fontRed + 1, 255, 255);
                    label1.BackColor = Color.FromArgb(255, fontRed + 1, 255, 255);
                    label2.BackColor = Color.FromArgb(255, fontRed + 1, 255, 255);
                    pictureBox1.BackColor = Color.FromArgb(255, fontRed + 1, 255, 255);
                    this.BackColor = Color.FromArgb(255, fontRed + 1, 255, 255);
                }
                if (windowed == "yes") {
                    this.FormBorderStyle = FormBorderStyle.Fixed3D;
                }
                if (onTop == "yes")
                {
                    this.TopMost = true;
                }


                setFontColor(fontRed, fontGreen, fontBlue);

                label2.Location = new Point(label2.Location.X - 20, label2.Location.Y);
            }
            else if(backgroundMode == "solidcolor"){

                //Solid Color
                int red = Convert.ToInt32(ConfigurationManager.AppSettings["Red"]);
                int green = Convert.ToInt32(ConfigurationManager.AppSettings["Green"]);
                int blue = Convert.ToInt32(ConfigurationManager.AppSettings["Blue"]);

                label2.Location = new Point(label2.Location.X - 20, label2.Location.Y);
                this.TransparencyKey = Color.FromArgb(0, 0, 0, 0);
                this.BackColor = Color.FromArgb(255, red, green, blue);
                setFontColor(fontRed, fontGreen, fontBlue);

                if (windowed == "yes")
                {
                    this.FormBorderStyle = FormBorderStyle.Fixed3D;
                }
                if (onTop == "yes")
                {
                    this.TopMost = true;
                }
            }

            
            
            timer1.Start();
            
            
        }

        private void setFontColor(int red, int green, int blue) {
            label1.ForeColor = Color.FromArgb(255, red, green, blue);
            label2.ForeColor = Color.FromArgb(255, red, green, blue);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            string time = "";
            //get current time
            int hh = DateTime.Now.Hour;
            int mm = DateTime.Now.Minute;
            int ss = DateTime.Now.Second;

            
            //padding leading zero
            if (hh < 10)
            {
                time += "0" + hh;
            }
            else
            {
                time += hh;
            }
            time += ":";

            if (mm < 10)
            {
                time += "0" + mm;
            }
            else
            {
                time += mm;
            }
            time += ":";

            if (ss < 10)
            {
                time += "0" + ss;
            }
            else
            {
                time += ss;
            }

         
            
            label2.Text = time;
            label1.Text = DateTime.Now.ToShortDateString();
        }

       


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
            
        }

        private void label2_DoubleClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void updateBackground() {


            string alpha = ConfigurationManager.AppSettings["Alpha"];
            string red = ConfigurationManager.AppSettings["Red"];
            string green = ConfigurationManager.AppSettings["Green"];
            string blue = ConfigurationManager.AppSettings["Blue"];

            this.WindowState = FormWindowState.Minimized;
            Rectangle aRectangle = new Rectangle(0, 0, this.Width, this.Height);
            SolidBrush br = new SolidBrush(Color.FromArgb(Convert.ToInt32(alpha), Convert.ToInt32(red), Convert.ToInt32(green), Convert.ToInt32(blue)));
            //SolidBrush br = new SolidBrush(Color.FromArgb(105, 0, 0, 0));
            Bitmap bitmap = new Bitmap(this.Width, this.Height);

            Size s = this.Size;
            Graphics g = Graphics.FromImage(bitmap as Image);
            g.CopyFromScreen(this.Location.X, this.Location.Y, 0, 0, s);
            g.FillRectangle(br, aRectangle);
            this.WindowState = FormWindowState.Normal;
           
            pictureBox1.Image = bitmap;

        }


        private void Form1_Move(object sender, EventArgs e)
        {
            if (windowed == "yes")
            {
                //Ignore the update event
            }
            else
            {
                updateBackground();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
        }
    }
}

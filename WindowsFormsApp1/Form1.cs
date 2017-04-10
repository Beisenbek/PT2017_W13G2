using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        State state = State.Pen;
        Point prevPoint;
        Graphics g;

        Color colorOrigin;
        Color colorFill;
        Bitmap bmp;
        Pen p = new Pen(Color.Black);

        public Form1()
        {
            InitializeComponent();
            bmp = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Image = bmp;
            g = Graphics.FromImage(bmp);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            state = State.Pen;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            state = State.Fill;
        }

        Queue<Point> q = new Queue<Point>();

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            switch (state)
            {
                case State.Pen:
                    prevPoint = e.Location;
                    break;
                case State.Fill:
                    q.Enqueue(e.Location);
                    colorOrigin = bmp.GetPixel(e.X, e.Y);
                    colorFill = p.Color;
                    Fill();
                    break;
                default:
                    break;
            }
        }

        private void Fill()
        {
            while(q.Count > 0)
            {
                Point curPoint = q.Dequeue();
                Step(curPoint.X + 1, curPoint.Y);
                Step(curPoint.X - 1, curPoint.Y);
                Step(curPoint.X, curPoint.Y + 1);
                Step(curPoint.X , curPoint.Y - 1);
            }

            pictureBox2.Refresh();
        }

        private void Step(int x, int y)
        {
            if (x < 0) return;
            if (y < 0) return;
            if (x >= bmp.Width) return;
            if (y >= bmp.Height) return;
            if (bmp.GetPixel(x, y) != colorOrigin) return;
            bmp.SetPixel(x, y, colorFill);
            q.Enqueue(new Point(x, y));
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            switch (state)
            {
                case State.Pen:
                    if(e.Button == MouseButtons.Left)
                    {
                        g.DrawLine(p, prevPoint, e.Location);
                        prevPoint = e.Location;
                        pictureBox2.Refresh();
                    }
                    break;
                case State.Fill:
                    break;
                default:
                    break;
            }
        }
    }

    enum State
    {
        Pen,
        Fill

    }
}

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV.UI;
using System.Drawing;
using Emgu.CV.CvEnum;

/*
 * Q2. Both Approaches work well.
 * Q3. Changing domain also works well
 */

namespace IA_Assignment0
{
    public partial class Form4 : Form
    {
        
        public Form4()
        {
            InitializeComponent();
            pictureBox1.Image = new Image<Gray, byte>("TimesNewRoman.gif").ToBitmap();
            pictureBox2.Image = new Image<Gray, byte>("SanSerif.gif").ToBitmap();
            pictureBox4.Image = new Image<Gray, byte>("TextImage.png").ToBitmap();
            pictureBox9.Image = new Image<Bgr, byte>("TextImage.png").ToBitmap();
            pictureBox10.Image = new Image<Hsv, byte>("TextImage.png").ToBitmap();
            pictureBox13.Image = new Image<Ycc, byte>("TextImage.png").ToBitmap();
            Q1();

        }

        /*
         * Q1. Convert Times New Roman to Sans Serif using Morphological Operations
         */
        private void Q1()
        {
            StructuringElementEx element1,element2,element3;

            Image<Gray, Byte> a = new Image<Gray, byte>("TimesNewRoman.gif");
            Image<Gray, Byte> b = new Image<Gray, byte>("TimesNewRoman.gif");
            Image<Gray, Byte> c = new Image<Gray, byte>("TimesNewRoman.gif");
            CvInvoke.cvThreshold(a, a, 2, 255, Emgu.CV.CvEnum.THRESH.CV_THRESH_BINARY_INV);
            element1 = new StructuringElementEx(4,8,2,4, Emgu.CV.CvEnum.CV_ELEMENT_SHAPE.CV_SHAPE_RECT);
            CvInvoke.cvErode(a, b, element1, 1);
            element2 = new StructuringElementEx(8, 2, 4, 1, Emgu.CV.CvEnum.CV_ELEMENT_SHAPE.CV_SHAPE_RECT);
            CvInvoke.cvErode(a, c, element2, 1);
            a = b.Add(c);
            element3 = new StructuringElementEx(4, 4, 2, 2, Emgu.CV.CvEnum.CV_ELEMENT_SHAPE.CV_SHAPE_RECT);
            CvInvoke.cvDilate(a,a,element3,1);
            pictureBox3.Image = a.ToBitmap();
        }
        
        /*
         * Q2. Segmentation Algorithm 1 (Contour Method) 
         */
        private void Q2a()
        {
            using (MemStorage stor = new MemStorage())
            {
                Image<Gray, Byte> a = new Image<Gray, byte>("TextImage.png");
                CvInvoke.cvThreshold(a, a, 2, 255, Emgu.CV.CvEnum.THRESH.CV_THRESH_BINARY_INV);
                Contour<Point> contours = a.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE, stor);
                Gray grey = new Gray(240);
                while (contours != null)
                {
                    if (contours.BoundingRectangle.Width > 10 && contours.BoundingRectangle.Height > 10)
                    {
                        Rectangle rect = contours.BoundingRectangle;
                        rect.X = rect.X - 5;
                        rect.Y = rect.Y - 5;
                        rect.Height = (rect.Height + 10);
                        rect.Width = (rect.Width + 10);
                        a.Draw(rect, grey, 5);
                    }
                    contours = contours.HNext;
                }
                pictureBox5.Image = a.ToBitmap();
            }            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Q2a();
           
        }

        private void Q2b()
        {
            Image<Gray, Byte> a = new Image<Gray, byte>("TextImage.png");
            CvInvoke.cvThreshold(a, a, 2, 255, Emgu.CV.CvEnum.THRESH.CV_THRESH_BINARY_INV);
            Emgu.CV.Cvb.CvBlobs blobs = new Emgu.CV.Cvb.CvBlobs();
            Emgu.CV.Cvb.CvBlobDetector blobdetector = new Emgu.CV.Cvb.CvBlobDetector();
            blobdetector.Detect(a, blobs);
            Gray grey = new Gray(240);
            foreach (Emgu.CV.Cvb.CvBlob b in blobs.Values)
            {

                if (b.Area > 50)
                {
                    Rectangle rect = b.BoundingBox;
                    rect.X = rect.X - 10;
                    rect.Y = rect.Y - 10;
                    rect.Height = (rect.Height + 20);
                    rect.Width = (rect.Width + 20);
                    a.Draw(rect, grey, 5);
                }
            }
            pictureBox6.Image = a.ToBitmap();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Q2b();
        }

        private void Q3a()
        {
            
            using (MemStorage stor = new MemStorage())
            {
                Image<Bgr, Byte> a = new Image<Bgr, byte>("TextImage.png");
                Image<Gray, Byte> d = new Image<Gray, byte>("TextImage.png");
                CvInvoke.cvThreshold(d, d, 2, 255, Emgu.CV.CvEnum.THRESH.CV_THRESH_BINARY_INV);
                Contour<Point> contours = d.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE, stor);
                Bgr red  = new Bgr(Color.Red);
                while (contours != null)
                {
                    if (contours.BoundingRectangle.Width > 10 && contours.BoundingRectangle.Height > 10)
                    {
                        Rectangle rect = contours.BoundingRectangle;
                        rect.X = rect.X - 5;
                        rect.Y = rect.Y - 5;
                        rect.Height = (rect.Height + 10);
                        rect.Width = (rect.Width + 10);
                        a.Draw(rect, red, 5);
                    }
                    contours = contours.HNext;
                }
                pictureBox8.Image = a.ToBitmap();
            } 

        }

        private void Q3b()
        {   
            
            using (MemStorage stor = new MemStorage())
            {
                Image<Hsv, Byte> b = new Image<Hsv, byte>("TextImage.png");
                Image<Gray, Byte> d = new Image<Gray, byte>("TextImage.png");
                CvInvoke.cvThreshold(d, d, 2, 255, Emgu.CV.CvEnum.THRESH.CV_THRESH_BINARY_INV);
                Contour<Point> contours = d.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE, stor);
                Hsv grey = new Hsv(160,120,120);
                while (contours != null)
                {
                    if (contours.BoundingRectangle.Width > 10 && contours.BoundingRectangle.Height > 10)
                    {
                        Rectangle rect = contours.BoundingRectangle;
                        rect.X = rect.X - 5;
                        rect.Y = rect.Y - 5;
                        rect.Height = (rect.Height + 10);
                        rect.Width = (rect.Width + 10);
                        b.Draw(rect, grey, 5);
                    }
                    contours = contours.HNext;
                }
                pictureBox11.Image = b.ToBitmap();
            } 

        }

        private void Q3c()
        {
            
            using (MemStorage stor = new MemStorage())
            {
                Image<Ycc, Byte> c = new Image<Ycc, byte>("TextImage.png");
                Image<Gray, Byte> d = new Image<Gray, byte>("TextImage.png");
                CvInvoke.cvThreshold(d, d, 2, 255, Emgu.CV.CvEnum.THRESH.CV_THRESH_BINARY_INV);
                Contour<Point> contours = d.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE, stor);
                Ycc grey = new Ycc(70,70,70);
                while (contours != null)
                {
                    if (contours.BoundingRectangle.Width > 10 && contours.BoundingRectangle.Height > 10)
                    {
                        Rectangle rect = contours.BoundingRectangle;
                        rect.X = rect.X - 5;
                        rect.Y = rect.Y - 5;
                        rect.Height = (rect.Height + 10);
                        rect.Width = (rect.Width + 10);
                        c.Draw(rect, grey, 5);
                    }
                    contours = contours.HNext;
                }
                pictureBox14.Image = c.ToBitmap();
            } 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Q3a();
            Q3b();
            Q3c();
        }

        private void Q3a1()
        {
            Image<Bgr, Byte> a = new Image<Bgr, byte>("TextImage.png");
            CvInvoke.cvThreshold(a, a, 2, 255, Emgu.CV.CvEnum.THRESH.CV_THRESH_BINARY_INV);
            Image<Gray, byte>[] channels_away = a.Split();
            CvInvoke.cvInRangeS(channels_away[0], new Gray(0).MCvScalar, new Gray(255).MCvScalar, channels_away[0]);
            CvInvoke.cvInRangeS(channels_away[1], new Gray(0).MCvScalar, new Gray(255).MCvScalar, channels_away[1]);
            CvInvoke.cvInRangeS(channels_away[2], new Gray(0).MCvScalar, new Gray(255).MCvScalar, channels_away[2]);
            channels_away[0]._And(channels_away[1]);
            channels_away[0]._And(channels_away[2]);
            Emgu.CV.Cvb.CvBlobs blobs = new Emgu.CV.Cvb.CvBlobs();
            Emgu.CV.Cvb.CvBlobs blobs1 = new Emgu.CV.Cvb.CvBlobs();
            Emgu.CV.Cvb.CvBlobs blobs2 = new Emgu.CV.Cvb.CvBlobs();
            Emgu.CV.Cvb.CvBlobs blobs3 = new Emgu.CV.Cvb.CvBlobs();
            Emgu.CV.Cvb.CvBlobDetector blobdetector = new Emgu.CV.Cvb.CvBlobDetector();
            blobdetector.Detect(a.Convert<Gray,byte>(), blobs);
            blobdetector.Detect(channels_away[0], blobs1);
            blobdetector.Detect(channels_away[1], blobs2);
            blobdetector.Detect(channels_away[2], blobs3);
            Bgr red = new Bgr(Color.Red);
            Bgr blue = new Bgr(Color.Blue);
            Bgr green = new Bgr(Color.Green);

            foreach (Emgu.CV.Cvb.CvBlob b in blobs.Values)
            {

                if (b.Area > 50)
                {
                    Rectangle rect = b.BoundingBox;
                    rect.X = rect.X - 10;
                    rect.Y = rect.Y - 10;
                    rect.Height = (rect.Height + 20);
                    rect.Width = (rect.Width + 20);
                    a.Draw(rect, red, 5);
                }
            }
            foreach (Emgu.CV.Cvb.CvBlob b in blobs1.Values)
            {

                if (b.Area > 50)
                {
                    Rectangle rect = b.BoundingBox;
                    rect.X = rect.X - 10;
                    rect.Y = rect.Y - 10;
                    rect.Height = (rect.Height + 20);
                    rect.Width = (rect.Width + 20);
                    a.Draw(rect, red, 5);
                }
            }
            foreach (Emgu.CV.Cvb.CvBlob b in blobs2.Values)
            {

                if (b.Area > 50)
                {
                    Rectangle rect = b.BoundingBox;
                    rect.X = rect.X - 10;
                    rect.Y = rect.Y - 10;
                    rect.Height = (rect.Height + 20);
                    rect.Width = (rect.Width + 20);
                    a.Draw(rect, blue, 5);
                }
            }
            foreach (Emgu.CV.Cvb.CvBlob b in blobs3.Values)
            {

                if (b.Area > 50)
                {
                    Rectangle rect = b.BoundingBox;
                    rect.X = rect.X - 10;
                    rect.Y = rect.Y - 10;
                    rect.Height = (rect.Height + 20);
                    rect.Width = (rect.Width + 20);
                    a.Draw(rect, green, 5);
                }
            }
            pictureBox7.Image = a.ToBitmap();
        }

        private void Q3b1()
        {
            Image<Hsv, Byte> a = new Image<Hsv, byte>("TextImage.png");
            CvInvoke.cvThreshold(a, a, 2, 255, Emgu.CV.CvEnum.THRESH.CV_THRESH_BINARY_INV);
            Image<Gray, byte>[] channels_away = a.Split();
            CvInvoke.cvInRangeS(channels_away[0], new Gray(0).MCvScalar, new Gray(255).MCvScalar, channels_away[0]);
            CvInvoke.cvInRangeS(channels_away[1], new Gray(0).MCvScalar, new Gray(255).MCvScalar, channels_away[1]);
            CvInvoke.cvInRangeS(channels_away[2], new Gray(0).MCvScalar, new Gray(255).MCvScalar, channels_away[2]);
            channels_away[0]._And(channels_away[1]);
            channels_away[0]._And(channels_away[2]);
            Emgu.CV.Cvb.CvBlobs blobs = new Emgu.CV.Cvb.CvBlobs();
            Emgu.CV.Cvb.CvBlobs blobs1 = new Emgu.CV.Cvb.CvBlobs();
            Emgu.CV.Cvb.CvBlobs blobs2 = new Emgu.CV.Cvb.CvBlobs();
            Emgu.CV.Cvb.CvBlobs blobs3 = new Emgu.CV.Cvb.CvBlobs();
            Emgu.CV.Cvb.CvBlobDetector blobdetector = new Emgu.CV.Cvb.CvBlobDetector();
            blobdetector.Detect(a.Convert<Gray, byte>(), blobs);
            blobdetector.Detect(channels_away[0], blobs1);
            blobdetector.Detect(channels_away[1], blobs2);
            blobdetector.Detect(channels_away[2], blobs3);
            Hsv zero = new Hsv(40, 120, 120);
            Hsv one = new Hsv(160,120,120);
            Hsv two = new Hsv(100,120,120);
            Hsv three = new Hsv(70,120,120);

            foreach (Emgu.CV.Cvb.CvBlob b in blobs.Values)
            {

                if (b.Area > 50)
                {
                    Rectangle rect = b.BoundingBox;
                    rect.X = rect.X - 10;
                    rect.Y = rect.Y - 10;
                    rect.Height = (rect.Height + 20);
                    rect.Width = (rect.Width + 20);
                    a.Draw(rect, zero, 5);
                }
            }
            foreach (Emgu.CV.Cvb.CvBlob b in blobs1.Values)
            {

                if (b.Area > 50)
                {
                    Rectangle rect = b.BoundingBox;
                    rect.X = rect.X - 10;
                    rect.Y = rect.Y - 10;
                    rect.Height = (rect.Height + 20);
                    rect.Width = (rect.Width + 20);
                    a.Draw(rect, one, 5);
                }
            }
            foreach (Emgu.CV.Cvb.CvBlob b in blobs2.Values)
            {

                if (b.Area > 50)
                {
                    Rectangle rect = b.BoundingBox;
                    rect.X = rect.X - 10;
                    rect.Y = rect.Y - 10;
                    rect.Height = (rect.Height + 20);
                    rect.Width = (rect.Width + 20);
                    a.Draw(rect, two, 5);
                }
            }
            foreach (Emgu.CV.Cvb.CvBlob b in blobs3.Values)
            {

                if (b.Area > 50)
                {
                    Rectangle rect = b.BoundingBox;
                    rect.X = rect.X - 10;
                    rect.Y = rect.Y - 10;
                    rect.Height = (rect.Height + 20);
                    rect.Width = (rect.Width + 20);
                    a.Draw(rect, three, 5);
                }
            }
            
            pictureBox12.Image = a.ToBitmap();
        }

        private void Q3c1()
        {
            Image<Ycc, Byte> a = new Image<Ycc, byte>("TextImage.png");
            Image<Gray, Byte> d = new Image<Gray, byte>("TextImage.png");
            CvInvoke.cvThreshold(d, d, 0, 255, Emgu.CV.CvEnum.THRESH.CV_THRESH_BINARY_INV);
            Image<Gray, byte>[] channels_away = a.Split();
            CvInvoke.cvInRangeS(channels_away[0], new Gray(0).MCvScalar, new Gray(255).MCvScalar, channels_away[0]);
            CvInvoke.cvInRangeS(channels_away[1], new Gray(0).MCvScalar, new Gray(255).MCvScalar, channels_away[1]);
            CvInvoke.cvInRangeS(channels_away[2], new Gray(0).MCvScalar, new Gray(255).MCvScalar, channels_away[2]);
            
            Emgu.CV.Cvb.CvBlobs blobs = new Emgu.CV.Cvb.CvBlobs();
            Emgu.CV.Cvb.CvBlobs blobs1 = new Emgu.CV.Cvb.CvBlobs();
            Emgu.CV.Cvb.CvBlobs blobs2 = new Emgu.CV.Cvb.CvBlobs();
            Emgu.CV.Cvb.CvBlobs blobs3 = new Emgu.CV.Cvb.CvBlobs();
            Emgu.CV.Cvb.CvBlobDetector blobdetector = new Emgu.CV.Cvb.CvBlobDetector();
            blobdetector.Detect(d, blobs);
            blobdetector.Detect(channels_away[0], blobs1);
            blobdetector.Detect(channels_away[1], blobs2);
            blobdetector.Detect(channels_away[2], blobs3);
            Ycc zero = new Ycc(100, 100, 100);
            Ycc one = new Ycc(70,70,70);
            Ycc two = new Ycc(80,80,80);
            Ycc three = new Ycc(90,90,90);

            foreach (Emgu.CV.Cvb.CvBlob b in blobs.Values)
            {

                if (b.Area > 50)
                {
                    Rectangle rect = b.BoundingBox;
                    rect.X = rect.X - 10;
                    rect.Y = rect.Y - 10;
                    rect.Height = (rect.Height + 20);
                    rect.Width = (rect.Width + 20);
                    a.Draw(rect, zero, 5);
                }
            }
            foreach (Emgu.CV.Cvb.CvBlob b in blobs1.Values)
            {

                if (b.Area > 50)
                {
                    Rectangle rect = b.BoundingBox;
                    rect.X = rect.X - 10;
                    rect.Y = rect.Y - 10;
                    rect.Height = (rect.Height + 20);
                    rect.Width = (rect.Width + 20);
                    a.Draw(rect, one, 5);
                }
            }
            foreach (Emgu.CV.Cvb.CvBlob b in blobs2.Values)
            {

                if (b.Area > 50)
                {
                    Rectangle rect = b.BoundingBox;
                    rect.X = rect.X - 10;
                    rect.Y = rect.Y - 10;
                    rect.Height = (rect.Height + 20);
                    rect.Width = (rect.Width + 20);
                    a.Draw(rect, two, 5);
                }
            }
            foreach (Emgu.CV.Cvb.CvBlob b in blobs3.Values)
            {

                if (b.Area > 50)
                {
                    Rectangle rect = b.BoundingBox;
                    rect.X = rect.X - 10;
                    rect.Y = rect.Y - 10;
                    rect.Height = (rect.Height + 20);
                    rect.Width = (rect.Width + 20);
                    a.Draw(rect, three, 5);
                }
            }
            
            pictureBox15.Image = a.ToBitmap();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Q3a1();
            Q3b1();
            Q3c1();
        }
    }
}

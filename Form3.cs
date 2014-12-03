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

namespace IA_Assignment0
{
    public partial class Form3 : Form
    {
        /*
         * Global Variables
         */

        //Varibale to Store Camera Input
        private Capture capture;        
        //Variable to check if Capture is in Progress
        private bool captureInProgress;
        private int homehuemin;
        private int homehuemax;
        private int homesatmin;
        private int homesatmax;
        private int homevalmin;
        private int homevalmax;
        private int awayhuemin;
        private int awayhuemax;
        private int awaysatmin;
        private int awaysatmax;
        private int awayvalmin;
        private int awayvalmax;

        private Image<Bgr, Byte> inputimage1;
        private Image<Gray, Byte> homeimage1;
        private Image<Gray, Byte> awayimage1;
        private Image<Bgr, Byte> inputimage2;
        private Image<Gray, Byte> homeimage2;
        private Image<Gray, Byte> awayimage2;
        private Image<Bgr, Byte> inputimage3;
        private Image<Gray, Byte> homeimage3;
        private Image<Gray, Byte> awayimage3;
        
        /*
         *  Initialise Form
         */
        public Form3()
        {
            InitializeComponent();
            CB_testimages.Items.Add("fake_ground");
            CB_testimages.Items.Add("real_ground");
            CB_hometeam.Items.Add("Red");
            CB_hometeam.Items.Add("Blue");
            CB_hometeam.Items.Add("Green");
            CB_hometeam.Items.Add("Yellow");
            CB_awayteam.Items.Add("Red");
            CB_awayteam.Items.Add("Blue");
            CB_awayteam.Items.Add("Green");
            CB_awayteam.Items.Add("Yellow");
            CB_openfrequency.Items.Add(1);
            CB_openfrequency.Items.Add(2);
            CB_openfrequency.Items.Add(3);
            CB_openfrequency.Items.Add(4);
            CB_closefrequency.Items.Add(1);
            CB_closefrequency.Items.Add(2);
            CB_closefrequency.Items.Add(3);
            CB_closefrequency.Items.Add(4);
            CB_dilatefrequency.Items.Add(1);
            CB_dilatefrequency.Items.Add(2);
            CB_dilatefrequency.Items.Add(3);
            CB_dilatefrequency.Items.Add(4);
            CB_kernelsize.Items.Add(3);
            CB_kernelsize.Items.Add(5);
            CB_kernelsize.Items.Add(7);
            CB_kerneltype.Items.Add("cross");
            CB_kerneltype.Items.Add("ellipse");
            CB_Noise.Enabled=false;
            CB_compare.Enabled=false;
            CB_spothome.Enabled=false;
            CB_spotaway.Enabled = false;
               

            CB_closefrequency.SelectedIndex=1;
            CB_openfrequency.SelectedIndex=0;
            CB_dilatefrequency.SelectedIndex=3;
            CB_kernelsize.SelectedIndex = 0;
            CB_kerneltype.SelectedIndex = 1;
            CB_hometeam.SelectedIndex = 0;
            CB_awayteam.SelectedIndex = 1;
            CB_testimages.SelectedIndex = 0;
            B_startlivefeed.Enabled = false;
            CB_testimages.Enabled = true;
            B_testvideo.Enabled = false;
            TB_radius.Value = 30;
            TB_minarea.Value = 50;
        }

        /*
         * Get a Frame from the Camera and Display it on LiveFeedBox 
         */
        private void ProcessFrame(object sender, EventArgs arg)
        {
            inputimage1 = capture.QueryFrame();
            PB_input_1.Image = inputimage1.ToBitmap();
            homeimage1 = inputimage1.Convert<Gray,byte>();
            awayimage1 = inputimage1.Convert<Gray, byte>();
            homeimage2 = inputimage2.Convert<Gray, byte>();
            awayimage2 = inputimage2.Convert<Gray, byte>();
            homeimage3 = inputimage3.Convert<Gray, byte>();
            awayimage3 = inputimage3.Convert<Gray, byte>();
            processAlgorithm();
            setOutput();
        }

        private void StaticImage()
        {
            homeimage1 = inputimage1.Convert<Gray, byte>();
            awayimage1 = inputimage1.Convert<Gray, byte>();
            homeimage2 = inputimage2.Convert<Gray, byte>();
            awayimage2 = inputimage2.Convert<Gray, byte>();
            homeimage3 = inputimage3.Convert<Gray, byte>();
            awayimage3 = inputimage3.Convert<Gray, byte>();
            processAlgorithm();
            setOutput();
        }

        private void setOutput()
        {
            PB_output1_1.Image = awayimage1.ToBitmap();
            PB_output2_1.Image = homeimage1.ToBitmap();
            PB_output1_2.Image = awayimage2.ToBitmap();
            PB_output2_2.Image = homeimage2.ToBitmap();
            PB_output1_3.Image = awayimage3.ToBitmap();
            PB_output2_3.Image = homeimage3.ToBitmap();

        }

        private void processAlgorithm()
        {
            if(CB_segmenthome.Checked==true){
                homeimage1 = segmentHome(inputimage1);
                homeimage2 = segmentHome(inputimage2);
                homeimage3 = segmentHome(inputimage3);
            }
            if (CB_segmentaway.Checked == true)
            {
                awayimage1 = segmentAway(inputimage1);
                awayimage2 = segmentAway(inputimage2);
                awayimage3 = segmentAway(inputimage3);
            }
            if (CB_Noise.Checked == true)
            {
                homeimage1 = denoise(homeimage1);
                awayimage1 = denoise(awayimage1);
                homeimage2 = denoise(homeimage2);
                awayimage2 = denoise(awayimage2);
                homeimage3 = denoise(homeimage3);
                awayimage3 = denoise(awayimage3);
            }
            if (CB_spothome.Checked == true)
            {
                homeimage1 = spotblob(homeimage1);
                homeimage2 = spotblob(homeimage2);
                homeimage3 = spotblob(homeimage3);
            }
            if (CB_spotaway.Checked == true)
            {
                awayimage1 = spotblob(awayimage1);
                awayimage2 = spotblob(awayimage2);
                awayimage3 = spotblob(awayimage3);
            }
            if (CB_compare.Checked == true)
            {
                Image<Gray, byte>[] temp=new Image<Gray, byte>[2];
                temp=Compare(awayimage1, homeimage1, 1);
                awayimage1 = temp[0];
                homeimage1 = temp[1];
                temp=Compare(awayimage2, homeimage2, 2);
                awayimage2 = temp[0];
                homeimage2 = temp[1];
                temp=Compare(awayimage3, homeimage3, 3);
                awayimage3 = temp[0];
                homeimage3 = temp[1];
            }
        }

        /*
         * 
         */
        private void ReleaseData()
        {
            if (capture != null)
                capture.Dispose();
        }

        /* 
         * StartButtonClick Listner
         */
        private void StartButton_Click(object sender, EventArgs e)
        {
            #region if capture is not created, create it now
            if (capture == null)
            {
                try
                {
                    capture = new Capture();
                }
                catch (NullReferenceException excpt)
                {
                    MessageBox.Show(excpt.Message);
                }
            }
            #endregion

            if (capture != null)
            {
                if (captureInProgress)
                {  
                    B_startlivefeed.Text = "Start!"; //
                    Application.Idle -= ProcessFrame;
                }
                else
                {
                    B_startlivefeed.Text = "Stop";
                    Application.Idle += ProcessFrame;
                }

                captureInProgress = !captureInProgress;
            }
        }

 
        private void setcolor(int i,int hmin,int hmax,int smin,int smax,int vmin,int vmax){
            if (i == 0)
            {
                homehuemin = hmin;
                homehuemax = hmax;
                homesatmin = smin;
                homesatmax = smax;
                homevalmin = vmin;
                homevalmax = vmax;
            }
            else if (i == 1)
            {
                awayhuemin = hmin;
                awayhuemax = hmax;
                awaysatmin = smin;
                awaysatmax = smax;
                awayvalmin = vmin;
                awayvalmax = vmax;
            }
            refreshcp();
        }

        private void refreshcp()
        {
            TB_awayhuemin.Value = awayhuemin;
            TB_awayhuemax.Value = awayhuemax;
            TB_homehuemin.Value = homehuemin;
            TB_homehuemax.Value = homehuemax;
            TB_awaysatmin.Value = awaysatmin;
            TB_awaysatmax.Value = awaysatmax;
            TB_homesatmin.Value = homesatmin;
            TB_homesatmax.Value = homesatmax;
            TB_awayvalmin.Value = awayvalmin;
            TB_awayvalmax.Value = awayvalmax;
            TB_homevalmin.Value = homevalmin;
            TB_homevalmax.Value = homevalmax;
        }
        
        private void CB_hometeam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CB_hometeam.SelectedItem.ToString() == "Red")
            {
                setcolor(0, 160, 179, 0, 255, 60, 255);
            }
            if (CB_hometeam.SelectedItem.ToString() == "Blue")
            {
                setcolor(0, 75, 130, 0, 255, 60, 255);
            }
            if (CB_hometeam.SelectedItem.ToString() == "Green")
            {
                setcolor(0, 38, 75, 0, 255, 60, 255);
            }
            if (CB_hometeam.SelectedItem.ToString() == "Yellow")
            {
                setcolor(0, 22, 38, 0, 255, 60, 255);
            }

        }

        private void CB_awayteam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CB_awayteam.SelectedItem.ToString() == "Red")
            {
                setcolor(1, 160, 179, 0, 255, 60, 255);
            }
            if (CB_awayteam.SelectedItem.ToString() == "Blue")
            {
                setcolor(1, 75, 130, 0, 255, 60, 255);
            }
            if (CB_awayteam.SelectedItem.ToString() == "Green")
            {
                setcolor(1, 38, 75, 0, 255, 60, 255);
            }
            if (CB_awayteam.SelectedItem.ToString() == "Yellow")
            {
                setcolor(1, 22, 38, 0, 255, 60, 255);
            }
        }

        private void TB_homehuemin_Scroll(object sender, EventArgs e)
        {
            homehuemin = TB_homehuemin.Value;
        }

        private void TB_homehuemax_Scroll(object sender, EventArgs e)
        {
            homehuemax = TB_homehuemax.Value;
        }

        private void TB_homesatmin_Scroll(object sender, EventArgs e)
        {
            homesatmin = TB_homesatmin.Value;
        }

        private void TB_homesatmax_Scroll(object sender, EventArgs e)
        {
            homesatmax = TB_homesatmax.Value;
        }

        private void TB_homevalmin_Scroll(object sender, EventArgs e)
        {
            homevalmin = TB_homevalmin.Value;
        }

        private void TB_homevalmax_Scroll(object sender, EventArgs e)
        {
            homevalmax = TB_homevalmax.Value;
        }

        private void TB_awayhuemin_Scroll(object sender, EventArgs e)
        {
            awayhuemin = TB_awayhuemin.Value;
        }

        private void TB_awayhuemax_Scroll(object sender, EventArgs e)
        {
            awayhuemax = TB_awayhuemax.Value;
        }

        private void TB_awaysatmin_Scroll(object sender, EventArgs e)
        {
            awaysatmin = TB_awaysatmin.Value;
        }

        private void TB_awaysatmax_Scroll(object sender, EventArgs e)
        {
            awaysatmax = TB_awaysatmax.Value;
        }

        private void TB_awayvalmin_Scroll(object sender, EventArgs e)
        {
            awayvalmin = TB_awayvalmin.Value;
        }

        private void TB_awayvalmax_Scroll(object sender, EventArgs e)
        {
            awayvalmax = TB_awayvalmax.Value;
        }

        private void configurefeed()
        {
            if (RB_livefeed.Checked == true)
            {
                B_startlivefeed.Enabled = true;
                CB_testimages.Enabled = false;
                B_testvideo.Enabled = false;
                captureInProgress = true;
            }
            else if(RB_testimage.Checked == true)
            {
                B_startlivefeed.Enabled = false;
                CB_testimages.Enabled = true;
                B_testvideo.Enabled = false;
                captureInProgress = false;
                inputimage1 = new Image<Bgr, byte>(CB_testimages.SelectedItem.ToString() + "_1.png");
                PB_input_1.Image = inputimage1.ToBitmap();
                inputimage2 = new Image<Bgr, byte>(CB_testimages.SelectedItem.ToString() + "_2.png");
                PB_input_2.Image = inputimage2.ToBitmap();
                inputimage3 = new Image<Bgr, byte>(CB_testimages.SelectedItem.ToString() + "_3.png");
                PB_input_3.Image = inputimage3.ToBitmap();
            }
            else
            {
                B_startlivefeed.Enabled = false;
                CB_testimages.Enabled = false;
                B_testvideo.Enabled = true;
                captureInProgress = false;
            }
        }

        private void RB_livefeed_CheckedChanged(object sender, EventArgs e)
        {
            configurefeed();
        }

        private void RB_testimage_CheckedChanged(object sender, EventArgs e)
        {
            configurefeed();
        }

        private void RB_testvideo_CheckedChanged(object sender, EventArgs e)
        {
            configurefeed();
        }

        private void CB_testimages_SelectedIndexChanged(object sender, EventArgs e)
        {
            inputimage1 = new Image<Bgr, byte>(CB_testimages.SelectedItem.ToString() + "_1.png");
            PB_input_1.Image = inputimage1.ToBitmap();
            inputimage2 = new Image<Bgr, byte>(CB_testimages.SelectedItem.ToString() + "_2.png");
            PB_input_2.Image = inputimage2.ToBitmap();
            inputimage3 = new Image<Bgr, byte>(CB_testimages.SelectedItem.ToString() + "_3.png");
            PB_input_3.Image = inputimage3.ToBitmap();
        }

        private Image<Gray, byte> denoise(Image<Gray, byte> a)
        {
            StructuringElementEx element;
            if (CB_kerneltype.SelectedItem.ToString() == "cross")
            {
                 element = new StructuringElementEx(int.Parse(CB_kernelsize.SelectedItem.ToString()), int.Parse(CB_kernelsize.SelectedItem.ToString()), int.Parse(CB_kernelsize.SelectedItem.ToString())/2, int.Parse(CB_kernelsize.SelectedItem.ToString())/2, Emgu.CV.CvEnum.CV_ELEMENT_SHAPE.CV_SHAPE_CROSS);
            }
            else
            {
                //MessageBox.Show((int.Parse(CB_kernelsize.SelectedItem.ToString())/2).ToString());
                element = new StructuringElementEx(int.Parse(CB_kernelsize.SelectedItem.ToString()), int.Parse(CB_kernelsize.SelectedItem.ToString()), int.Parse(CB_kernelsize.SelectedItem.ToString())/2, int.Parse(CB_kernelsize.SelectedItem.ToString())/2, Emgu.CV.CvEnum.CV_ELEMENT_SHAPE.CV_SHAPE_ELLIPSE);
            }
            CvInvoke.cvMorphologyEx(a, a, IntPtr.Zero, element, Emgu.CV.CvEnum.CV_MORPH_OP.CV_MOP_OPEN, int.Parse(CB_openfrequency.SelectedItem.ToString()));
            CvInvoke.cvMorphologyEx(a, a, IntPtr.Zero, element, Emgu.CV.CvEnum.CV_MORPH_OP.CV_MOP_CLOSE, int.Parse(CB_closefrequency.SelectedItem.ToString()));
            a._Dilate(int.Parse(CB_dilatefrequency.SelectedItem.ToString()));
            return a;
        }

        private Image<Gray, byte> segmentHome(Image<Bgr, byte> a)
        {
            Image<Hsv, byte> Home = a.Convert<Hsv, byte>();
            Image<Gray, byte>[] channels_home = Home.Split();
            CvInvoke.cvInRangeS(channels_home[0], new Gray(homehuemin).MCvScalar, new Gray(homehuemax).MCvScalar, channels_home[0]);
            CvInvoke.cvInRangeS(channels_home[1], new Gray(homesatmin).MCvScalar, new Gray(homesatmax).MCvScalar, channels_home[1]);
            CvInvoke.cvInRangeS(channels_home[2], new Gray(homevalmin).MCvScalar, new Gray(homevalmax).MCvScalar, channels_home[2]);
            channels_home[0]._And(channels_home[1]);
            channels_home[0]._And(channels_home[2]);
            return channels_home[0];
        }

        private Image<Gray, byte> segmentAway(Image<Bgr, byte> a)
        {
            Image<Hsv, byte> Away = a.Convert<Hsv, byte>();
            Image<Gray, byte>[] channels_away = Away.Split();
            CvInvoke.cvInRangeS(channels_away[0], new Gray(awayhuemin).MCvScalar, new Gray(awayhuemax).MCvScalar, channels_away[0]);
            CvInvoke.cvInRangeS(channels_away[1], new Gray(awaysatmin).MCvScalar, new Gray(awaysatmax).MCvScalar, channels_away[1]);
            CvInvoke.cvInRangeS(channels_away[2], new Gray(awayvalmin).MCvScalar, new Gray(awayvalmax).MCvScalar, channels_away[2]);
            channels_away[0]._And(channels_away[1]);
            channels_away[0]._And(channels_away[2]);
            return channels_away[0];
        }

        private Image<Gray, byte> spotblob(Image<Gray,byte> a)
        {
            Emgu.CV.Cvb.CvBlobs blobs = new Emgu.CV.Cvb.CvBlobs();
            Emgu.CV.Cvb.CvBlobDetector blobdetector = new Emgu.CV.Cvb.CvBlobDetector();
            blobdetector.Detect(a, blobs);
            Gray white = new Gray(125);
            foreach (Emgu.CV.Cvb.CvBlob b in blobs.Values)
            {
             
                if (b.Area > int.Parse(TB_minarea.Value.ToString()))
                {
                    CircleF cir = new CircleF(b.Centroid, int.Parse(TB_radius.Value.ToString()));
                    a.Draw(cir, white, 5);
                }
            } 
            return a;
        }

        private Image<Gray, byte>[] Compare(Image<Gray, byte> a, Image<Gray, byte> h, int i)
        {
            Emgu.CV.Cvb.CvBlobs blobs_a = new Emgu.CV.Cvb.CvBlobs();
            Emgu.CV.Cvb.CvBlobs blobs_h = new Emgu.CV.Cvb.CvBlobs();
            
            Emgu.CV.Cvb.CvBlobDetector blobdetector = new Emgu.CV.Cvb.CvBlobDetector();
            blobdetector.Detect(h, blobs_h);
            blobdetector.Detect(a, blobs_a);
            float val_a, val_h;
            if (i == 1)
            {
                val_a = 0;
                val_h = 0;
            }
            else if (i == 2)
            {
                val_a = a.Size.Width;
                val_h = h.Size.Width;
            }
            else
            {
                val_a = 0;
                val_h = 0;
            }

            foreach (Emgu.CV.Cvb.CvBlob b in blobs_h.Values)
            {

                if (b.Area > int.Parse(TB_minarea.Value.ToString()))
                {
                    if (i == 1)
                    {
                        if (b.Centroid.X >= val_h)
                        {
                            val_h = b.Centroid.X;
                        }
                    }
                    else if (i == 2)
                    {
                        if (b.Centroid.X <= val_h)
                        {
                            val_h = b.Centroid.X;
                        }
                    }
                    else
                    {
                        if (b.Centroid.Y >= val_h)
                        {
                            val_h = b.Centroid.Y;
                        }
                    }
                }
            }

            foreach (Emgu.CV.Cvb.CvBlob b in blobs_a.Values)
            {

                if (b.Area > int.Parse(TB_minarea.Value.ToString()))
                {
                    if (i == 1)
                    {
                        if (b.Centroid.X >= val_a)
                        {
                            val_a = b.Centroid.X;
                        }
                    }
                    else if (i == 2)
                    {
                        if (b.Centroid.X <= val_a)
                        {
                            val_a = b.Centroid.X;
                        }
                    }
                    else
                    {
                        if (b.Centroid.Y >= val_a)
                        {
                            val_a = b.Centroid.Y;
                        }
                    }
                }
            }

            if (i == 1)
            {
                Gray white = new Gray(125);
                LineSegment2D lin_a = new LineSegment2D(new Point((int)val_a,(int)0),new Point((int)val_a,(int)(a.Height-1)));
                a.Draw(lin_a, white, 5);
                LineSegment2D lin_h = new LineSegment2D(new Point((int)val_h,(int)0),new Point((int)val_h,(int)(h.Height-1)));
                h.Draw(lin_h, white, 5);
                if (val_a >= val_h)
                {
                    L_output_1.Text = "Camera 1 : In Offside Position!";
                }
                else
                {
                    L_output_1.Text = "Camera 2 : Not in Offside Position!";
                }
            }
            else if (i == 2)
            {
                Gray white = new Gray(125);
                LineSegment2D lin_a = new LineSegment2D(new Point((int)val_a,(int)0),new Point((int)val_a,(int)(a.Height-1)));
                a.Draw(lin_a, white, 5);
                LineSegment2D lin_h = new LineSegment2D(new Point((int)val_h,(int)0),new Point((int)val_h,(int)(h.Height-1)));
                h.Draw(lin_h, white, 5);
                if (val_a <= val_h)
                {
                    L_output_2.Text = "Camera 2 : In Offside Position!";
                }
                else
                {
                    L_output_2.Text = "Camera 2 : Not in Offside Position!";
                }
            }
            else
            {
                Gray white = new Gray(125);
                LineSegment2D lin_a = new LineSegment2D(new Point((int)0, (int)val_a), new Point((int)(a.Width - 1), (int)val_a));
                a.Draw(lin_a, white, 5);
                LineSegment2D lin_h = new LineSegment2D(new Point((int)0, (int)val_h), new Point((int)(h.Width - 1), (int)val_h));
                h.Draw(lin_h, white, 5);
                if (val_a >= val_h)
                {
                    L_output_3.Text = "Camera 3 : In Offside Position!";
                }
                else
                {
                    L_output_3.Text = "Camera 3 : Not in Offside Position!";
                }
            }

            Image<Gray, byte>[] Bhej = new Image<Gray,byte>[2];
            Bhej[0] = a;
            Bhej[1] = h;
            return Bhej;
        }

        private void configureAlgorithm()
        {
            if (CB_segmenthome.Checked == true && CB_segmentaway.Checked == true)
            {
                CB_Noise.Enabled = true;
            }
            else
            {
                CB_Noise.Enabled = false;
                CB_Noise.Checked = false;
            }
            
            if (CB_Noise.Checked == true)
            {
                CB_spotaway.Enabled = true;
                CB_spothome.Enabled = true;
            }
            else
            {
                CB_spotaway.Enabled = false;
                CB_spothome.Enabled = false;
                CB_spotaway.Checked = false;
                CB_spothome.Checked = false;
            }

            if (CB_spothome.Checked == true && CB_spotaway.Checked == true)
            {
                CB_compare.Enabled = true;
                
            }
            else
            {
                CB_compare.Enabled = false;
                CB_compare.Checked = false;
            }

        }

        private void CB_segmenthome_CheckedChanged(object sender, EventArgs e)
        {
            configureAlgorithm();
        }

        private void CB_segmentaway_CheckedChanged(object sender, EventArgs e)
        {
            configureAlgorithm();
        }

        private void CB_Noise_CheckedChanged(object sender, EventArgs e)
        {
            configureAlgorithm();
        }

        private void CB_spothome_CheckedChanged(object sender, EventArgs e)
        {
            configureAlgorithm();
        }

        private void CB_spotaway_CheckedChanged(object sender, EventArgs e)
        {
            configureAlgorithm();
        }

        private void CB_compare_CheckedChanged(object sender, EventArgs e)
        {
            configureAlgorithm();
        }

        private void B_all_Click(object sender, EventArgs e)
        {
            configureAlgorithm();
        }

        private void B_process_Click(object sender, EventArgs e)
        {
            StaticImage();
        }

        private void PB_input_1_Click(object sender, EventArgs e)
        {

        }

       
    }
}

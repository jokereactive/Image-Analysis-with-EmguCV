using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;


namespace IA_Assignment0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Image<Gray, byte> img1 = new Image<Gray, byte>("cameraman.tif");
            comboBox1.Items.Add("cameraman");
            comboBox1.Items.Add("house");
            comboBox1.Items.Add("jetplane");
            comboBox1.Items.Add("lake");
            comboBox1.Items.Add("lena_color_256");
            comboBox1.Items.Add("lena_color_512");
            comboBox1.Items.Add("lena_gray_256");
            comboBox1.Items.Add("lena_gray_512");
            comboBox1.Items.Add("livingroom");
            comboBox1.Items.Add("mandril_color");
            comboBox1.Items.Add("mandril_gray");
            comboBox1.Items.Add("peppers_color");
            comboBox1.Items.Add("peppers_gray");
            comboBox1.Items.Add("pirate");
            comboBox1.Items.Add("walkbridge");
            comboBox1.Items.Add("woman_blonde");
            comboBox1.Items.Add("woman_darkhair");
            
            pictureBox1.Image = img1.ToBitmap();
            comboBox1.SelectedIndex = comboBox1.FindString("cameraman");

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /*
         * Inversion Code 
         * Use Picture Box 1 as input. Picture Box 4 as Output.
        */
        private void button1_Click(object sender, EventArgs e)
        {
            Image<Gray, byte> img1 = new Image<Gray, byte>(comboBox1.SelectedItem.ToString() + ".tif"); 
            Image<Gray, byte> result = new Image<Gray, byte>(img1.Size);

            for (int i = 0; i < img1.Height; i++)
            {
                for (int j = 0; j < img1.Width; j++)
                {
                    result.Data[i, j, 0] = (byte)(255-img1.Data[i, j, 0]);
                }
            }

            pictureBox4.Image = result.ToBitmap();
       

        }

        /*
         * Gamma Code 
         * Use Edit Box: Text Box 2 for c, Text box 3 for gamma. 
         * Use Picture Box 1 as input. Picture Box 4 as Output.
        */
        private void button2_Click(object sender, EventArgs e)
        {
            Image<Gray, byte> img1 = new Image<Gray, byte>(comboBox1.SelectedItem.ToString() + ".tif");
            Image<Gray, byte> result = new Image<Gray, byte>(img1.Size);

            for (int i = 0; i < img1.Height; i++)
            {
                for (int j = 0; j < img1.Width; j++)
                {

                    float c = 0;
                    if (textBox2.Text != "")
                    {
                        c = float.Parse(textBox2.Text);
                    }
                    float gamma = 0;
                    if (textBox3.Text != "")
                    {
                        gamma = float.Parse(textBox3.Text);
                    }
                    result.Data[i, j, 0] = (byte)((c*(Math.Pow((img1.Data[i, j, 0]/255.0),gamma)))*255);
                }
            }

            pictureBox4.Image = result.ToBitmap();
       
         
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Image<Gray, byte> img1 = new Image<Gray, byte>(comboBox1.SelectedItem.ToString() + ".tif"); ;
            pictureBox1.Image = img1.ToBitmap();
            zedGraphControl1.Hide();
          }

        /*
         * Blur Code  
         * Use Picture Box 1 as input. Picture Box 4 as Output.
        */
        private void button3_Click(object sender, EventArgs e)
        {
            Image<Gray, byte> img1 = new Image<Gray, byte>(comboBox1.SelectedItem.ToString() + ".tif");
            Image<Gray, byte> result = new Image<Gray, byte>(img1.Size);
            for (int i = 1; i < img1.Height-1; i++)
            {
                for (int j = 1; j < img1.Width-1; j++)
                {
                    result.Data[i, j, 0] = (byte)((img1.Data[i+1, j, 0]+img1.Data[i+1, j+1, 0]+img1.Data[i+1, j-1, 0]+img1.Data[i-1, j, 0]+img1.Data[i-1, j+1, 0]+img1.Data[i-1, j-1, 0]+img1.Data[i, j, 0]+img1.Data[i, j+1, 0]+img1.Data[i, j-1, 0]) / 9);
                }
            }
            pictureBox4.Image = result.ToBitmap();
        }
        /*
         * Sharpen Code 
         * Use Picture Box 1 as input. Picture Box 4 as Output.
        */
        private void button4_Click(object sender, EventArgs e)
        {
            Image<Gray, byte> img1 = new Image<Gray, byte>(comboBox1.SelectedItem.ToString() + ".tif");
            Image<Gray, byte> result = new Image<Gray, byte>(img1.Size);
            for (int i = 1; i < img1.Height - 1; i++)
            {
                for (int j = 1; j < img1.Width - 1; j++)
                {
                    result.Data[i, j, 0] = (byte)((img1.Data[i, j, 0] * 5 - img1.Data[i + 1, j, 0] - img1.Data[i - 1, j, 0] - img1.Data[i, j + 1, 0] - img1.Data[i, j - 1, 0]));
                }
            }
            pictureBox4.Image = result.ToBitmap();

        }

        /*
         * Histogram Plot 
         * Use Picture Box 1 as input. Picture Box 3 as Output.
        */
        private void button5_Click(object sender, EventArgs e)
        {
            zedGraphControl1.Show();
            zedGraphControl1.Refresh();
            zedGraphControl1.Invalidate();
            zedGraphControl1.GraphPane.CurveList.Clear();
            zedGraphControl1.GraphPane.GraphObjList.Clear();
            int[] a=new int[256];
            for(int i=0;i<256;i++){
                a[i]=0;
            }
            Image<Gray, byte> img1 = new Image<Gray, byte>(comboBox1.SelectedItem.ToString() + ".tif");
            for (int i = 0; i < img1.Height; i++)
            {
                for (int j = 0; j < img1.Width; j++)
                {
                    a[img1.Data[i, j, 0]]++;
                }
            }
            GraphPane myPane = new GraphPane();
            PointPairList listPointsOne = new PointPairList();
            myPane = zedGraphControl1.GraphPane;
            zedGraphControl1.IsEnableVZoom = false;
            zedGraphControl1.IsEnableVPan = false;
            myPane.Title.Text = "Histogram Plot";
            myPane.XAxis.Title.Text = "Pixel Value";
            myPane.YAxis.Title.Text = "Frequency of Value";
            for (int i = 0; i < 256; i++)
            {
                listPointsOne.Add(i, a[i]);
            }
            BarItem myBar = myPane.AddBar("Plot Histogram", listPointsOne, Color.Red);
            zedGraphControl1.AxisChange(); 
            
        }


        /*
         * Scale Code 
         * Use Picture Box 1 as input. Picture Box 2 as Output. Text Box 1 for x and Text Box 7 for y
        */
        private void button8_Click(object sender, EventArgs e)
        {
            Image<Gray, byte> img1 = new Image<Gray, byte>(comboBox1.SelectedItem.ToString() + ".tif");
            Image<Gray, byte> result = new Image<Gray, byte>(img1.Size);
            for (int i = 0; i < img1.Height; i++)
            {
                for (int j = 0; j < img1.Width; j++)
                {
                    float dx=float.Parse(textBox1.Text);
                    float dy=float.Parse(textBox7.Text);
                    int xn;
                    int yn;
                    xn=Convert.ToInt16(j/dx);
                    yn=Convert.ToInt16(i/dy);
                    if(xn<img1.Width && yn<img1.Height){
                        result.Data[j, i, 0] = img1.Data[xn,yn,0];        
                    }
                }
            }
            pictureBox2.Image = result.ToBitmap();
        }


        /*
         * Translate Code 
         * Use Picture Box 1 as input. Picture Box 2 as Output. Text Box 4 for x and Text Box 5 for y
        */
        private void button6_Click(object sender, EventArgs e)
        {
            Image<Gray, byte> img1 = new Image<Gray, byte>(comboBox1.SelectedItem.ToString() + ".tif");
            Image<Gray, byte> result = new Image<Gray, byte>(img1.Size);
            for (int i = 0; i < img1.Height; i++)
            {
                for (int j = 0; j < img1.Width; j++)
                {
                    int dx = int.Parse(textBox4.Text);
                    int dy = int.Parse(textBox5.Text);
                    int xn;
                    int yn;
                    xn = j + dx;
                    yn = i + dy;
                    if (xn < img1.Width && yn < img1.Height)
                    {
                        result.Data[xn, yn, 0] = img1.Data[j, i, 0];
                    }
                }
            }
            pictureBox2.Image = result.ToBitmap();
        }

        /*
        * Rotate Code 
        * Use Picture Box 1 as input. Picture Box 2 as Output. Text Box 6 for Theta
       */
        private void button7_Click(object sender, EventArgs e)
        {
            Image<Gray, byte> img1 = new Image<Gray, byte>(comboBox1.SelectedItem.ToString() + ".tif");
            Image<Gray, byte> result = new Image<Gray, byte>(img1.Width*4,img1.Height*4);
            for (int i = 0; i < img1.Height; i++)
            {
                for (int j = 0; j < img1.Width; j++)
                {
                    int theta = int.Parse(textBox6.Text);
                    double radian = (Math.PI * theta) / 180;
                    int xn;
                    int yn;
                    xn = Convert.ToInt32(i * Math.Cos(radian) - j * Math.Sin(radian));
                    yn = Convert.ToInt32(i * Math.Sin(radian) + j * Math.Cos(radian));
                    //if (xn < img1.Width && yn < img1.Height && xn > 0 && yn > 0)
                    {
                        result.Data[img1.Width * 2 + xn, img1.Height * 2 + yn, 0] = img1.Data[i, j, 0];
                    }
                }
            }
                       
            pictureBox2.Image = result.ToBitmap();
        }

        /*
         * Affine Transformation
         * Use Picture Box 1 as input. Picture Box 2 as Output. 
         * T1=Text Box 8
         * T2=Text Box 9
         * T3=Text Box 13
         * T4=Text Box 12
         * T5=Text Box 11
         * T6=Text Box 10
        */
        private void button9_Click(object sender, EventArgs e)
        {

            float t11, t12, t21, t22, t31, t32;
            Image<Gray, byte> img1 = new Image<Gray, byte>(comboBox1.SelectedItem.ToString() + ".tif");
            Image<Gray, byte> result = new Image<Gray, byte>(img1.Size);
            for (int i = 0; i < img1.Height; i++)
            {
                for (int j = 0; j < img1.Width; j++)
                {
                    t11 = float.Parse(textBox8.Text);
                    t12 = float.Parse(textBox9.Text);
                    t21 = float.Parse(textBox13.Text);
                    t22 = float.Parse(textBox12.Text);
                    t31 = float.Parse(textBox11.Text);
                    t32 = float.Parse(textBox10.Text);
                    int xn;
                    int yn;
                    xn =  Convert.ToInt16(j*t11 + i*t21 +t31);
                    yn =  Convert.ToInt16(j*t12 + i*t22 +t32);
                    xn = xn < 0 ? 0 : xn;
                    yn = yn < 0 ? 0 : yn;
                    xn = xn > 255 ? 255 : xn;
                    yn = yn > 255 ? 255 : yn;
                    if (xn < img1.Width && yn < img1.Height)
                    {
                        result.Data[xn, yn, 0] = img1.Data[j, i, 0];
                    }
                }
            }
            pictureBox2.Image = result.ToBitmap();
        }

  
        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

       

      

  
    }
}

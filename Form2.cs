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
    public partial class Form2 : Form
    {
        public Form2()
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
            comboBox2.Items.Add("3");
            comboBox2.Items.Add("5");
            comboBox2.Items.Add("11");
            comboBox3.Items.Add("IH");
            comboBox3.Items.Add("BH");
            comboBox3.Items.Add("GH");
            comboBox3.Items.Add("IL");
            comboBox3.Items.Add("BL");
            comboBox3.Items.Add("GL");

            pictureBox1.Image = img1.ToBitmap();
            comboBox1.SelectedIndex = comboBox1.FindString("cameraman");

        }
        /*
         * Function to Convulate a small matrix over an image
         */
        private Emgu.CV.Image<Gray,byte> convulate(Emgu.CV.Image<Gray,byte> img1, Matrix<int> m,int den)
        {
            Image<Gray, byte> result = new Image<Gray, byte>(img1.Size);
            int size = m.Rows;
            for (int i = 0; i < img1.Height; i++)
            {
                for (int j = 0; j < img1.Width; j++)
                {
                    float sum = (float)0.0;
                    for (int h = (0 - size / 2); h <= (0 + size / 2); h++)
                    {
                        for (int g = (0 - size / 2); g <= (0 + size / 2); g++)
                        {
                            int x, y;
                            if (i + h < 0)
                            {
                                x = 0;
                            }
                            else if(i + h >= img1.Height)
                            {
                                x = img1.Height-1;
                            }            
                            else
                            {
                                x = i + h;
                            }
                            if (j + g < 0)
                            {
                                y = 0;
                            }
                            else if( j + g >= img1.Width)
                            {
                                y = img1.Width - 1;
                            }
                            else
                            {
                                y = j + g;
                            }
                            sum += (float) img1.Data[x, y, 0] * (float) m[(h + (size / 2)), (g + (size / 2))];
                            //MessageBox.Show(sum.ToString());
                        }
                    }
                    float put = (float) (((float)sum / (float)(den)));
                    //MessageBox.Show(put.ToString());
                    float put1 =(float) (put > 255 ? 255 : put);
                    put1 = put < 0 ? 0 : put;
                    put1 = (int)put1;
                    result.Data[i, j, 0] = (byte)((int)put1);
                }
            }
            return result;
        }

        private Emgu.CV.Image<Gray, float> convulatef(Emgu.CV.Image<Gray, float> img1, Matrix<float> m, int den)
        {
            Image<Gray, float> result = new Image<Gray, float>(img1.Size);
            int size = m.Rows;
            for (int i = 0; i < img1.Height; i++)
            {
                for (int j = 0; j < img1.Width; j++)
                {
                    float sum = (float)0.0;
                    for (int h = (0 - size / 2); h <= (0 + size / 2); h++)
                    {
                        for (int g = (0 - size / 2); g <= (0 + size / 2); g++)
                        {
                            int x, y;
                            if (i + h < 0)
                            {
                                x = 0;
                            }
                            else if (i + h >= img1.Height)
                            {
                                x = img1.Height - 1;
                            }
                            else
                            {
                                x = i + h;
                            }
                            if (j + g < 0)
                            {
                                y = 0;
                            }
                            else if (j + g >= img1.Width)
                            {
                                y = img1.Width - 1;
                            }
                            else
                            {
                                y = j + g;
                            }
                            sum += (float)img1.Data[x, y, 0] * (float)m[(h + (size / 2)), (g + (size / 2))];
                            //MessageBox.Show(sum.ToString());
                        }
                    }
                    float put = (float)(((float)sum / (float)(den)));
                    //MessageBox.Show(put.ToString());
                    float put1 = (float)(put > 255 ? 255 : put);
                    put1 = put < 0 ? 0 : put;
                    put1 = (int)put1;
                    result.Data[i, j, 0] = (byte)((int)put1);
                }
            }
            return result;
        }


        private void Form2_Load(object sender, EventArgs e)
        {

        }

        /*
         * Averagin Filter 
         * Use Picture Box 1, ComboBox2 as input. Picture Box 4 as Output.
        */
        private void button1_Click_1(object sender, EventArgs e)
        {
            Image<Gray, byte> img1 = new Image<Gray, byte>(comboBox1.SelectedItem.ToString() + ".tif");
            Image<Gray, byte> result = new Image<Gray, byte>(img1.Size);
            int size = int.Parse(comboBox2.Text);
            Matrix<int> matrix = new Matrix<int>(size, size);
            matrix.SetValue(1);
            pictureBox4.Image = convulate(img1,matrix,size*size).ToBitmap();
        }

        /*
         * Spatial Gaussian Filter
         * Use Picture Box 1 as input. Picture Box 4 as Output.
        */
        private void button2_Click_1(object sender, EventArgs e)
        {
            Image<Gray, byte> img1 = new Image<Gray, byte>(comboBox1.SelectedItem.ToString() + ".tif");
            Image<Gray, byte> result = new Image<Gray, byte>(img1.Size);
            int size = 3;
            Matrix<int> matrix = new Matrix<int>(size, size);
            matrix[0,0]=1;
            matrix[0, 1] = 2;
            matrix[0, 2] = 1;
            matrix[1, 0] = 2;
            matrix[1, 1] = 4;
            matrix[1, 2] = 2;
            matrix[2, 0] = 1;
            matrix[2, 1] = 2;
            matrix[2, 2] = 1;
            pictureBox4.Image = convulate(img1, matrix, 16).ToBitmap();
            
        }

        /*
         * Gradient Filter  
         * Use Picture Box 1 as input. Picture Box 4 as Output.
        */
        private void button3_Click_1(object sender, EventArgs e)
        {
            Image<Gray, byte> img1 = new Image<Gray, byte>(comboBox1.SelectedItem.ToString() + ".tif");
            Image<Gray, byte> result1 = new Image<Gray, byte>(img1.Size);
            Image<Gray, byte> result2 = new Image<Gray, byte>(img1.Size);
            Image<Gray, byte> result = new Image<Gray, byte>(img1.Size);
            int size = 3;
            Matrix<int> matrix = new Matrix<int>(size, size);
            /*
             *Matrix for x axis 
             */
            matrix[0, 0] = -1;
            matrix[0, 1] = 0;
            matrix[0, 2] = 1;
            matrix[1, 0] = -2;
            matrix[1, 1] = 0;
            matrix[1, 2] = 2;
            matrix[2, 0] = -1;
            matrix[2, 1] = 0;
            matrix[2, 2] = 1;
            result1 = convulate(img1, matrix, 1);
            
                    
            pictureBox4.Image = result1.ToBitmap();
            
            /*
             *Matrix for y axis 
             */
            matrix[0, 0] = 1;
            matrix[0, 1] = 2;
            matrix[0, 2] = 1;
            matrix[1, 0] = 0;
            matrix[1, 1] = 0;
            matrix[1, 2] = 0;
            matrix[2, 0] = -1;
            matrix[2, 1] = -2;
            matrix[2, 2] = -1;
            result2 = convulate(img1, matrix, 1);

            for (int i = 0; i < img1.Height; i++)
            {
                for (int j = 0; j < img1.Width; j++)
                {
                    result.Data[i, j, 0] = (byte)((Math.Sqrt(Math.Pow(result1.Data[i, j, 0],2) + Math.Pow(result2.Data[i, j, 0],2)))>255?255:(Math.Sqrt(Math.Pow(result1.Data[i, j, 0],2) + Math.Pow(result2.Data[i, j, 0],2))));
                }
            }
            pictureBox4.Image = result.ToBitmap();
        }
        /*
         * Laplacian Filter
         * Use Picture Box 1 as input. Picture Box 4 as Output.
        */
        private void button4_Click_1(object sender, EventArgs e)
        {
            Image<Gray, byte> img1 = new Image<Gray, byte>(comboBox1.SelectedItem.ToString() + ".tif");
            Image<Gray, byte> result = new Image<Gray, byte>(img1.Size);
            int size = 3;
            Matrix<int> matrix = new Matrix<int>(size, size);
            matrix[0, 0] = 0;
            matrix[0, 1] = 1;
            matrix[0, 2] = 0;
            matrix[1, 0] = 1;
            matrix[1, 1] = -4;
            matrix[1, 2] = 1;
            matrix[2, 0] = 0;
            matrix[2, 1] = 1;
            matrix[2, 2] = 0;
            result = convulate(img1, matrix, 1);
            pictureBox4.Image = result.ToBitmap();
        }

        /*
         * Sharpen Image
         * Use Picture Box 1 as input. Picture Box 3 as Output.
        */
        private void button5_Click(object sender, EventArgs e)
        {
            Image<Gray, byte> img1 = new Image<Gray, byte>(comboBox1.SelectedItem.ToString() + ".tif");
            Image<Gray, byte> result = new Image<Gray, byte>(img1.Size);
            int size = 3;
            Matrix<int> matrix = new Matrix<int>(size, size);
            matrix[0, 0] = 0;
            matrix[0, 1] = 1;
            matrix[0, 2] = 0;
            matrix[1, 0] = 1;
            matrix[1, 1] = -4;
            matrix[1, 2] = 1;
            matrix[2, 0] = 0;
            matrix[2, 1] = 1;
            matrix[2, 2] = 0;
            result = convulate(img1, matrix, 1);
            for (int i = 0; i < img1.Height; i++)
            {
                for (int j = 0; j < img1.Width; j++)
                {
                    result.Data[i, j, 0] = (byte)(-result.Data[i, j, 0]+img1.Data[i,j,0]);
                }
            }
            pictureBox4.Image = result.ToBitmap();
        }

        /*
         * FFT
         * Use Picture Box 1 as input. Picture Box 3 as Output.
         * Code help taken from http://stackoverflow.com/questions/16812950/how-do-i-compute-dft-and-its-reverse-with-emgu , http://stackoverflow.com/questions/7628864/fourier-transform-emgucv
         * */

        private void button10_Click(object sender, EventArgs e)
        {
            Image<Gray, float> image = new Image<Gray, float>(comboBox1.SelectedItem.ToString() + ".tif");
            
            IntPtr complexImage = CvInvoke.cvCreateImage(image.Size, Emgu.CV.CvEnum.IPL_DEPTH.IPL_DEPTH_32F, 2);

            CvInvoke.cvSetZero(complexImage); 
            CvInvoke.cvSetImageCOI(complexImage, 1);
            CvInvoke.cvCopy(image, complexImage, IntPtr.Zero);
            CvInvoke.cvSetImageCOI(complexImage, 0);

            Matrix<float> dft = new Matrix<float>(image.Rows, image.Cols, 2);
            CvInvoke.cvDFT(complexImage, dft, Emgu.CV.CvEnum.CV_DXT.CV_DXT_FORWARD, 0);

            
            Matrix<float> outReal = new Matrix<float>(image.Size);
            
            Matrix<float> outIm = new Matrix<float>(image.Size);
            CvInvoke.cvSplit(dft, outReal, outIm, IntPtr.Zero, IntPtr.Zero);

            CvInvoke.cvPow(outReal, outReal, 2.0);
            CvInvoke.cvPow(outIm, outIm, 2.0);

            CvInvoke.cvAdd(outReal, outIm, outReal, IntPtr.Zero);
            CvInvoke.cvPow(outReal, outReal, 0.5);

            CvInvoke.cvAddS(outReal, new MCvScalar(1.0), outReal, IntPtr.Zero); // 1 + Mag
            CvInvoke.cvLog(outReal, outReal); // log(1 + Mag)

            
            int x = outReal.Cols / 2;
            int y = outReal.Rows / 2;

            Matrix<float> q0 = outReal.GetSubRect(new Rectangle(0, 0, x, y));
            Matrix<float> q1 = outReal.GetSubRect(new Rectangle(x, 0, x, y));
            Matrix<float> q2 = outReal.GetSubRect(new Rectangle(0, y, x, y));
            Matrix<float> q3 = outReal.GetSubRect(new Rectangle(x, y, x, y));
            Matrix<float> tmp = new Matrix<float>(q0.Size);

            q0.CopyTo(tmp);
            q3.CopyTo(q0);
            tmp.CopyTo(q3);
            q1.CopyTo(tmp);
            q2.CopyTo(q1);
            tmp.CopyTo(q2);

            CvInvoke.cvNormalize(outReal, outReal, 0.0, 255.0, Emgu.CV.CvEnum.NORM_TYPE.CV_MINMAX, IntPtr.Zero);

            Image<Gray, float> fftImage = new Image<Gray, float>(outReal.Size);
            CvInvoke.cvCopy(outReal, fftImage, IntPtr.Zero);

            //pictureBox1.Image = image.ToBitmap();
            pictureBox4.Image = fftImage.ToBitmap();

            Image<Gray, float> temp = new Image<Gray, float>(dft.Size);
            CvInvoke.cvDFT(dft, temp, Emgu.CV.CvEnum.CV_DXT.CV_DXT_INVERSE, 0);
            pictureBox7.Image = temp.ToBitmap();
            
        }


        Matrix<float> GLP(Image<Gray, float> image, Matrix<float> forwardDft, int d0)
        {
            Matrix<float> filter = new Matrix<float>(image.Rows, image.Cols, 2);
            Matrix<float> filteredimage = new Matrix<float>(image.Rows, image.Cols, 2);
            for (int i = 0; i < filter.Rows; i++)
            {
                for (int j = 0; j < filter.Cols; j++)
                {
                    filter[i, j] = (float)Math.Pow(Math.E, -1 * (Math.Pow(value(i, j, filter), 2) / (2 * (float)Math.Pow(d0, 2))));
                }
            }
            int x = filter.Cols / 2;
            int y = filter.Rows / 2;

            Matrix<float> q0 = filter.GetSubRect(new Rectangle(0, 0, x, y));
            Matrix<float> q1 = filter.GetSubRect(new Rectangle(x, 0, x, y));
            Matrix<float> q2 = filter.GetSubRect(new Rectangle(0, y, x, y));
            Matrix<float> q3 = filter.GetSubRect(new Rectangle(x, y, x, y));
            Matrix<float> tmp = new Matrix<float>(q0.Rows, q0.Cols, 2);

            q0.CopyTo(tmp);
            q3.CopyTo(q0);
            tmp.CopyTo(q3);
            q1.CopyTo(tmp);
            q2.CopyTo(q1);
            tmp.CopyTo(q2);

            CvInvoke.cvMulSpectrums(forwardDft, filter, filteredimage, 0);
            
            return filteredimage;
        }
        Matrix<float> GHP(Image<Gray, float> image, Matrix<float> forwardDft, int d0)
        {
            Matrix<float> filter = new Matrix<float>(image.Rows, image.Cols, 2);
            Matrix<float> filteredimage = new Matrix<float>(image.Rows, image.Cols, 2);
            for (int i = 0; i < filter.Rows; i++)
            {
                for (int j = 0; j < filter.Cols; j++)
                {
                    filter[i, j] = 1 - (float)Math.Pow(Math.E, -1 * (Math.Pow(value(i, j, filter), 2) / (2 * (float)Math.Pow(d0, 2))));
                }
            }
            int x = filter.Cols / 2;
            int y = filter.Rows / 2;

            Matrix<float> q0 = filter.GetSubRect(new Rectangle(0, 0, x, y));
            Matrix<float> q1 = filter.GetSubRect(new Rectangle(x, 0, x, y));
            Matrix<float> q2 = filter.GetSubRect(new Rectangle(0, y, x, y));
            Matrix<float> q3 = filter.GetSubRect(new Rectangle(x, y, x, y));
            Matrix<float> tmp = new Matrix<float>(q0.Rows, q0.Cols, 2);

            q0.CopyTo(tmp);
            q3.CopyTo(q0);
            tmp.CopyTo(q3);
            q1.CopyTo(tmp);
            q2.CopyTo(q1);
            tmp.CopyTo(q2);

            CvInvoke.cvMulSpectrums(forwardDft, filter, filteredimage, 0);
           
            return filteredimage;
        }
        Matrix<float> ILP(Image<Gray, float> image, Matrix<float> forwardDft, int d0)
        {
            Matrix<float> filter = new Matrix<float>(image.Rows, image.Cols, 2);
            Matrix<float> filteredimage = new Matrix<float>(image.Rows, image.Cols, 2);
            for (int i = 0; i < filter.Rows; i++)
            {
                for (int j = 0; j < filter.Cols; j++)
                {
                    if (value(i, j, filter) <= d0)
                    {
                        filter[i, j] = 1;
                    }
                    else
                    {
                        filter[i, j] = 0;
                    }
                }
            }
            int x = filter.Cols / 2;
            int y = filter.Rows / 2;

            Matrix<float> q0 = filter.GetSubRect(new Rectangle(0, 0, x, y));
            Matrix<float> q1 = filter.GetSubRect(new Rectangle(x, 0, x, y));
            Matrix<float> q2 = filter.GetSubRect(new Rectangle(0, y, x, y));
            Matrix<float> q3 = filter.GetSubRect(new Rectangle(x, y, x, y));
            Matrix<float> tmp = new Matrix<float>(q0.Rows, q0.Cols, 2);

            q0.CopyTo(tmp);
            q3.CopyTo(q0);
            tmp.CopyTo(q3);
            q1.CopyTo(tmp);
            q2.CopyTo(q1);
            tmp.CopyTo(q2);

            CvInvoke.cvMulSpectrums(forwardDft, filter, filteredimage, 0);
            
            return filteredimage;
        }

        Matrix<float> BLP(Image<Gray, float> image, Matrix<float> forwardDft, int d0, int n)
        {
            Matrix<float> filter = new Matrix<float>(image.Rows, image.Cols, 2);
            Matrix<float> filteredimage = new Matrix<float>(image.Rows, image.Cols, 2);
            for (int i = 0; i < filter.Rows; i++)
            {
                for (int j = 0; j < filter.Cols; j++)
                {
                    filter[i, j] = 1 / (float)(1 + (Math.Pow(value(i, j, filter) / d0, 2 * n)));
                }
            }

            int x = filter.Cols / 2;
            int y = filter.Rows / 2;

            Matrix<float> q0 = filter.GetSubRect(new Rectangle(0, 0, x, y));
            Matrix<float> q1 = filter.GetSubRect(new Rectangle(x, 0, x, y));
            Matrix<float> q2 = filter.GetSubRect(new Rectangle(0, y, x, y));
            Matrix<float> q3 = filter.GetSubRect(new Rectangle(x, y, x, y));
            Matrix<float> tmp = new Matrix<float>(q0.Rows, q0.Cols, 2);

            q0.CopyTo(tmp);
            q3.CopyTo(q0);
            tmp.CopyTo(q3);
            q1.CopyTo(tmp);
            q2.CopyTo(q1);
            tmp.CopyTo(q2);

            CvInvoke.cvMulSpectrums(forwardDft, filter, filteredimage, 0);

            return filteredimage;
        }

        Matrix<float> BHP(Image<Gray, float> image, Matrix<float> forwardDft, int d0, int n)
        {
            Matrix<float> filter = new Matrix<float>(image.Rows, image.Cols, 2);
            Matrix<float> filteredimage = new Matrix<float>(image.Rows, image.Cols, 2);
            for (int i = 0; i < filter.Rows; i++)
            {
                for (int j = 0; j < filter.Cols; j++)
                {
                    filter[i, j] = 1 / (float)(1 + (Math.Pow(d0 / value(i, j, filter), 2 * n)));
                }
            }
            int x = filter.Cols / 2;
            int y = filter.Rows / 2;

            Matrix<float> q0 = filter.GetSubRect(new Rectangle(0, 0, x, y));
            Matrix<float> q1 = filter.GetSubRect(new Rectangle(x, 0, x, y));
            Matrix<float> q2 = filter.GetSubRect(new Rectangle(0, y, x, y));
            Matrix<float> q3 = filter.GetSubRect(new Rectangle(x, y, x, y));
            Matrix<float> tmp = new Matrix<float>(q0.Rows, q0.Cols, 2);

            q0.CopyTo(tmp);
            q3.CopyTo(q0);
            tmp.CopyTo(q3);
            q1.CopyTo(tmp);
            q2.CopyTo(q1);
            tmp.CopyTo(q2);

            CvInvoke.cvMulSpectrums(forwardDft, filter, filteredimage, 0);
            
            return filteredimage;
        }
        Matrix<float> IHP(Image<Gray, float> image, Matrix<float> forwardDft, int d0)
        {
            Matrix<float> filter = new Matrix<float>(image.Rows, image.Cols, 2);
            
            Matrix<float> filteredimage = new Matrix<float>(image.Rows, image.Cols, 2);
            for (int i = 0; i < filter.Rows; i++)
            {
                for (int j = 0; j < filter.Cols; j++)
                {
                    if (value(i, j, filter) >= d0)
                    {
                        filter[i, j] = 1;
                    }
                    else
                    {
                        filter[i, j] = 0;
                    }
                }
            }
            
            int x = filter.Cols / 2;
            int y = filter.Rows / 2;

            Matrix<float> q0 = filter.GetSubRect(new Rectangle(0, 0, x, y));
            Matrix<float> q1 = filter.GetSubRect(new Rectangle(x, 0, x, y));
            Matrix<float> q2 = filter.GetSubRect(new Rectangle(0, y, x, y));
            Matrix<float> q3 = filter.GetSubRect(new Rectangle(x, y, x, y));
            Matrix<float> tmp = new Matrix<float>(q0.Rows,q0.Cols,2);

            q0.CopyTo(tmp);
            q3.CopyTo(q0);
            tmp.CopyTo(q3);
            q1.CopyTo(tmp);
            q2.CopyTo(q1);
            tmp.CopyTo(q2);

            
            CvInvoke.cvMulSpectrums(forwardDft, filter, filteredimage, 0);
            
            return filteredimage;
        }
        
        double value(int i, int j, Matrix<float> filter)
        {
            return Math.Pow(Math.Pow(i - (filter.Rows / 2.0), 2) + Math.Pow(j - (filter.Cols / 2.0), 2), 1 / 2.0);
        }
        /*
         * FFT Filter
         * Use Picture Box 1 as input, Combo Box 3 as input. Picture Box 3 and 6 as Output.
         * 
         */
        private void button11_Click(object sender, EventArgs e)
        {
            Image<Gray, float> image = new Image<Gray, float>(comboBox1.SelectedItem.ToString() + ".tif");

            IntPtr complexImage = CvInvoke.cvCreateImage(image.Size, Emgu.CV.CvEnum.IPL_DEPTH.IPL_DEPTH_32F, 2);

            CvInvoke.cvSetZero(complexImage);  // Initialize all elements to Zero
            CvInvoke.cvSetImageCOI(complexImage, 1);
            CvInvoke.cvCopy(image, complexImage, IntPtr.Zero);
            CvInvoke.cvSetImageCOI(complexImage, 0);

            Matrix<float> dft = new Matrix<float>(image.Rows, image.Cols, 2);
            CvInvoke.cvDFT(complexImage, dft, Emgu.CV.CvEnum.CV_DXT.CV_DXT_FORWARD, 0);

            int n, d0;
            n = int.Parse(textBox1.Text);
            d0 = int.Parse(textBox2.Text);
            switch(comboBox3.Text){
                case "IH":
                    MessageBox.Show("starting IH");
                    dft= IHP(image, dft, d0);
                    break;
                case "BH": 
                    dft= BHP(image, dft, d0, n);
                    break;
                case "GH": 
                    dft= GHP(image, dft, d0);
                    break;
                case "IL": 
                    dft= ILP(image, dft, d0);
                    break;
                case "BL": 
                    dft= BLP(image, dft, d0,n);
                    break;
                case "GL":
                    dft = GLP(image, dft, d0);
                    break;
            }

            //The Real part of the Fourier Transform
            Matrix<float> outReal = new Matrix<float>(image.Size);
            //The imaginary part of the Fourier Transform
            Matrix<float> outIm = new Matrix<float>(image.Size);
            CvInvoke.cvSplit(dft, outReal, outIm, IntPtr.Zero, IntPtr.Zero);



            CvInvoke.cvPow(outReal, outReal, 2.0);
            CvInvoke.cvPow(outIm, outIm, 2.0);

            CvInvoke.cvAdd(outReal, outIm, outReal, IntPtr.Zero);
            CvInvoke.cvPow(outReal, outReal, 0.5);

            CvInvoke.cvAddS(outReal, new MCvScalar(1.0), outReal, IntPtr.Zero); // 1 + Mag
            CvInvoke.cvLog(outReal, outReal); // log(1 + Mag)

            // Swap quadrants
            int x = outReal.Cols / 2;
            int y = outReal.Rows / 2;

            Matrix<float> q0 = outReal.GetSubRect(new Rectangle(0, 0, x, y));
            Matrix<float> q1 = outReal.GetSubRect(new Rectangle(x, 0, x, y));
            Matrix<float> q2 = outReal.GetSubRect(new Rectangle(0, y, x, y));
            Matrix<float> q3 = outReal.GetSubRect(new Rectangle(x, y, x, y));
            Matrix<float> tmp = new Matrix<float>(q0.Size);

            q0.CopyTo(tmp);
            q3.CopyTo(q0);
            tmp.CopyTo(q3);
            q1.CopyTo(tmp);
            q2.CopyTo(q1);
            tmp.CopyTo(q2);

            CvInvoke.cvNormalize(outReal, outReal, 0.0, 255.0, Emgu.CV.CvEnum.NORM_TYPE.CV_MINMAX, IntPtr.Zero);

            Image<Gray, float> fftImage = new Image<Gray, float>(outReal.Size);
            CvInvoke.cvCopy(outReal, fftImage, IntPtr.Zero);

            //pictureBox1.Image = image.ToBitmap();
            pictureBox4.Image = fftImage.ToBitmap();

            Image<Gray, float> temp = new Image<Gray, float>(dft.Size);
            CvInvoke.cvDFT(dft, temp, Emgu.CV.CvEnum.CV_DXT.CV_DXT_INVERSE, 0);
            pictureBox7.Image = temp.ToBitmap();
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            Image<Gray, byte> img1 = new Image<Gray, byte>(comboBox1.SelectedItem.ToString() + ".tif");
            pictureBox1.Image = img1.ToBitmap();
        }

        /*
         * Laplacian + Sharpen + Sobel Operator 
         */
        private void button6_Click(object sender, EventArgs e)
        {
            Image<Gray, byte> img1 = new Image<Gray, byte>(comboBox1.SelectedItem.ToString() + ".tif");
            Image<Gray, byte> result = new Image<Gray, byte>(img1.Size);
            int size = 3;
            Matrix<int> matrix = new Matrix<int>(size, size);
            matrix[0, 0] = 0;
            matrix[0, 1] = 1;
            matrix[0, 2] = 0;
            matrix[1, 0] = 1;
            matrix[1, 1] = -4;
            matrix[1, 2] = 1;
            matrix[2, 0] = 0;
            matrix[2, 1] = 1;
            matrix[2, 2] = 0;
            result = convulate(img1, matrix, 1);
            for (int i = 0; i < img1.Height; i++)
            {
                for (int j = 0; j < img1.Width; j++)
                {
                    result.Data[i, j, 0] = (byte)(result.Data[i, j, 0] + img1.Data[i, j, 0]);
                }
            }

            Image<Gray, byte> result1 = new Image<Gray, byte>(img1.Size);
            Image<Gray, byte> result2 = new Image<Gray, byte>(img1.Size);
            /*
             *Matrix for x axis 
             */
            matrix[0, 0] = -1;
            matrix[0, 1] = 0;
            matrix[0, 2] = 1;
            matrix[1, 0] = -2;
            matrix[1, 1] = 0;
            matrix[1, 2] = 2;
            matrix[2, 0] = -1;
            matrix[2, 1] = 0;
            matrix[2, 2] = 1;
            result1 = convulate(result, matrix, 1);
            /*
             *Matrix for y axis 
             */
            matrix[0, 0] = 1;
            matrix[0, 1] = 2;
            matrix[0, 2] = 1;
            matrix[1, 0] = 0;
            matrix[1, 1] = 0;
            matrix[1, 2] = 0;
            matrix[2, 0] = -1;
            matrix[2, 1] = -2;
            matrix[2, 2] = -1;
            result2 = convulate(result, matrix, 1);

            for (int i = 0; i < img1.Height; i++)
            {
                for (int j = 0; j < img1.Width; j++)
                {
                    result.Data[i, j, 0] = (byte)((Math.Sqrt(Math.Pow(result1.Data[i, j, 0], 2) + Math.Pow(result2.Data[i, j, 0], 2))) > 255 ? 255 : (Math.Sqrt(Math.Pow(result1.Data[i, j, 0], 2) + Math.Pow(result2.Data[i, j, 0], 2))));
                }
            }
            pictureBox7.Image = result.ToBitmap();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            label8.Show();
            label9.Show();
            Image<Gray, float> image = new Image<Gray, float>("old.jpg");
            pictureBox1.Image = image.ToBitmap();
            IntPtr complexImage = CvInvoke.cvCreateImage(image.Size, Emgu.CV.CvEnum.IPL_DEPTH.IPL_DEPTH_32F, 2);

            CvInvoke.cvSetZero(complexImage);  
            CvInvoke.cvSetImageCOI(complexImage, 1);
            CvInvoke.cvCopy(image, complexImage, IntPtr.Zero);
            CvInvoke.cvSetImageCOI(complexImage, 0);

            Matrix<float> dft = new Matrix<float>(image.Rows, image.Cols, 2);
            CvInvoke.cvDFT(complexImage, dft, Emgu.CV.CvEnum.CV_DXT.CV_DXT_FORWARD, 0);

            int d0=30;
            dft = GLP(image, dft, d0);

           
            Matrix<float> outReal = new Matrix<float>(image.Size);
            
            Matrix<float> outIm = new Matrix<float>(image.Size);
            CvInvoke.cvSplit(dft, outReal, outIm, IntPtr.Zero, IntPtr.Zero);



            CvInvoke.cvPow(outReal, outReal, 2.0);
            CvInvoke.cvPow(outIm, outIm, 2.0);

            CvInvoke.cvAdd(outReal, outIm, outReal, IntPtr.Zero);
            CvInvoke.cvPow(outReal, outReal, 0.5);

            CvInvoke.cvAddS(outReal, new MCvScalar(1.0), outReal, IntPtr.Zero); 
            CvInvoke.cvLog(outReal, outReal); 

            
            int x = outReal.Cols / 2;
            int y = outReal.Rows / 2;

            Matrix<float> q0 = outReal.GetSubRect(new Rectangle(0, 0, x, y));
            Matrix<float> q1 = outReal.GetSubRect(new Rectangle(x, 0, x, y));
            Matrix<float> q2 = outReal.GetSubRect(new Rectangle(0, y, x, y));
            Matrix<float> q3 = outReal.GetSubRect(new Rectangle(x, y, x, y));
            Matrix<float> tmp = new Matrix<float>(q0.Size);

            q0.CopyTo(tmp);
            q3.CopyTo(q0);
            tmp.CopyTo(q3);
            q1.CopyTo(tmp);
            q2.CopyTo(q1);
            tmp.CopyTo(q2);

            CvInvoke.cvNormalize(outReal, outReal, 0.0, 255.0, Emgu.CV.CvEnum.NORM_TYPE.CV_MINMAX, IntPtr.Zero);

            Image<Gray, float> fftImage = new Image<Gray, float>(outReal.Size);
            CvInvoke.cvCopy(outReal, fftImage, IntPtr.Zero);
            Image<Gray, float> temp = new Image<Gray, float>(dft.Size);
            CvInvoke.cvDFT(dft, temp, Emgu.CV.CvEnum.CV_DXT.CV_DXT_INVERSE, 0);
            pictureBox4.Image = temp.ToBitmap();

            Image<Gray, float> result = new Image<Gray, float>(temp.Size);
            int size = 3;
            Matrix<float> matrix = new Matrix<float>(size, size);
            matrix[0, 0] = 1;
            matrix[0, 1] = 1;
            matrix[0, 2] = 1;
            matrix[1, 0] = 1;
            matrix[1, 1] = -8;
            matrix[1, 2] = 1;
            matrix[2, 0] = 1;
            matrix[2, 1] = 1;
            matrix[2, 2] = 1;
            result = convulatef(temp, matrix, 1);
            for (int i = 0; i < temp.Height; i++)
            {
                for (int j = 0; j < temp.Width; j++)
                {
                    result.Data[i, j, 0] = (float)(-result.Data[i, j, 0] + temp.Data[i, j, 0]);
                }
            }
            pictureBox7.Image = result.ToBitmap();
            //imag1 = result.ToBitmap();
        }

        Image<Gray, byte> imag1 = new Image<Gray, byte>("old.jpg");

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            var se = e as MouseEventArgs;
            Image<Gray, byte> result = new Image<Gray, byte>(imag1.Size);
            result = imag1;
            //MessageBox.Show(result.Data[100,100,0].ToString());
            int size = 5;
            Matrix<int> matrix = new Matrix<int>(size, size);
            matrix.SetValue(1);
            //MessageBox.Show("x:" + se.X.ToString() + "y:" + se.Y.ToString());
                for (int i = (se.X - 10); i < (se.X + 10); i++)
                {
                    for (int j = (se.Y - 10); j < (se.Y + 10); j++)
                    {
                        float sum = (float)0.0;
                        for (int h = (0 - size / 2); h <= (0 + size / 2); h++)
                        {
                            for (int g = (0 - size / 2); g <= (0 + size / 2); g++)
                            {
                                int x=0,y=0;
                                x = i + h;
                                y = j + g;
                                sum += (float)result.Data[x, y, 0] ;
                                //MessageBox.Show(sum.ToString());
                            }
                        }
                        float put = (float)(((float)sum / (float)(size * size)));
                        //MessageBox.Show(put.ToString());
                        float put1 = (float)(put > 255 ? 255 : put);
                        put1 = put < 0 ? 0 : put;
                        put1 = (int)put1;
                        //MessageBox.Show("i:"+i.ToString()+"j:"+j.ToString());
                        result.Data[i, j, 0] = (byte)((int)put1);
                    }
                }
            pictureBox7.Image=result.ToBitmap();
        }


    }
}

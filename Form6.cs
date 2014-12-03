using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using C5;

using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV;
using Microsoft.VisualBasic;
using System.Threading;

namespace IA_Assignment0
{
    public partial class Form6 : Form
    {

        //Image<Gray, float> iB;
        //Image<Gray, float> iB2;
        String s;
        int choice;
        Image<Bgr, float> a;
        Image<Bgr, float> making;
        int x;
        int y;
        float y1;
        float i1;
        DateTime start;
        float q1;
        BackgroundWorker m_oWorker;
        public Form6()
        {
            InitializeComponent();
            s = " ";
            start=DateTime.Now;
            a = new Image<Bgr, float>(50,50);
            making = new Image<Bgr, float>("example.png");
            m_oWorker = new BackgroundWorker();
            x = 0;
            y = 0;
            choice = 3;
            y1 = 0;
            q1 = 0;
            i1 = 0;
            // Create a background worker thread that ReportsProgress &
            // SupportsCancellation
            // Hook up the appropriate events.
            m_oWorker.DoWork += new DoWorkEventHandler(m_oWorker_DoWork);
            m_oWorker.ProgressChanged += new ProgressChangedEventHandler
                    (m_oWorker_ProgressChanged);
            m_oWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler
                    (m_oWorker_RunWorkerCompleted);
            m_oWorker.WorkerReportsProgress = true;
            m_oWorker.WorkerSupportsCancellation = true;
            
        }

        private void Form6_Load(object sender, EventArgs e)
        {
           
            
        }

        private IImage subtractImages(IImage image1, IImage image2)
        {
            Image<Gray, float> i1 = (Image<Gray, float>)image1;
            Image<Gray, float> i2 = (Image<Gray, float>)image2;
            Image<Gray, float> iResult = new Image<Gray, float>(i1.Size);
            for (int i = 0; i < i1.Height; i++)
                for (int j = 0; j < i1.Width; j++)
                    iResult.Data[i, j, 0] = (float)(i1.Data[i, j, 0] - i2.Data[i, j, 0]);
            return iResult;
        }

         List<Region> lRegions;
        int globalX, globalY;
        private void button_Click(object sender, EventArgs e)
        {
            

            
        }

        void MyThreadMethod()
        {
            
            
        }

        private void LiveUpdate()
        {
            float r2 = (float)(y1 + .956 * i1 + .62 * q1) * 255;
            float g1 = (float)(y1 - .272 * i1 - .647 * q1) * 255;
            float b0 = (float)(y1 - 1.108 * i1 + 1.705 * q1) * 255;
            making.Data[x, y, 0] = b0;
            making.Data[x, y, 1] = g1;
            making.Data[x, y, 2] = r2;
        }

        private void setGlobalNormal()
        {
            s = " ";
            a = new Image<Bgr, float>(50, 50);
            making = new Image<Bgr, float>("example.png");
            x = 0;
            y = 0;
            
            y1 = 0;
            q1 = 0;
            i1 = 0;
        }

        private Image<Bgr, float> getColor(float y,float i, float q)
        {
            Matrix<float>[] yiq = new Matrix<float>[3];
            yiq[0] = new Matrix<float>(50, 50);
            yiq[1] = new Matrix<float>(50, 50);
            yiq[2] = new Matrix<float>(50, 50);
            for (int k = 0; k < 50; k++)
            {
                for (int j = 0; j < 50; j++)
                {
                    yiq[0].Data[k, j] = y;
                    yiq[1].Data[k, j] = i;
                    yiq[2].Data[k, j] = q;
                }
            }
            Image<Bgr, float> a = convertYIQToRGB(yiq);
            return a;
        }

        private void writeToFile(Image<Gray, byte> imageConnectedComponents)
        {
            String str = "";
            for (int i = 0; i < imageConnectedComponents.Height; i++)
            {
                for (int j = 0; j < imageConnectedComponents.Width; j++)
                {
                    str += String.Format("{0,2}", imageConnectedComponents.Data[i, j, 0]);
                }
                str += "\n";
            }
            System.IO.File.WriteAllText(@"E:\WriteText.txt", str);
        }

        private Matrix<float> BW()
        {
            Matrix<float> m;
            Image<Gray, float> image = new Image<Gray, float>("example.png");
            m = new Matrix<float>(image.Height, image.Cols);
            for (int i = 0; i < image.Height; i++)
                for (int j = 0; j < image.Width; j++)
                    m.Data[i, j] = image.Data[i, j, 0] / 255;
            return m;
        }

        private Image<Bgr, float> convertYIQToRGB(Matrix<float>[] yiq)
        {
            Image<Bgr, float> image = new Image<Bgr, float>(yiq[0].Cols, yiq[0].Rows);
            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    float Y = yiq[0].Data[i, j];
                    float I = yiq[1].Data[i, j];
                    float Q = yiq[2].Data[i, j];
                    float r = (float)(Y + .956 * I + .62 * Q) * 255;
                    float g = (float)(Y - .272 * I - .647 * Q) * 255;
                    float b = (float)(Y - 1.108 * I + 1.705 * Q) * 255;
                    image.Data[i, j, 2] = r;
                    image.Data[i, j, 1] = g;
                    image.Data[i, j, 0] = b;
                }
            }
            return image;
        }

        private void GetXandY(int x, int y, Image<Gray, int> imageBW, Image<Gray, byte> imageChromaMask, Image<Gray, byte> imageConnectedComponents)
        {
            int rows = imageBW.Rows;
            int cols = imageBW.Cols;
            IPriorityQueue<Node> Q = new IntervalHeap<Node>();
            Node newNode = new Node();
            newNode.x = x;
            newNode.y = y;
            newNode.value = 0;
            IPriorityQueueHandle<Node>[,] h = new IPriorityQueueHandle<Node>[rows, cols];
            Matrix<int> visited = new Matrix<int>(rows, cols);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    visited[i, j] = VISIT_UNVISITED;

            Q.Add(ref h[x, y], newNode);
            //int count = 0;
            //al = new ArrayList<int[]>(3);

            while (Q.Count > 0)
            {
                Node node = Q.DeleteMin();
                if (imageConnectedComponents.Data[node.x, node.y, 0] > 0)// && imageChromaMask.Data[node.x, node.y, 0] < 200)
                {
                    
                    globalX = (int)(lRegions[imageConnectedComponents.Data[node.x, node.y, 0] - 1].center.x);//globalX = node.x;
                    globalY = (int)(lRegions[imageConnectedComponents.Data[node.x, node.y, 0] - 1].center.y);//globalY = node.y;
                    return;
                }
                visited[node.x, node.y] = VISIT_VISITED;
                addSurroundingNodes(ref Q, ref h, node, imageBW, ref visited);
            }
        }

        ArrayList<int> alX,alY,alWeights,alRegs;
        static int VISIT_UNVISITED = 0;
        static int VISIT_RELAXED = 1;
        static int VISIT_VISITED = 2;
        private void GetXandY2(int x, int y, Image<Gray, int> imageBW, Image<Gray, byte> imageChromaMask, Image<Gray, byte> imageConnectedComponents)
        {
            int rows = imageBW.Rows;
            int cols = imageBW.Cols;
            IPriorityQueue<Node> Q = new IntervalHeap<Node>();
            Node newNode = new Node();
            newNode.x = x;
            newNode.y = y;
            newNode.value = 0;
            IPriorityQueueHandle<Node>[,] h = new IPriorityQueueHandle<Node>[rows, cols];
            Matrix<int> visited = new Matrix<int>(rows, cols);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    visited[i, j] = VISIT_UNVISITED;

            Q.Add(ref h[x, y], newNode);
            //int count = 0;
            alX = new ArrayList<int>(lRegions.Count);
            alY = new ArrayList<int>(lRegions.Count);
            alWeights = new ArrayList<int>(lRegions.Count);
            alRegs = new ArrayList<int>(lRegions.Count);

            while (Q.Count > 0)
            {
                Node node = Q.DeleteMin();
                if (imageConnectedComponents.Data[node.x, node.y, 0] > 0)// && imageChromaMask.Data[node.x, node.y, 0] < 200)
                {
                    globalX = (int)(lRegions[imageConnectedComponents.Data[node.x, node.y, 0] - 1].center.x);//globalX = node.x;
                    globalY = (int)(lRegions[imageConnectedComponents.Data[node.x, node.y, 0] - 1].center.y);//globalY = node.y;
                    if (!alRegs.Contains(imageConnectedComponents.Data[node.x, node.y, 0]))
                    {
                        alRegs.Add(imageConnectedComponents.Data[node.x, node.y, 0]);
                        alX.Add(globalX);
                        alY.Add(globalY);
                        alWeights.Add(node.value);
                        if (alRegs.Count == lRegions.Count)
                            return;
                    }
                    //return;
                }
                visited[node.x, node.y] = VISIT_VISITED;
                addSurroundingNodes(ref Q, ref h, node, imageBW, ref visited);
            }
        }

        private void addSurroundingNodes(ref IPriorityQueue<Node> Q, ref IPriorityQueueHandle<Node>[,] h, Node node, Image<Gray, int> imageBW, ref Matrix<int> visited)
        {
            int rows = imageBW.Rows;
            int cols = imageBW.Cols;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                        continue;
                    int x = node.x + i;
                    int y = node.y + j;
                    if (!isInRange(imageBW, x, y))
                        continue;
                    if (visited[x, y] == VISIT_UNVISITED)
                    {
                        Node newNode = new Node();
                        newNode.x = x;
                        newNode.y = y;
                        newNode.value = node.value + Math.Abs(imageBW.Data[x, y, 0] - imageBW.Data[node.x, node.y, 0]);
                        Q.Add(ref h[x, y], newNode);
                        visited[x, y] = VISIT_RELAXED;
                    }
                    else if (visited[x, y] == VISIT_RELAXED)
                    {
                        Node oldNode;
                        Q.Find(h[x, y], out oldNode);
                        int value = node.value + Math.Abs(imageBW.Data[x, y, 0] - imageBW.Data[node.x, node.y, 0]);
                        if (Math.Abs(oldNode.value) > Math.Abs(value))
                        {
                            oldNode.value = value;
                            Q.Replace(h[x, y], oldNode);
                        }
                    }
                }
            }
        }

        private bool isInRange(Image<Gray, int> imageBW, int x, int y)
        {
            if (x < 0 || x >= imageBW.Height)
                return false;
            if (y < 0 || y >= imageBW.Width)
                return false;
            return true;
        }

        private Image<Gray, float> convertMatrixToImage(Matrix<float> matrix)
        {
            Image<Gray, float> image = new Image<Gray, float>(matrix.Cols, matrix.Rows);
            for (int i = 0; i < matrix.Rows; i++)
                for (int j = 0; j < matrix.Cols; j++)
                    image.Data[i, j, 0] = matrix.Data[i, j];
            return image;
        }

        private Image<Gray, byte> convertIToBWImage(Matrix<float> matrix)
        {
            Image<Gray, byte> bwOfI = new Image<Gray, byte>(matrix.Cols, matrix.Rows);
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Cols; j++)
                {
                    bwOfI.Data[i, j, 0] = (byte)((Math.Abs(matrix[i, j]) > .00001) ? 255 : 0);
                }
            }
            return bwOfI;
        }

        private Matrix<float>[] convertRGBToYIQ(Image<Bgr, float> imageMarked)
        {
            //http://en.wikipedia.org/wiki/YIQ
            Matrix<float>[] yiq = new Matrix<float>[3];
            yiq[0] = new Matrix<float>(imageMarked.Rows, imageMarked.Cols);
            yiq[1] = new Matrix<float>(imageMarked.Rows, imageMarked.Cols);
            yiq[2] = new Matrix<float>(imageMarked.Rows, imageMarked.Cols);
            for (int i = 0; i < imageMarked.Height; i++)
            {
                for (int j = 0; j < imageMarked.Width; j++)
                {
                    float r = (float)imageMarked.Data[i, j, 2] / 255;
                    float g = (float)imageMarked.Data[i, j, 1] / 255;
                    float b = (float)imageMarked.Data[i, j, 0] / 255;
                    float Y = (float)(.299 * r + .587 * g + .114 * b);
                    float I = (float)(.596 * r - .275 * g - .321 * b);
                    float Q = (float)(.212 * r - .523 * g + .311 * b);
                    I = (float)Math.Round(I * 10000); I /= 10000;
                    Q = (float)Math.Round(Q * 10000); Q /= 10000;
                    yiq[0][i, j] = Y;
                    yiq[1][i, j] = I;
                    yiq[2][i, j] = Q;
                }
            }
            return yiq;
        }

        public Image<Gray, byte> LabelConnectedComponents(Image<Gray, byte> binary, int startLabel)
        {
            //http://stackoverflow.com/questions/22301008/connected-component-labeling-in-emgu-opencv
            Contour<Point> contours = binary.FindContours(
                Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_NONE,
                Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_CCOMP);

            //MessageBox.Show("Contours found");

            int count = startLabel;
            for (Contour<Point> cont = contours;
                        cont != null;
                        cont = cont.HNext)
            {
                CvInvoke.cvDrawContours(
                binary,
                cont,
                new MCvScalar(count),
                new MCvScalar(0),
                2,
                -1,
                Emgu.CV.CvEnum.LINE_TYPE.EIGHT_CONNECTED,
                new Point(0, 0));
                ++count;
            }

            for (int i=0; i < binary.Height; i++)
            {
                for (int j=0; j < binary.Width; j++)
                {
                    if (binary.Data[i,j,0] > 8)
                    {
                        
                        binary.Data[i,j,0] = 0;
                    }
                }
            }

                //writeToFile(binary);
                //MessageBox.Show("Count of components"+count.ToString());
            return binary;
        }

        public Image<Gray, float> QuantizeImage(Image<Gray, float> imageToBeCorrected, Image<Gray, byte> imageMask)
        {
            //http://stackoverflow.com/questions/22301008/connected-component-labeling-in-emgu-opencv
            int startLabel = 1;
            Contour<Point> contours = imageMask.FindContours(
                Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_NONE,
                Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_CCOMP);

            //MessageBox.Show("Contours found");

            int count = startLabel;
            for (Contour<Point> cont = contours;
                        cont != null;
                        cont = cont.HNext)
            {
                CvInvoke.cvDrawContours(
                imageMask,
                cont,
                new MCvScalar(count),
                new MCvScalar(0),
                2,
                -1,
                Emgu.CV.CvEnum.LINE_TYPE.EIGHT_CONNECTED,
                new Point(0, 0));
                ++count;
            }
            

            for (int i = 0; i < imageMask.Height; i++)
            {
                for (int j = 0; j < imageMask.Width; j++)
                {
                    if (imageMask.Data[i, j, 0] > 8)
                    {
                        
                        imageMask.Data[i, j, 0] = 0;
                    }
                }
            }

            //initializing for means
            Image<Gray, float> imageCorrected = new Image<Gray, float>(imageToBeCorrected.Width, imageToBeCorrected.Height);
            float[] values = new float[count]; //one size is extra; => easy mapping
            for (int i = 0; i < count; i++)
                values[i] = (float)0.0;
            int[] counts = new int[count]; //one size is extra; => easy mapping
            for (int i = 0; i < count; i++)
                counts[i] = 0;

            //counting the values and their counts for means
            for (int i = 0; i < imageMask.Height; i++)
            {
                for (int j = 0; j < imageMask.Width; j++)
                {
                    int region = imageMask.Data[i, j, 0];
                    if (region == 0)
                        continue;
                    if (region > count)
                        continue;
                    values[region] += imageToBeCorrected.Data[i, j, 0];
                    counts[region]++;
                }
            }

            //calculating means
            float[] means = new float[count];
            for (int i = 1; i < count; i++)
                means[i] = values[i] / counts[i];
            means[0] = 0;

            //filling means
            for (int i = 0; i < imageMask.Height; i++)
            {
                for (int j = 0; j < imageMask.Width; j++)
                {
                    int data = imageMask.Data[i, j, 0];
                    if (data > count)
                        data = 0;
                    imageCorrected.Data[i, j, 0] = means[data]; // *255 to check image
                }
            }

            lRegions = new List<Region>();
            for (Contour<Point> cont = contours;
                        cont != null;
                        cont = cont.HNext)
            {
                Seq<Point> boundary = cont.GetConvexHull(Emgu.CV.CvEnum.ORIENTATION.CV_CLOCKWISE);
                Region region = new Region();
                region.contour = cont;
                region.boundary = boundary;
                region.center = cont.GetMoments().GravityCenter;
                region.points = cont.ApproxPoly(cont.Perimeter * 0.015);
                lRegions.Add(region);
            }
            return imageCorrected;
        }

        void m_oWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // The background process is complete. We need to inspect
            // our response to see if an error occurred, a cancel was
            // requested or if we completed successfully.  
            if (e.Cancelled)
            {
                listBox1.Items.Add("Task Cancelled.");
            }

            // Check to see if an error occurred in the background process.

            else if (e.Error != null)
            {
                listBox1.Items.Add("Error while performing background operation.");
            }
            else
            {
                // Everything completed normally.
                listBox1.Items.Add("Task Completed...");
                setGlobalNormal();
            }

            //Change the status of the buttons on the UI accordingly
            button1.Enabled = true;
            button2.Enabled = true;
            button4.Enabled = true;
            
        }

        /// <summary>
        /// Notification is performed here to the progress bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_oWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            // This function fires on the UI thread so it's safe to edit

            // the UI control directly, no funny business with Control.Invoke :)

            // Update the progressBar with the integer supplied to us from the

            // ReportProgress() function.  
            if (e.ProgressPercentage == 1)
            {
                start = DateTime.Now;
                label9.Text = string.Format("{0:HH:mm:ss tt}", start);
                listBox1.Items.Add("RGB to YIQ to start");
                setGlobalNormal();
            }
            else if (e.ProgressPercentage == 2)
            {
                listBox1.Items.Add("RGB to YIQ done.");
                progressBar1.Value = 5;
            }
            else if (e.ProgressPercentage == 3)
            {
                listBox1.Items.Add("Make Chroma Mask from I of the YIQ Image");
            }
            else if (e.ProgressPercentage == 4)
            {
                listBox1.Items.Add("Chroma Mask Created.");
                progressBar1.Value = 10;
            }
            else if (e.ProgressPercentage == 5)
            {
                listBox1.Items.Add("Conversion of image - I - Start");
            }
            else if (e.ProgressPercentage == 6)
            {
                listBox1.Items.Add("Conversion of image - I - Finish");
                progressBar1.Value = 15;
            }
            else if (e.ProgressPercentage == 7)
            {
                listBox1.Items.Add("Quantization of image - I - Start");
            }
            else if (e.ProgressPercentage == 8)
            {
                listBox1.Items.Add("Quantization of image - I - Finish");
                progressBar1.Value = 16;
            }
            else if (e.ProgressPercentage == 9)
            {
                listBox1.Items.Add("Conversion of image - Q - Start");
            }
            else if (e.ProgressPercentage == 10)
            {
                listBox1.Items.Add("Conversion of image - Q - Finish");
                progressBar1.Value = 18;
            }
            else if (e.ProgressPercentage == 11)
            {
                listBox1.Items.Add("Quantization of image - Q - Start");
            }
            else if (e.ProgressPercentage == 12)
            {
                listBox1.Items.Add("Quantization of image - Q - Finish");
                progressBar1.Value = 20;
            }
            else if (e.ProgressPercentage == 13)
            {
                listBox1.Items.Add("Label Connected Components using ChromaMask");
            }
            else if (e.ProgressPercentage == 14)
            {
                listBox1.Items.Add("Labelling Done.");
                progressBar1.Value = 35;
            }
            else if (e.ProgressPercentage == 15)
            {
                listBox1.Items.Add("Begin Calculating Intrinsic Distance of Every Pixel.");
                
            }
            else if (e.ProgressPercentage == 100)
            {
                DateTime end = DateTime.Now;
                label10.Text = string.Format("{0:HH:mm:ss tt}", end);
                TimeSpan span = end.Subtract(start);
                label11.Text = "Time Taken = "+span.Minutes.ToString()+"Minutes and "+span.Seconds.ToString()+" seconds";
                progressBar1.Value = 100;
            }

                
            else
            {
                listBox1.Items.Add(s);
                pictureBox1.Image = a.ToBitmap();
                progressBar1.Value = e.ProgressPercentage;
                label5.Text = x.ToString();
                label6.Text = y.ToString();
                
            }
        }

        /// <summary>
        /// Time consuming operations go here </br>
        /// i.e. Database operations,Reporting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_oWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // The sender is the BackgroundWorker object we need it to
            // report progress and check for cancellation.
            //NOTE : Never play with the UI thread here...
            
            
                lRegions = new List<Region>();
                Image<Gray, int> imageBW = new Image<Gray, int>("example.png");
                imageOriginal.Image = imageBW;
                Image<Bgr, float> imageMarked = new Image<Bgr, float>("example_marked.bmp");
                imageResult.Image = imageMarked;

                
                Thread.Sleep(100);
                m_oWorker.ReportProgress(1);

                Matrix<float>[] yiq = new Matrix<float>[3];
                yiq = convertRGBToYIQ(imageMarked);
                Image<Bgr, float> checkImage = convertYIQToRGB(yiq);
                imageResult2.Image = checkImage;

                
                Thread.Sleep(100);
                m_oWorker.ReportProgress(2);

                
                Thread.Sleep(100);
                m_oWorker.ReportProgress(3);
                Image<Gray, byte> imageChromaMask = (Image<Gray, byte>)convertIToBWImage(yiq[1]);
                imageResult2.Image = imageChromaMask;

                
                Thread.Sleep(100);
                m_oWorker.ReportProgress(4);

                
                Thread.Sleep(100);
                m_oWorker.ReportProgress(5);

                Image<Gray, float> imageI = convertMatrixToImage(yiq[1]);
                imageResult2.Image = imageI;

                
                Thread.Sleep(100);
                m_oWorker.ReportProgress(6);

                
                Thread.Sleep(100);
                m_oWorker.ReportProgress(7);

                Image<Gray, float> imageQuantizedI = QuantizeImage(imageI, imageChromaMask);
                imageResult2.Image = imageQuantizedI;

                
                Thread.Sleep(100);
                m_oWorker.ReportProgress(8);

                
                Thread.Sleep(100);
                m_oWorker.ReportProgress(9);

                Image<Gray, float> imageQ = convertMatrixToImage(yiq[2]);
                imageResult2.Image = imageQ;

                
                Thread.Sleep(100);
                m_oWorker.ReportProgress(10);

            
                Thread.Sleep(100);
                m_oWorker.ReportProgress(11);    

                Image<Gray, float> imageQuantizedQ = QuantizeImage(imageQ, imageChromaMask);
                imageResult2.Image = imageQuantizedQ;

                
                Thread.Sleep(100);
                m_oWorker.ReportProgress(12);

                //now we have contour data in lRegions
                Matrix<float>[] yiqFinal = new Matrix<float>[3];
                yiqFinal[0] = BW();
                yiqFinal[1] = new Matrix<float>(yiq[0].Rows, yiq[0].Cols);
                yiqFinal[2] = new Matrix<float>(yiq[0].Rows, yiq[0].Cols);
                //COLORIZATION

                
                Thread.Sleep(100);
                m_oWorker.ReportProgress(13);

                Image<Gray, byte> imageConnectedComponents = LabelConnectedComponents(imageChromaMask, 1);//do NOT remove
                Image<Gray, byte> imageDistinctConnectedComponents = imageConnectedComponents.Clone();//do NOT remove
                for (int i = 0; i < imageConnectedComponents.Height; i++)
                {
                    for (int j = 0; j < imageConnectedComponents.Width; j++)
                    {
                        imageDistinctConnectedComponents.Data[i, j, 0] = (byte)(imageDistinctConnectedComponents.Data[i, j, 0] * 20);
                    }
                }
                imageResult2.Image = imageDistinctConnectedComponents;
                writeToFile(imageConnectedComponents);

            
                Thread.Sleep(100);
                m_oWorker.ReportProgress(14);
                
                Thread.Sleep(100);
                m_oWorker.ReportProgress(15);

                for (int i = 0; i < imageMarked.Height; i++)
                {
                    for (int j = 0; j < imageMarked.Width; j++)
                    {
                        x = i;
                        y = j;
                        if (imageConnectedComponents.Data[i, j, 0] > 0)
                        {
                            yiqFinal[1].Data[i, j] = yiq[1].Data[(int)lRegions[imageConnectedComponents.Data[i, j, 0] - 1].center.y, (int)lRegions[imageConnectedComponents.Data[i, j, 0] - 1].center.x] * yiqFinal[0].Data[(int)lRegions[imageConnectedComponents.Data[i, j, 0] - 1].center.y, (int)lRegions[imageConnectedComponents.Data[i, j, 0] - 1].center.x]; ;
                            yiqFinal[2].Data[i, j] = yiq[2].Data[(int)lRegions[imageConnectedComponents.Data[i, j, 0] - 1].center.y, (int)lRegions[imageConnectedComponents.Data[i, j, 0] - 1].center.x] * yiqFinal[0].Data[(int)lRegions[imageConnectedComponents.Data[i, j, 0] - 1].center.y, (int)lRegions[imageConnectedComponents.Data[i, j, 0] - 1].center.x];
                            s = ("X: " + i.ToString() + ", " + "Y: " + j.ToString() + ", I: " + yiqFinal[1].Data[i, j].ToString() + ", Q: " + yiqFinal[2].Data[i, j].ToString());
                            a = getColor(yiq[0].Data[globalY, globalX], yiqFinal[1].Data[i, j], yiqFinal[2].Data[i, j]);
                            y1 = yiqFinal[0].Data[i,j];
                            i1 = yiqFinal[1].Data[(int)lRegions[imageConnectedComponents.Data[i, j, 0] - 1].center.y, (int)lRegions[imageConnectedComponents.Data[i, j, 0] - 1].center.x];
                            q1 = yiqFinal[2].Data[(int)lRegions[imageConnectedComponents.Data[i, j, 0] - 1].center.y, (int)lRegions[imageConnectedComponents.Data[i, j, 0] - 1].center.x];
                            LiveUpdate();
                            
                            m_oWorker.ReportProgress(26 + i);

                            continue;
                        }

                        globalX = globalY = -1;

                        

                        if (choice == 1)
                        {
                            #region getNearestCenter
                            Point point = new Point(-1000, -1000);
                            double distance = double.MaxValue;
                            for (int k = 0; k < lRegions.Count; k++)
                            {
                                MCvPoint2D64f mcvp = lRegions[k].center;
                                int xx = (int)mcvp.y;
                                int yy = (int)mcvp.x;
                                if (point.X == -1000)
                                    point = new Point(xx, yy);
                                double tempDistance = Math.Pow(i - xx, 2) + Math.Pow(j - yy, 2);
                                tempDistance = Math.Sqrt(tempDistance);
                                if (tempDistance < distance)
                                {
                                    point = new Point(xx, yy);
                                    distance = tempDistance;
                                }
                            }
                            #endregion getNearestCenter
                            globalX = point.Y;
                            globalY = point.X;
                        }
                        else if (choice == 2)
                        {
                            GetXandY2(i, j, imageBW, imageChromaMask, imageConnectedComponents);
                            int weightSum = 0;
                            double II = 0.0;
                            double QQ = 0.0;
                            for (int z = 0; z < lRegions.Count; z++)
                            {
                                int xx = alY[z];
                                int yy = alX[z];
                                weightSum += alWeights[z];
                                II = alWeights[z] * yiq[1].Data[xx, yy] * yiq[0].Data[xx, yy];
                                QQ = alWeights[z] * yiq[2].Data[xx, yy] * yiq[0].Data[xx, yy]; ;
                            }
                            II /= weightSum;
                            QQ /= weightSum;
                            yiqFinal[1].Data[i, j] = (float)II;
                            yiqFinal[2].Data[i, j] = (float)QQ;
                            s = ("X: " + i.ToString() + ", " + "Y: " + j.ToString() + ", I: " + yiqFinal[1].Data[i, j].ToString() + ", Q: " + yiqFinal[2].Data[i, j].ToString());
                        }
                        else if (choice == 3)
                        {
                            GetXandY(i, j, imageBW, imageChromaMask, imageConnectedComponents);
                        }

                        if (choice != 2)
                        {
                            yiqFinal[1].Data[i, j] = yiq[1].Data[globalY, globalX] * yiqFinal[0].Data[globalY, globalX];
                            yiqFinal[2].Data[i, j] = yiq[2].Data[globalY, globalX] * yiqFinal[0].Data[globalY, globalX];
                            y1 = yiq[0].Data[i, j];
                            i1 = yiqFinal[1].Data[i, j];
                            q1 = yiqFinal[2].Data[i, j];
                            s = ("X: " + i.ToString() + ", " + "Y: " + j.ToString() + ", I: " + yiqFinal[1].Data[i, j].ToString() + ", Q: " + yiqFinal[2].Data[i, j].ToString());
                            a = getColor(yiq[0].Data[globalY, globalX], yiqFinal[1].Data[i, j], yiqFinal[2].Data[i, j]);
                        }
                        LiveUpdate();
                        m_oWorker.ReportProgress(26+i);
                       
                    }
                    imageResult2.Image = making;
                    Thread.Sleep(2);
                }

                Image<Bgr, float> finale = convertYIQToRGB(yiqFinal);
                imageResult2.Image = finale;
                
               
                if (m_oWorker.CancellationPending)
                {

                    e.Cancel = true;
                    m_oWorker.ReportProgress(0);
                    return;
                }
            

            //Report 100% completion on operation completed
            m_oWorker.ReportProgress(100);
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Change the status of the buttons on the UI accordingly
            //The start button is disabled as soon as the background operation is started
            //The Cancel button is enabled so that the user can stop the operation 
            //at any point of time during the execution
            button1.Enabled = false;
            button2.Enabled = false;
            button4.Enabled = false;
            
            choice = 3;
            // Kickoff the worker thread to begin it's DoWork function.
            m_oWorker.RunWorkerAsync();
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            //Change the status of the buttons on the UI accordingly
            //The start button is disabled as soon as the background operation is started
            //The Cancel button is enabled so that the user can stop the operation 
            //at any point of time during the execution
            button1.Enabled = false;
            button2.Enabled = false;
            button4.Enabled = false;
            
            choice = 1;
            // Kickoff the worker thread to begin it's DoWork function.
            m_oWorker.RunWorkerAsync();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Change the status of the buttons on the UI accordingly
            //The start button is disabled as soon as the background operation is started
            //The Cancel button is enabled so that the user can stop the operation 
            //at any point of time during the execution
            button2.Enabled = false;
            button1.Enabled = false;
            button4.Enabled = false;
            
            choice = 2;

            // Kickoff the worker thread to begin it's DoWork function.
            m_oWorker.RunWorkerAsync();
        }


    }

    public class Region
    {
        public Contour<Point> contour;
        public Seq<Point> points;
        public Seq<Point> boundary;
        public MCvPoint2D64f center;
    }

    public class Node : IComparable, IComparable<Node>
    {
        public int value;
        public int x;
        public int y;
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }
            Node other = obj as Node; // avoid double casting 
            if (other == null)
            {
                throw new ArgumentException("A RatingInformation object is required for comparison.", "obj");
            }
            return this.CompareTo(other);
        }

        public int CompareTo(Node other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return 1;
            }
            // Ratings compare opposite to normal string order,  
            // so reverse the value returned by String.CompareTo. 
            return Compare(this, other);
        }

        public static int Compare(Node left, Node right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return 0;
            }

            return Math.Abs(left.value).CompareTo(Math.Abs(right.value));
        }

        // Omitting Equals violates rule: OverrideMethodsOnComparableTypes. 
        public override bool Equals(object obj)
        {
            Node other = obj as Node; //avoid double casting 
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }
            return this.CompareTo(other) == 0;
        }

        // Omitting getHashCode violates rule: OverrideGetHashCodeOnOverridingEquals. 
        public override int GetHashCode()
        {
            return x ^ y ^ value;
        }

        // Omitting any of the following operator overloads  
        // violates rule: OverrideMethodsOnComparableTypes. 
        public static bool operator ==(Node left, Node right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }
            return left.Equals(right);
        }
        public static bool operator !=(Node left, Node right)
        {
            return !(left == right);
        }
        public static bool operator <(Node left, Node right)
        {
            return (Compare(left, right) < 0);
        }
        public static bool operator >(Node left, Node right)
        {
            return (Compare(left, right) > 0);
        }

    }

   


}

using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using CSharp;

namespace NeonApp
{
    public unsafe partial class Form1 : Form
    {

        [DllImport(@"C:\Users\Maja\source\repos\NeonApp\x64\Debug\Asm.dll")]
        static extern int DetectEdges(byte* inputPixels,
        byte* outputPixels,
        int length);

        private int[] threadOptions = { 1, 2, 4, 8, 16, 32, 64 };
        private int defaultThreads;
        private bool useAsm = true;
        private EdgeDetection cSharp = new EdgeDetection();
        public Form1()
        {
            InitializeComponent();
            defaultThreads = Environment.ProcessorCount;
            threadOptions = threadOptions.Append(defaultThreads).OrderBy(x => x).Distinct().ToArray();
            InitializeTrackBar();
        }
        private void InitializeTrackBar()
        {
            trackBarThreads.Maximum = threadOptions.Length - 1;
            trackBarThreads.Value = Array.IndexOf(threadOptions, defaultThreads);
            threadLabel.Text = $"Number of Threads: {defaultThreads}";
        }

        private void chooseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Select Image";
            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.tiff;*.webp|" +
                               "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                               "PNG (*.png)|*.png|" +
                               "GIF (*.gif)|*.gif|" +
                               "Bitmap (*.bmp)|*.bmp|" +
                               "TIFF (*.tiff)|*.tiff|" +
                               "WebP (*.webp)|*.webp";
            openFileDialog1.FilterIndex = 1;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    imagePathTextBox.Text = openFileDialog1.FileName;
                    using (var stream = new System.IO.FileStream(openFileDialog1.FileName, System.IO.FileMode.Open))
                    {
                        Image img = Image.FromStream(stream);
                        chooseImage.Image?.Dispose();
                        chosenImage.Image = new Bitmap(img);
                        pictureBoxOriginal.Image?.Dispose();


                        pictureBoxOriginal.Image = new Bitmap(img);
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show($"Błąd podczas wczytywania pliku: {ex.Message}",
                    "Błąd wczytywania obrazu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                }
            }

        }

        private void trackBarThreads_Scroll(object sender, EventArgs e)
        {
            threadLabel.Text = $"Number of Threads: {threadOptions[trackBarThreads.Value]}";
            // numberOfThreadsTests = threadOptions[trackBarThreads.Value];
            defaultThreads = threadOptions[trackBarThreads.Value];

        }
        private void restoreDefault_Click(object sender, EventArgs e)
        {
            defaultThreads = Environment.ProcessorCount;
            trackBarThreads.Value = Array.IndexOf(threadOptions, defaultThreads);
            threadLabel.Text = $"Number of Threads: {defaultThreads}";
        }

        // Create a struct to hold the parameters
        private unsafe struct ThreadParameters
        {
            public int StartPixel;
            public int EndPixel;
            public byte* OriginalPtr;
            public byte* EdgesPtr;

            public ThreadParameters(int start, int end, byte* original, byte* edges)
            {
                StartPixel = start;
                EndPixel = end;
                OriginalPtr = original;
                EdgesPtr = edges;
            }
        }

        private unsafe void ProcessImageSegment(object parameters)
        {
            var threadParams = (ThreadParameters)parameters;
            if (useAsm)
            {
                DetectEdges(
                threadParams.OriginalPtr + (threadParams.StartPixel * 4),
                threadParams.EdgesPtr + (threadParams.StartPixel * 4),
                threadParams.EndPixel - threadParams.StartPixel);
            }
            else
            {
                cSharp.DetectEdges(
                threadParams.OriginalPtr + (threadParams.StartPixel * 4),
                threadParams.EdgesPtr + (threadParams.StartPixel * 4),
                threadParams.EndPixel - threadParams.StartPixel);
            }

        }

        private void cSharp_radioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (cSharp_radioBtn.Checked)
            {
                useAsm = false;
            }
        }

        private void asm_radioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (asm_radioBtn.Checked)
            {
                useAsm = true;
            }
        }

        private unsafe void Convert_Click(object sender, EventArgs e)
        {
            if (pictureBoxOriginal.Image == null)
            {
                MessageBox.Show("Please select an image first.");
                return;
            }

            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            Bitmap original = new Bitmap(pictureBoxOriginal.Image);
            Bitmap edges = new Bitmap(original.Width, original.Height);
            Bitmap result = new Bitmap(original);

            BitmapData originalData = original.LockBits(
                new Rectangle(0, 0, original.Width, original.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            BitmapData edgesData = edges.LockBits(
                new Rectangle(0, 0, edges.Width, edges.Height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppArgb);

            try
            {
                byte* ptrOrig = (byte*)originalData.Scan0.ToPointer();
                byte* ptrEdges = (byte*)edgesData.Scan0.ToPointer();

                int totalPixels = original.Width * original.Height;
                int pixelsPerThread = totalPixels / defaultThreads;

                // Create and start threads
                var threads = new List<Thread>();

                for (int i = 0; i < defaultThreads; i++)
                {
                    int startPixel = i * pixelsPerThread;
                    int endPixel = (i == defaultThreads - 1) ? totalPixels : (i + 1) * pixelsPerThread;

                    var parameters = new ThreadParameters(startPixel, endPixel, ptrOrig, ptrEdges);
                    var thread = new Thread(ProcessImageSegment);
                    threads.Add(thread);
                    thread.Start(parameters);
                }

                // Wait for all threads to complete
                foreach (var thread in threads)
                {
                    thread.Join();
                }

            }
            finally
            {
                original.UnlockBits(originalData);
                edges.UnlockBits(edgesData);
            }

            ApplyGlowEffect(edges, result, Color.FromArgb(255, 0, 255));

            stopwatch.Stop();
            timeLabel.Text = $"Processing time: {stopwatch.ElapsedMilliseconds}ms using {threadOptions[trackBarThreads.Value]} threads";

            if (pictureBoxNeon.Image != null)
            {
                pictureBoxNeon.Image.Dispose();
            }
            pictureBoxNeon.Image = result;
            original.Dispose();
            edges.Dispose();
        }



        private void ApplyGlowEffect(Bitmap edges, Bitmap result, Color glowColor)
        {
            BitmapData edgesData = edges.LockBits(new Rectangle(0, 0, edges.Width, edges.Height),
         ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData resultData = result.LockBits(new Rectangle(0, 0, result.Width, result.Height),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            double baseAlpha = 40.0;      // Reduced intensity
            double distanceScale = 4.0;    // Larger fade distance
            int glowRadius = 6;           // Increased glow radius
            try
            {
                unsafe
                {
                    byte* ptrEdges = (byte*)edgesData.Scan0;
                    byte* ptrResult = (byte*)resultData.Scan0;
                    int stride = edgesData.Stride;
                    int width = edges.Width;
                    int height = edges.Height;

                    Parallel.For(2, height - 2, y =>
                    {
                        byte* row = ptrEdges + (y * stride);
                        byte* resultRow = ptrResult + (y * stride);

                        for (int x = 2; x < width - 2; x++)
                        {
                            int pixelOffset = x * 4;
                            if (row[pixelOffset + 2] > 30) // Check red channel for edge
                            {
                                // Set edge pixel to white
                                resultRow[pixelOffset] = 255;     // B
                                resultRow[pixelOffset + 1] = 255; // G
                                resultRow[pixelOffset + 2] = 255; // R
                                resultRow[pixelOffset + 3] = 255; // A

                                // Apply glow to surrounding pixels
                                for (int i = -4; i <= 4; i++)
                                {
                                    for (int j = -4; j <= 4; j++)
                                    {
                                        if (i == 0 && j == 0) continue;

                                        int newY = y + i;
                                        int newX = x + j;
                                        if (newX >= 0 && newX < width && newY >= 0 && newY < height)
                                        {
                                            int targetOffset = (newY * stride) + (newX * 4);
                                            double distance = Math.Sqrt(i * i + j * j);
                                            int alpha = (int)(150 * Math.Exp(-distance / 2));

                                            if (alpha > 0)
                                            {
                                                ptrResult[targetOffset] = (byte)((ptrResult[targetOffset] * (255 - alpha) + glowColor.B * alpha) / 255);
                                                ptrResult[targetOffset + 1] = (byte)((ptrResult[targetOffset + 1] * (255 - alpha) + glowColor.G * alpha) / 255);
                                                ptrResult[targetOffset + 2] = (byte)((ptrResult[targetOffset + 2] * (255 - alpha) + glowColor.R * alpha) / 255);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    });
                }
            }
            finally
            {
                edges.UnlockBits(edgesData);
                result.UnlockBits(resultData);
            }
        }

        private void TestsButton_Click(object sender, EventArgs e)
        {
            if (pictureBoxOriginal.Image == null)
            {
                MessageBox.Show("Please select an image first.");
                return;
            }
            double[] averageTimes = new double[threadOptions.Length];
            for (int i = 0; i < threadOptions.Length; i++)
            {
                Label currentLabel = this.Controls.Find($"t{threadOptions[i]}Asm", true).FirstOrDefault() as Label;
                int numberOfThreadsTests = threadOptions[i];
                long totalTime = 0;
                for (int j = 0; j < 10; j++)
                {


                    var stopwatch = new System.Diagnostics.Stopwatch();
                    stopwatch.Start();

                    Convert_Click(null, null);

                    stopwatch.Stop();
                    totalTime += stopwatch.ElapsedMilliseconds;

                    Application.DoEvents();
                }
                double averageTime = totalTime / 10.0;
                averageTimes[i] = averageTime;
                currentLabel.Text = $"{threadOptions[i]} threads: {averageTime:F2}ms";
            }
        }
        

      
    }
}

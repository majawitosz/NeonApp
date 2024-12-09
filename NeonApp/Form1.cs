using System.Collections.Concurrent;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using CSharp;

namespace NeonApp
{
    public unsafe partial class Form1 : Form
    {

        [DllImport(@"C:\Users\Maja\source\repos\NeonApp\x64\Debug\Asm.dll")]
        static extern void DetectEdges(byte* inputRowPrev, byte* inputRowCurrent, byte* inputRowNext, 
        byte* outputPixels);

        private int[] threadOptions = { 1, 2, 4, 8, 16, 32, 64 };
        private int defaultThreads;
        private bool useAsm = true;
        private EdgeDetection cSharp = new EdgeDetection();

        public unsafe struct BlockParameters
        {
            public int StartX;
            public int StartY;
            public int BlockWidth;
            public int BlockHeight;
            public int ImageWidth;
            public int ImageHeight;
            public int Stride;
            public byte* OriginalPtr;
            public byte* EdgesPtr;
            public bool IsLastBlock;
        }
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


        private unsafe void ProcessImageWithSelectedThreads(byte* ptrOrig, byte* ptrEdges, int imageWidth, int imageHeight)
        {
            const int BLOCK_SIZE = 8; // Optymalny rozmiar bloku dla pamięci podręcznej
            //int adjustedBlockSize = BLOCK_SIZE - (BLOCK_SIZE % 4);  // Upewniamy się że jest wielokrotnością 4

            // Obliczamy liczbę bloków
            int horizontalBlocks = (imageWidth / BLOCK_SIZE) + (imageWidth % BLOCK_SIZE == 0 ? 0 : 1);
            int verticalBlocks = (imageHeight / BLOCK_SIZE) + (imageHeight % BLOCK_SIZE == 0 ? 0 : 1);
            int totalBlocks = horizontalBlocks * verticalBlocks;

            // Używamy liczby wątków wybranej przez użytkownika
            int selectedThreadCount = threadOptions[trackBarThreads.Value];

            // Tworzymy kolejkę bloków
            var blockTasks = new ConcurrentQueue<BlockParameters>();

            // Dzielimy obraz na bloki
            for (int y = 0; y < imageHeight; y += BLOCK_SIZE)
            {
                for (int x = 0; x < imageWidth; x += BLOCK_SIZE)
                {
                    blockTasks.Enqueue(new BlockParameters
                    {
                        StartX = x,
                        StartY = y,
                        BlockWidth = Math.Min(BLOCK_SIZE, imageWidth - x),
                        BlockHeight = Math.Min(BLOCK_SIZE, imageHeight - y),
                        ImageWidth = imageWidth,
                        ImageHeight = imageHeight,
                        Stride = imageWidth * 4,
                        OriginalPtr = ptrOrig,
                        EdgesPtr = ptrEdges,
                        IsLastBlock = (x + BLOCK_SIZE >= imageWidth) && (y + BLOCK_SIZE >= imageHeight)
                    });
                }
            }

            // Tworzymy wybraną liczbę wątków
            var threads = new List<Thread>();
            var countdownEvent = new CountdownEvent(selectedThreadCount);

            for (int i = 0; i < selectedThreadCount; i++)
            {
                var thread = new Thread(() =>
                {
                    try
                    {
                        while (blockTasks.TryDequeue(out BlockParameters blockParams))
                        {
                            ProcessBlock(blockParams);
                        }
                    }
                    finally
                    {
                        countdownEvent.Signal();
                    }
                });

                threads.Add(thread);
                thread.Start();
            }

            countdownEvent.Wait();
        }

        private unsafe void ProcessBlock(BlockParameters blockParams)
        {
            if (useAsm)
            {
                try
                {
                    for (int y = 0; y < blockParams.BlockHeight; y++)
                    {
                        int currentY = blockParams.StartY + y;
                        if (currentY == 0 || currentY == blockParams.ImageHeight - 1)
                            continue;

                        long rowOffsetCurrent = (long)currentY * blockParams.Stride;
                        long rowOffsetPrev = (long)(currentY - 1) * blockParams.Stride;  // Wiersz powyżej
                        long rowOffsetNext = (long)(currentY + 1) * blockParams.Stride;  // Wiersz poniżej

                        int startX = blockParams.StartX;
                        int pixelsToProcess = Math.Min(blockParams.BlockWidth, blockParams.ImageWidth - startX);
                     
                        if (startX == 0 || startX + pixelsToProcess == blockParams.ImageWidth)
                            continue;

                        byte* inputRowPrev = blockParams.OriginalPtr + rowOffsetPrev + (startX * 4);
                        byte* inputRowCurrent = blockParams.OriginalPtr + rowOffsetCurrent + (startX * 4);
                        byte* inputRowNext = blockParams.OriginalPtr + rowOffsetNext + (startX * 4);
                        byte* outputRow = blockParams.EdgesPtr + rowOffsetCurrent + (startX * 4);
  
                        DetectEdges(inputRowPrev, inputRowCurrent, inputRowNext, outputRow);
                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error processing block: {ex.Message}");
                    throw;
                }
            }
            else
            {
                try
                {
                    for (int y = 0; y < blockParams.BlockHeight; y++)
                    {
                        int currentY = blockParams.StartY + y;
                        if (currentY >= blockParams.ImageHeight) continue;


                        long rowOffset = (long)currentY * blockParams.Stride;
                        int startX = blockParams.StartX;
                        int pixelsToProcess = Math.Min(blockParams.BlockWidth,
                            blockParams.ImageWidth - startX);

                        if (pixelsToProcess <= 0) continue;

                        // Sprawdzenie czy nie wykraczamy poza granice
                        if (startX + pixelsToProcess > blockParams.ImageWidth)
                            continue;

                        // Bezpieczne obliczenie offsetów
                        byte* inputRow = blockParams.OriginalPtr + rowOffset + (startX * 4);
                        byte* outputRow = blockParams.EdgesPtr + rowOffset + (startX * 4);

                        // Upewniamy się, że długość jest wielokrotnością 4 (dla SIMD)
                        int alignedLength = (pixelsToProcess + 3) & ~3;

                        // Przetwarzamy cały wiersz bloku na raz
                        cSharp.DetectEdges(
                            inputRow,
                            outputRow,
                            alignedLength
                        );
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing block: {ex.Message}");
                    throw;
                }
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

 
            using (Bitmap original = new Bitmap(pictureBoxOriginal.Image))
            using (Bitmap edges = new Bitmap(original.Width, original.Height))
            {
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

                    ProcessImageWithSelectedThreads(ptrOrig, ptrEdges, original.Width, original.Height);
                }
                finally
                {
                    original.UnlockBits(originalData);
                    edges.UnlockBits(edgesData);
                }
                stopwatch.Stop();
                timeLabel.Text = $"Processing time: {stopwatch.ElapsedMilliseconds}ms using {threadOptions[trackBarThreads.Value]} threads";

                if (pictureBoxNeon.Image != null)
                {
                    pictureBoxNeon.Image.Dispose();
                }
                pictureBoxNeon.Image = (Bitmap)edges.Clone();

                //Bitmap result = new Bitmap(original);
                //ApplyGlowEffect(edges, result, Color.FromArgb(255, 0, 255));

                //stopwatch.Stop();
                //timeLabel.Text = $"Processing time: {stopwatch.ElapsedMilliseconds}ms using {threadOptions[trackBarThreads.Value]} threads";

                //if (pictureBoxNeon.Image != null)
                //{
                //    pictureBoxNeon.Image.Dispose();
                //}
                //pictureBoxNeon.Image = result;
            }
        }

        private void ApplyGlowEffect(Bitmap edges, Bitmap result, Color glowColor)
        {
            BitmapData edgesData = edges.LockBits(
                new Rectangle(0, 0, edges.Width, edges.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            BitmapData resultData = result.LockBits(
                new Rectangle(0, 0, result.Width, result.Height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppArgb);
            try
            {
                unsafe
                {
                    byte* ptrEdges = (byte*)edgesData.Scan0;
                    byte* ptrResult = (byte*)resultData.Scan0;
                    int stride = edgesData.Stride;
                    int width = edges.Width;
                    int height = edges.Height;
                    int glowRadius = 6;
                    double baseAlpha = 40.0;
                    double distanceScale = 4.0;

                    for (int y = glowRadius; y < height - glowRadius; y++)
                    {
                        byte* row = ptrEdges + (y * stride);
                        byte* resultRow = ptrResult + (y * stride);

                        for (int x = glowRadius; x < width - glowRadius; x++)
                        {
                            int pixelOffset = x * 4;
                            if (row[pixelOffset + 2] > 250)
                            {
                                for (int i = -glowRadius; i <= glowRadius; i++)
                                {
                                    for (int j = -glowRadius; j <= glowRadius; j++)
                                    {
                                        int newY = y + i;
                                        int newX = x + j;
                                        if (newX >= 0 && newX < width && newY >= 0 && newY < height)
                                        {
                                            int targetOffset = (newY * stride) + (newX * 4);
                                            double distance = Math.Sqrt(i * i + j * j);
                                            int alpha = (int)(baseAlpha * Math.Exp(-distance / distanceScale));

                                            ptrResult[targetOffset] = (byte)Math.Min(255, ((ptrResult[targetOffset] * (255 - alpha) + glowColor.B * alpha) / 255));
                                            ptrResult[targetOffset + 1] = (byte)Math.Min(255, ((ptrResult[targetOffset + 1] * (255 - alpha) + glowColor.G * alpha) / 255));
                                            ptrResult[targetOffset + 2] = (byte)Math.Min(255, ((ptrResult[targetOffset + 2] * (255 - alpha) + glowColor.R * alpha) / 255));
                                            ptrResult[targetOffset + 3] = 255;
                                        }
                                    }
                                }
                            }
                        }
                    }
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

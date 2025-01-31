using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using CSharp;
using static System.Windows.Forms.AxHost;

namespace NeonApp
{
    public unsafe partial class Form1 : Form
    {

        [DllImport(@"C:\Users\Maja\source\repos\NeonApp\x64\Debug\Asm.dll")]
        static extern void DetectEdges(byte* inputRowPrev, byte* inputRowCurrent, byte* inputRowNext,
        byte* outputPixels);

        private int[] threadOptions = { 1, 2, 4, 7, 8, 16, 32, 64 };
        private int defaultThreads;
        private bool useAsm = true;
        private Color selectedGlowColor = Color.FromArgb(255, 0, 255);
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
            const int BLOCK_SIZE_WIDTH = 6;
            const int BLOCK_SIZE_HEIGHT = 3;

            int selectedThreadCount = threadOptions[trackBarThreads.Value];

            int totalBlocksY = (imageHeight + BLOCK_SIZE_HEIGHT - 1) / BLOCK_SIZE_HEIGHT;
            int totalBlocksX = (imageWidth + 6 - 1) / 6;
            int totalBlocks = totalBlocksY * totalBlocksX;

            selectedThreadCount = Math.Min(selectedThreadCount, totalBlocks);
            if (selectedThreadCount <= 0) selectedThreadCount = 1;

            var blockTasks = new ConcurrentQueue<BlockParameters>();

            for (int y = 0; y < imageHeight; y += BLOCK_SIZE_HEIGHT)
            {
                for (int x = 0; x < imageWidth; x += BLOCK_SIZE_WIDTH)
                {

                    int blockWidth = Math.Min(BLOCK_SIZE_WIDTH, imageWidth - x);
                    int blockHeight = Math.Min(BLOCK_SIZE_HEIGHT, imageHeight - y);

                    blockTasks.Enqueue(new BlockParameters
                    {
                        StartX = x,
                        StartY = y,
                        BlockWidth = blockWidth,
                        BlockHeight = blockHeight,
                        ImageWidth = imageWidth,
                        ImageHeight = imageHeight,
                        Stride = imageWidth * 4,
                        OriginalPtr = ptrOrig,
                        EdgesPtr = ptrEdges
                    });
                }
            }

            try
            {

                using (var countdownEvent = new CountdownEvent(selectedThreadCount))
                {
                    var threads = new List<Thread>();

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
                            catch (Exception ex)
                            {

                                Debug.WriteLine($"Thread error: {ex.Message}");
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing image: {ex.Message}");
            }
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

                        long rowOffsetPrev = (long)(currentY - 1) * blockParams.Stride;
                        long rowOffsetCurrent = (long)currentY * blockParams.Stride;
                        long rowOffsetNext = (long)(currentY + 1) * blockParams.Stride;

                        int startX = blockParams.StartX;
                        int pixelsToProcess = Math.Min(blockParams.BlockWidth, blockParams.ImageWidth - startX);
                        if (startX == 0 || startX + pixelsToProcess == blockParams.ImageWidth)
                            continue;

                        byte* inputRowPrev = blockParams.OriginalPtr + rowOffsetPrev + (startX * 4);
                        byte* inputRowCurrent = blockParams.OriginalPtr + rowOffsetCurrent + (startX * 4);
                        byte* inputRowNext = blockParams.OriginalPtr + rowOffsetNext + (startX * 4);

                        byte* tempOutput = stackalloc byte[32];

                        byte* outputRow = blockParams.EdgesPtr + rowOffsetCurrent + (startX * 4);
                        DetectEdges(inputRowPrev, inputRowCurrent, inputRowNext, tempOutput);

                        for (int i = 8; i < 32; i++)
                        {
                            outputRow[i] = tempOutput[i];
                        }

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
                        if (currentY == 0 || currentY == blockParams.ImageHeight - 1)
                            continue;

                        long rowOffsetPrev = (long)(currentY - 1) * blockParams.Stride;  // Wiersz powyżej
                        long rowOffsetCurrent = (long)currentY * blockParams.Stride;
                        long rowOffsetNext = (long)(currentY + 1) * blockParams.Stride;  // Wiersz poniżej

                        int startX = blockParams.StartX;
                        int pixelsToProcess = Math.Min(blockParams.BlockWidth, blockParams.ImageWidth - startX);
                        if (startX == 0 || startX + pixelsToProcess == blockParams.ImageWidth)
                            continue;
                        byte* inputRowPrev = blockParams.OriginalPtr + rowOffsetPrev + (startX * 4);
                        byte* inputRowCurrent = blockParams.OriginalPtr + rowOffsetCurrent + (startX * 4);
                        byte* inputRowNext = blockParams.OriginalPtr + rowOffsetNext + (startX * 4);
                        byte* tempOutput = stackalloc byte[32];
                        byte* outputRow = blockParams.EdgesPtr + rowOffsetCurrent + (startX * 4);

                        cSharp.DetectEdges(inputRowPrev, inputRowCurrent, inputRowNext, outputRow);

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
            using (Bitmap result = new Bitmap(original))
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

                //Color neonColor = Color.FromArgb(255, 0, 255);


                ApplyGlowEffect(edges, result, selectedGlowColor);

                //stopwatch.Stop();
                //timeLabel.Text = $"Processing time: {stopwatch.ElapsedMilliseconds}ms using {threadOptions[trackBarThreads.Value]} threads";

                if (pictureBoxNeon.Image != null)
                {
                    pictureBoxNeon.Image.Dispose();
                }
                pictureBoxNeon.Image = (Bitmap)result.Clone();
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
                byte* ptrEdges = (byte*)edgesData.Scan0;
                byte* ptrResult = (byte*)resultData.Scan0;
                int stride = edgesData.Stride;
                int width = edges.Width;
                int height = edges.Height;
                int glowRadius = 4;
                double baseAlpha = 25.0;
                double distanceScale = 2.0;


                double[] alphaValues = new double[glowRadius + 1];
                for (int i = 0; i <= glowRadius; i++)
                {
                    alphaValues[i] = baseAlpha * Math.Exp(-i / distanceScale);
                }

                for (int y = glowRadius; y < height - glowRadius; y++)
                {
                    byte* row = ptrEdges + (y * stride);
                    byte* resultRow = ptrResult + (y * stride);

                    for (int x = glowRadius; x < width - glowRadius; x++)
                    {
                        int pixelOffset = x * 4;
                        if (row[pixelOffset + 2] > 250)  // Wykryta krawędź
                        {
                            // Uproszczona wersja nakładania poświaty
                            for (int i = -glowRadius; i <= glowRadius; i += 1)
                            {
                                int newY = y + i;
                                if (newY < 0 || newY >= height) continue;

                                for (int j = -glowRadius; j <= glowRadius; j += 1)
                                {
                                    int newX = x + j;
                                    if (newX < 0 || newX >= width) continue;

                                    int distance = Math.Max(Math.Abs(i), Math.Abs(j));
                                    if (distance > glowRadius) continue;

                                    int targetOffset = (newY * stride) + (newX * 4);
                                    double alpha = alphaValues[distance];

                                    // Szybsze mieszanie kolorów
                                    byte blendB = (byte)((ptrResult[targetOffset] * (255 - alpha) + glowColor.B * alpha) / 255);
                                    byte blendG = (byte)((ptrResult[targetOffset + 1] * (255 - alpha) + glowColor.G * alpha) / 255);
                                    byte blendR = (byte)((ptrResult[targetOffset + 2] * (255 - alpha) + glowColor.R * alpha) / 255);

                                    ptrResult[targetOffset] = blendB;
                                    ptrResult[targetOffset + 1] = blendG;
                                    ptrResult[targetOffset + 2] = blendR;
                                    ptrResult[targetOffset + 3] = 255;
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

     

        private void svaeButton_Click(object sender, EventArgs e)
        {
            if (pictureBoxNeon.Image == null)
            {
                MessageBox.Show("Please process an image first.", "No Image",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp";
            saveFileDialog.Title = "Save Processed Image";
            saveFileDialog.DefaultExt = "png";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string extension = Path.GetExtension(saveFileDialog.FileName).ToLower();
                    ImageFormat format = ImageFormat.Png; // Default format

                    switch (extension)
                    {
                        case ".jpg":
                        case ".jpeg":
                            format = ImageFormat.Jpeg;
                            break;
                        case ".bmp":
                            format = ImageFormat.Bmp;
                            break;
                    }

                    // Save the image
                    pictureBoxNeon.Image.Save(saveFileDialog.FileName, format);

                    MessageBox.Show("Image saved successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving image: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton rb && rb.Checked)
            {
                switch (rb.Name)
                {
                    case "radioButton1":
                        selectedGlowColor = Color.FromArgb(255, 0, 255); // Pink
                        break;
                    case "radioButton2":
                        selectedGlowColor = Color.FromArgb(0, 255, 0);   // Green
                        break;
                    case "radioButton3":
                        selectedGlowColor = Color.FromArgb(0, 255, 255); // Cyan
                        break;
                    case "radioButton4":
                        selectedGlowColor = Color.FromArgb(255, 165, 0); // Orange
                        break;
                    case "radioButton5":
                        selectedGlowColor = Color.FromArgb(255, 255, 0); // Yellow
                        break;
                }
            }
        }

      
    }
}

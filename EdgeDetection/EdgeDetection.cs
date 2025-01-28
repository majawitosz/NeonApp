namespace CSharp
{
    public unsafe class EdgeDetection
    {
        private const float R_WEIGHT = 0.299f;
        private const float G_WEIGHT = 0.587f;
        private const float B_WEIGHT = 0.114f;
        private const int THRESHOLD = 80;

        // Sobel operators
        private static readonly int[] GX = { -1, 0, 1,
                                           -2, 0, 2,
                                           -1, 0, 1 };

        private static readonly int[] GY = { -1, -2, -1,
                                            0,  0,  0,
                                            1,  2,  1 };

        public void DetectEdges(byte* inputRowPrev, byte* inputRowCurrent, byte* inputRowNext, byte* outputRow)
        {
            // Process 6 pixels at a time
            for (int x = 0; x < 6; x++)
            {
                float[] grayValues = new float[9]; // 3x3 window
                int pixelIndex = 0;

                // Collect grayscale values for 3x3 window
                for (int row = -1; row <= 1; row++)
                {
                    byte* currentRow;
                    if (row == -1) currentRow = inputRowPrev;
                    else if (row == 0) currentRow = inputRowCurrent;
                    else currentRow = inputRowNext;

                    for (int col = -1; col <= 1; col++)
                    {
                        int offset = (x + col + 1) * 4;
                        byte b = currentRow[offset];
                        byte g = currentRow[offset + 1];
                        byte r = currentRow[offset + 2];

                        grayValues[pixelIndex++] = r * R_WEIGHT + g * G_WEIGHT + b * B_WEIGHT;
                    }
                }

                // Calculate Sobel gradients
                float gx = 0, gy = 0;
                for (int i = 0; i < 9; i++)
                {
                    gx += grayValues[i] * GX[i];
                    gy += grayValues[i] * GY[i];
                }

                // Calculate magnitude
                float magnitude = (float)Math.Sqrt(gx * gx + gy * gy);

                // Apply threshold
                uint outputColor = magnitude > THRESHOLD ? 0xFFFFFFFF : 0xFF000000;

                // Write output
                int outputOffset = x * 4;
                *(uint*)(outputRow + outputOffset) = outputColor;
            }
        }
    }
}
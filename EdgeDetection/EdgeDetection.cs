namespace CSharp
{
    public unsafe class EdgeDetection
    {

        private const int THRESHOLD = 15;
        private const uint ALPHA_MASK = 0xFF000000;
        private const uint WHITE_PIXEL = 0xFFFFFFFF;
        private const uint BLACK_PIXEL = 0xFF000000;
        public int DetectEdges(byte* inputPixels, byte* outputPixels, int length)
        {
            for (int i = 0; i < length - 1; i++)
            {
                int currentOffset = i * 4;
                int nextOffset = (i + 1) * 4;

                // Extract current pixel components
                byte currentBlue = inputPixels[currentOffset];
                byte currentGreen = inputPixels[currentOffset + 1];
                byte currentRed = inputPixels[currentOffset + 2];

                // Extract next pixel components
                byte nextBlue = inputPixels[nextOffset];
                byte nextGreen = inputPixels[nextOffset + 1];
                byte nextRed = inputPixels[nextOffset + 2];

                // Calculate absolute differences
                int blueDiff = Math.Abs(currentBlue - nextBlue);
                int greenDiff = Math.Abs(currentGreen - nextGreen);
                int redDiff = Math.Abs(currentRed - nextRed);

                // Sum up the differences
                int totalDiff = blueDiff + greenDiff + redDiff;

                // Compare with threshold and set output pixel
                uint outputColor = totalDiff > THRESHOLD ? WHITE_PIXEL : BLACK_PIXEL;

                // Store result
                *(uint*)(outputPixels + currentOffset) = outputColor;
            }

            // Handle the last pixel
            int lastOffset = (length - 1) * 4;
            *(uint*)(outputPixels + lastOffset) = BLACK_PIXEL;

            return 0;
        }
    }
}

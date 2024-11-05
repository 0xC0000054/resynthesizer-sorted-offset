using System.Diagnostics;

namespace ResynthesizerSortedOffsetTest
{
    struct Point(int x, int y)
    {
        public int X = x;
        public int Y = y;
    }

    internal readonly struct CartesianLessPointComparer : IComparer<Point>
    {
        public int Compare(Point point1, Point point2)
        {
            int point1Cartesian = (point1.X * point1.X) + (point1.Y * point1.Y);
            int point2Cartesian = (point2.X * point2.X) + (point2.Y * point2.Y);

            return point1Cartesian.CompareTo(point2Cartesian);
        }
    }

    internal class Program
    {
        static List<Point> SortedOffsets(int sourceImageWidth, int sourceImageHeight)
        {
            ulong length = ((2 * (ulong)sourceImageWidth) - 1) * ((2 * (ulong)sourceImageHeight) - 1);

            List<Point> offsets = new(checked((int)length));

            for (int y = -sourceImageHeight + 1; y < sourceImageHeight; y++)
            {
                for (int x = -sourceImageWidth + 1; x < sourceImageWidth; x++)
                {
                    offsets.Add(new Point(x, y));
                }
            }

            CartesianLessPointComparer comparer = new();

            offsets.Sort(comparer.Compare);

            return offsets;
        }

        static void Main()
        {
            const int SourceImageWidth = 7072;
            const int SourceImageHeight = 6654;

            Console.WriteLine("Sorting offsets for image size {0}x{1}...", SourceImageWidth, SourceImageHeight);

            Stopwatch stopwatch = new();
            stopwatch.Start();

            List<Point> offsets = SortedOffsets(SourceImageWidth, SourceImageHeight);

            stopwatch.Stop();

            Console.WriteLine($"Sorted {offsets.Count:n0} offsets in {stopwatch.ElapsedMilliseconds} ms.");
            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }
    }
}

namespace Nearest_Neighbor
{
    public partial class Form1 : Form
    {
        private List<Point> points = new();
        private List<Point> tour = new();
        private int pointRadius = 5;
        private Pen linePen = new(Color.Black, 2);
        public Form1()
        {
            InitializeComponent();
            ReadPointsFromFile("points.txt");
            FindNearestNeighborTour();
            DisplayDistances();
        }
        private void ReadPointsFromFile(string fileName)
        {
            try
            {
                using StreamReader sr = new(fileName);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] coordinates = line.Split(' ');
                    int x = int.Parse(coordinates[0]);
                    int y = int.Parse(coordinates[1]);
                    points.Add(new Point(x, y));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка чтения файла: " + ex.Message);
            }
        }
        private void FindNearestNeighborTour()
        {
            tour.Clear();
            List<Point> unvisited = new(points); 
            if (unvisited.Count == 0)
            {
                MessageBox.Show("Нет точек");
                return;
            }

            Point currentPoint = unvisited[0];
            tour.Add(currentPoint);
            unvisited.RemoveAt(0);
            double totalDistance = 0;

            while (unvisited.Count > 0)
            {
                double minDistance = double.MaxValue;
                Point nearestPoint = Point.Empty;

                foreach (Point point in unvisited)
                {
                    double distance = Distance(currentPoint, point);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestPoint = point;
                    }
                }
                tour.Add(nearestPoint);
                totalDistance += minDistance;
                unvisited.Remove(nearestPoint);
                currentPoint = nearestPoint;
            }
            totalDistance += Distance(tour[tour.Count - 1], tour[0]);
            tour.Add(tour[0]);

            string totalDistanceText = "Общий путь: " + totalDistance.ToString("0.00") + Environment.NewLine;
            richTextBox1.AppendText(totalDistanceText);

            for (int i = 0; i < tour.Count - 1; i++)
            {
                double distance = Distance(tour[i], tour[i + 1]);
                string distanceText = "Расстояние от " + tour[i] + " до " + tour[i + 1] + ": " + distance.ToString("0.00") + Environment.NewLine;
                richTextBox1.AppendText(distanceText);
            }
        }
        private double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }
        private void DisplayDistances()
        {
            richTextBox1.Dock = DockStyle.Right;
            richTextBox1.Width = 350;
            this.Controls.Add(richTextBox1);
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.DrawLine(linePen, 0, 500, 1000, 500);
            g.DrawLine(linePen, 500, 0, 500, 1000);

            for (int i = 0; i < tour.Count - 1; i++)
            {
                g.DrawLine(linePen, tour[i], tour[i + 1]);
            }

            foreach (Point p in points)
            {
                g.FillEllipse(Brushes.Red, p.X - pointRadius, p.Y - pointRadius, 2 * pointRadius, 2 * pointRadius);
            }
        }
    }
}
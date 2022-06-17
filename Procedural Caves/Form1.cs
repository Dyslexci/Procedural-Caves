using System.Drawing;
using System.Drawing.Imaging;

namespace Procedural_Caves
{
    public partial class Form1 : Form
    {
        Board board;
        public Form1()
        {
            InitializeComponent();
            Generate();
        }

        private void RegenerateButton_Click(object sender, EventArgs e) { Generate(); }
        private void pictureBox1_SizeChanged(object sender, EventArgs e) { Generate(); }
        private void SizeNud_ValueChanged(object sender, EventArgs e) { Generate(); }
        private void DensityNud_ValueChanged(object sender, EventArgs e) { Generate(); }

        int currentIteration = 0;
        void Generate()
        {
            currentIteration = 0;
            Reset();
        }

        void Reset()
        {
            board = new Board(pictureBox1.Width, pictureBox1.Height, (int)SizeNud.Value);
            board.RandomInit((double)DensityNud.Value / 100);
            Render();
        }

        private void DelayNud_ValueChanged(object sender, EventArgs e) { timer1.Interval = (int)DelayNud.Value; }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (currentIteration >= IterationsNud.Value) return;
            board.Advance();
            Render();
            currentIteration++;
        }

        Bitmap currentBmp;
        private void SaveButton_Click(object sender, EventArgs e)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            pathLabel.Text = "Saved to: " + path + "\\img.jpg!";
            if(currentBmp != null)
                currentBmp.Save(path + "\\img.jpg", ImageFormat.Jpeg);
            //currentBmp.Save("../../../img.jpg", ImageFormat.Jpeg);
        }

        private void Render()
        {
            using (var bmp = new Bitmap(board.width, board.height))
            using (var gfx = Graphics.FromImage(bmp))
            using (var brush = new SolidBrush(Color.LightGreen))
            {
                gfx.Clear(ColorTranslator.FromHtml("#2f3539"));
                var cellSize = new Size(board.cellSize, board.cellSize);

                for (int col = 0; col < board.columns; col++)
                {
                    for (int row = 0; row < board.rows; row++)
                    {
                        var cell = board.cells[col, row];
                        if (cell.isWall)
                        {
                            var cellLocation = new Point(col * board.cellSize, row * board.cellSize);
                            var cellRect = new Rectangle(cellLocation, cellSize);
                            gfx.FillRectangle(brush, cellRect);
                        }
                    }
                }

                pictureBox1.Image?.Dispose();
                pictureBox1.Image = (Bitmap)bmp.Clone();
                if(currentBmp != null)
                    currentBmp.Dispose();
                currentBmp = (Bitmap)bmp.Clone();
            }
        }
    }
}
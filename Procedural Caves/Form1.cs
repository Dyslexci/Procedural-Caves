using System.Drawing;
using System.Drawing.Imaging;

namespace Procedural_Caves
{
    /// <summary>
    /// Runs from the Program.cs main - handles GUI interaction and board population/simulation.
    /// </summary>
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

        /// <summary>
        /// Generates a new procedural image and resets the current iterations so the timer can continue.
        /// </summary>
        int currentIteration = 0;
        void Generate()
        {
            currentIteration = 0;
            Reset();
        }

        /// <summary>
        /// Resets the board to a new random initialisation.
        /// </summary>
        void Reset()
        {
            board = new Board(pictureBox1.Width, pictureBox1.Height, (int)SizeNud.Value);
            board.RandomInit((double)DensityNud.Value / 100);
            Render();
        }

        private void DelayNud_ValueChanged(object sender, EventArgs e) { timer1.Interval = (int)DelayNud.Value; }
        /// <summary>
        /// Advances the state of the board until the maximum number of iterations are reached.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (currentIteration >= IterationsNud.Value) return;
            board.Advance();
            Render();
            currentIteration++;
        }

        Bitmap currentBmp;
        /// <summary>
        /// Saves the currently held bitmap file to the user's pictures folder.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            pathLabel.Text = "Saved to: " + path + "\\img.jpg!";
            if(currentBmp != null)
                currentBmp.Save(path + "\\img.jpg", ImageFormat.Jpeg);
            //currentBmp.Save("../../../img.jpg", ImageFormat.Jpeg);
        }

        /// <summary>
        /// Renders the current board to a bitmap image which can be displayed/saved.
        /// </summary>
        private void Render()
        {
            using (var bmp = new Bitmap(board.width, board.height))
            using (var gfx = Graphics.FromImage(bmp))
            using (var brush = new SolidBrush(ColorTranslator.FromHtml("#2f3539")))
            {
                gfx.Clear(Color.LightGreen);
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
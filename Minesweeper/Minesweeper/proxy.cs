using System;
using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper
{
    public class Proxy
    {
        private readonly Form1 form;
        private readonly int rows;
        private readonly int cols;
        private readonly int totalMines;
        private readonly int cellSize;
        private readonly int marginTop;

        private PictureBox[,] pictureBoxes;
        private bool[,] mines;
        private bool[,] visited;
        private bool[,] flagged;
        private int openCellsCount = 0;
        private bool gameEnded = false;

        public Proxy(Form1 form, int rows, int cols, int totalMines, int cellSize, int marginTop)
        {
            this.form = form;
            this.rows = rows;
            this.cols = cols;
            this.totalMines = totalMines;
            this.cellSize = cellSize;
            this.marginTop = marginTop;

            pictureBoxes = new PictureBox[rows, cols];
            mines = new bool[rows, cols];
            visited = new bool[rows, cols];
            flagged = new bool[rows, cols];
        }

        public void InitializeGame()
        {
            PlaceMines(totalMines);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    pictureBoxes[i, j] = new PictureBox();
                    pictureBoxes[i, j].Name = "pictureBox_" + i.ToString() + "_" + j.ToString();
                    pictureBoxes[i, j].SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBoxes[i, j].Width = cellSize;
                    pictureBoxes[i, j].Height = cellSize;
                    pictureBoxes[i, j].Left = j * cellSize;
                    pictureBoxes[i, j].Top = marginTop + i * cellSize;
                    pictureBoxes[i, j].Image = Properties.Resources.unvi;
                    pictureBoxes[i, j].MouseClick += new MouseEventHandler(form.PictureBox_MouseClick);
                    form.Controls.Add(pictureBoxes[i, j]);
                }
            }
        }

        public void GameOver(bool win)
        {
            gameEnded = true;
        }

        public void PlaceMines(int totalMines)
        {
            Random random = new Random();
            int minesPlaced = 0;

            while (minesPlaced < totalMines)
            {
                int row = random.Next(rows);
                int col = random.Next(cols);

                if (!mines[row, col])
                {
                    mines[row, col] = true;
                    minesPlaced++;
                }
            }
        }

        public void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            int row = Convert.ToInt32(pictureBox.Name.Split('_')[1]);
            int col = Convert.ToInt32(pictureBox.Name.Split('_')[2]);

            if (gameEnded)
                return;

            if (e.Button == MouseButtons.Left)
            {
                if (flagged[row, col])
                    return;

                if (mines[row, col])
                {
                    pictureBox.Image = Properties.Resources.mine;
                    GameOver(false);
                }
                else
                {
                    OpenCell(row, col);
                    if (openCellsCount == (rows * cols) - totalMines)
                    {
                        GameOver(true);
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (!visited[row, col])
                {
                    if (flagged[row, col])
                    {
                        pictureBox.Image = Properties.Resources.unvi;
                        flagged[row, col] = false;
                    }
                    else
                    {
                        pictureBox.Image = Properties.Resources.flag;
                        flagged[row, col] = true;
                    }
                }
            }

            if (openCellsCount >= 90)
            {
                GameOver(false);
            }
        }

        public void OpenCell(int row, int col)
        {
            if (row < 0 || row >= rows || col < 0 || col >= cols || visited[row, col] || mines[row, col])
                return;

            visited[row, col] = true;
            pictureBoxes[row, col].Image = GetCellImage(row, col);
            openCellsCount++;

            if (CountAdjacentMines(row, col) == 0)
            {
                OpenCell(row - 1, col - 1);
                OpenCell(row - 1, col);
                OpenCell(row - 1, col + 1);
                OpenCell(row, col - 1);
                OpenCell(row, col + 1);
                OpenCell(row + 1, col - 1);
                OpenCell(row + 1, col);
                OpenCell(row + 1, col + 1);
            }
        }

        public int CountAdjacentMines(int row, int col)
        {
            int count = 0;
            for (int i = Math.Max(0, row - 1); i <= Math.Min(rows - 1, row + 1); i++)
            {
                for (int j = Math.Max(0, col - 1); j <= Math.Min(cols - 1, col + 1); j++)
                {
                    if (i == row && j == col)
                        continue;

                    if (mines[i, j])
                        count++;
                }
            }
            return count;
        }

        public Image GetCellImage(int row, int col)
        {
            int adjacentMines = CountAdjacentMines(row, col);
            switch (adjacentMines)
            {
                case 0:
                    return Properties.Resources.empty;
                case 1:
                    return Properties.Resources.one;
                case 2:
                    return Properties.Resources.two;
                case 3:
                    return Properties.Resources.three;
                case 4:
                    return Properties.Resources.four;
                case 5:
                    return Properties.Resources.five;
                case 6:
                    return Properties.Resources.six;
                case 7:
                    return Properties.Resources.seven;
                case 8:
                    return Properties.Resources.eight;
                default:
                    return null;
            }
        }
    }
}

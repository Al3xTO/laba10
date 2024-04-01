using System;
using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class Form1 : Form
    {
        private const int rows = 10; 
        private const int cols = 10; 
        private const int totalMines = 10; 
        private const int cellSize = 30; 
        private const int marginTop = 40; 

        private Proxy proxy;

        public Form1()
        {
            InitializeComponent();
            proxy = new Proxy(this, rows, cols, totalMines, cellSize, marginTop);
            proxy.InitializeGame();
        }

        

        public void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            proxy.PictureBox_MouseClick(sender, e);
        }

        public void OpenCell(int row, int col)
        {
            proxy.OpenCell(row, col);
        }

        public Image GetCellImage(int row, int col)
        {
            return proxy.GetCellImage(row, col);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace LightsOut___Universal
{
    public class LightsOutGame
    {
        private int gridSize;
        public bool[,] grid;           // Store the on/off state of the grid
        private Random rand;
        public string[,] object_grid;

        public const int MaxGridSize = 6;
        public const int MinGridSize = 4;



        public int GridSize
        {
            get
            {
                return gridSize;
            }
            set
            {
                if (value >= MinGridSize && value <= MaxGridSize)
                {
                    gridSize = value;
                    grid = new bool[gridSize, gridSize];
                    object_grid = new string[gridSize, gridSize];
                    NewGame();
                }
            }
        }

        public String Grid
        {
            get
            {
                return ToString();
            }
            set
            {
                StringToGrid(value);
            }
        }

        public LightsOutGame()
        {
            rand = new Random();
            GridSize = MinGridSize;


        }

        public bool GetGridValue(int row, int col)
        {
            return grid[row, col];
        }

        public string GetObjectGridValue(int row, int col)
        {
            return object_grid[row, col];
        }

        public void NewGame()
        {

            List<string> icons = new List<string>()
            {
                "apple", "apple", "orange", "orange", "carrot", "carrot", "kiwi", "kiwi", "orange-cat", "orange-cat", "pineapple", "pineapple", "pineapple2", "pineapple2", "pomegranate", "pomegranate"
            };


            for (int r = 0; r < gridSize; r++)
            {
                for (int c = 0; c < gridSize; c++)
                {
                    if (r < 0 || r >= gridSize || c < 0 || c >= gridSize)
                    {
                        return;
                    }

                        int randomNum = rand.Next(icons.Count);
                        grid[r, c] = false;

                        if (icons.Count != 0)
                        {
                            object_grid[r, c] = icons[randomNum];
                            icons.RemoveAt(randomNum);
                        }
                        
                }
            }
            
        }

        public void Move(int row, int col)
        {
            if (row < 0 || row >= gridSize || col < 0 || col >= gridSize)
            {
                throw new ArgumentException("Row or column is outside the legal range of 0 to " 
                    + (gridSize - 1));
            }

            // Invert selected box and all surrounding boxes
            if (row >= 0 && row < gridSize && col >= 0 && col < gridSize)
            {
                grid[row, col] = !grid[row, col];
            }
        }

        public bool IsGameOver()
        {
            for (int r = 0; r < gridSize; r++)
            {
                for (int c = 0; c < gridSize; c++)
                {
                    if (!grid[r, c])
                    {
                        return false;
                    }
                }
            }

            // All values must be false (off)
            return true;
        }

        // Restore grid fromt the string representation
        private void StringToGrid(string gridStr)
        {
            // Make sure given grid string fits in the grid
            if (gridStr.Length != GridSize * GridSize)
                return;

            int i = 0;
            for (int r = 0; r < GridSize; r++)
            {
                for (int c = 0; c < GridSize; c++)
                {
                    grid[r, c] = gridStr[i] == 'T';
                    i++;
                }
            }
        }
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            for (int r = 0; r < GridSize; r++)
                for (int c = 0; c < GridSize; c++)
                    builder.Append(grid[r, c] ? "T" : "F");

            return builder.ToString();
        }
    }
}

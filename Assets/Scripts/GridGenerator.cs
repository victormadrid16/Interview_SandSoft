using System;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Sandsoft.Match3
{
    public class GridGenerator
    {
        private const int MaxSize = 8;
        private const int MinElementsToMatch = 3;
        private const int MinPossibleMatches = 3;

        public Grid Generate()
        {
            var random = new Random();
            var grid = new Grid();
            
            var elementTypes = Enum.GetNames(typeof(GemType)).Length;
            
            int[,] gridElements;
            do
            {
                gridElements  = GenerateNewGrid(random, elementTypes, MaxSize, MaxSize);
            } while (!HasEnoughPossibleMatches(gridElements, MinPossibleMatches));
            
            grid.SetElements(gridElements);
            
            return grid;
        }

        private bool HasEnoughPossibleMatches(int[,] grid, int minPossibleMatches)
        {
            var matches = GetPossibleMatches(grid);
            
            return matches >= minPossibleMatches;
        }

        private int GetPossibleMatches(int[,] grid)
        {
            var sizeX =grid.GetLength(0);
            var sizeY =grid.GetLength(1);
            var matches = 0;
            for (var row = 0; row < sizeX; row++)
            {
                for (var col = 0; col < sizeY; col++)
                {
                    if (IsPossibleMatch(grid, row, col))
                    {
                        matches++;
                    }
                }
            }

            return matches;
        }

        private bool IsPossibleMatch(int[,] grid, int row, int col)
        {
            if (IsMatchOnSwap(grid, row, col, row + 1, col))
            {
                return true;
            }

            if (IsMatchOnSwap(grid, row, col, row - 1, col))
            {
                return true;
            }
            
            if (IsMatchOnSwap(grid, row, col, row, col + 1))
            {
                return true;
            }
            
            if (IsMatchOnSwap(grid, row, col, row, col - 1))
            {
                return true;
            }

            return false;
        }
        
        private int[,] GenerateNewGrid(Random random, int elementTypes, int sizeX, int sizeY)
        {
            var grid = new int[sizeX, sizeY];
            
            for (var row = 0; row < sizeX; row++)
            {
                for (var col = 0; col < sizeY; col++)
                {
                    var value = GetRandomValue(random, elementTypes);

                    while (IsMatch(grid, value, row, col))
                    {
                        value = GetRandomValue(random, elementTypes);
                    }
                    grid[row, col] = value;
                }
            }

            return grid;
        }

        private bool IsMatchOnSwap(int[,] grid, int currentRow, int currentCol, int targetRow, int targetCol)
        {
            if (IsOutOfBounds(grid, targetCol, targetRow))
            {
                return false;
            }

            var targetValue = grid[currentRow, currentCol];
            var tempGrid = Swap(grid, currentRow, currentCol, targetRow, targetCol);
            
            if (SameElementsRow(tempGrid, targetValue, targetRow, targetCol))
            {
                return true;
            }
            
            if (SameElementsColumn(tempGrid, targetValue, targetRow, targetCol))
            {
                return true;
            }

            return false;
        }

        private int[,] Swap(int[,] grid, int currentRow, int currentCol, int targetRow, int targetCol)
        {
            var rows = grid.GetLength(0);
            var cols = grid.GetLength(1);

            var tempGrid = new int[rows, cols];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    tempGrid[row, col] = grid[row, col];
                }
            }

            var temp = tempGrid[currentRow, currentCol];
            tempGrid[currentRow, currentCol] = tempGrid[targetRow, targetCol];
            tempGrid[targetRow, targetCol] = temp;

            return tempGrid;
        }

        private bool IsMatch(int[,] grid, int value, int row, int col)
        {
            if (SameElementsRow(grid, value, row, col))
            {
                return true;
            }
            
            if (SameElementsColumn(grid, value, row, col))
            {
                return true;
            }

            return false;
        }

        private bool SameElementsRow(int[,] grid, int value, int row, int col)
        {
            var sameElementsCount = 1;
            for (int i = 1; i < MinElementsToMatch; i++)
            {
                if (!IsSameValue(grid, value, row + i, col))
                {
                    break;
                }
                
                sameElementsCount++;
            }
           
            for (int i = 1; i < MinElementsToMatch; i++)
            {
                if (!IsSameValue(grid, value, row - i, col))
                {
                    break;
                }
                
                sameElementsCount++;
            }
            
            return sameElementsCount >= MinElementsToMatch;
        }
        
        private bool SameElementsColumn(int[,] grid, int value, int row, int col)
        {
            var sameElementsCount = 1;
            for (int i = 1; i < MinElementsToMatch; i++)
            {
                if (!IsSameValue(grid, value, row, col + i))
                {
                    break;
                }
                
                sameElementsCount++;
            }
           
            for (int i = 1; i < MinElementsToMatch; i++)
            {
                if (!IsSameValue(grid, value, row, col - i))
                {
                    break;
                }
                
                sameElementsCount++;
            }

            return sameElementsCount >= MinElementsToMatch;
        }

        private bool IsSameValue(int[,] grid, int value, int row, int col)
        {
            if (IsOutOfBounds(grid, row, col))
            {
                return false;
            }

            var isSame = grid[row, col] == value;
            return isSame;
        }

        private bool IsOutOfBounds(int[,] grid, int row, int col)
        {
            if (row >= grid.GetLength(0) || row < 0)
            {
                return true;
            }

            if (col >= grid.GetLength(1) || col < 0)
            {
                return true;
            }

            return false;
        }

        private int GetRandomValue(Random random, int elementTypes)
        {
            var value = random.Next(1, elementTypes + 1);
            return value;
        }
    }
}
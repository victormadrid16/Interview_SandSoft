namespace Sandsoft.Match3
{
    public class Grid
    {
        private int[,] elements;

        public int Width => elements.GetLength(0);
        public int Height => elements.GetLength(1);

        public void SetElements(int[,] elements)
        {
            this.elements = elements;
        }
        
        public GemType GetElement(int row, int col)
        {
            return (GemType)elements[row, col];
        }
        
    }
}
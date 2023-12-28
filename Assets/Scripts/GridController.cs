using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sandsoft.Match3
{
    public class GridController : MonoBehaviour
    {
        [SerializeField] 
        private GemsGenerator gemsGenerator;
      
        
        private GridGenerator gridGenerator;
        private Grid currentGrid;
       
        
        private void Awake()
        {
            gridGenerator = new GridGenerator();
        
        }

        private void Start()
        {
            GenerateGrid();
        }

       
        public void OnButtonClick()
        {
            ClearGrid();
            GenerateGrid();
        }
        
        private void GenerateGrid()
        {
            currentGrid = gridGenerator.Generate();

            var width = currentGrid.Width;
            var height = currentGrid.Height;

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    var element = currentGrid.GetElement(i, j);
                    gemsGenerator.CreateGem(element, i, j, transform);
                }
            }

        }

        private void ClearGrid()
        {
            gemsGenerator.ClearGems();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sandsoft.Match3
{
    public class GemsGenerator : MonoBehaviour
    {
        [SerializeField] 
        private List<GemPool> pools;
        [SerializeField] 
        private float GemWidth;
        [SerializeField] 
        private float GemHeight;
        
        private List<Gem> currentGems;
        private Dictionary<GemType, GemPool> poolsDict;

        private void Awake()
        {
            currentGems = new List<Gem>();
            pools.ForEach(x => x.Initialize(OnGetGem, OnReleaseGem));
            poolsDict = pools.ToDictionary(x => x.Prefab.Type);
        }

        public void CreateGem(GemType type, int row, int col, Transform parent)
        {
            var targetPosition = transform.position + Vector3.right * row * GemWidth + Vector3.up * col * GemHeight;
            var gem = poolsDict[type].Get(targetPosition, Quaternion.identity, parent);
            currentGems.Add(gem);
        }

        public void ClearGems()
        {
            currentGems.ForEach(gem => poolsDict[gem.Type].Release(gem));
            currentGems.Clear();
        }
        
        private void OnGetGem(Gem gem)
        {
            gem.gameObject.SetActive(true);
        }
    
        private void OnReleaseGem(Gem gem)
        {
            gem.gameObject.SetActive(false);
        }
    }
}
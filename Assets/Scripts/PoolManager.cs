using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Sandsoft.Match3
{
    public class PoolManager<T> : MonoBehaviour where T : MonoBehaviour
    {
        public T Prefab;
        [SerializeField] private int initialPoolSize = 2;

        private ObjectPool<T> objectPool;

        public void Initialize(Action<T> onGet = null, Action<T> onRelease = null, Action<T> onDestroy = null)
        {
            objectPool = new ObjectPool<T>(OnCreate, onGet, onRelease, onDestroy, true, initialPoolSize);
        }

        protected virtual T OnCreate()
        {
            T obj = Instantiate(Prefab, transform);
            obj.gameObject.SetActive(false);
            return obj;
        }

        public virtual T Get(Vector3 position, Quaternion direction, Transform parent)
        {
            T spawnedObject = objectPool.Get();
            spawnedObject.gameObject.SetActive(true);
            Transform trans = spawnedObject.transform;
            Quaternion rotation = Prefab.transform.rotation * direction;
            trans.position = position;
            trans.rotation = rotation;
            trans.SetParent(parent);
            return spawnedObject;
        }

        public virtual void Release(T target)
        {
            objectPool.Release(target);
            target.gameObject.SetActive(false);
            target.transform.SetParent(transform);
        }
    }
}
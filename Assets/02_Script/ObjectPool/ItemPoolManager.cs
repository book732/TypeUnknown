using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

public class ItemPoolManager : MonoBehaviour, IDisposable
{
    public static ItemPoolManager Instance;

    [System.Serializable]
    public class ItemPoolData
    {
        public ItemData data;
        public ItemPickup prefab;
    }
    [SerializeField] private List<ItemPoolData> poolDatas;

    private Dictionary<ItemData, IObjectPool<ItemPickup>> _pools;

    private void Awake()
    {
        Instance = this;
        _pools = new Dictionary<ItemData, IObjectPool<ItemPickup>>();

        foreach (var pooldata in poolDatas)
        {
            _pools[pooldata.data] = new ObjectPool<ItemPickup>(
                createFunc: () => Object.Instantiate(pooldata.prefab),
                actionOnGet: obj => obj.gameObject.SetActive(true),
                actionOnRelease: obj => obj.gameObject.SetActive(false),
                actionOnDestroy: obj => Object.Destroy(obj.gameObject),
                defaultCapacity: 10,
                maxSize: 30

                );
        }

    }

    public ItemPickup Get(ItemData itemData)
    {
        var item = _pools[itemData].Get();
        return item;
    }


    private bool _isDisposed = false;
    public void Dispose()
    {
        // 皋葛府 秦力贸府
        if (_isDisposed) return;

        foreach (var pool in _pools.Values)
        {
            pool.Clear();
        }

        _pools.Clear();
        _isDisposed = true;
    }

    public void Release(ItemPickup item)
    {
        _pools[item.itemData].Release(item);
    }

}

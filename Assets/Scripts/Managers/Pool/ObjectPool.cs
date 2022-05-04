using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T>
{
    private readonly List<ObjectPoolContainer<T>> _list;
    private readonly Dictionary<T, ObjectPoolContainer<T>> _lookup;
    private readonly Func<ObjectPool<T>, T> _factoryFunc;

    private int _lastIndex;
    private int _usedItemsCount;

    public int Count => _list.Count;

    public int CountUsedItems => _usedItemsCount;

    public ObjectPool(Func<ObjectPool<T>, T> factoryFunc, int initialSize)
    {
        _factoryFunc = factoryFunc;

        _list = new List<ObjectPoolContainer<T>>(initialSize);
        _lookup = new Dictionary<T, ObjectPoolContainer<T>>(initialSize);

        Warm(initialSize);
    }

    public T GetItem()
    {
        var container = GetObjectContainer();

        container.Consume();
        ++_usedItemsCount;

        return container.Item;
    }

    public void ReleaseItem(T item)
    {
        if (_lookup.TryGetValue(item, out var container))
        {
            container.Release();
            --_usedItemsCount;
        }
        else
            Debug.LogWarning("This object pool does not contain the item provided: " + item);
    }

    public void ReleaseAll()
    {
        foreach (var container in _list)
            container.Release();
    }

    private void Warm(int capacity)
    {
        for (int i = 0; i < capacity; i++)
            CreateContainer();
    }

    private ObjectPoolContainer<T> CreateContainer()
    {
        var container = new ObjectPoolContainer<T>(_factoryFunc(this));

        _list.Add(container);
        _lookup.Add(container.Item, container);

        return container;
    }

    private ObjectPoolContainer<T> GetObjectContainer()
    {
        for (int i = 0, count = _list.Count; i < count; ++i)
        {
            ++_lastIndex;

            if (_lastIndex > count - 1)
                _lastIndex = 0;

            if (!_list[_lastIndex].Used)
            {
                return _list[_lastIndex];
            }
        }

        return CreateContainer();
    }
}
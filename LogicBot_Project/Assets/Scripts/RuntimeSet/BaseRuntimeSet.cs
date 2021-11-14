using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseRuntimeSet<T> : ScriptableObject
{

    public class RuntimeSetEventArgs<TObject> : EventArgs
    {
        public TObject obj;
        public bool removed;
    }
        
    public event EventHandler<RuntimeSetEventArgs<T>> onRuntimeSetChanged;
    
    [SerializeField] protected List<T> _items = new List<T>();

    public void Initialize()
    {
        _items.Clear();
    }

    public T GetItemIndex(int index)
    {
        return _items[index];
    }

    public void AddToList(T thingToAdd)
    {
        if (!_items.Contains(thingToAdd))
        {
            _items.Add(thingToAdd);
            onRuntimeSetChanged?.Invoke(this, new RuntimeSetEventArgs<T>
            {
                obj =  thingToAdd, 
                removed = false
            } );
        }
    }

    public void RemoveFromList(T thingToRemove)
    {
        if (_items.Contains(thingToRemove))
        {
            _items.Remove(thingToRemove);
            onRuntimeSetChanged?.Invoke(this, new RuntimeSetEventArgs<T>
            {
                obj =  thingToRemove, 
                removed = true
            } );
        }
    }

    public int Count()
    {
        if (_items != null) return _items.Count;
        return 0;
    }

}

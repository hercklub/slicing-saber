
using System.Collections.Generic;
using UnityEngine;

public class UndoRedoBuffer<T> where T : class
{
    private List<T> _data;
    private int _capacity;
    private int _cursor;

    public UndoRedoBuffer(int capacity)
    {
        this._capacity = capacity;
        this._data = new List<T>(capacity + 1);
    }

    public void Add(T item)
    {
        if (this._data.Count >= this._capacity)
            this._data.RemoveAt(this._data.Count - 1);
        if (this._cursor > 0)
        {
            for (int index = 0; index < this._cursor; ++index)
                this._data.RemoveAt(0);
        }

        this._data.Insert(0, item);
        this._cursor = 0;
    }

    public T Undo()
    {
        if (this._cursor + 1 >= this._data.Count)
            return default(T);
        ++this._cursor;
        Debug.Log(_cursor);
        return this._data[this._cursor];
    }

    public T Redo()
    {
        if (this._cursor == 0)
            return default(T);
        --this._cursor;
        return this._data[this._cursor];
    }

    public void Clear()
    {
        this._data.Clear();
        this._cursor = 0;
    }
}
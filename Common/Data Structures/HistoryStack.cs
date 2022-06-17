using System;
using System.Collections.Generic;
using System.Linq;

namespace BuilderEssentials.Common.DataStructures;

public class HistoryStack<T> where T : class
{
    private LinkedList<T> items = new LinkedList<T>();
    public List<T> Items => items.ToList();
    public int Count => items.Count;
    public int Capacity { get;}
    public HistoryStack(int capacity) {
        Capacity = capacity;
    }

    public void Push(T item) {
        if (items.Count == Capacity) {
            items.RemoveFirst();
        }
        
        items.AddLast(item);
    }

    public T Pop() {
        if (items.Count == 0)
            return default;
        
        var ls = items.Last;
        items.RemoveLast();
        return ls == null ? default : ls.Value;
    }

    public T Peek() => items.Last?.Value ?? null;
}
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace BuilderEssentials.Common.DataStructures;

public class HistoryStack<T> where T : class
{
    private LinkedList<T> items = new();
    public List<T> Items => items.ToList();
    public int Count => items.Count;
    public int Capacity { get; private set; }
    public HistoryStack(int capacity) => Capacity = capacity;

    public void ChangeCapacity(int capacity) {
        //So we don't need to reload the mod in case the capacity changes
        while (capacity < items.Count)
            items.RemoveFirst();

        Capacity = capacity;
    }

    public void Push(T item) {
        if (items.Count == Capacity) { items.RemoveFirst(); }

        items.AddLast(item);
    }

    public T Pop() {
        if (items.Count == 0)
            return default;

        LinkedListNode<T> ls = items.Last;
        items.RemoveLast();
        return ls == null ? default : ls.Value;
    }

    public T Peek() => items.Last?.Value ?? null;

    public void AddRange(List<T> list) => list.ForEach(item => Push(item));
}
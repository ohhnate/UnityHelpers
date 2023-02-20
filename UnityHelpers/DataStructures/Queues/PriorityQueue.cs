// PriorityQueue.cs - A generic priority queue implementation for sorting elements by priority value.
// Version 1.0.1
// Author: Nate
// Website: https://github.com/ohhnate
//
// This PriorityQueue implementation associates each element with a priority value, where elements with
// higher priority values are dequeued before elements with lower priority values. The priority queue can
// be initialized with a custom comparer for the priority values.
// No accreditation is required but it would be highly appreciated <3

using System;
using System.Collections.Generic;

/// <summary>
/// A priority queue implementation that associates each element with a priority value.
/// Elements with higher priority values are dequeued before elements with lower priority values.
/// The priority queue can be initialized with a custom comparer for the priority values.
/// </summary>
/// <typeparam name="T">The type of the elements in the queue.</typeparam>
/// <typeparam name="TU">The type of the priority values associated with the elements.</typeparam>
public class PriorityQueue<T, TU> where T : IComparable<T>
{
    private readonly List<TU> _data;
    private readonly List<T> _priorities;
    public int Count => _data.Count;

    /// <summary>
    /// Initializes a new instance of the PriorityQueue class and sorts by priority.
    /// </summary>
    public PriorityQueue(Comparison<T> comparison)
    {
        _data = new List<TU>();
        _priorities = new List<T>();
        _priorities.Sort(comparison);
    }

    /// <summary>
    /// Adds an item to the queue with the specified priority.
    /// </summary>
    /// <param name="priority">The priority of the item to be added.</param>
    /// <param name="item">The item to be added.</param>
    public void Enqueue(T priority, TU item)
    {
        _priorities.Add(priority);
        _data.Add(item);
        int childIndex = Count - 1;
        while (childIndex > 0)
        {
            int parentIndex = (childIndex - 1) / 2;
            if (_priorities[childIndex].CompareTo(_priorities[parentIndex]) >= 0)
            {
                break;
            }
            SwapElements(childIndex, parentIndex);
            childIndex = parentIndex;
        }
    }

    /// <summary>
    /// Removes and returns the item with the highest priority from the queue.
    /// </summary>
    /// <returns>The item with the highest priority in the queue.</returns>
    public TU Dequeue()
    {
        if (Count == 0)
        {
            throw new InvalidOperationException("Priority queue is empty");
        }
        TU frontItem = _data[0];
        int lastIndex = Count - 1;
        _data[0] = _data[lastIndex];
        _priorities[0] = _priorities[lastIndex];
        _data.RemoveAt(lastIndex);
        _priorities.RemoveAt(lastIndex);
        int parentIndex = 0;
        while (true)
        {
            int leftChildIndex = parentIndex * 2 + 1;
            if (leftChildIndex >= Count)
            {
                break;
            }
            int rightChildIndex = leftChildIndex + 1;
            if (rightChildIndex < Count && _priorities[rightChildIndex].CompareTo(_priorities[leftChildIndex]) < 0)
            {
                leftChildIndex = rightChildIndex;
            }
            if (_priorities[parentIndex].CompareTo(_priorities[leftChildIndex]) <= 0)
            {
                break;
            }
            SwapElements(parentIndex, leftChildIndex);
            parentIndex = leftChildIndex;
        }
        return frontItem;
    }

    /// <summary>
    /// Swaps the elements at indices i and j in both the data and priority arrays via deconstruction.
    /// </summary>
    /// <param name="i">The index of the first element.</param>
    /// <param name="j">The index of the second element.</param>
    private void SwapElements(int i, int j)
    {
        (_priorities[i], _priorities[j]) = (_priorities[j], _priorities[i]);
        (_data[i], _data[j]) = (_data[j], _data[i]);
    }

    /// <summary>
    /// Returns the item with the highest priority in the queue without removing it.
    /// </summary>
    /// <returns>The item with the highest priority in the queue.</returns>
    public TU Peek()
    {
        if (Count == 0)
        {
            throw new InvalidOperationException("Priority queue is empty");
        }
        return _data[0];
    }

    /// <summary>
    /// Determines whether the priority queue is empty.
    /// </summary>
    /// <returns><c>true</c> if the priority queue is empty; otherwise, <c>false</c>.</returns>
    public bool IsEmpty()
    {
        return Count == 0;
    }
}
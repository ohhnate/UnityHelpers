// PriorityQueueExample.cs - A simple example of using the PriorityQueue class to prioritize Person objects.
// Version 1.0.0
// Author: Nate
// Website: https://github.com/ohhnate

// This example demonstrates how to use the PriorityQueue class to sort elements by priority value.
// The elements are represented by Person objects, which are associated with a priority value based on their age.
// The PriorityQueue is initialized with a custom comparer that prioritizes higher ages over lower ages.
//
// No accreditation is required but it would be highly appreciated <3

using System;
using UnityEngine;

public class PriorityQueueExample : MonoBehaviour
{
    // A simple class to represent a person
    private class Person : IComparable<Person>
    {
        public string Name { get; }
        public int Age { get; }
        
        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }
        
        public int CompareTo(Person other)
        {
            return Age.CompareTo(other.Age);
        }
    }

    // The priority queue to store our people
    private PriorityQueue<int, Person> _priorityQueue;

    private void Start()
    {
        // Create a new priority queue, where higher ages have higher priority
        _priorityQueue = new PriorityQueue<int, Person>((p1, p2) => p2.CompareTo(p1));

        // Add some people to the queue
        _priorityQueue.Enqueue(20, new Person("Tanju", 20));
        _priorityQueue.Enqueue(12, new Person("Sylvia", 12));
        _priorityQueue.Enqueue(350, new Person("Keina", 350));
        _priorityQueue.Enqueue(35, new Person("The CEG", 35));
        _priorityQueue.Enqueue(60, new Person("Guava", 60));
        _priorityQueue.Enqueue(9999999, new Person("HAM", 999999));

        // Peek at the person with the highest priority
        Debug.Log("The person with the highest priority is: " + _priorityQueue.Peek().Name);

        // Dequeue and print each person in order of priority
        while (!_priorityQueue.IsEmpty())
        {
            Person nextPerson = _priorityQueue.Dequeue();
            Debug.Log("Person removed by priority: " + nextPerson.Name + ", Age: " + nextPerson.Age);
        }
    }
}
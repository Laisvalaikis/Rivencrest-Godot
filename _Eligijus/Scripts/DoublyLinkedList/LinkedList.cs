using System;
using System.Collections;
using System.Collections.Generic;
using Godot;

public class LinkedList<T>
{
    
    public LinkedListNode<T> First { get; set; }
    public LinkedListNode<T> Last { get; set; }
    public int Count { get; private set; }

    public void AddFirst(T nodeToAdd)
    {
        LinkedListNode<T> node = new LinkedListNode<T>(nodeToAdd);
        node.Value = nodeToAdd;
        if (First.Value is null)
        {
            First = node;
            Last = First;
        }
        else
        {
            node.Next = First;
            First.Previous = node;
            First = node;
        }
        Count++;
    }
    
    public void AddLast(T nodeToAdd)
    {
        LinkedListNode<T> node = new LinkedListNode<T>(nodeToAdd);
        node.Value = nodeToAdd;
        if (First is null)
        {
            First = node;
            Last = node;
        }
        else if (First is not null && Last is not null)
        {
            Last.Next = node;
            node.Previous = Last;
            Last = node;
        }
        Count++;
    }
    
    public void AddLast(LinkedListNode<T> node)
    {
        if (First is null)
        {
            First = node;
            Last = node;
        }
        else
        {
            Last.Next = node;
            node.Previous = Last;
            Last = node;
        }
        Count++;
    }

    public bool Contains(T value)
    {
        for (LinkedListNode<T> element = First; element != null; element = element.Next)
        {
            if (element.Value.Equals(value))
            {
                return true;
            }
        }
        return false;
    }
    
    public bool ContainsType(Type value)
    {
        for (LinkedListNode<T> element = First; element != null; element = element.Next)
        {
            if (element.Value.Equals(value))
            {
                return true;
            }
        }
        return false;
    }

    public void Remove(LinkedListNode<T> valueToRemove)
    {
        if (valueToRemove is null)
        {
            return;
        }
        var next = valueToRemove.Next;
        var prev = valueToRemove.Previous;
        if (prev is not null)
        {
            prev.Next = next;
            if (next is null)
            {
                Last = prev;
                valueToRemove.Previous = null;
            }
        }
        if (next is not null)
        {
            next.Previous = prev;
            if (prev is null)
            {
                First = next;
                valueToRemove.Next = null;
            }
        }
        if (next is null && prev is null)
        {
            First = null;
            Last = null;
        }
        
        valueToRemove.Previous = null;
        valueToRemove.Next = null;
        Count--;
        
    }

    public void MoveToEnd(LinkedListNode<T> node)
    {
        Remove(node);
        AddLast(node);
    }
    
    public LinkedListNode<T>? Find(T valueToFind)
    {
        for (LinkedListNode<T> element = First; element != null; element = element.Next)
        {
            if (element.Value.Equals(valueToFind))
            {
                return element;
            }
        }
        return null;
    }
    
    public LinkedListNode<T>? FindType(Type valueToFind)
    {
        for (LinkedListNode<T> element = First; element != null; element = element.Next)
        {
            if (element.Value.Equals(valueToFind))
            {
                return element;
            }
        }
        return null;
    }

    public void Clear()
    {
        First = null;
        Last = null;
    }

}
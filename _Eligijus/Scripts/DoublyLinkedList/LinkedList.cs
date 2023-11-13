using System.Collections;
using System.Collections.Generic;
using Godot;

public class LinkedList<T> : IEnumerable<LinkedListNode<T>>
{
    
    public LinkedListNode<T>? First { get; set; }
    public LinkedListNode<T>? Last { get; set; }
    public int Count { get; private set; }
    
    public IEnumerator<LinkedListNode<T>> GetEnumerator()
    {
        LinkedListNode<T>? currentNode = First;
        while (currentNode is not null)
        {
            yield return currentNode;
            currentNode = currentNode.Next;
        }
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    public void AddFirst(T nodeToAdd)
    {
        LinkedListNode<T> node = new LinkedListNode<T>(nodeToAdd);
        node.Value = nodeToAdd;
        if (First is null)
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
        else
        {
            Last.Next = node;
            node.Previous = Last;
            Last = node;
        }
        Count++;
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
        }
        if (next is not null)
        {
            next.Previous = prev;
        }

        if (next is null)
        {
            Last = prev;
            if (prev is not null && prev.Previous is null)
            {
                First = prev;
            }
            else if(prev is null)
            {
                First = prev;
            }
        }

        

        Count--;
        
    }
    
    
    public LinkedListNode<T>? Find(LinkedListNode<T> valueToFind)
    {
        var aux = First;
        while (aux is not null)
        {
            if (EqualityComparer<T>.Default.Equals(aux.Value, valueToFind.Value))
            {
                return aux;
            }
            aux = aux.Next;
        }
        return default;
    }
    
}
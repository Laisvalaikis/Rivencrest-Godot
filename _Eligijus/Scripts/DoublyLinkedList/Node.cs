
public class LinkedListNode<T>
{
    public T Value { get; set; }
    public LinkedListNode<T>? Next { get; set; }
    public LinkedListNode<T>? Previous { get; set; }
    public LinkedListNode(T value)
    {
        Value = value;
    }
    
    public LinkedListNode()
    {
        
    }
}
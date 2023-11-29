

public class TeamObject
{
    private LinkedList<Object> objects;

    public void AddObject(Object objectToAdd)
    {
        if (objects is null)
        {
            objects = new LinkedList<Object>();
        }
        objects.AddLast(objectToAdd);
    }

    public void RemoveObject(Object objectToRemove)
    {
        if (objects is null)
        {
            objects = new LinkedList<Object>();
        }
        else if (objects is not null && objects.Contains(objectToRemove))
        {
            LinkedListNode<Object> objectNode = objects.Find(objectToRemove);
            objects.Remove(objectNode);
        }
    }

    public LinkedList<Object> GetObjects()
    {
        if (objects is null)
        {
            objects = new LinkedList<Object>();
        }
        return objects;
    }
    

}
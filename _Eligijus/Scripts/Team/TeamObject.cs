
using System.Collections.Generic;

public class TeamObject
{
    private List<Object> objects;

    public void AddObject(Object objectToAdd)
    {
        if (objects is null)
        {
            objects = new List<Object>();
        }
        objects.Add(objectToAdd);
    }

    public void RemoveObject(Object objectToRemove)
    {
        if (objects is null)
        {
            objects = new List<Object>();
        }
        else if (objects is not null && objects.Contains(objectToRemove))
        {
            objects.Remove(objectToRemove);
        }
    }

    public List<Object> GetObjects()
    {
        if (objects is null)
        {
            objects = new List<Object>();
        }
        return objects;
    }
    

}
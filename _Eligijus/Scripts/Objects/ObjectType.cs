using System;
public class ObjectType<T>
{
    public T data;
    public Type objectType;
    
    public ObjectType(T data, Type type)
    {
        this.data = data;
        objectType = type;
    }
    public PlayerInformation GetPlayerInformation()
    {
        return data as PlayerInformation;
    }
    
    public Player GetPlayer()
    {
        return data as Player;
    }
    
    public ObjectInformation GetObjectInformation()
    {
        return data as ObjectInformation;
    }

    public Object GetObject()
    {
        return data as Object;
    }
}
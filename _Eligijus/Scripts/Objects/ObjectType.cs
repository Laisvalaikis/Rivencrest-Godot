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
    
    public Object GetObjectInformation()
    {
        return data as Object;
    }

    public Object GetObject()
    {
        return data as Object;
    }
}
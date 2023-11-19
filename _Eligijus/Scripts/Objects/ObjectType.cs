public class ObjectType<T>
{
    public T data;

    public ObjectType()
    {
        
    }
    public Player GetPlayer()
    {
        return data as Player;
    }

    public Object GetObject()
    {
        return data as Object;
    }
}
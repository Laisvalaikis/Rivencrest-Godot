
public class ObjectInformation<T>
{
    public T objectData;

    public ObjectData GetObjectData()
    {
        return objectData as ObjectData;
    }
    
    public PlayerInformationData GetPlayerInformationData()
    {
        return objectData as PlayerInformationData;
    }

}
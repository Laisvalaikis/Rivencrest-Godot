
public class ObjectInformation<T>
{
    public T objectData;

    public ObjectInformation(T data)
    {
        objectData = data;
    }

    public ObjectData GetObjectData()
    {
        return objectData as ObjectData;
    }
    
    public PlayerInformationDataNew GetPlayerInformationData()
    {
        return objectData as PlayerInformationDataNew;
    }

}
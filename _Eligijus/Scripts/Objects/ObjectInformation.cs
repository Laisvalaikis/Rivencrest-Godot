using System;

public class ObjectInformation<T>
{
    public T objectData;
    public Type objectType;
    public ObjectInformation(T data, Type type)
    {
        objectData = data;
        objectType = type;
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
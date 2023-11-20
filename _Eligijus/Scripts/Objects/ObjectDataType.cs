using System;

public class ObjectDataType<T>
{
    public T objectData;
    public Type objectType;
    public ObjectDataType(T data, Type type)
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
﻿
public class ObjectInformation<T>
{
    public T objectData;

    public ObjectData GetObjectData()
    {
        return objectData as ObjectData;
    }
    
    public PlayerInformationDataNew GetPlayerInformationData()
    {
        return objectData as PlayerInformationDataNew;
    }

}
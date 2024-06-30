using System;

[Serializable]
public class DataContainer<T>
{
    public T[] Data;
    public DataContainer(int num)
    {
        Data = new T[num];
    }
}
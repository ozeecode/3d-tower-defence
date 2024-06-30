using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class FileIO
{
    public static void WriteToBinaryFile<T>(string filePath, T objectToWrite)
    {
        using (Stream stream = File.Open(filePath, FileMode.Create))
        {
            var binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(stream, objectToWrite);
        }
    }

    public static T ReadFromBinaryFile<T>(string filePath)
    {
        using (Stream stream = File.Open(filePath, FileMode.Open))
        {
            try
            {
                var binaryFormatter = new BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
            catch (System.Exception)
            {

                return default;
            }
            
        }
    }

    public static void DeleteFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}

using System.IO;
using UnityEngine;

public static class SaveIO
{
    private static readonly string baseSavePath;

    static SaveIO()
    {
        baseSavePath = Application.persistentDataPath + "/bin/";
        if (!File.Exists(baseSavePath))
            Directory.CreateDirectory(baseSavePath);
    }

    public static void SaveData<T>(T data, string fileName, string extention = ".dat")
    {
        //Debug.Log(baseSavePath + fileName);
        FileIO.WriteToBinaryFile(baseSavePath + fileName + extention, data);
    }

    public static T LoadData<T>(string fileName, string extention = ".dat")
    {
        string filePath = baseSavePath + fileName + extention;
        if (File.Exists(filePath))
        {
            return FileIO.ReadFromBinaryFile<T>(filePath);
        }
        return default;
    }

    public static void DeleteDataFile(string fileName, string extention = ".dat")
    {
        string filePath = baseSavePath + fileName + extention;
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    public static void SerioslyDeleteAllFiles()
    {
        Debug.Log(baseSavePath);
        DeleteDirectory(baseSavePath);
        Directory.CreateDirectory(baseSavePath);
    }

    //from this answer https://stackoverflow.com/a/329502
    private static void DeleteDirectory(string target_dir)
    {
        string[] files = Directory.GetFiles(target_dir);
        string[] dirs = Directory.GetDirectories(target_dir);

        foreach (string file in files)
        {
            File.SetAttributes(file, FileAttributes.Normal);
            File.Delete(file);
        }

        foreach (string dir in dirs)
        {
            DeleteDirectory(dir);
        }

        Directory.Delete(target_dir, false);
    }
}

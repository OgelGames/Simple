using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ExtraMethods
{
    public static void SaveFile<T>(T item, string filePath, string fileName, string fileExtension = "") where T : class
    {
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        using (FileStream stream = new FileStream(filePath + "/" + fileName + fileExtension, FileMode.Create))
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(stream, item);
        }
    }

    public static T LoadFile<T>(string filePath, string fileName, string fileExtension = "") where T : class
    {
        if (!File.Exists(filePath + "/" + fileName + fileExtension))
        {
            Debug.LogWarningFormat("File does not exist!   Requested file: {0}{1}/{2}", filePath, fileName, fileExtension);
            return null;
        }
        using (FileStream stream = new FileStream(filePath + "/" + fileName + fileExtension, FileMode.Open))
        {
            BinaryFormatter serializer = new BinaryFormatter();
            return (T)serializer.Deserialize(stream);
        }
    }

    public static string GenerateRandomString(int length, string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_")
    {
        System.Random random = new System.Random();

        char[] randomString = new char[length];

        for (int i = 0; i < length; i++)
        {
            randomString[i] = characters[random.Next(characters.Length)];
        }
        return new string(randomString);
    }
}

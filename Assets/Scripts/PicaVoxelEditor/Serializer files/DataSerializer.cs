using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

/// <summary>
/// Allows us to store any primitive data as a local JSON file in the system. Can be used for saving gameObjects and scenes as well as creating a save file.
/// </summary>
public class DataSerializer 
{
    /// <summary>
    /// This function serializes the data and saves it as a json file. It accepts all format of data in primitive type as only serializable data is passed. 
    /// It also takes the relative path where this data is stored and places the json file there. The JSON file is stored locally.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="RelativePath"></param>
    /// <param name="Data"></param>
    public void SaveData<T>(string RelativePath,T Data)
    {
        string path = Application.persistentDataPath + RelativePath;
        try
        {
            if (File.Exists(path))
            {
                Debug.Log("Data Exists and is being overwritten");
                File.Delete(path);
            }
            else
            {
                Debug.Log("Writing new File");
            }

            using FileStream stream = File.Create(path);
            stream.Close();
            File.WriteAllText(path, JsonConvert.SerializeObject(Data));

        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
            Debug.Log("Storing Failed");
        }
    }

    /// <summary>
    /// This function loads the data that has been saved by the Serializer and returns the same.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="RelativePath"></param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
    public T LoadData<T>(string RelativePath)
    {
        string path=Application.persistentDataPath + RelativePath;
        if(!File.Exists(path))
        {
            Debug.LogError($"Cannot load file at {path}. File does not exist!");
            throw new FileNotFoundException($"{path} does not exist!");
        }

        T data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
        return data;
    }
}

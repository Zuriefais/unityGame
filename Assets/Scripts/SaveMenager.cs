using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using System;
using System.Linq;

public class SaveMenager
{
    public static void Save<T>(T sv, string fileName)
    {
        string jsonSave = JsonConvert.SerializeObject(sv);
        string path = Path.Combine(Application.dataPath, fileName);
        File.WriteAllText(path, jsonSave);
    }

    public static T Load<T>(string fileName) where T: new()
    {
        string path = Path.Combine(Application.dataPath, fileName);
        string file;
        try {
            file = File.ReadAllText(path);
        }
        catch {
            path = Path.Combine(Application.dataPath, fileName);
            File.WriteAllText(path, "");
            file = null;
        }
        return JsonConvert.DeserializeObject<T>(file);
    }

    public static void SaveList<T>(List<T> sv, string fileName)
    {
        string jsonSave = JsonConvert.SerializeObject(sv);
        string path = Path.Combine(Application.dataPath, fileName);
        File.WriteAllText(path, jsonSave);
    }

    public static List<T> ReadList<T>(string fileName)
    {
        string path = Path.Combine(Application.dataPath, fileName);
        string file;
        try
        {
            file = File.ReadAllText(path);
        }
        catch
        {
            path = Path.Combine(Application.dataPath, fileName);
            File.WriteAllText(path, "");
            file = null;
        }

        List<T> res = JsonHelper.FromJson<T>(file).ToList();

        return res;

    }

}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}

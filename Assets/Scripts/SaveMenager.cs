using System.IO;
using UnityEngine;

public class SaveMenager
{
    public static void Save<T>(T sv, string fileName)
    {
        string jsonSave = JsonUtility.ToJson(sv);
        string path = Path.Combine(Application.dataPath,fileName);
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
        
        return JsonUtility.FromJson<T>(file);
    }

}

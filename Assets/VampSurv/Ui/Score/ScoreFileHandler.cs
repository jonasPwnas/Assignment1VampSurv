
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ScoreFileHandler
{
    private static string scoreFileName = "HorribleNightScores.json";
    
    public static void SaveToJson(ScoreEntry toSave)
    {
        List<ScoreEntry> tempList = ReadFromJson<ScoreEntry>();
        tempList.Add(toSave);
        if (tempList.Count > 10)
        {
            tempList.Sort((a, b) => b.scoreScore.CompareTo(a.scoreScore));
            tempList.RemoveRange(10, tempList.Count - 10);
        }
        
        Debug.Log("Saving score to file: " + scoreFileName);
        string content = JsonHelper.ToJson<ScoreEntry>(tempList.ToArray());
        WriteFile(GetPath(scoreFileName), content);
    }

    public static List<T> ReadFromJson<T>()
    {
        string content = ReadFile(GetPath(scoreFileName));
        if (string.IsNullOrEmpty(content) || content == "{}")
        {
            List<ScoreEntry> tempList = new List<ScoreEntry>(10);
            
            tempList.Add(new ScoreEntry("Bobson", 100612));
            tempList.Add(new ScoreEntry("Guts", 19777666));
            tempList.Add(new ScoreEntry("Biff", 1900612));
            tempList.Add(new ScoreEntry("Åke J", 17323411));
            tempList.Add(new ScoreEntry("Ulla-Klas", Random.Range(160000, 15251634)));
            tempList.Add(new ScoreEntry("Sniper-Bengt", Random.Range(160000, 15251634)));
            tempList.Add(new ScoreEntry("Naruto", Random.Range(160000, 15251634)));
            tempList.Add(new ScoreEntry("Griffith", 33));
            
            return tempList as List<T>;
        }
        
        List<T> toReturn = JsonHelper.FromJson<T>(content).ToList();
        return toReturn;
    }

    private static string GetPath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }

    private static void WriteFile(string path, string content)
    {
        FileStream fileStream = new FileStream(path, FileMode.Create);
        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(content);
        }
    }
    
    private static string ReadFile(string path)
    {
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                return reader.ReadToEnd();
            }
        }
        
        return "";
    }
    
}

public static class JsonHelper 
{
    public static T[] FromJson<T> (string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>> (json);
        return wrapper.Items;
    }

    public static string ToJson<T> (T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T> ();
        wrapper.Items = array;
        return JsonUtility.ToJson (wrapper);
    }

    public static string ToJson<T> (T[] array, bool prettyPrint) 
    {
        Wrapper<T> wrapper = new Wrapper<T> ();
        wrapper.Items = array;
        return JsonUtility.ToJson (wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T> 
    {
        public T[] Items;
    }
}
using System.IO;
using UnityEngine;
using SimpleJSON;

public class JSONReader : MonoBehaviour 
{
    public static JSONNode Read(string filename)
    {
        string jsonString = File.ReadAllText("Assets/Resources/" + filename + ".json");

        JSONNode list = JSON.Parse(jsonString);

        return list;
    }
}
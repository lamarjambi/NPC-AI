using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JSONManager : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GetJsonFromUrl("https://www.reddit.com/r/gaming.json", ReceivedJson));
    }

    IEnumerator GetJsonFromUrl(string url, System.Action<string> callback)
    {
        string jsonText;
        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("User-Agent", "UnityApp/1.0"); // Reddit requires this!!
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            jsonText = www.error;
        }
        else
        {
            jsonText = www.downloadHandler.text;
        }

        callback(jsonText);
        www.Dispose();
    }

    public void ReceivedJson(string jsonText)
    {
        JSONReceiver receiver = JsonUtility.FromJson<JSONReceiver>(jsonText);
        Child[] posts = receiver.data.children;

        foreach (Child post in posts)
        {
            print(post.data.score + " | " + post.data.title + " | " + post.data.author);
        }
    }
}
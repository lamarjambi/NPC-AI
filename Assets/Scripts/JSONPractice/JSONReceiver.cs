using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JSONReceiver
{
    public string kind;
    public ListingData data;
}

[System.Serializable]
public class ListingData
{
    public string after;
    public string before;
    public int dist;
    public Child[] children;
}

[System.Serializable]
public class Child
{
    public string kind;
    public PostData data;
}

[System.Serializable]
public class PostData
{
    public string id;
    public string title;
    public string author;
    public string url;
    public string permalink;
    public string thumbnail;
    public string selftext;
    public int score;
    public int num_comments;
    public float upvote_ratio;
    public bool is_video;
}
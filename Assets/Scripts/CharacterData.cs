// CharacterData.cs
using UnityEngine;

[CreateAssetMenu(menuName = "Chat/Character")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    [TextArea(4, 10)]
    public string systemPrompt; // e.g. "You are Xerla, a stoic robot warrior..."
}
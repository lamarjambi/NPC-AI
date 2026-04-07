using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ChatGPTWrapper;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Characters - must match order of characterNames")]
    [SerializeField] ChatGPTConversation[] chatGPTs;   
    [SerializeField] string[] characterNames;

    [Header("UI")]
    [SerializeField] TMP_InputField iF_PlayerTalk;
    [SerializeField] TextMeshProUGUI tX_AIReply;
    [SerializeField] GameObject chatPanel;
    [SerializeField] TextMeshProUGUI tX_CharacterName; 

    ChatGPTConversation activeChatGPT;
    string activeCharacterName;

    void Awake()
    {
        if (instance == null) instance = this;

        // Init all conversations upfront
        foreach (var c in chatGPTs) c.Init();
    }

    // Called by MugshotButton when player clicks a character
    public void SelectCharacter(int index)
    {
        activeChatGPT = chatGPTs[index];
        activeCharacterName = characterNames[index];

        tX_CharacterName.text = activeCharacterName;
        tX_AIReply.text = "";
        chatPanel.SetActive(true);

        // Send the opening greeting
        activeChatGPT.SendToChatGPT("{\"player_said\":\"Hi!\", \"instructions\":\"Give a short greeting only, 1-2 sentences max. Save the details for later.\"}");
    }

    public void ReceiveChatGPTReply(string message)
    {
        string displayText = message; // fallback: show raw reply if JSON parse fails

        try
        {
            // Strip markdown code fences (e.g. ```json ... ```)
            if (message.Contains("```"))
            {
                int newline = message.IndexOf('\n', message.IndexOf("```"));
                int fenceEnd = message.LastIndexOf("```");
                if (newline >= 0 && fenceEnd > newline)
                    message = message.Substring(newline + 1, fenceEnd - newline - 1).Trim();
            }

            // Extract the first {...} block in case of surrounding text
            int jsonStart = message.IndexOf('{');
            int jsonEnd = message.LastIndexOf('}');
            if (jsonStart >= 0 && jsonEnd > jsonStart)
                message = message.Substring(jsonStart, jsonEnd - jsonStart + 1);

            NPCJSONReceiver npcJSON = JsonUtility.FromJson<NPCJSONReceiver>(message);
            if (npcJSON != null && npcJSON.reply_to_player != null)
                displayText = npcJSON.reply_to_player;
        }
        catch (Exception e)
        {
            Debug.Log("JSON parse failed: " + e.Message);
        }

        tX_AIReply.text = "<color=#ff7082>" + activeCharacterName + ": </color>" + displayText;
    }

    public void SubmitChatMessage()
    {
        if (activeChatGPT == null || iF_PlayerTalk.text == "") return;

        activeChatGPT.SendToChatGPT("{\"player_said\":\"" + iF_PlayerTalk.text + "\"}");
        iF_PlayerTalk.text = "";
    }

    public void CloseChat()
    {
        chatPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter)) SubmitChatMessage();
    }
}
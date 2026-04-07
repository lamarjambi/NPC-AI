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
    [SerializeField] ChatGPTConversation[] chatGPTs;   // one per character
    [SerializeField] string[] characterNames;           // e.g. "Uterra", "Mooksai", "Xerla"

    [Header("UI")]
    [SerializeField] TMP_InputField iF_PlayerTalk;
    [SerializeField] TextMeshProUGUI tX_AIReply;
    [SerializeField] GameObject chatPanel;              // the whole chat UI panel
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
        activeChatGPT.SendToChatGPT("{\"player_said\":\"Hello! Who are you?\"}");
    }

    public void ReceiveChatGPTReply(string message)
    {
        try
        {
            if (!message.EndsWith("}"))
            {
                if (message.Contains("}"))
                    message = message.Substring(0, message.LastIndexOf("}") + 1);
                else
                    message += "}";
            }
            message = message.Replace("\\", "\\\\");
            message = message.Replace("\\\\\"", "\\\"");

            NPCJSONReceiver npcJSON = JsonUtility.FromJson<NPCJSONReceiver>(message);
            tX_AIReply.text = "<color=#ff7082>" + activeCharacterName + ": </color>" + npcJSON.reply_to_player;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            tX_AIReply.text = "<color=#ff7082>" + activeCharacterName + ": </color>" + "...";
        }
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
        if (Input.GetButtonUp("Submit")) SubmitChatMessage();
    }
}
using System;
using System.Collections.Generic;
using HeneGames.DialogueSystem;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    public List<DialogueNode> nodes;
}

[Serializable]
public class DialogueNode
{
    public DialogueCharacter speaker;
    
    [TextArea(2, 5)]
    public string dialogueText;
    public List<DialogueChoice> choices;
    public UnityEvent onDialogueFinished;
}

[Serializable]
public class DialogueChoice
{
    public string choiceText;
    public DialogueData nextDialogue;
}

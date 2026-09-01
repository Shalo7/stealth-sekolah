using System.Collections.Generic;
using UnityEngine;

public class DialogueEvent : MonoBehaviour
{
    [SerializeField] List<DialogueData> dialogues;

    public void StartDialogueEvent(int dialogueIndex)
    {
        if (dialogues == null || dialogues.Count == 0)
        {
            Debug.LogWarning("No dialogues on " + gameObject.name);
            return;
        }
        if (dialogueIndex < 0 || dialogueIndex >= dialogues.Count)
        {
            Debug.LogWarning("Dialogue index out of range: " + dialogueIndex);
            return;
        }

        DiaManager.instance.StartDia(dialogues[dialogueIndex]);
    }
}

using System.Collections;
using ElmanGameDevTools.PlayerSystem;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DiaManager : MonoBehaviour
{
    public static DiaManager instance;
    [Header("UI")]
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] Image speakerBust;
    [SerializeField] TMP_Text speakerNameText;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] Transform choiceContainer;
    [SerializeField] Button choiceButtonPrefab;

    [Header("Lock Player")]
    [SerializeField] PlayerController playerController;
    //[SerializeField] MonoBehaviour playerInteract;

    [Header("Typewriter")]
    [SerializeField] float typingSpeed = 0.03f;

    public bool isDialogueActive { get; private set; }

    private DialogueData currentDialogue;
    private int currentNodeIndex;

    private DialogueEvent currentDialogueEvent;

    Coroutine typingCoroutine;
    bool isTyping;

    private void Awake()
    {
        if (instance != this && instance != null) return;
        instance = this;
        
        dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (!isDialogueActive) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                FinishTyping();
            }
            else
            {
                ContinueButton();
            }
        }
    }

    public void StartDia(DialogueData dialogue, DialogueEvent dE)
    {
        if (dialogue == null) return;
        currentDialogue = dialogue;
        currentNodeIndex = 0;
        
        isDialogueActive = true;
        LockPlayer();

        dialoguePanel.SetActive(true);

        ShowNode();
        currentDialogueEvent = dE;
        DirectPlayerToNPC(true);
    }

    private void ShowNode()
    {
        ClearChoices();
        
        DialogueNode node = currentDialogue.nodes[currentNodeIndex];

        speakerNameText.text = node.speaker.characterName;
        speakerBust.sprite = node.speaker.characterPhoto;
        
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeDialogue(node.dialogueText));
        //dialogueText.text = node.dialogueText;

        if (node.choices.Count > 0)
        {
            foreach (DialogueChoice choice in node.choices)
            {
                ChoiceButton(choice);
            }
        }
    }

    IEnumerator TypeDialogue(string text)
    {
        isTyping = true;

        dialogueText.text = "";

        foreach (char letter in text)
        {
            dialogueText.text += letter;

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        typingCoroutine = null;
    }

    void FinishTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        DialogueNode node = currentDialogue.nodes[currentNodeIndex];

        dialogueText.text = node.dialogueText;

        isTyping = false;
    }

    private void ContinueButton()
    {
        currentNodeIndex++;

        if (currentNodeIndex >= currentDialogue.nodes.Count)
        {
            EndDialogue();
            return;
        }
        
        ShowNode();
        // Button button = Instantiate(choiceButtonPrefab, choiceContainer);

        // button.GetComponentInChildren<TMP_Text>().text = "Continue";

        // button.onClick.AddListener(() =>
        // {
        //     currentNodeIndex++;
        //     if (currentNodeIndex >= currentDialogue.nodes.Count)
        //     {
        //         EndDialogue();
        //     }
        //     else
        //     {
        //         ShowNode();
        //     }
        // });
    }

    private void ChoiceButton(DialogueChoice choice)
    {
        Button button = Instantiate(choiceButtonPrefab, choiceContainer);

        button.GetComponentInChildren<TMP_Text>().text = choice.choiceText;

        button.onClick.AddListener(() =>
        {
            if (choice.nextDialogue != null)
            {
                StartDia(choice.nextDialogue, currentDialogueEvent);
            }
            else
            {
                EndDialogue();
            }
        });
    }

    private void ClearChoices()
    {
        foreach (Transform child in choiceContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        DirectPlayerToNPC(false);
        UnlockPlayer();

        dialoguePanel.SetActive(false);

        currentDialogue = null;
        currentDialogueEvent = null;
    }

    private void LockPlayer()
    {
        playerController.enabled = false;
        //playerInteract.enabled = false;
    }

    private void UnlockPlayer()
    {
        playerController.enabled = true;
        //playerInteract.enabled = true;
    }

    //Directs player attention to the NPC they are interacting
    Coroutine CO_DirectPlayerToNPC;
    private float directLookSpd = 3f;
    IEnumerator IE_DirectPlayerToNPC()
    {
        while(true)
        {
            if (currentDialogueEvent == null) {CO_DirectPlayerToNPC = null; break;}
            Vector3 dirObject = currentDialogueEvent.transform.position - playerController.transform.position;
            dirObject.y = 0f;
            Quaternion targetRotationObject = Quaternion.LookRotation(dirObject);
            Quaternion cameraRotationObject = Quaternion.Euler(Vector3.zero);
        
            if (dirObject.sqrMagnitude > 0.001f)
            {
                playerController.transform.rotation = Quaternion.Slerp(playerController.transform.rotation, targetRotationObject, directLookSpd * Time.deltaTime);
            }

            playerController.playerCamera.transform.rotation = Quaternion.Slerp(playerController.playerCamera.transform.rotation, targetRotationObject, directLookSpd * Time.deltaTime);
            
            yield return null; 
        }
    }
    private void DirectPlayerToNPC(bool t)
    {
        if (playerController == null) return;
        if (CO_DirectPlayerToNPC != null) return;
        if (t)
        {
            CO_DirectPlayerToNPC = StartCoroutine(IE_DirectPlayerToNPC());     
        }
        else
        {
            if (CO_DirectPlayerToNPC == null) return;
            StopCoroutine(CO_DirectPlayerToNPC);
            CO_DirectPlayerToNPC = null;
        }
    }
}

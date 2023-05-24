using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Npc : MonoBehaviour
{

    public string[] dialogueLines;  // Array to hold the dialogue lines
    public TextMeshProUGUI dialogueText;  // Reference to the TextMeshProUGUI component
    public float scrollSpeed = 0.05f;  // Scroll speed for typing text
    public GameObject dialogueBox;  // Reference to the dialogue box object

    private bool canInteract = false;
    private bool isDialogueActive = false;
    private int currentLine = 0;

    private bool isTyping = false;  // Flag to check if text is being typed
    private string currentText = "";  // Current partially typed text
    private Coroutine typingCoroutine;  // Reference to the typing coroutine

    void Update()
    {
        // Check for player input to interact with the NPC
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            if (!isDialogueActive)
            {
                StartCoroutine(StartDialogue());
            }
            else
            {
                if (!isTyping)
                {
                    if (currentLine < dialogueLines.Length - 1)
                    {
                        currentLine++;
                        StartTyping(dialogueLines[currentLine]);
                    }
                    else
                    {
                        isDialogueActive = false;
                        currentLine = 0;
                        ClearDialogueText();
                        dialogueBox.SetActive(false);  // Disable the dialogue box object
                    }
                }
                else if (isTyping)
                {
                    StopCoroutine(typingCoroutine);
                    dialogueText.text = dialogueLines[currentLine];
                    isTyping = false;
                }
            }
        }
    }

    IEnumerator StartDialogue()
    {
        isDialogueActive = true;
        currentLine = 0;
        StartTyping(dialogueLines[currentLine]);
        dialogueBox.SetActive(true);  // Enable the dialogue box object

        while (isDialogueActive && currentLine < dialogueLines.Length)
        {
            // Wait for player input to proceed to the next line
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.KeypadEnter));

            if (currentLine < dialogueLines.Length - 1)
            {
                currentLine++;
                StartTyping(dialogueLines[currentLine]);
            }
            else
            {
                isDialogueActive = false;
                currentLine = 0;
                ClearDialogueText();
                dialogueBox.SetActive(false);  // Disable the dialogue box object
            }
        }
    }

    void StartTyping(string text)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        currentText = "";
        typingCoroutine = StartCoroutine(TypeText(text));
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;

        for (int i = 0; i <= text.Length; i++)
        {
            currentText = text.Substring(0, i);
            dialogueText.text = currentText;
            yield return new WaitForSeconds(scrollSpeed);  // Adjust the scroll speed

            // Check for player input to instantly show the full text
            if (Input.GetKeyDown(KeyCode.E))
            {
                dialogueText.text = text;
                break;
            }
        }

        isTyping = false;
    }

    void ClearDialogueText()
    {
        dialogueText.text = "";
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player enters the NPC's trigger zone
        if (other.CompareTag("Player"))
        {
            canInteract = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player exits the NPC's trigger zone
        if (other.CompareTag("Player"))
        {
            canInteract = false;
            ClearDialogueText();
            dialogueBox.SetActive(false);  // Disable the dialogue box object
        }
    }
}

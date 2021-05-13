using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    [System.Serializable]
    public struct DialogueItem
    {
        public bool self;
        public string text;
    };

    [SerializeField] GameObject      system;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] Image           portrait;

    
    DialogueItem[]  currentDialogue;
    int             dialogueIndex;
    Character       selfCharacter;
    Character       otherCharacter;
    SpriteRenderer  selfSpriteRenderer;
    SpriteRenderer  otherSpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        system.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentDialogue != null)
        {
            if (Input.GetButtonDown("Use"))
            {
                dialogueIndex++;

                if (dialogueIndex < currentDialogue.Length)
                {
                    DisplayDialogue();
                }
                else
                {
                    system.SetActive(false);
                    currentDialogue = null;

                    Character[] characters = FindObjectsOfType<Character>();
                    foreach (var character in characters)
                    {
                        character.enabled = true;
                    }
                }
            }
        }
    }

    public void StartDialog(DialogueItem[] dialogue, SpriteRenderer selfObject, SpriteRenderer otherObject)
    {
        Character[] characters = FindObjectsOfType<Character>();
        foreach (var character in characters)
        {
            character.enabled = false;
        }

        system.SetActive(true);

        currentDialogue = dialogue;
        selfSpriteRenderer = selfObject;
        selfCharacter = selfSpriteRenderer.GetComponent<Character>();
        if (selfCharacter == null)
        {
            selfCharacter = selfSpriteRenderer.GetComponentInParent<Character>();
        }
        otherSpriteRenderer = otherObject;
        otherCharacter = otherSpriteRenderer.GetComponent<Character>();
        if (otherCharacter == null)
        {
            otherCharacter = otherSpriteRenderer.GetComponentInParent<Character>();
        }
        dialogueIndex = 0;

        DisplayDialogue();
    }

    void DisplayDialogue()
    {
        dialogueText.text = currentDialogue[dialogueIndex].text;

        if (currentDialogue[dialogueIndex].self)
        {
            nameText.text = selfCharacter.name;
            portrait.sprite = selfSpriteRenderer.sprite;
            portrait.color = selfSpriteRenderer.color;
        }
        else
        {
            nameText.text = otherCharacter.name;
            portrait.sprite = otherSpriteRenderer.sprite;
            portrait.color = otherSpriteRenderer.color;
        }
    }
}

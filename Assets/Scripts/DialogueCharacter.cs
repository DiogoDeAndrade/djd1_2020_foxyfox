using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCharacter : Character
{
    [SerializeField]
    DialogueSystem.DialogueItem[]  dialogue;

    public void StartDialogue(SpriteRenderer otherObject)
    {
        DialogueSystem ds = FindObjectOfType<DialogueSystem>();
        ds.StartDialog(dialogue, spriteRenderer, otherObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActionController : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    TypewriterEffect typewriterEffect;
    public string[] stringArray;

    public void Start(){
        Debug.Log("Text started.");
    }

    public void ButtonClicked()
    {
        Debug.Log("Text changed.");
        typewriterEffect = dialogueText.GetComponent<TypewriterEffect>();
        typewriterEffect.stringArray = this.stringArray;
        typewriterEffect.i = 0;
        typewriterEffect.Start();
        Debug.Log("Text changed.");
    }
}

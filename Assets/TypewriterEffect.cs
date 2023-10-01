using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textMeshPro;
	[SerializeField] AudioSource dialogueBit;
	[SerializeField] AudioSource dialogueClose;

    public string[] stringArray;

    [SerializeField] float timeBtwnChars;
    [SerializeField] float timeBtwnSentences;

    public int i = 0;

    public void Start()
    {
		timeBtwnChars = 0.036F;
		timeBtwnSentences = 1.4F;
        EndCheck();
    }

    public void EndCheck()
    {
        if (i <= stringArray.Length - 1)
        {
            _textMeshPro.text = stringArray[i];
            StartCoroutine(TextVisible());
        }
    }

    private IEnumerator TextVisible()
    {
        _textMeshPro.ForceMeshUpdate();
        int totalVisibleCharacters = _textMeshPro.textInfo.characterCount;
        int counter = 0;

        while (true)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);
            _textMeshPro.maxVisibleCharacters = visibleCount;

            if (visibleCount >= totalVisibleCharacters)
            {
                i += 1;
				dialogueClose.Play();
                Invoke("EndCheck", timeBtwnSentences);
                break;
            }

            counter += 1;
			dialogueBit.Play();
            yield return new WaitForSeconds(timeBtwnChars);
        }
    }
}
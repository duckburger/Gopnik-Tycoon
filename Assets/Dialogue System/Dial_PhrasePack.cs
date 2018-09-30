using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PhraseType
{
    preAction,
    duringAction,
    postAction,
    preActPosResponse,
    preActNegResponse
}
[CreateAssetMenu(menuName = "Gopnik/PhraseAsset")]
public class Dial_PhrasePack : ScriptableObject
{
    [TextArea(3, 10)]
    [SerializeField] List<string> preActionPhrases = new List<string>();
    [TextArea(3, 10)]
    [SerializeField] List<string> duringActionPhrases = new List<string>();
    [TextArea(3, 10)]
    [SerializeField] List<string> postActionPhrases = new List<string>();


    [TextArea(3, 10)]
    [SerializeField] List<string> preActionPosResponses = new List<string>();
    [TextArea(3, 10)]
    [SerializeField] List<string> preActionNegResponses = new List<string>();

    public string GetRandomPhrase(PhraseType typeToGet)
    {
        int index;
        switch (typeToGet)
        {
            case PhraseType.preAction:
                index = Random.Range(0, preActionPhrases.Count);
                return this.preActionPhrases[index];
                break;
            case PhraseType.duringAction:
                index = Random.Range(0, duringActionPhrases.Count);
                return this.duringActionPhrases[index];
                break;
            case PhraseType.postAction:
                index = Random.Range(0, postActionPhrases.Count);
                return this.postActionPhrases[index];
                break;
            case PhraseType.preActPosResponse:
                index = Random.Range(0, preActionPosResponses.Count);
                return this.preActionPosResponses[index];
                break;
            case PhraseType.preActNegResponse:
                index = Random.Range(0, preActionNegResponses.Count);
                return this.preActionNegResponses[index];
                break;
            default:
                break;
        }
        return null;
    }
}

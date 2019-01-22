using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ModalWindowData", menuName = "Gopnik/Modal Window Data")]
public class ModalWindowData : ScriptableObject
{
    public string titleText;
    [TextArea(3, 10)]
    public string bodyText;
    public string acceptButtonText;
    public string declineButtonText;
    
}

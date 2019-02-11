using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Sprite List", menuName ="Gopnik/Sprite List")]
public class ScriptableSpriteList : ScriptableObject
{
    public List<Sprite> sprites = new List<Sprite>(); 
}

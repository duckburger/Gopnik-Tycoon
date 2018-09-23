using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(menuName = "Gopnik/Gopnik Data")]
public class ScriptableGopnikData : ScriptableObject
{
    public Sprite idleStateSprite;
    public Sprite fightingStateSprite;
    public Sprite chattingStateSprite;
    public Sprite razvodStateSprite;
}

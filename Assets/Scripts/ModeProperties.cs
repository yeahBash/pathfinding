using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Mode Properties", menuName = "Mode Properties")]
public class ModeProperties : ScriptableObject
{
    public EditMode mode;
    public Sprite buttonSprite;
    public Texture2D cursorTexture;
    public Color cellColor;
}

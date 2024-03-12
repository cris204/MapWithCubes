using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Block type", menuName = "ScriptableObjects/Blocks", order = 1)]
public class BlockTypeConfig : ScriptableObject
{
    public Sprite leftTexture;
    public Sprite rightTexture;
    public Sprite topTexture;
    public Sprite bottomTexture;
    public Sprite frontTexture;
    public Sprite backTexture;
}

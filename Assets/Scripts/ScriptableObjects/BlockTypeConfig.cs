using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Block type", menuName = "ScriptableObjects/Blocks", order = 1)]
public class BlockTypeConfig : ScriptableObject
{
    public BlockType BlockType;
    
    public Sprite LeftTexture;
    public Sprite RightTexture;
    public Sprite TopTexture;
    public Sprite BottomTexture;
    public Sprite FrontTexture;
    public Sprite BackTexture;
}

public enum BlockType
{
    Grass,
    Dirt,
    Stone,
    Lava,
    Snow,
    Air
}

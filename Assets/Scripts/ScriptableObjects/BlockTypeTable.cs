using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Block type Table", menuName = "ScriptableObjects/Blocks Table", order = 1)]
public class BlockTypeTable  : ScriptableObject
{
    public List<BlockTypeConfig> BlockTypeConfigs;

    public BlockTypeConfig GetBlockTypeByType(BlockType blockType)
    {
        foreach (BlockTypeConfig blockConfig in BlockTypeConfigs)
        {
            if (blockConfig.BlockType == blockType)
            {
                return blockConfig;
            }
        }

        return null;
    }
    
    
}

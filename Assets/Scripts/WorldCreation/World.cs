using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class World : MonoBehaviour
{
    public static int WORLD_HEIGHT = 8;
    public static int CHUNK_SIZE = 8;
    public static int WORLD_SIZE = 2;
    
    [SerializeField] private Material textureAtlas;
    [SerializeField] private BlockTypeTable blockTypeTable;
    
    public static Dictionary<string, Chunk> WORLD_CHUNKS;

    public static string BuildChunkName(Vector3 position)
    {
        return $"{position.x}_{position.y}_{position.z}";
    }

    private void Start()
    {
        WORLD_CHUNKS = new Dictionary<string, Chunk>();
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        StartCoroutine(BuildWorldHeight());
    }

    private IEnumerator BuildWorldHeight()
    {
        for (int z = 0; z < WORLD_SIZE; z++)
        {
            for (int x = 0; x < WORLD_SIZE; x++)
            {
                for (int y = 0; y < WORLD_HEIGHT; y++)
                {
                    Vector3 chunkPosition = new Vector3(x*CHUNK_SIZE, y * CHUNK_SIZE, z * CHUNK_SIZE);
                    Chunk chunk = new Chunk(chunkPosition, textureAtlas, blockTypeTable);
                    chunk.chunkGameObject.transform.parent = transform;
                    WORLD_CHUNKS.Add(chunk.chunkGameObject.name, chunk);
                }
            }
        }


        foreach (KeyValuePair<string, Chunk> chunk in WORLD_CHUNKS)
        {
            chunk.Value.DrawChunk();
            yield return null;
        }
    }
    
}

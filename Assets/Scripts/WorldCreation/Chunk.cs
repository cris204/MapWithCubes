using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class Chunk
{
    private Material cubeMaterial;
    public Block[,,] ChunkData;
    public GameObject chunkGameObject;
    private BlockTypeTable blockTypeTable;
    public Chunk(Vector3 position, Material cubeMaterial, BlockTypeTable blockTypeTable)
    {
        chunkGameObject = new GameObject(World.BuildChunkName(position));
        chunkGameObject.transform.position = position;
        this.cubeMaterial = cubeMaterial;
        this.blockTypeTable = blockTypeTable;
        BuildChunk();
    }

    public void BuildChunk()
    {
        ChunkData = new Block[World.CHUNK_SIZE, World.CHUNK_SIZE, World.CHUNK_SIZE];
        
        for (int z = 0; z < World.CHUNK_SIZE; z++)
        {
            for (int y = 0; y < World.CHUNK_SIZE; y++)
            {
                for (int x = 0; x < World.CHUNK_SIZE; x++)
                {
                    Vector3 position = new Vector3(x, y, z);
                    if (UnityEngine.Random.Range(0, 100) < 50)
                    {
                        ChunkData[x, y, z] = new Block(BlockType.Air, position, chunkGameObject, this, blockTypeTable.GetBlockTypeByType(BlockType.Air));
                    }
                    else
                    {
                        ChunkData[x, y, z] = new Block(BlockType.Snow, position, chunkGameObject, this, blockTypeTable.GetBlockTypeByType(BlockType.Snow));
                    }
                }
            }
        }
    }

    public void DrawChunk()
    {
        for (int z = 0; z < World.CHUNK_SIZE; z++)
        {
            for (int y = 0; y < World.CHUNK_SIZE; y++)
            {
                for (int x = 0; x < World.CHUNK_SIZE; x++)
                {
                    ChunkData[x,y,z].Draw();
                }
            }
        }

        CombineQuads();
    }
    
    private void CombineQuads()
    {
        // Combine children meshes
        MeshFilter[] meshFilters = chunkGameObject.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combineInstances = new CombineInstance[meshFilters.Length];

        int i = 0;

        while (i < meshFilters.Length)
        {
            combineInstances[i].mesh = meshFilters[i].sharedMesh;
            combineInstances[i].transform = meshFilters[i].transform.localToWorldMatrix;
            i++;
        }

        // Create a new mesh and add combined mesh
        MeshFilter meshFilter = (MeshFilter)chunkGameObject.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = new Mesh();
        meshFilter.mesh.CombineMeshes(combineInstances);
        
        //Create renderer
        MeshRenderer meshRenderer = (MeshRenderer)chunkGameObject.AddComponent(typeof(MeshRenderer));
        meshRenderer.material = cubeMaterial;
        
        //Delete uncombined children
        foreach (Transform quad in chunkGameObject.transform)
        {
            GameObject.Destroy(quad.gameObject);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Block
{
    private enum CubSide
    {
        Bottom,
        Top,
        Right,
        Left,
        Back,
        Front
    }
    
    private Material cubeMat;
    private BlockType blockType;
    private BlockTypeConfig blockTypeConfig;
    private Vector3 position;
    private GameObject parent;
    private bool isSolid;
    private Chunk chunkOwner;
    
    Vector2[,] blockUVs = { 
        /*Grass Top*/ {new Vector2( 0.75f, 0.1875f ), new Vector2( 0.8125f, 0.1875f),   new Vector2( 0.75f, 0.25f ),new Vector2( 0.8125f, 0.25f )},
        /*Grass Side*/{new Vector2( 0.1875f, 0.9375f ), new Vector2( 0.25f, 0.9375f),  new Vector2( 0.1875f, 1.0f ),new Vector2( 0.25f, 1.0f )},
        /*Dirt*/	  {new Vector2( 0.125f, 0.9375f ), new Vector2( 0.1875f, 0.9375f), new Vector2( 0.125f, 1.0f ),new Vector2( 0.1875f, 1.0f )},
        /*Stone*/	  {new Vector2( 0, 0.875f ), new Vector2( 0.0625f, 0.875f),  new Vector2( 0, 0.9375f ),new Vector2( 0.0625f, 0.9375f )},
        /*Lava*/	  {new Vector2( 0.875f, 0.0625f ), new Vector2( 0.9375f, 0.0625f), new Vector2( 0.875f, 0.125f ),new Vector2( 0.9375f, 0.125f )},
    };

    public Block(BlockType blockType, Vector3 position, GameObject parent, Chunk chunkOwner, BlockTypeConfig blockTypeConfig)
    {
        this.blockType = blockType;
        this.position = position;
        this.parent = parent;
        isSolid = blockType != BlockType.Air;
        this.chunkOwner = chunkOwner;
        this.blockTypeConfig = blockTypeConfig;
    }

    public void Draw()
    {
        if (blockType == BlockType.Air)
        {
            return;
        } 
        
        if(!HasSolidNeighbour((int)position.x,(int)position.y,(int)position.z + 1))
        {
            CreateQuad(CubSide.Front);
        }
        
        if(!HasSolidNeighbour((int)position.x,(int)position.y,(int)position.z - 1))
        {
            CreateQuad(CubSide.Back);
        }
        
        if(!HasSolidNeighbour((int)position.x,(int)position.y + 1,(int)position.z))
        {
            CreateQuad(CubSide.Top);
        }
        
        if(!HasSolidNeighbour((int)position.x,(int)position.y - 1,(int)position.z))
        {
            CreateQuad(CubSide.Bottom);
        }
        
        if(!HasSolidNeighbour((int)position.x + 1,(int)position.y,(int)position.z))
        {
            CreateQuad(CubSide.Right);
        }
        if(!HasSolidNeighbour((int)position.x - 1,(int)position.y,(int)position.z))
        {
            CreateQuad(CubSide.Left);
        }

    }

    private void CreateQuad(CubSide side)
    {
        Mesh mesh = new Mesh();
        mesh.name = $"S_Mesh_{side}";

        Vector3[] vertices = new Vector3[4];
        Vector3[] normals = new Vector3[4];
        Vector2[] uvs = new Vector2[4];
        int[] triangles = new int[6];
        
        //All possible vertices
        Vector3 p0 = new Vector3(-0.5f, -0.5f, 0.5f);
        Vector3 p1 = new Vector3(0.5f, -0.5f, 0.5f);
        Vector3 p2 = new Vector3(0.5f, -0.5f, -0.5f);
        Vector3 p3 = new Vector3(-0.5f, -0.5f, -0.5f);
        Vector3 p4 = new Vector3(-0.5f, 0.5f, 0.5f);
        Vector3 p5 = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 p6 = new Vector3(0.5f, 0.5f, -0.5f);
        Vector3 p7 = new Vector3(-0.5f, 0.5f, -0.5f);

        //all possible UVs
        Vector2 uv00;
        Vector2 uv10;
        Vector2 uv01;
        Vector2 uv11;

        if (blockType == BlockType.Grass)
        {
            if (side == CubSide.Top)
            {
                uv00 = blockUVs[0, 0];
                uv10 = blockUVs[0, 1];
                uv01 = blockUVs[0, 2];
                uv11 = blockUVs[0, 3];
            }
            else if (side == CubSide.Bottom)
            {
                // +1 because in the matrix we have two grass, so we need to get the dirt index +1
                uv00 = blockUVs[(int)BlockType.Dirt + 1, 0];
                uv10 = blockUVs[(int)BlockType.Dirt + 1, 1];
                uv01 = blockUVs[(int)BlockType.Dirt + 1, 2];
                uv11 = blockUVs[(int)BlockType.Dirt + 1, 3];
            }
            else 
            {
                uv00 = blockUVs[(int)blockType + 1, 0];
                uv10 = blockUVs[(int)blockType + 1, 1];
                uv01 = blockUVs[(int)blockType + 1, 2];
                uv11 = blockUVs[(int)blockType + 1, 3];
            }
        }
        else if (blockType == BlockType.Snow)
        {
            // Order set by unity texture UVs
            if (side == CubSide.Top)
            {
                uv01 = blockTypeConfig.TopTexture.uv[0];
                uv10 = blockTypeConfig.TopTexture.uv[1];
                uv11 = blockTypeConfig.TopTexture.uv[2];
                uv00 = blockTypeConfig.TopTexture.uv[3];
            }
            else if (side == CubSide.Bottom)
            {
                uv01 = blockTypeConfig.BottomTexture.uv[0];
                uv10 = blockTypeConfig.BottomTexture.uv[1];
                uv11 = blockTypeConfig.BottomTexture.uv[2];
                uv00 = blockTypeConfig.BottomTexture.uv[3];
            }
            else 
            {
                uv01 = blockTypeConfig.LeftTexture.uv[0];
                uv10 = blockTypeConfig.LeftTexture.uv[1];
                uv11 = blockTypeConfig.LeftTexture.uv[2];
                uv00 = blockTypeConfig.LeftTexture.uv[3];
            }
        }
        else
        {
            uv00 = blockUVs[(int)blockType + 1, 0];
            uv10 = blockUVs[(int)blockType + 1, 1];
            uv01 = blockUVs[(int)blockType + 1, 2];
            uv11 = blockUVs[(int)blockType + 1, 3];
        }
        


        //The order of vertices is important to generate the triangles triangles = new int[] { 3, 1, 0, 3, 2, 1 };
        switch (side)
        {
            case CubSide.Front:
                vertices = new Vector3[] { p4, p5, p1, p0 };
                normals = new Vector3[] {Vector3.forward,
                    Vector3.forward,
                    Vector3.forward,
                    Vector3.forward};


                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case CubSide.Bottom:
                vertices = new Vector3[] { p0, p1, p2, p3 };
                normals = new Vector3[] {Vector3.down,
                    Vector3.down,
                    Vector3.down,
                    Vector3.down};


                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case CubSide.Top:
                vertices = new Vector3[] { p7, p6, p5, p4 };
                normals = new Vector3[] {Vector3.up,
                    Vector3.up,
                    Vector3.up,
                    Vector3.up};


                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                
                break;
            case CubSide.Right:
                vertices = new Vector3[] { p5, p6, p2, p1 };
                normals = new Vector3[] {Vector3.right,
                    Vector3.right,
                    Vector3.right,
                    Vector3.right};


                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case CubSide.Left:
                vertices = new Vector3[] { p7, p4, p0, p3 };
                normals = new Vector3[] {Vector3.left,
                    Vector3.left,
                    Vector3.left,
                    Vector3.left};


                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case CubSide.Back:
                vertices = new Vector3[] { p6, p7, p3, p2 };
                normals = new Vector3[] {Vector3.back,
                    Vector3.back,
                    Vector3.back,
                    Vector3.back};


                uvs = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangles = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
        }


        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();

        GameObject quad = new GameObject($"quad_{side}");
        quad.transform.position = position;
        quad.transform.parent = parent.transform;
        MeshFilter meshFilter = (MeshFilter) quad.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = mesh;
        

        //This is using to see how is creating block by block but not required
        // MeshRenderer meshRenderer = (MeshRenderer) quad.AddComponent(typeof(MeshRenderer));
        // meshRenderer.material = cubeMat;

    }

    private int ConvertBlockIndexToLocal(int value)
    {
        // If the value is -1 we are crossing to the neighbour and we will get the bigger index of neighbour chunk
        if (value == -1)
        {
            value = World.CHUNK_SIZE - 1;
        }
        // If the value is World.CHUNK_SIZE we are in the limit of the chunk, so we need to get the index 0 from next chunk
        else if (value == World.CHUNK_SIZE)
        {
            value = 0;
        }

        return value;
    }

    public bool HasSolidNeighbour(int x, int y, int z)
    {
        Block[,,] chunks;
        
        if (x < 0 || x >= World.CHUNK_SIZE ||
            y < 0 || y >= World.CHUNK_SIZE ||
            z < 0 || z >= World.CHUNK_SIZE)
        {
            Vector3 neighbourChunkPos = parent.transform.position + new Vector3(
                                            (x - (int)position.x) * World.CHUNK_SIZE,
                                            (y - (int)position.y) * World.CHUNK_SIZE,
                                            (z - (int)position.z) * World.CHUNK_SIZE);

            string neighbourChunkName = World.BuildChunkName(neighbourChunkPos);

            x = ConvertBlockIndexToLocal(x);
            y = ConvertBlockIndexToLocal(y);
            z = ConvertBlockIndexToLocal(z);

            Chunk neighbourChunk;

            if (World.WORLD_CHUNKS.TryGetValue(neighbourChunkName, out neighbourChunk))
            {
                chunks = neighbourChunk.ChunkData;
            }
            else
            {
                return false;
            }
            
        }
        else
        {
            chunks = chunkOwner.ChunkData;
        }
        
        // Other way is calculating if the min and max is inside chunk limits (X >= Chunk X limit) { Return false }
        try
        {
            return chunks[x, y, z].isSolid;
        }
        catch(IndexOutOfRangeException ex){}

        return false;

    }
    
}

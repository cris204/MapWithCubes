using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateQuads : MonoBehaviour
{
    [SerializeField] private Material cubeMat;
    [SerializeField] private BlockType blockType;
    [SerializeField] private BlockTypeConfig blockTypeConfig;

    private enum CubSide
    {
        Bottom,
        Top,
        Right,
        Left,
        Back,
        Front
    }
    private enum BlockType
    {
        Grass,
        Dirt,
        Stone,
        Lava,
        Snow
    }
    
    Vector2[,] blockUVs = { 
        /*Grass Top*/ {new Vector2( 0.125f, 0.375f ), new Vector2( 0.1875f, 0.375f),   new Vector2( 0.125f, 0.4375f ),new Vector2( 0.1875f, 0.4375f )},
        /*Grass Side*/{new Vector2( 0.1875f, 0.9375f ), new Vector2( 0.25f, 0.9375f),  new Vector2( 0.1875f, 1.0f ),new Vector2( 0.25f, 1.0f )},
        /*Dirt*/	  {new Vector2( 0.125f, 0.9375f ), new Vector2( 0.1875f, 0.9375f), new Vector2( 0.125f, 1.0f ),new Vector2( 0.1875f, 1.0f )},
        /*Stone*/	  {new Vector2( 0, 0.875f ), new Vector2( 0.0625f, 0.875f),  new Vector2( 0, 0.9375f ),new Vector2( 0.0625f, 0.9375f )},
        /*Lava*/	  {new Vector2( 0.875f, 0.0625f ), new Vector2( 0.9375f, 0.0625f), new Vector2( 0.875f, 0.125f ),new Vector2( 0.9375f, 0.125f )},
        /*Snow Top*/	  {new Vector2( 0.125f, 0.6875f ), new Vector2( 0.1875f, 0.6875f), new Vector2( 0.125f, 0.75f ),new Vector2( 0.1875f, 0.75f )},
        /*Snow Side*/	  {new Vector2( 0.25f, 0.6875f ), new Vector2( 0.3125f, 0.6875f), new Vector2( 0.25f, 0.75f ),new Vector2( 0.3125f, 0.75f )}
    };

    private void Start()
    {
        CreateQuad(CubSide.Front);
        CreateQuad(CubSide.Bottom);
        CreateQuad(CubSide.Top);
        CreateQuad(CubSide.Right);
        CreateQuad(CubSide.Left);
        CreateQuad(CubSide.Back);
        
        CombineQuads();
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
        }else if (blockType == BlockType.Snow)
        {
            // Order set by unity texture UVs
            if (side == CubSide.Top)
            {
                uv01 = blockTypeConfig.topTexture.uv[0];
                uv10 = blockTypeConfig.topTexture.uv[1];
                uv11 = blockTypeConfig.topTexture.uv[2];
                uv00 = blockTypeConfig.topTexture.uv[3];
            }
            else if (side == CubSide.Bottom)
            {
                uv01 = blockTypeConfig.bottomTexture.uv[0];
                uv10 = blockTypeConfig.bottomTexture.uv[1];
                uv11 = blockTypeConfig.bottomTexture.uv[2];
                uv00 = blockTypeConfig.bottomTexture.uv[3];
            }
            else 
            {
                uv01 = blockTypeConfig.leftTexture.uv[0];
                uv10 = blockTypeConfig.leftTexture.uv[1];
                uv11 = blockTypeConfig.leftTexture.uv[2];
                uv00 = blockTypeConfig.leftTexture.uv[3];
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
        quad.transform.parent = gameObject.transform;
        MeshFilter meshFilter = (MeshFilter) quad.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = mesh;
        MeshRenderer meshRenderer = (MeshRenderer) quad.AddComponent(typeof(MeshRenderer));
        meshRenderer.material = cubeMat;

    }

    private void CombineQuads()
    {
        // Combine children meshes
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combineInstances = new CombineInstance[meshFilters.Length];

        int i = 0;

        while (i < meshFilters.Length)
        {
            combineInstances[i].mesh = meshFilters[i].sharedMesh;
            combineInstances[i].transform = meshFilters[i].transform.localToWorldMatrix;
            i++;
        }

        // Create a new mesh and add combined mesh
        MeshFilter meshFilter = (MeshFilter)gameObject.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = new Mesh();
        meshFilter.mesh.CombineMeshes(combineInstances);
        
        //Create renderer
        MeshRenderer meshRenderer = (MeshRenderer)gameObject.AddComponent(typeof(MeshRenderer));
        meshRenderer.material = cubeMat;
        
        //Delete uncombined children
        foreach (Transform quad in transform)
        {
            Destroy(quad.gameObject);
        }
    }
}

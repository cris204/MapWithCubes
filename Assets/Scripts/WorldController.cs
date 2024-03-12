using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public GameObject block;
    public int worldSize = 2;

    private void Start()
    {
        StartCoroutine(WorldCreator());
    }

    private IEnumerator WorldCreator()
    {
        for (int z = 0; z < worldSize; z++)
        {
            for (int y = 0; y < worldSize; y++)
            {
                for (int x = 0; x < worldSize; x++)
                {
                    Vector3 position = new Vector3(x, y, z);
                    GameObject cube = Instantiate(block, position, Quaternion.identity);
                    cube.name = $"{x} {y} {z}";
                }

                yield return null;
            }
        }
    }
}

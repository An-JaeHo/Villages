using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTree : MonoBehaviour
{
    public GameObject treePrefeb;

    private Vector2 topLeft, bottomRigh;
    private List<Vector2> lastPos;
    private int spawnCount;
    private bool check;

    void Start()
    {
        topLeft = new Vector2(-12, 8);
        bottomRigh = new Vector2(13, -17);
        lastPos = new List<Vector2>();
        check = true;
        spawnCount = 50;
    }

    
    void Update()
    {
        if(check)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                SpawnTrees();

                if(i == 19)
                {
                    check = false;
                }
            }
        }
        
    }

    private void SpawnTrees()
    {
        Vector2 spawnPos = new Vector2((int)Random.Range(topLeft.x, bottomRigh.x), (int)Random.Range(topLeft.y, bottomRigh.y));

        if (lastPos != null)
        {
            for (int j = 0; j < lastPos.Count; j++)
            {
                if (lastPos[j] == spawnPos)
                {
                    Debug.Log("ss");
                    SpawnTrees();
                    return;
                }
            }
        }
        
        Instantiate(treePrefeb, spawnPos, Quaternion.identity);
        lastPos.Add(spawnPos);
    }
}

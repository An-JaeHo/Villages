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

        for (int i = -4; i < 5; i++)
        {
            for (int j = 3; j < 10; j++)
            {
                lastPos.Add(new Vector2(i, j));
            }
            
        }
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
                    SpawnTrees();
                    return;
                }
            }
        }
        
        GameObject tree =  Instantiate(treePrefeb, spawnPos, Quaternion.identity);

        tree.GetComponent<SpriteRenderer>().sortingOrder = 0;

        switch (spawnPos.y)
        {
            case 9:
                tree.GetComponent<SpriteRenderer>().sortingOrder = 0;
                break;
            case 8:
                tree.GetComponent<SpriteRenderer>().sortingOrder = 1;
                break;
            case 7:
                tree.GetComponent<SpriteRenderer>().sortingOrder = 2;
                break;
            case 6:
                tree.GetComponent<SpriteRenderer>().sortingOrder = 3;
                break;
            case 5:
                tree.GetComponent<SpriteRenderer>().sortingOrder = 4;
                break;
            case 4:
                tree.GetComponent<SpriteRenderer>().sortingOrder = 5;
                break;
            case 3:
                tree.GetComponent<SpriteRenderer>().sortingOrder = 6;
                break;
            case 2:
                tree.GetComponent<SpriteRenderer>().sortingOrder = 7;
                break;
            case 1:
                tree.GetComponent<SpriteRenderer>().sortingOrder = 8;
                break;
            case 0:
                tree.GetComponent<SpriteRenderer>().sortingOrder = 9;
                break;
            case -1:
                tree.GetComponent<SpriteRenderer>().sortingOrder = 10;
                break;
            case -2:
                tree.GetComponent<SpriteRenderer>().sortingOrder = 11;
                break;
            case -3:
                tree.GetComponent<SpriteRenderer>().sortingOrder = 12;
                break;
            case -4:
                tree.GetComponent<SpriteRenderer>().sortingOrder = 13;
                break;
            case -5:
                tree.GetComponent<SpriteRenderer>().sortingOrder = 14;
                break;
            case -6:
                tree.GetComponent<SpriteRenderer>().sortingOrder = 15;
                break;
            case -7:
                tree.GetComponent<SpriteRenderer>().sortingOrder = 16;
                break;
            case -8:
                tree.GetComponent<SpriteRenderer>().sortingOrder = 17;
                break;
            case -9:
                tree.GetComponent<SpriteRenderer>().sortingOrder = 18;
                break;
            case -10:
                tree.GetComponent<SpriteRenderer>().sortingOrder = 19;
                break;
            case -11:
                tree.GetComponent<SpriteRenderer>().sortingOrder = 20;
                break;
            case -12:
                tree.GetComponent<SpriteRenderer>().sortingOrder = 21;
                break;
            case -13:
                tree.GetComponent<SpriteRenderer>().sortingOrder = 22;
                break;
            case -14:
                tree.GetComponent<SpriteRenderer>().sortingOrder = 23;
                break;
            case -15:
                tree.GetComponent<SpriteRenderer>().sortingOrder = 24;
                break;
            case -16:
                tree.GetComponent<SpriteRenderer>().sortingOrder = 25;
                break;
            case -17:
                tree.GetComponent<SpriteRenderer>().sortingOrder = 26;
                break;
            case -18:
                tree.GetComponent<SpriteRenderer>().sortingOrder = 27;
                break;
            case -19:
                tree.GetComponent<SpriteRenderer>().sortingOrder = 28;
                break;
            case -20:
                tree.GetComponent<SpriteRenderer>().sortingOrder = 29;
                break;

            default:
                break;
        }

        lastPos.Add(spawnPos);
    }
}

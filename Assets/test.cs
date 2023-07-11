using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class test : MonoBehaviour
{

    public Tilemap map;
    public TileBase tile;

    private void Start()
    {
        FinishedBruning(new Vector3Int(0, 0, 0));
    }

    public void FinishedBruning(Vector3Int position)
    {
        map.SetTile(position, tile);
        map.RefreshAllTiles();
    }

    public void TryToSpread(Vector3Int position,float spreadChance)
    {
        
    }
}

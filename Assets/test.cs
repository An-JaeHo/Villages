using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public Tilemap map;
    public TileBase tile;
    public TileBase[] allTiles;
    private void Start()
    {
        FinishedBruning(new Vector3Int(0, 0, 0));

    }

    public void FinishedBruning(Vector3Int position)
    {
        map.SetTile(position, tile);
        map.RefreshAllTiles();

        BoundsInt size = map.cellBounds;
        allTiles = map.GetTilesBlock(size);
    }

    public void TryToSpread(Vector3Int position,float spreadChance)
    {
        
    }
}

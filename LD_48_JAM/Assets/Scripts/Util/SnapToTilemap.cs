using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SnapToTilemap : MonoBehaviour
{
    public Tilemap tilemap;
    
    private void OnDrawGizmos()
    {
        if(tilemap == null)
        {
            tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
        }
        if (!Application.isPlaying && transform.hasChanged)
        {
            SnapToTileMap();
        }
    }

    public void SnapToTileMap()
    {
        Vector3 snappedGrid = tilemap.GetCellCenterWorld(tilemap.WorldToCell(transform.position));
        
        transform.position = snappedGrid;
    }
}

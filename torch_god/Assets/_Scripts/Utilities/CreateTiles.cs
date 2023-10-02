using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class CreateFloors : MonoBehaviour
{

    public int width, height;

    public Vector2Int startPosition;

    public Tile tile;

    public Tilemap tilemap;

    public bool clearTiles = false;

    void Update()
    {
        
        if (Application.isEditor)
        {
            for (int i = 0; i < width; i++)
            {
                for(int j = 0; j < height;j++)
                {
                    tilemap.SetTile(new Vector3Int(startPosition.x + i, startPosition.y + j), tile);
                }
            }
            if (clearTiles)
            {
                tilemap.ClearAllTiles();
            }
        }

        
    }
}

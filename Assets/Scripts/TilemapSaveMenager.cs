using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Newtonsoft.Json;

public class TilemapSaveMenager : MonoBehaviour
{
    Dictionary<string, Tilemap> tilemaps = new Dictionary<string, Tilemap>();

    IEnumerator AutoSave()
    {
        while (true)
        {
            Save();
            yield return new WaitForSeconds(10f);
        }
    }

    void Start()
    {
        initTilemaps();
        StartCoroutine(AutoSave());
    }

    private void initTilemaps()
    {
        Tilemap[] maps = FindObjectsOfType<Tilemap>();
        foreach (Tilemap map in maps)
        {
            tilemaps.Add(map.name, map);
        }
    }

    void Save()
    {
        List<TilemapData> data = new List<TilemapData>();
        foreach (var tileMap in tilemaps)
        {
            TilemapData mapData = new TilemapData();
            mapData.key = tileMap.Key;
            foreach (var pos in tileMap.Value.cellBounds.allPositionsWithin)
            {
                if (tileMap.Value.HasTile(pos))
                {
                    TileBase levelTile = tileMap.Value.GetTile(pos);
                    var ti = new TileInfo(levelTile, pos);
                    mapData.tiles.Add(ti);
                }
            }
            data.Add(mapData);
        }
        string JsonData = JsonConvert.SerializeObject(data);
        SaveMenager.Save(data, "tilemap.json");
    }

    void Load()
    {
        List<TilemapData> data = SaveMenager.ReadList<TilemapData>("tilemap.json");
        foreach (var mapData in data)
        {
            if (!tilemaps.ContainsKey(mapData.key))
            {
                Debug.LogError("Found saved data for tilemap called '" + mapData.key + "', but Tilemap does not exist in scene.");
                continue;
            }

            var map = tilemaps[mapData.key];

            map.ClearAllTiles();

            if (mapData.tiles != null && mapData.tiles.Count > 0)
            {
                foreach (var tile in mapData.tiles)
                {
                    map.SetTile(tile.position, tile.tile);
                }
            }
        }
    }
    [Serializable]
    public class TilemapData
    {
        public string key;
        public List<TileInfo> tiles = new();
    }

    [Serializable]
    public class TileInfo
    {
        public TileBase tile;
        public Vector3Int position;
        public TileInfo(TileBase tile, Vector3Int pos)
        {
            this.tile = tile;
            position = pos;
        }
    }
}

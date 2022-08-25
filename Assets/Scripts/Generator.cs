using System;
using UnityEngine;

public class Generator : MonoBehaviour {

    public int mapWidth;
    public int mapHeight;
    public int seed;
    public float noiseScale;
    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;
    public Vector2 offset;
    public bool autoUpdate;

    public void Generate() {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

        MapDisplay display = FindObjectOfType<MapDisplay>();
        display.DrawNoiseMap(noiseMap);
    }

    private void OnValidate() {
        if (mapWidth < 1) {
            mapWidth = 1;
        }

        if (mapHeight < 1) {
            mapHeight = 1;
        }

        if (octaves < 0) {
            octaves = 0;
        }

        if (lacunarity < 1) {
            lacunarity = 1;
        }
    }
}
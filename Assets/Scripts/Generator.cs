using System;
using UnityEngine;

public class Generator : MonoBehaviour {
    
    public enum DrawMode {
        noise,
        color,
        mesh
    }

    public DrawMode drawMode;

    const int chunkSize = 241;
    [Range(0, 6)] public int levelOfDetail;
    
    public int seed;
    public float noiseScale;
    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;
    public Vector2 offset;
    public float heightMultiplier;
    public AnimationCurve meshHeightCurve;
    public bool autoUpdate;

    public TerrainType[] TerrainTypes;
    
    public void Generate() {
        float[,] noiseMap = Noise.GenerateNoiseMap(chunkSize, chunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);

        Color[] colorMap = new Color[chunkSize * chunkSize];
        for (int y = 0; y < chunkSize; y++) {
            for (int x = 0; x < chunkSize; x++) {
                float height = noiseMap[x, y];
                for (int i = 0; i < TerrainTypes.Length; i++) {
                    if (height < TerrainTypes[i].height) {
                        colorMap[y * chunkSize + x] = TerrainTypes[i].color;
                        break;
                    }
                }
            }
        }
        
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.noise) {
            display.DrawTexture(TextureGenerator.TextureFromNoiseMap(noiseMap));
        } else if (drawMode == DrawMode.color) {
            display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, chunkSize, chunkSize));
        } else if (drawMode == DrawMode.mesh) {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, heightMultiplier, meshHeightCurve, levelOfDetail),
                TextureGenerator.TextureFromColorMap(colorMap, chunkSize, chunkSize));
        }
    }

    private void OnValidate() {
        if (octaves < 0) {
            octaves = 0;
        }

        if (lacunarity < 1) {
            lacunarity = 1;
        }
    }
}

[System.Serializable]
public struct TerrainType {
    public string name;
    public float height;
    public Color color;
}
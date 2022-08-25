using System;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteTerrain : MonoBehaviour {
    public const float maxViewDistance = 300;
    public Transform viewer;

    public static Vector2 viewerPosition;
    private int chunkSize;
    private int chunksVisible;

    private Dictionary<Vector2, TerrainChunk> TerrainChunks = new Dictionary<Vector2, TerrainChunk>();
    private List<TerrainChunk> PreviousChunks = new List<TerrainChunk>();

    private void Start() {
        chunkSize = Generator.chunkSize - 1;
        chunksVisible = Mathf.RoundToInt(maxViewDistance / chunkSize);
    }

    private void Update() {
        Vector3 position = viewer.position;
        viewerPosition = new Vector2(position.x, position.z);
        UpdateChunks();
    }

    void UpdateChunks() {

        for (int i = 0; i < PreviousChunks.Count; i++) {
            PreviousChunks[i].SetVisible(false);
        }
        PreviousChunks.Clear();
        
        Vector2 currentChunk = new Vector2(Mathf.RoundToInt(viewerPosition.x / chunkSize),
            Mathf.RoundToInt(viewerPosition.y / chunkSize));

        for (int y = -chunksVisible; y <= chunksVisible; y++) {
            for (int x = -chunksVisible; x <= chunksVisible; x++) {
                Vector2 chunk = new Vector2(x, y) + currentChunk;
                if (TerrainChunks.ContainsKey(chunk)) {
                    TerrainChunks[chunk].UpdateChunk();
                    if (TerrainChunks[chunk].IsVisible()) {
                        PreviousChunks.Add(TerrainChunks[chunk]);
                    }
                }
                else {
                    TerrainChunks.Add(chunk, new TerrainChunk(chunk, chunkSize, transform));
                }
            }
        }
    }
    
    
    public class TerrainChunk {
        private GameObject meshObject;
        private Vector2 position;
        private Bounds Bounds;

        public TerrainChunk(Vector2 coord, int size, Transform parent) {
            position = coord * size;
            Bounds = new Bounds(position, Vector2.one * size);
            Vector3 pos3 = new Vector3(position.x, 0, position.y);
            
            meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            meshObject.transform.position = pos3;
            meshObject.transform.localScale = Vector3.one * size / 10f;
            meshObject.transform.parent = parent;
            
            SetVisible(false);
        }

        public void UpdateChunk() {
            float viewerDist = Mathf.Sqrt(Bounds.SqrDistance(viewerPosition));
            bool visible = viewerDist <= maxViewDistance;
            SetVisible(visible);
        }

        public void SetVisible(bool visible) {
            meshObject.SetActive(visible);
        }

        public bool IsVisible() {
            return meshObject.activeSelf;
        }
    }
}
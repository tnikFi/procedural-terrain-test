using UnityEngine;

public static class MeshGenerator {
    public static MeshData GenerateTerrainMesh(float[,] heightMap) {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        Vector2 startCorner = new Vector2((width - 1) / -2f, (height - 1) / 2f);

        MeshData meshData = new MeshData(width, height);
        int vertexIndex = 0;
        
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                meshData.vertices[vertexIndex] = new Vector3(startCorner.x + x, heightMap[x, y], startCorner.y - y);
                meshData.uvs[vertexIndex] = new Vector2(x / (float) width, y / (float) height);
                
                if (x < width - 1 && y < height - 1) {
                    meshData.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                    meshData.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        return meshData;
    }
}

public class MeshData {
    public Vector3[] vertices;
    public int[] tris;
    public Vector2[] uvs;

    private int triangleIndex;

    public MeshData(int width, int height) {
        vertices = new Vector3[width * height];
        uvs = new Vector2[width * height];
        tris = new int[(width - 1) * (height - 1) * 6];
    }

    public void AddTriangle(int a, int b, int c) {
        tris[triangleIndex] = a;
        tris[triangleIndex+1] = b;
        tris[triangleIndex+2] = c;
        triangleIndex += 3;
    }

    public Mesh CreateMesh() {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }
}
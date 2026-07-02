using UnityEngine;

public class MeshTriangleCounter : MonoBehaviour
{
    void Start()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>(true);

        long totalTriangles = 0;
        long totalVertices = 0;

        foreach (MeshFilter mf in meshFilters)
        {
            if (mf.sharedMesh == null)
                continue;

            Mesh mesh = mf.sharedMesh;

            totalVertices += mesh.vertexCount;

            for (int i = 0; i < mesh.subMeshCount; i++)
            {
                totalTriangles += (long)mesh.GetIndexCount(i) / 3;
            }
        }
        Debug.Log("MeshTriangleCounter =================================");
        Debug.Log("MeshTriangleCounter Mesh Filters: " + meshFilters.Length);
        Debug.Log("MeshTriangleCounter Total Vertices: " + totalVertices);
        Debug.Log("MeshTriangleCounter Total Triangles: " + totalTriangles);
        Debug.Log("MeshTriangleCounter =================================");
    }
}
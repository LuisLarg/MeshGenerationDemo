using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class MeshCreator : MonoBehaviour
{
    #region PUBLIC PROPERTIES
    public float width = 1;
    public float height = 1;
    public float length = 1;
    #endregion

    #region PRIVATE PROPERTIES
    private List<Vector3> m_vertices;
    private List<Vector3> m_normals;
    private List<Vector2> m_uvs;
    private List<int> m_indices;
    #endregion

    /// <summary>
    /// Make sure all list are instantiated.
    /// </summary>
    private void CheckInstances()
    {
        m_vertices = m_vertices ?? (m_vertices = new List<Vector3>());
        m_normals = m_normals ?? (m_normals = new List<Vector3>());
        m_indices = m_indices ?? (m_indices = new List<int>());
        m_uvs = m_uvs ?? (m_uvs = new List<Vector2>());
    }

    /// <summary>
    /// Add a triangle based on three indices.
    /// </summary>
    /// <param name="index1">First indice.</param>
    /// <param name="index2">Second indice.</param>
    /// <param name="index3">Third indice.</param>
    private void AddTriangle(int index1, int index2, int index3)
    {
        m_indices.Add(index1);
        m_indices.Add(index2);
        m_indices.Add(index3);
    }

    /// <summary>
    /// Create a custom mesh.
    /// </summary>
    /// <param name="name">Name of the mesh (optional)</param>
    /// <returns>The mesh created.</returns>
    private Mesh CreateMesh(string name = "mesh")
    {
        Mesh mesh = new Mesh();
        mesh.name = name;

        mesh.vertices = m_vertices.ToArray();
        mesh.triangles = m_indices.ToArray();

        // Check if we have the correct amount of normals
        if (m_normals.Count == m_vertices.Count)
            mesh.normals = m_normals.ToArray();

        // Check if we have the correct amount of uvs
        if (m_uvs.Count == m_vertices.Count)
            mesh.uv = m_uvs.ToArray();

        mesh.RecalculateBounds();

        return mesh;
    }

    /// <summary>
    /// Create a quad mesh and assing it to it's components.
    /// </summary>
    public void CreateQuadMesh()
    {
        ClearData();
        SetupQuadMesh(Vector3.zero, Vector3.right * width, Vector3.forward * length);
        Mesh quadMesh = CreateMesh("Quad");

        GetComponent<MeshFilter>().sharedMesh = quadMesh;
        GetComponent<MeshCollider>().sharedMesh = quadMesh;
        GetComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
    }

    /// <summary>
    /// Setup mesh data for creating a quad.
    /// </summary>
    /// <param name="offset">Te initial vertice position.</param>
    /// <param name="widthDir">To which direction it's width must grow.</param>
    /// <param name="lengthDir">To which direction it's length must grow.</param>
    private void SetupQuadMesh(Vector3 offset, Vector3 widthDir, Vector3 lengthDir)
    {
        // The middle position of the quad's face
        Vector3 normal = Vector3.Cross(lengthDir, widthDir).normalized;

        // Vertex 0
        m_vertices.Add(offset);
        m_uvs.Add(new Vector2(0, 0));
        m_normals.Add(normal);

        // Vertex 1
        m_vertices.Add(offset + lengthDir);
        m_uvs.Add(new Vector2(1, 0));
        m_normals.Add(normal);

        // Vertex 2
        m_vertices.Add(offset + lengthDir + widthDir);
        m_uvs.Add(new Vector2(1, 1));
        m_normals.Add(normal);

        // Vertex 3
        m_vertices.Add(offset + widthDir);
        m_uvs.Add(new Vector2(1, 1));
        m_normals.Add(normal);

        int baseIndex = m_vertices.Count - 4;

        AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
        AddTriangle(baseIndex, baseIndex + 2, baseIndex + 3);
    }

    /// <summary>
    /// Create a cube mesh and assing it to it's components.
    /// </summary>
    public void CreateCubeMesh()
    {
        ClearData();
        SetupCubeMesh(width, height, length);
        Mesh cubeMesh = CreateMesh("Cube");

        GetComponent<MeshFilter>().sharedMesh = cubeMesh;
        GetComponent<MeshCollider>().sharedMesh = cubeMesh;
        GetComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
    }

    /// <summary>
    /// Setup mesh data for creating a cube.
    /// </summary>
    /// <param name="width">It's width.</param>
    /// <param name="height">It's height.</param>
    /// <param name="length">It's lenght or depth.</param>
    private void SetupCubeMesh(float width, float height, float length)
    {
        Vector3 up = Vector3.up * height;
        Vector3 right = Vector3.right * width;
        Vector3 foward = Vector3.forward * length;

        // Set the pivot at the middle of the cube
        Vector3 farCorner = (up + right + foward) / 2;
        Vector3 nearCorner = -farCorner;

        // Starting from the nearest bottom left corner of the cube
        SetupQuadMesh(nearCorner, foward, right);
        SetupQuadMesh(nearCorner, right, up);
        SetupQuadMesh(nearCorner, up, foward);

        // Starting from the farest upper right corner of the cube
        SetupQuadMesh(farCorner, -right, -foward);
        SetupQuadMesh(farCorner, -up, -right);
        SetupQuadMesh(farCorner, -foward, -up);
    }

    /// <summary>
    /// Clear all mesh related data.
    /// </summary>
    private void ClearData()
    {
        CheckInstances();

        m_vertices.Clear();
        m_indices.Clear();
        m_normals.Clear();
        m_uvs.Clear();
    }
}

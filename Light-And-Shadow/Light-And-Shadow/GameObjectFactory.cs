using Light_And_Shadow.Behaviors;
using Light_And_Shadow.Components;
using Light_And_Shadow.Shapes;
using OpenTK_OBJ;
using OpenTK.Mathematics;

namespace Light_And_Shadow;

/// <summary>
/// Provides methods to load different game objects.
/// </summary>
public static class GameObjectFactory
{
    public static GameObject CreateTriangle(Game gameInstance)
    {
        Material triangleMaterial = new Material("Shaders/shader.vert", "Shaders/shader.frag");
        Renderer triangleRenderer = new Renderer(triangleMaterial, new TriangleMesh());
        GameObject triangleObject = new GameObject(gameInstance)
        {
            Renderer = triangleRenderer
        };
        triangleObject.Transform.Position = new Vector3(0, 0, -5);
        return triangleObject;
    }

    public static GameObject CreateCube(Game gameInstance)
    {
        Texture wallTexture = new Texture("Textures/wall.jpg");
        var uniforms = new Dictionary<string, object> { { "texture0", wallTexture } };
        Material cubeMaterial = new Material("Shaders/shader.vert", "Shaders/shader.frag", uniforms);
        Renderer cubeRenderer = new Renderer(cubeMaterial, new CubeMesh());
        GameObject cubeObject = new GameObject(gameInstance)
        {
            Renderer = cubeRenderer
        };
        cubeObject.Transform.Position = new Vector3(0, 0, 0);
        //cubeObject.AddComponent<MoveObjectBehaviour>();
        return cubeObject;
    }

    public static (GameObject, Mesh) CreateObjModel(Game gameInstance, string modelName)
    {
        var objLoader = new OBJLoader();
        objLoader.Load($"Models/{modelName}.obj");

        var modelData = new Model
        {
            Vertices = objLoader.Vertices,
            TextureCoords = objLoader.TextureCoords,
            Normals = objLoader.Normals, 
            Indices = objLoader.Indices.Select(i => (uint)i).ToList(),
            TextureIndices = objLoader.TextureIndices
        };


        // Build a complete interleaved vertex array
        List<float> vertexArray = new();
        List<uint> indices = new();

        Dictionary<(int, int, int), uint> indexMapping = new();


        for (int i = 0; i < modelData.Indices.Count; i++)
        {
            int vertexIndex = (int)modelData.Indices[i];
            int texCoordIndex = modelData.TextureIndices[i];
            int normalIndex = objLoader.NormalIndices[i];

            var key = (vertexIndex, texCoordIndex, normalIndex);

            if (!indexMapping.TryGetValue(key, out uint mappedIndex))
            {
                // Add position
                vertexArray.Add(modelData.Vertices[vertexIndex].X);
                vertexArray.Add(modelData.Vertices[vertexIndex].Y);
                vertexArray.Add(modelData.Vertices[vertexIndex].Z);

                // Add texcoord
                vertexArray.Add(modelData.TextureCoords[texCoordIndex].X);
                vertexArray.Add(modelData.TextureCoords[texCoordIndex].Y);

                // Add normal
                vertexArray.Add(modelData.Normals[normalIndex].X);
                vertexArray.Add(modelData.Normals[normalIndex].Y);
                vertexArray.Add(modelData.Normals[normalIndex].Z);

                mappedIndex = (uint)(vertexArray.Count / 8 - 1);
                indexMapping[key] = mappedIndex;
            }

            indices.Add(mappedIndex);
        }

        Mesh modelMesh = new Mesh(vertexArray.ToArray(), indices.ToArray(), 8);

        GameObject modelObject = new GameObject(gameInstance);
        

        return (modelObject, modelMesh);
    }
}
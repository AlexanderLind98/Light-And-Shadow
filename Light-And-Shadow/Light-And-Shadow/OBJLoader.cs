using System.Globalization;
using OpenTK.Mathematics;

namespace Light_And_Shadow
{
    /// <summary>
    /// A loader for OBJ files, supporting vertices, texture coordinates, normals, and faces.
    /// </summary>
    public class OBJLoader
    {
        // Loaded vertices, texture coordinates, and normals
        public List<Vector3> Vertices { get; private set; } = new();
        public List<Vector2> TextureCoords { get; private set; } = new();
        public List<int> TextureIndices { get; private set; } = new();
        public List<Vector3> Normals { get; private set; } = new();

        // Indices for the faces (for simplicity, this loader triangulates the faces using a triangle fan)
        public List<int> Indices { get; private set; } = new();

        /// <summary>
        /// Loads an OBJ file from the specified file path.
        /// </summary>
        /// <param name="filePath">The path to the OBJ file.</param>
        public void Load(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Skip empty lines.
                    var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 0)
                        continue;

                    try
                    {
                        switch (parts[0])
                        {
                            case "v": // Vertex position
                                if (parts.Length < 4)
                                {
                                    Console.WriteLine($"Warning: Invalid vertex line (expected at least 4 parts): {line}");
                                    continue;
                                }
                                Vertices.Add(new Vector3(
                                    float.Parse(parts[1], CultureInfo.InvariantCulture),
                                    float.Parse(parts[2], CultureInfo.InvariantCulture),
                                    float.Parse(parts[3], CultureInfo.InvariantCulture)
                                ));
                                break;

                            case "vt": // Texture coordinate
                                if (parts.Length < 3)
                                {
                                    Console.WriteLine($"Warning: Invalid texture coordinate line (expected at least 3 parts): {line}");
                                    continue;
                                }
                                TextureCoords.Add(new Vector2(
                                    float.Parse(parts[1], CultureInfo.InvariantCulture),
                                    float.Parse(parts[2], CultureInfo.InvariantCulture)
                                ));
                                break;

                            case "vn": // Vertex normal
                                if (parts.Length < 4)
                                {
                                    Console.WriteLine($"Warning: Invalid normal line (expected at least 4 parts): {line}");
                                    continue;
                                }
                                Normals.Add(new Vector3(
                                    float.Parse(parts[1], CultureInfo.InvariantCulture),
                                    float.Parse(parts[2], CultureInfo.InvariantCulture),
                                    float.Parse(parts[3], CultureInfo.InvariantCulture)
                                ));
                                break;

                            case "f": // Face
                                if (parts.Length < 4)
                                {
                                    Console.WriteLine($"Warning: Invalid face line: {line}");
                                    continue;
                                }

                                var vertexIndices = new List<int>();
                                var textureIndices = new List<int>();

                                for (int i = 1; i < parts.Length; i++)
                                {
                                    string[] subParts = parts[i].Split('/');

                                    // Vertex index
                                    int vIndex = int.Parse(subParts[0], CultureInfo.InvariantCulture) - 1;
                                    vertexIndices.Add(vIndex);

                                    // Texture coordinate index (if present)
                                    if (subParts.Length > 1 && !string.IsNullOrWhiteSpace(subParts[1]))
                                    {
                                        int vtIndex = int.Parse(subParts[1], CultureInfo.InvariantCulture) - 1;
                                        textureIndices.Add(vtIndex);
                                    }
                                }

                                // Triangulate faces using triangle fan approach
                                for (int i = 1; i < vertexIndices.Count - 1; i++)
                                {
                                    Indices.Add(vertexIndices[0]);
                                    Indices.Add(vertexIndices[i]);
                                    Indices.Add(vertexIndices[i + 1]);

                                    if (textureIndices.Count == vertexIndices.Count)
                                    {
                                        TextureIndices.Add(textureIndices[0]);
                                        TextureIndices.Add(textureIndices[i]);
                                        TextureIndices.Add(textureIndices[i + 1]);
                                    }
                                }
                                break;

                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing line: {line}\nException: {ex.Message}");
                    }
                }
            }
        }
    }
}

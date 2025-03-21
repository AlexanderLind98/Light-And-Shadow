using OpenTK.Mathematics;

namespace Light_And_Shadow
{
    /// <summary>
    /// Represents a 3D model containing geometry data.
    /// </summary>
    public class Model 
    {
        /// <summary>
        /// Gets or sets the list of vertex positions.
        /// </summary>
        public List<Vector3> Vertices { get; set; } = new List<Vector3>();

        /// <summary>
        /// Gets or sets the list of vertex normals.
        /// </summary>
        public List<Vector3> Normals { get; set; } = new List<Vector3>();

        /// <summary>
        /// Gets or sets the list of texture coordinates.
        /// </summary>
        public List<Vector2> TextureCoords { get; set; } = new List<Vector2>();

        /// <summary>
        /// Gets or sets the list of indices for drawing elements.
        /// </summary>
        public List<uint> Indices { get; set; } = new List<uint>();

        /// <summary>
        /// Gets or sets the list of texture indices for drawing elements.
        /// </summary>
        public List<int> TextureIndices { get; set; } = new List<int>(); 
    }
}
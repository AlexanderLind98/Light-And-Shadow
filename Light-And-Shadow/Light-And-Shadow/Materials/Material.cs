using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using Light_And_Shadow;

namespace OpenTK_OBJ
{
    /// <summary>
    /// Represents a material with a shader and uniforms.
    /// </summary>
    public class Material
    {
        protected Shader shader;
        protected Dictionary<string, object> uniforms = new Dictionary<string, object>();
        private Dictionary<int, Texture> textures = new Dictionary<int, Texture>();

        protected void UpdateUniforms()
        {
            foreach (KeyValuePair<string,object> uniform in uniforms)
            {
                SetUniform(uniform.Key, uniform.Value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Material"/> class.
        /// </summary>
        /// <param name="vertPath">Vertex shader path.</param>
        /// <param name="fragPath">Fragment shader path.</param>
        public Material(string vertPath, string fragPath)
        {
            shader = new Shader(vertPath, fragPath);
        }
        
        public Material(string vertPath,string fragPath, Dictionary<string,object> uniforms)
        {
            shader = new Shader(vertPath, fragPath);
            foreach (KeyValuePair<string,object> uniform in uniforms)
            {
                SetUniform(uniform.Key, uniform.Value);
            }
        }

        public Material()
        {
            shader = new Shader("Shaders/materialLightShader.vert", "Shaders/materialLightShader.frag");
        }

        /// <summary>
        /// Sets a uniform for the shader.
        /// </summary>
        /// <param name="name">The uniform name.</param>
        /// <param name="uniform">The uniform value.</param>
        public void SetUniform(string name, object uniform)
        {
            // Clean slate: Only set supported types explicitly.
            shader.Use();
            if (uniform is int uniformInt)
            {
                shader.SetInt(name, uniformInt);
            }
            else if (uniform is float uniformFloat)
            {
                shader.SetFloat(name, uniformFloat);
            }
            else if (uniform is Vector3 uniformVec3)
            {
                shader.SetVector3(name, uniformVec3);
            }
            else if (uniform is Matrix4 uniformMatrix)
            {
                shader.SetMatrix(name, uniformMatrix);
            }
            else if (uniform is Texture tex)
            {
                int textureUnit = textures.Count;
                shader.SetInt(name, textureUnit);
                textures.Add(textureUnit, tex);
            }
            else
            {
                Console.WriteLine("Unsupported shader uniform type.");
                return;
            }

            uniforms[name] = uniform;
        }

        /// <summary>
        /// Activates the shader and binds textures.
        /// </summary>
        public void UseShader()
        {
            foreach (var kv in textures)
            {
                kv.Value.Use(TextureUnit.Texture0 + kv.Key);
            }
            shader.Use();
        }
    }
}

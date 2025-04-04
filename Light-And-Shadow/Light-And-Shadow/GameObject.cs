﻿using System;
using System.Collections.Generic;
using Light_And_Shadow.Behaviors;
using Light_And_Shadow.Components;
using Light_And_Shadow.Worlds;
using OpenTK_OBJ;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Light_And_Shadow
{
    public class GameObject
    {
        // Using PascalCase properties for clarity.
        public Transform Transform { get; set; }
        public Renderer Renderer { get; set; }
        private GameWindow gameWindow;
        private List<Behaviour> behaviours = new List<Behaviour>();

        public GameObject(GameWindow gameWindow)
        {
            this.gameWindow = gameWindow;
            Transform = new Transform();
        }

        // Overload to directly assign a Renderer.
        public GameObject(Renderer renderer, GameWindow gameWindow) : this(gameWindow)
        {
            Renderer = renderer;
        }

        public void AddComponent<T>(params object?[] args) where T : Behaviour
        {
            // Always pass this GameObject and the GameWindow as the first parameters.
            int initialParameters = 2;
            int totalParams = (args?.Length ?? 0) + initialParameters;
            object?[] parameters = new object?[totalParams];
            parameters[0] = this;
            parameters[1] = gameWindow;
            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    parameters[i + 2] = args[i];
                }
            }
            Behaviour component = (Behaviour)Activator.CreateInstance(typeof(T), parameters);
            behaviours.Add(component);
        }

        public T GetComponent<T>() where T : Behaviour
        {
            foreach (var component in behaviours)
            {
                if (component is T found)
                    return found;
            }
            return null;
        }

        public void Update(FrameEventArgs args)
        {
            foreach (var behaviour in behaviours)
            {
                behaviour.Update(args);
            }
        }

        public void Draw(Matrix4 viewProjection, Matrix4 lightSpaceMatrix,  Camera camera, World currentWorld, int debugMode = 0)
        {
            if (Renderer != null)
            {
                // Calculate the model matrix.
                Matrix4 model = Transform.CalculateModel();
                
                // Calculate the Model-View-Projection matrix.
                Matrix4 mvp = model * viewProjection;
                
                // Draw the object.
                Renderer.Draw(mvp, lightSpaceMatrix, model, camera, debugMode, currentWorld);
            }
        }
        
        public void RenderDepth(Shader shader)
        {
            if (Renderer != null)
            {
                // Calculate the model matrix.
                Matrix4 model = Transform.CalculateModel();
                
                Renderer.RenderDepth(shader, model);
            }
        }
        
        public void Dispose()
        {
            Renderer?.Mesh?.Dispose();
            if (Renderer?.Material is IDisposable dMat)
                dMat.Dispose();
        }
    }
}

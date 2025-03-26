using Light_And_Shadow.Materials;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Light_And_Shadow.Worlds;

public class PresenterWorld : World
{
    private GameObject presenterOpen;
    private GameObject presenterClosed;
    private bool isSpeaking = false;

    public PresenterWorld(Game game) : base(game) { }

    protected override void ConstructWorld()
    {
        base.ConstructWorld();

        presenterClosed = new GameObjectBuilder(Game)
            .Model("Presenter_Closed")
            .Material(new mat_default())
            .Position(0, 0, 0)
            .Build();

        presenterOpen = new GameObjectBuilder(Game)
            .Model("Presenter_Open")
            .Material(new mat_default())
            .Position(0, 0, 0)
            .Build();

        GameObjects.Add(presenterClosed);
        GameObjects.Add(presenterOpen);

        UpdatePresenterVisibility();
    }

    public override void HandleInput(KeyboardState input)
    {
        // Skift mellem åben/lukket mund
        if (input.IsKeyPressed(Keys.M))
        {
            isSpeaking = !isSpeaking;
            UpdatePresenterVisibility();
        }

        // Skift mellem lyskomponenter
        if (input.IsKeyPressed(Keys.D1))
        {
            Game.DebugMode = 1; // Ambient
        }

        if (input.IsKeyPressed(Keys.D2))
        {
            Game.DebugMode = 2; // Diffuse
        }

        if (input.IsKeyPressed(Keys.D3))
        {
            Game.DebugMode = 3; // Specular
        }

        if (input.IsKeyPressed(Keys.D4))
        {
            Game.DebugMode = 0; // Full lighting
        }
    }

    private void UpdatePresenterVisibility()
    {
        presenterClosed.Renderer.Material.SetUniform("material.ambient", isSpeaking ? new Vector3(0.1f) : new Vector3(0.7f));
        presenterOpen.Renderer.Material.SetUniform("material.ambient", isSpeaking ? new Vector3(0.7f) : new Vector3(0.1f));
    }
}
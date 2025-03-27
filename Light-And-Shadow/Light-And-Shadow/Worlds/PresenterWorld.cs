using Light_And_Shadow.Materials;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Light_And_Shadow.Worlds;

/// <summary>
/// A world that presents slides using textured quads,
/// allowing the user to cycle through them interactively.
/// </summary>
public class PresenterWorld : World
{
    private readonly List<GameObject> _slides = new();
    private int _currentSlideIndex = 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="PresenterWorld"/> class.
    /// </summary>
    /// <param name="game">The game instance.</param>
    public PresenterWorld(Game game) : base(game) { }

    /// <inheritdoc />
    protected override void ConstructWorld()
    {
        base.ConstructWorld();

        // Load slides
        AddSlide("Textures/IntroSlide.png");
        AddSlide("Textures/RenderPipeline.png");
        AddSlide("Textures/VertexArray.png");
        AddSlide("Textures/Rasterization.png");

        UpdateSlideVisibility();
    }

    /// <inheritdoc />
    public override void HandleInput(KeyboardState input)
    {
        if (input.IsKeyPressed(Keys.P)) ChangeSlide(1);
        if (input.IsKeyPressed(Keys.O)) ChangeSlide(-1);

        // Toggle debug visualizations
        if (input.IsKeyPressed(Keys.D1)) Game.DebugMode = 1;
        if (input.IsKeyPressed(Keys.D2)) Game.DebugMode = 2;
        if (input.IsKeyPressed(Keys.D3)) Game.DebugMode = 3;
        if (input.IsKeyPressed(Keys.D4)) Game.DebugMode = 0;
    }

    /// <summary>
    /// Changes the current slide based on input direction.
    /// </summary>
    /// <param name="direction">Direction to move the slide index (+1 or -1).</param>
    private void ChangeSlide(int direction)
    {
        int newIndex = Math.Clamp(_currentSlideIndex + direction, 0, _slides.Count - 1);
        if (newIndex != _currentSlideIndex)
        {
            _currentSlideIndex = newIndex;
            Console.WriteLine($"Slide: {_currentSlideIndex + 1}/{_slides.Count}");
            UpdateSlideVisibility();
        }
    }

    /// <summary>
    /// Adds a textured quad slide to the world.
    /// </summary>
    /// <param name="texturePath">The file path to the slide texture.</param>
    private void AddSlide(string texturePath)
    {
        var slide = GameObjectFactory.CreateTexturedQuad(Game, texturePath);
        slide.Transform.Position = new Vector3(0, 0, 100); // Initially hidden
        _slides.Add(slide);
        GameObjects.Add(slide);
    }

    /// <summary>
    /// Updates the visibility of slides to ensure only the current one is shown.
    /// </summary>
    private void UpdateSlideVisibility()
    {
        for (int i = 0; i < _slides.Count; i++)
        {
            _slides[i].Transform.Position = (i == _currentSlideIndex)
                ? new Vector3(0, 0, 4.5f)     // In front of camera
                : new Vector3(0, 0, 10f);    // far behind camera
        }
    }
}

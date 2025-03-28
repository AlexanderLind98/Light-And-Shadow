using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace Light_And_Shadow;

public class ShadowFramebuffer
{
    public int SHADOW_WIDTH = 1024, SHADOW_HEIGHT = 1024;
    public int depthMapFBO;
    public int depthMap;
    public Matrix4 lightSpaceMatrix;
    public Shader simpleDepthShader;

    public ShadowFramebuffer()
    {
        // 1. Create FBO frame Buffer object
        depthMapFBO = GL.GenFramebuffer();

        // 2. Create a 2D texture as the frame buffer's depth buffer
        depthMap = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, depthMap);

        // Texture parameters
        GL.TexImage2D(TextureTarget.Texture2D,
            level: 0,
            internalformat: PixelInternalFormat.DepthComponent,
            width: SHADOW_WIDTH,
            height: SHADOW_HEIGHT,
            border: 0,
            format: PixelFormat.DepthComponent,
            type: PixelType.Float,
            pixels: IntPtr.Zero);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);

        float[] borderColor = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, borderColor);

        // 3. Attach depth texture as FBO's depth buffer
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, depthMapFBO);
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, depthMap, 0);
        GL.DrawBuffer(DrawBufferMode.None);
        GL.ReadBuffer(ReadBufferMode.None);

        if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
        {
            Console.WriteLine("ERROR: Framebuffer is not complete!");
        }

        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        // 4. Load shader (no matrices here)
        simpleDepthShader = new Shader("shaders/DepthShader.vert", "shaders/DepthShader.frag");
    }
    
    public void ConfigureShaderFromDirection(Vector3 lightDirection)
    {
        Vector3 lightDir = Vector3.Normalize(lightDirection);
        Vector3 sceneCenter = new Vector3(0f, 0f, 0f); // monkey pos
        Vector3 lightPos = sceneCenter - lightDir * 5f; // light in front of monkey

        Matrix4 lightView = Matrix4.LookAt(lightPos, sceneCenter, Vector3.UnitY);
        Matrix4 lightProjection = Matrix4.CreateOrthographic(10f, 10f, 0.1f, 10f); // Zoom monkey

        lightSpaceMatrix = lightProjection * lightView;

        simpleDepthShader.Use();
        simpleDepthShader.SetMatrix("lightSpaceMatrix", lightSpaceMatrix);
    }



    public void Dispose()
    {
        simpleDepthShader?.Dispose();
        if (depthMap != 0) GL.DeleteTexture(depthMap);
        if (depthMapFBO != 0) GL.DeleteFramebuffer(depthMapFBO);
    }
}
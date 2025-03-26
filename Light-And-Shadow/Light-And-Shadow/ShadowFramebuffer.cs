using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace Light_And_Shadow;

public class ShadowFramebuffer
{
    private const int SHADOW_WIDTH = 1024, SHADOW_HEIGHT = 1024;
    private int depthMapFBO;
    private int depthMap;

    public ShadowFramebuffer(GameWindow window)
    {
        // 1. Create FBO frame Buffer object
        depthMapFBO = GL.GenFramebuffer();

        // 2. Create a 2D texture as the frame buffer's depth buffer
        int depthMap = GL.GenTexture();
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
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

        
        float[] borderColor = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, borderColor);

        // 3. Attach depth texture as FBO's depth buffer
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, depthMapFBO);
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, depthMap, 0);
        GL.DrawBuffer(DrawBufferMode.None);
        GL.ReadBuffer(ReadBufferMode.None);
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        
        GL.Viewport(0, 0, SHADOW_WIDTH, SHADOW_HEIGHT);
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, depthMapFBO);
        GL.Clear(ClearBufferMask.DepthBufferBit);
        ConfigureShaderAndMatricies();
        
        //Render scene
        
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        
        //2
        
        GL.Viewport(0, 0, window.ClientSize.X, window.ClientSize.Y);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        ConfigureShaderAndMatricies();
        GL.BindTexture(TextureTarget.Texture2D, depthMap);
        
        //Render scene
    }

    private void ConfigureShaderAndMatricies()
    {
        float near_plane = 1.0f;
        float far_plane = 7.5f;
        Shader simpleDepthShader = new Shader("shapers/DepthShader.vert", "shapers/DepthShader.frag");
        
        Matrix4 lightProjection = Matrix4.CreateOrthographic(-10, 10, near_plane, far_plane);
        Matrix4 lightView = Matrix4.LookAt(new Vector3(-2.0f, 4.0f, -1.0f), Vector3.Zero, Vector3.UnitY);
        Matrix4 lightSpaceMatrix = lightProjection * lightView;
        
        
        simpleDepthShader.Use();
        GL.UniformMatrix4(1, false, ref lightSpaceMatrix);
        // GL.UniformMatrix4(1, true, ref lightSpaceMatrix); - use in case of emergency
    }
}
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
        
        simpleDepthShader = new Shader("shaders/DepthShader.vert", "shaders/DepthShader.frag");
        float near_plane = 1.0f;
        float far_plane = 7.5f;

        Matrix4 lightProjection = Matrix4.CreateOrthographic(-10, 10, near_plane, far_plane);
        //Matrix4 lightView = Matrix4.LookAt(new Vector3(-2.0f, 4.0f, -1.0f), Vector3.Zero, Vector3.UnitY);
        //Matrix4 lightView = Matrix4.LookAt(new Vector3(-0.2f, -1.0f, -0.3f), Vector3.Zero, Vector3.UnitY);
        Matrix4 lightView = Matrix4.LookAt(new Vector3(2.0f, 2.0f, 0.0f), Vector3.Zero, Vector3.UnitY);
        
        
        
        lightSpaceMatrix = lightProjection * lightView;
    }

    public void ConfigureShaderAndMatricies()
    {
        simpleDepthShader.Use();
        GL.UniformMatrix4(1, true, ref lightSpaceMatrix);
    }

    public void Dispose()
    {
        simpleDepthShader?.Dispose();
        if(depthMap != 0) GL.DeleteTexture(depthMap);
        if(depthMapFBO != 0) GL.DeleteFramebuffer(depthMapFBO);
    }
}
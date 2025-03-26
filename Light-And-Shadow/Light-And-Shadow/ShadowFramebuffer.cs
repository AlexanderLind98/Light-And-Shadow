using OpenTK.Graphics.OpenGL4;

namespace Light_And_Shadow;

public class ShadowFramebuffer
{
    private const int SHADOW_WIDTH = 1024, SHADOW_HEIGHT = 1024;
    private int depthMapFBO;
    private int depthMap;

    public ShadowFramebuffer()
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

    }


}
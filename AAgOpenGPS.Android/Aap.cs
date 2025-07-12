using System;

using Android.Views;
using Android.Content;
using Android.Util;
using Android.Opengl;
using Android.App;
using Graph = Android.Graphics;

using Java.Nio;

namespace AAgOpenGPS.Android
{
    class Aap
    {
        //private string VERTEX_SHADER_NAME = "shaders/vertexShader.vert"
        //private string FRAGMENT_SHADER_NAME = "shaders/fragmentShader.frag"
        private string VERTEX_SHADER =
                "uniform mat4 uVPMatrix;" +
                "attribute vec4 a_Position;" +
                "attribute vec2 a_TexCoord;" +
                "attribute vec2 v_TexCoord;" +
                "void main() {" +
                "   gl_Position = uVPMatrix * a_Position;" +
                "   v_TexCoord = vec2(a_TexCoord.x, (1.0 - (a_TexCoord.y)));" +
                "}";
        private string FRAGMENT_SHADER =
            "precision mediump float;" +
                "uniform sampler2D u_Texture;" +
                "varying vec2 v_TexCoord;" +
                "void main() {" +
                "   gl_FragColor = texture2D(u_Texture, v_TexCoord);" +
                "}";
                /**********
         private string VERT_SHADER =
                "uniform mat4 uVPMatrix;" +
                "attribute vec4 a_Position;" +
                "void main() {" +
                 "   gl_Position =  uVPMatrix * a_Position;" +
               "}";
          private string FRAG_SHADER =
            "precision highp float;" +    
            "void main(void){" +
            "   gl_FragColor = vec4(1.0f, 1.0f, 1.0f, 1.0f);" +
            "}";
        **********/    
        static int COORDINATES_PER_VERTEX = 2;
        static int VERTEX_STRIDE = COORDINATES_PER_VERTEX * 4;

        static float[] QUADRANT_COORDINATES = new float[] { 
			//x,    y
			-0.5f, 0.5f,
            -0.5f, -0.5f,
            0.5f, -0.5f,
            0.5f, 0.5f,
            };
        static float[] TEXTURE_COORDINATES = new float[]
        {
			//x,    y
			0.0f, 1.0f,
            0.0f, 0.0f,
            1.0f, 0.0f,
            1.0f, 1.0f,
        };

        private static readonly short[] DRAW_ORDER = new short[] { 0, 1, 2, 0, 2, 3 };

        private float[] vPMatrix = new float[16];
        private float[] projectionMatrix = new float[16];
        private float[] viewMatrix = new float[16];

        private FloatBuffer quadrantCoordinatesBuffer;
        private FloatBuffer textureCoordinatesBuffer;
        private ShortBuffer drawOrderBuffer;

        private int quadPositionHandle;
        private int texPositionHandle;
        private int textureUniformHandle;
        private int viewProjectionMatrixHandle;
        private int program;
        private int[] textureUnit = new int[1];
       // private float ratio = 0;


        public Aap()
        {

            ByteBuffer qcb = ByteBuffer.AllocateDirect(QUADRANT_COORDINATES.Length * 4).Order(ByteOrder.NativeOrder());
            quadrantCoordinatesBuffer = qcb.AsFloatBuffer();
            quadrantCoordinatesBuffer.Put(QUADRANT_COORDINATES);
            quadrantCoordinatesBuffer.Position(0);

            ByteBuffer tcb = ByteBuffer.AllocateDirect(TEXTURE_COORDINATES.Length * 4).Order(ByteOrder.NativeOrder());
            textureCoordinatesBuffer = tcb.AsFloatBuffer();
            textureCoordinatesBuffer.Put(TEXTURE_COORDINATES);
            textureCoordinatesBuffer.Position(0);

            ByteBuffer dob = ByteBuffer.AllocateDirect(DRAW_ORDER.Length * 2).Order(ByteOrder.NativeOrder());
            drawOrderBuffer = dob.AsShortBuffer();
            drawOrderBuffer.Put(DRAW_ORDER);
            drawOrderBuffer.Position(0);

            int vertexShader = MyGLRenderer.LoadShader(GLES20.GlVertexShader,
                                                       VERTEX_SHADER);
            int fragmentShader = MyGLRenderer.LoadShader(GLES20.GlFragmentShader,
                                                         FRAGMENT_SHADER);

            program = GLES20.GlCreateProgram();
            GLES20.GlAttachShader(program, vertexShader);
            GLES20.GlAttachShader(program, fragmentShader);
            GLES20.GlLinkProgram(program);

            //textureUniformHandle = GLES20.GlGetUniformLocation(program, "u_Texture");
            //View projection transformation matrix handler
           
            //Enable blend
            GLES20.GlEnable(GLES20.GlBlend);
            //Uses to prevent transparent area to turn in black
            GLES20.GlBlendFunc(GLES20.GlOne, GLES20.GlOneMinusSrcAlpha);
             Graph.Bitmap textureBitmap = null;
             
              GLES20.GlActiveTexture(GLES20.GlTexture0);
            GLES20.GlGenTextures(textureUnit.Length, textureUnit, 0);
            GLES20.GlBindTexture(GLES20.GlTexture2d, textureUnit[0]);
            try {
            // Read the texture.
		        Graph.BitmapFactory.Options options = new Graph.BitmapFactory.Options();
		        options.InScaled = false;
		        var stream = Application.Context.Assets.Open("test.png");
		        textureBitmap = Graph.BitmapFactory.DecodeStream(stream, null, options);
		    } catch(Exception e) {
		       Console.WriteLine(e.Message);
		    }    

                     
            GLUtils.TexImage2D(GLES20.GlTexture2d, 0, textureBitmap, 0);
            GLES20.GlGenerateMipmap(GLES20.GlTexture2d);

            textureBitmap.Recycle();
            //MyGLRenderer.CheckGlError ("loading texture");
           
        }
        /***
                public void OnDrawFrame(Javax.Microedition.Khronos.Opengles.IGL10 gl)
                {
                    // Draw background color
                    GLES20.GlClear((int)GLES20.GlColorBufferBit);

                    // Set the camera position (View matrix)
                    Matrix.SetLookAtM(viewMatrix, 0, 0f, 0f, 3f, 0f, 0f, 0f, 0f, 1.0f, 0.0f);

                    // Calculate the projection and view transformation
                    Matrix.MultiplyMM(vPMatrix, 0, projectionMatrix, 0, viewMatrix, 0);


                    GLES20.GlUseProgram(program);

                    // Attach the object texture.
                    GLES20.GlBindTexture(GLES20.GlTexture2d, textureUnit[0]);
                    GLES20.GlUniform1i(textureUniformHandle, 0);

                    // Pass the projection and view transformation to the shader
                    GLES20.GlUniformMatrix4fv(viewProjectionMatrixHandle, 1, false, vPMatrix, 0);

                    //Pass quadrant position to shader
                    GLES20.GlVertexAttribPointer(
                        quadPositionHandle,
                        COORDINATES_PER_VERTEX,
                         GLES20.GlFloat,
                        false,
                        VERTEX_STRIDE,
                        quadrantCoordinatesBuffer
                    );

                    //Pass texture position to shader
                    GLES20.GlVertexAttribPointer(
                        texPositionHandle,
                        COORDINATES_PER_VERTEX,
                         GLES20.GlFloat,
                        false,
                        VERTEX_STRIDE,
                        textureCoordinatesBuffer
                    );

                    // Enable attribute handlers
                    GLES20.GlEnableVertexAttribArray(quadPositionHandle);
                    GLES20.GlEnableVertexAttribArray(texPositionHandle);

                    //Draw shape
                    GLES20.GlDrawElements(
                        GLES20.GlTriangles,
                        DRAW_ORDER.Length,
                        GLES20.GlUnsignedShort,
                        drawOrderBuffer
                    );

                    // Disable vertex arrays
                    GLES20.GlDisableVertexAttribArray(quadPositionHandle);
                    GLES20.GlDisableVertexAttribArray(texPositionHandle);
                }

                public void OnSurfaceChanged(Javax.Microedition.Khronos.Opengles.IGL10 gl, int width, int height)
                {
                    // Adjust the viewport based on geometry changes,
                    // such as screen rotation
                    GLES20.GlViewport(0, 0, width, height);

                    ratio = (float)width / height;

                    // this projection matrix is applied to object coordinates
                    // in the onDrawFrame() method
                    Matrix.FrustumM(projectionMatrix, 0, -ratio, ratio, -1, 1, 3, 7);//using perspective view

                }
        ****/
        //public void OnSurfaceCreated(Javax.Microedition.Khronos.Opengles.IGL10 gl, Javax.Microedition.Khronos.Egl.EGLConfig config)
        public void Draw(float[] vPMatrix)
        {
            
            GLES20.GlBindTexture(GLES20.GlTexture2d, textureUnit[0]);
            GLES20.GlUniform1i(textureUniformHandle, 0);
            GLES20.GlUseProgram(program);

            //GLES20.GlBindTexture(GLES20.GlTexture2d, textureUnit[0]);
          //  GLES20.GlUniform1i(textureUniformHandle, 0);
          
           viewProjectionMatrixHandle = GLES20.GlGetUniformLocation(program, "uVPMatrix");

            //texPositionHandle = GLES20.GlGetAttribLocation(program, "a_TexCoord");

            quadPositionHandle = GLES20.GlGetAttribLocation(program, "a_Position");
             GLES20.GlEnableVertexAttribArray(quadPositionHandle);
             GLES20.GlVertexAttribPointer(
                quadPositionHandle,
                COORDINATES_PER_VERTEX,
                 GLES20.GlFloat,
                false,
                VERTEX_STRIDE,
                quadrantCoordinatesBuffer
            );
            
           
             texPositionHandle = GLES20.GlGetAttribLocation(program, "a_TexCoord");
             GLES20.GlEnableVertexAttribArray(texPositionHandle);
            GLES20.GlVertexAttribPointer(
               texPositionHandle,
               COORDINATES_PER_VERTEX,
                GLES20.GlFloat,
               false,
               VERTEX_STRIDE,
               textureCoordinatesBuffer
           );

            
            textureUniformHandle = GLES20.GlGetUniformLocation(program, "u_Texture");
            //View projection transformation matrix handler
            viewProjectionMatrixHandle = GLES20.GlGetUniformLocation(program, "uVPMatrix");
            GLES20.GlUniformMatrix4fv(viewProjectionMatrixHandle, 1, false, vPMatrix, 0);
           
            //Draw shape
            GLES20.GlDrawElements(
                GLES20.GlTriangles,
                DRAW_ORDER.Length,
                GLES20.GlUnsignedShort,
                drawOrderBuffer
            );


            GLES20.GlDisableVertexAttribArray(quadPositionHandle);
           GLES20.GlDisableVertexAttribArray(texPositionHandle);

        }

    }
}

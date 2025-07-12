using System;
using System.Collections.Generic;
using System.Numerics;

using Android.Views;
using Android.Content;
using Android.Util;
using Android.Opengl;

using Java.Nio;

namespace AAgOpenGPS.Android
{
    class GridHelper
    {  
    
 	    private List<Vector3> Vertices = new List<Vector3>();
        //private List<Vector3i> Indices = new List<Vector3i>();
   
        static float[] gridCoords = new float[] {
			1.0f, 1.0f, 0.0f,
			1.0f, -1.0f, 0.0f,
			0.8f, 1.0f, 0.0f,
			0.8f, -1.0f, 0.0f,
			0.4f, 1.0f, 0.0f,
			0.4f, -1.0f, 0.0f,
			0.3f, 1.0f, 0.0f,
			0.3f, -1.0f, 0.0f,
			0.2f, 1.0f, 0.0f,
			0.2f, -1.0f, 0.0f,
			0.1f, 1.0f, 0.0f,
			0.1f, -1.0f, 0.0f,
			0.0f, 1.0f, 0.0f,
			0.0f, -1.0f, 0.0f,
			-0.2f, 1.0f, 0.0f,
			-0.2f, -1.0f, 0.0f,
			-0.1f, 1.0f, 0.0f,
			-0.1f, -1.0f, 0.0f,
			-0.3f, 1.0f, 0.0f,
			-0.3f, -1.0f, 0.0f,
			-0.4f, 1.0f, 0.0f,
			-0.4f, -1.0f, 0.0f
		};

        private short[] drawOrder = new short[] {
        0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27,28,29, //horizontal
        30, 31, 32, 33, 34, 35    
        }; // order to draw vertices

        private string vertexShaderCode =
            // This matrix member variable provides a hook to manipulate
            // the coordinates of the objects that use this vertex shader
            "uniform mat4 uMVPMatrix;" +

                "attribute vec4 vPosition;" +
                "void main() {" +
                // the matrix must be included as a modifier of gl_Position
                "  gl_Position = vPosition * uMVPMatrix;" +
                "}";
        private string fragmentShaderCode =
            "precision mediump float;" +
                "uniform vec4 vColor;" +
                "void main() {" +
                "  gl_FragColor = vColor;" +
                "}";
        private FloatBuffer vertexBuffer;
        private ShortBuffer drawListBuffer;
        private int mProgram;
        private int mPositionHandle;
        private int mColorHandle;
        private int mMVPMatrixHandle;

        // number of coordinates per vertex in this array
        static int COORDS_PER_VERTEX = 3;

        private int vertexStride = COORDS_PER_VERTEX * 4; // 4 bytes per vertex

        // Set color with red, green, blue and alpha (opacity) values
        /****
		float[] color = new float[] { 
			0.2f, 
			0.709803922f, 
			0.898039216f, 
			1.0f
		};
		*****/
        float[] color = new float[] {
            1.0f,
            1.0f,
            1.0f,
            1.0f
        };

        /****       
       import java.awt.Color;
       import java.util.ArrayList;
       import java.util.List;

       Color color1 = new Color(color1.getRGB());
       Color color2 = new Color(color2.getRGB());

       int center = divisions / 2;
       float step = size / divisions;
       float halfSize = size / 2;

       List<Float> Vertices = new ArrayList<>();
       List<Float> colors = new ArrayList<>();

       for (int i = 0, j = 0, k = (int) -halfSize; i <= divisions; i++, k += step) {
           Vertices.Add(-halfSize);
           Vertices.Add(0f);
           Vertices.Add((float) k);
           Vertices.Add(halfSize);
           Vertices.Add(0f);
           Vertices.Add((float) k);

           Vertices.Add((float) k);
           Vertices.Add(0f);
           Vertices.Add(-halfSize);
           Vertices.Add((float) k);
           Vertices.Add(0f);
           Vertices.Add(halfSize);

           Color color = (i == center) ? color1 : color2;

           float[] colorArray = new float[3];
           colorArray[0] = color.getRed() / 255f;
           colorArray[1] = color.getGreen() / 255f;
           colorArray[2] = color.getBlue() / 255f;

           for (float c : colorArray) {
               colors.Add(c);
           }
           for (float c : colorArray) {
               colors.Add(c);
           }
       }

       *************/
        public GridHelper(int slices, int size)
        {
             for (int j = 0; j <= slices; ++j)
            {
                for (int i = 0; i <= slices; ++i)
                {
                    float x = (float)i / slices;
                    float y = 1.0f; //float)j / slices;//0;
                    float z = 0;//(float)j / slices;
                    Vertices.Add(new Vector3(x, y, z));
                    x = (float)i / slices;
                    y = -1.0f; //float)j / slices;//0;
                    z = 0;//(float)j / slices;
                    Vertices.Add(new Vector3(x, y, z));
                    x = (float)-i / slices;
                    y = 1.0f; //float)j / slices;//0;
                    z = 0;//(float)j / slices;
                    Vertices.Add(new Vector3(x, y, z));
                    x = (float)-i / slices;
                    y = -1.0f; //float)j / slices;//0;
                    z = 0;//(float)j / slices;
                    Vertices.Add(new Vector3(x, y, z));
                    //horizontal
                    x = -1.0f;
                    y = (float)-i / slices;
                    z = 0;
                    Vertices.Add(new Vector3(x, y, z));
                    x = 1.0f;
                    y = (float)-i / slices;
                    z = 0;
                    Vertices.Add(new Vector3(x, y, z));
                }
            }


            // initialize vertex byte buffer for shape coordinates
            ByteBuffer bb = ByteBuffer.AllocateDirect(
                    // (# of coordinate values * 4 bytes per float)
                     Vertices.Count * 3 * 4);
            bb.Order(ByteOrder.NativeOrder());
            vertexBuffer = bb.AsFloatBuffer();
           // vertexBuffer.Put(gridCoords);
           // vertexBuffer.Position(0);
           
            foreach (Vector3 vertex in Vertices)
            {
                vertexBuffer.Put(vertex.X);
                vertexBuffer.Put(vertex.Y);
                vertexBuffer.Put(vertex.Z);
            }
            vertexBuffer.Position(0);


            ByteBuffer dlb = ByteBuffer.AllocateDirect(
                    // (# of coordinate values * 2 bytes per short)
                    drawOrder.Length * 2);
            dlb.Order(ByteOrder.NativeOrder());
            drawListBuffer = dlb.AsShortBuffer();
            drawListBuffer.Put(drawOrder);
            drawListBuffer.Position(0);


            // initialize byte buffer for the draw list
            // ByteBuffer dlb = ByteBuffer.AllocateDirect(
            // (# of coordinate values * 2 bytes per short)
            //      Indices.Count * 3 * 2);
            //  dlb.Order(ByteOrder.NativeOrder());
            //  drawListBuffer = dlb.AsShortBuffer();
            // // drawListBuffer.Put (Indices);
            //drawListBuffer.Position (0);

            // prepare shaders and OpenGL program
            int vertexShader = MyGLRenderer.LoadShader(GLES20.GlVertexShader,
                                                       vertexShaderCode);
            int fragmentShader = MyGLRenderer.LoadShader(GLES20.GlFragmentShader,
                                                         fragmentShaderCode);

            mProgram = GLES20.GlCreateProgram();             // create empty OpenGL Program
            GLES20.GlAttachShader(mProgram, vertexShader);   // Add the vertex shader to program
            GLES20.GlAttachShader(mProgram, fragmentShader); // Add the fragment shader to program
            GLES20.GlLinkProgram(mProgram);                  // create OpenGL program executables
        }


        public void Draw(float[] mvpMatrix)
        {
            // Add program to OpenGL environment
            GLES20.GlUseProgram(mProgram);

            // get handle to vertex shader's vPosition member
            mPositionHandle = GLES20.GlGetAttribLocation(mProgram, "vPosition");

            // Enable a handle to the triangle Vertices
            GLES20.GlEnableVertexAttribArray(mPositionHandle);

            // Prepare the triangle coordinate data
            GLES20.GlVertexAttribPointer(mPositionHandle, COORDS_PER_VERTEX,
                                         GLES20.GlFloat, false,
                                         COORDS_PER_VERTEX * 4, vertexBuffer);

            // get handle to fragment shader's vColor member
            mColorHandle = GLES20.GlGetUniformLocation(mProgram, "vColor");

            // Set color for drawing the triangle
            GLES20.GlUniform4fv(mColorHandle, 1, color, 0);

            // get handle to shape's transformation matrix
            mMVPMatrixHandle = GLES20.GlGetUniformLocation(mProgram, "uMVPMatrix");
            MyGLRenderer.CheckGlError("glGetUniformLocation");

            // Apply the projection and view transformation
            GLES20.GlUniformMatrix4fv(mMVPMatrixHandle, 1, false, mvpMatrix, 0);
            MyGLRenderer.CheckGlError("glUniformMatrix4fv");

            // Draw the grid
            GLES20.GlDrawElements(GLES20.GlLines, drawOrder.Length,
                                  GLES20.GlUnsignedShort, drawListBuffer);

            // Disable vertex array
            GLES20.GlDisableVertexAttribArray(mPositionHandle);
        }

    }
}




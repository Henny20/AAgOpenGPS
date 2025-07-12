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
    class Grid
    {
        private List<Vector3> Vertices = new List<Vector3>();
        private List<Vector3i> Indices = new List<Vector3i>();

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
            0.0f,
            0.0f,
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

List<Float> vertices = new ArrayList<>();
List<Float> colors = new ArrayList<>();

for (int i = 0, j = 0, k = (int) -halfSize; i <= divisions; i++, k += step) {
    vertices.add(-halfSize);
    vertices.add(0f);
    vertices.add((float) k);
    vertices.add(halfSize);
    vertices.add(0f);
    vertices.add((float) k);
    
    vertices.add((float) k);
    vertices.add(0f);
    vertices.add(-halfSize);
    vertices.add((float) k);
    vertices.add(0f);
    vertices.add(halfSize);

    Color color = (i == center) ? color1 : color2;

    float[] colorArray = new float[3];
    colorArray[0] = color.getRed() / 255f;
    colorArray[1] = color.getGreen() / 255f;
    colorArray[2] = color.getBlue() / 255f;

    for (float c : colorArray) {
        colors.add(c);
    }
    for (float c : colorArray) {
        colors.add(c);
    }
}

*************/
        public Grid(int slices)
        {
            for (int j = 0; j <= slices; ++j)
            {
                for (int i = 0; i <= slices; ++i)
                {
                    float x = (float)i / slices;
                    float y = 0;
                    float z = (float)j / slices;
                    Vertices.Add(new Vector3(x, y, z));
                }
            }

            for (int j = 0; j < slices; ++j)
            {
                for (int i = 0; i < slices; ++i)
                {
                    int row1 = j * (slices + 1);
                    int row2 = (j + 1) * (slices + 1);

                    Indices.Add(new Vector3i(row1 + i, row1 + i + 1, row2 + i + 1));
                    Indices.Add(new Vector3i(row1 + i, row2 + i + 1, row2 + i));

                }
            }
            
            // initialize vertex byte buffer for shape coordinates
            ByteBuffer bb = ByteBuffer.AllocateDirect(
                    // (# of coordinate values * 4 bytes per float)
                    Vertices.Count * 3 * 4);
            bb.Order(ByteOrder.NativeOrder());
            vertexBuffer = bb.AsFloatBuffer();
            //vertexBuffer.Put (Vertices);
            //vertexBuffer.Position (0);

            // initialize byte buffer for the draw list
            ByteBuffer dlb = ByteBuffer.AllocateDirect(
                     // (# of coordinate values * 2 bytes per short)
                     Indices.Count * 3 * 2);
            dlb.Order(ByteOrder.NativeOrder());
            drawListBuffer = dlb.AsShortBuffer();
            //drawListBuffer.Put (Indices);
            //drawListBuffer.Position (0);

            foreach (Vector3 vertex in Vertices)
            {
                vertexBuffer.Put(vertex.X);
                vertexBuffer.Put(vertex.Y);
                vertexBuffer.Put(vertex.Z);
            }
            vertexBuffer.Position(0);

            foreach (Vector3i index in Indices)
            {
                drawListBuffer.Put((short)(index.X));
                drawListBuffer.Put((short)(index.Y));
                drawListBuffer.Put((short)(index.Z));
                Console.WriteLine("CCCC " + index);
            }
            drawListBuffer.Position(0);



            // prepare shaders and OpenGL program
            int vertexShader = MyGLRenderer.LoadShader(GLES20.GlVertexShader,
                                                       vertexShaderCode);
            int fragmentShader = MyGLRenderer.LoadShader(GLES20.GlFragmentShader,
                                                         fragmentShaderCode);

            mProgram = GLES20.GlCreateProgram();             // create empty OpenGL Program
            GLES20.GlAttachShader(mProgram, vertexShader);   // add the vertex shader to program
            GLES20.GlAttachShader(mProgram, fragmentShader); // add the fragment shader to program
            GLES20.GlLinkProgram(mProgram);                  // create OpenGL program executables
        }


        public void Draw(float[] mvpMatrix)
        {
            // Add program to OpenGL environment
            GLES20.GlUseProgram(mProgram);

            // get handle to vertex shader's vPosition member
            mPositionHandle = GLES20.GlGetAttribLocation(mProgram, "vPosition");

            // Enable a handle to the triangle vertices
            GLES20.GlEnableVertexAttribArray(mPositionHandle);

            // Prepare the triangle coordinate data
            GLES20.GlVertexAttribPointer(mPositionHandle, COORDS_PER_VERTEX,
                                         GLES20.GlFloat, false,
                                         3 * 4, vertexBuffer);

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

            // Draw the square

            GLES20.GlDrawElements(GLES20.GlLines, Indices.Count,
                                  GLES20.GlUnsignedShort, drawListBuffer);

            // Disable vertex array
            GLES20.GlDisableVertexAttribArray(mPositionHandle);
        }

    }
}

public struct Vector3
{
    public float X, Y, Z;

    public Vector3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public static int SizeInBytes => sizeof(float) * 3;
}

public struct Vector3i
{
    public int X, Y, Z;

    public Vector3i(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }
}


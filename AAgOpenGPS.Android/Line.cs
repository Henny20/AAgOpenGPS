using System;
using AAgOpenGPS.ViewModels;
using Android.Views;
using Android.Content;
using Android.Util;
using Android.Opengl;

using Java.Nio;


namespace AAgOpenGPS.Android
{
    public class Line
    {
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
        private int mProgram;
        private int mPositionHandle;
        private int mColorHandle;
        private int mMVPMatrixHandle;

        // number of coordinates per vertex in this array
        static int COORDS_PER_VERTEX = 3;

        static float[] lineCoords = new float[6];
        private int vertexCount = lineCoords.Length / COORDS_PER_VERTEX;
        private int vertexStride = COORDS_PER_VERTEX * 4; // 4 bytes per vertex

        // Set color with red, green, blue and alpha (opacity) values
        float[] color = new float[4];

        public Line()
        {
            // initialize vertex byte buffer for shape coordinates
            ByteBuffer bb = ByteBuffer.AllocateDirect(
                    // (number of coordinate values * 4 bytes per float)
                    lineCoords.Length * 4);
            // use the device hardware's native byte order
            bb.Order(ByteOrder.NativeOrder());

            // create a floating point buffer from the ByteBuffer
            vertexBuffer = bb.AsFloatBuffer();
            // add the coordinates to the FloatBuffer
            //vertexBuffer.Put (lineCoords);
            // set the buffer to read the first coordinate
            //vertexBuffer.Position (0);

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

        public void SetVerts(float v0, float v1, float v2, float v3, float v4, float v5)
        {
            lineCoords[0] = v0;
            lineCoords[1] = v1;
            lineCoords[2] = v2;
            lineCoords[3] = v3;
            lineCoords[4] = v4;
            lineCoords[5] = v5;

            vertexBuffer.Put(lineCoords);
            // set the buffer to read the first coordinate
            vertexBuffer.Position(0);
        }

        public void SetColor(float red, float green, float blue, float alpha)
        {
            color[0] = red;
            color[1] = green;
            color[2] = blue;
            color[3] = alpha;
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
                                         vertexStride, vertexBuffer);

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

            // Draw the triangle
            GLES20.GlDrawArrays(GLES20.GlLines, 0, vertexCount);

            // Disable vertex array
            GLES20.GlDisableVertexAttribArray(mPositionHandle);
        }
    }
}


using System;

using Android.Views;
using Android.Content;
using Android.Util;
using Android.Opengl;
using Android.App;
using Graph = Android.Graphics;

using Java.Nio;

namespace AAgOpenGPS.Android;

class TriLine {

    private string vertexShaderCode =
            "uniform mat4 uMVPMatrix;" +
            "attribute vec4 vPosition;" +
            "void main() {" +
            "  gl_Position = uMVPMatrix * vPosition;" +
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

    static int COORDS_PER_VERTEX = 3;
    float[] triLineCoords;
    private short[] drawOrder;
    
    private int vertexStride = COORDS_PER_VERTEX * 4; // 4 bytes per vertex

    float[] color = { 0.2f, 0.709803922f, 0.898039216f, 1.0f };

    /**
     * Sets up the drawing object data for use in an OpenGL ES context.
     */
    public TriLine(float[] coords, int countVertices) {
        /********
        triLineCoords = coords.Clone();
       // Log.i("999999",""+countVertices);
        drawOrder = new short[countVertices*3 - 6];
        short v1 = 1;
        short v2 = 2;
        for (short i = 0; i < (countVertices-2); i+=1) {
			drawOrder[i*3 ] = i;
			drawOrder[i*3 + 1] = (short) (i + 1);
			drawOrder[i*3 + 2] = (short) (i + 2);        	
        }
        **********/
        triLineCoords = (float[])coords.Clone();
        drawOrder = new short[countVertices * 3 - 6];
        short v1 = 1;
        short v2 = 2;
        for (short i = 0; i < (countVertices - 2); i += 1)
        {
            drawOrder[i * 3] = i;
            drawOrder[i * 3 + 1] = (short)(i + 1);
            drawOrder[i * 3 + 2] = (short)(i + 2);
        }
        //Log.i("999999",Arrays.toString(drawOrder));

        // initialize vertex byte buffer for shape coordinates
        ByteBuffer bb = ByteBuffer.AllocateDirect(
        // (# of coordinate values * 4 bytes per float)
                triLineCoords.Length * 4);
        bb.Order(ByteOrder.NativeOrder());
        vertexBuffer = bb.AsFloatBuffer();
        vertexBuffer.Put(coords);
        vertexBuffer.Position(0);

        // initialize byte buffer for the draw list
        ByteBuffer dlb = ByteBuffer.AllocateDirect(
                // (# of coordinate values * 2 bytes per short)
                drawOrder.Length * 2);
        dlb.Order(ByteOrder.NativeOrder());
        drawListBuffer = dlb.AsShortBuffer();
        drawListBuffer.Put(drawOrder);
        drawListBuffer.Position(0);

        // prepare shaders and OpenGL program
        int vertexShader = MyGLRenderer.LoadShader(
                GLES20.GlVertexShader,
                vertexShaderCode);
        int fragmentShader = MyGLRenderer.LoadShader(
                GLES20.GlFragmentShader,
                fragmentShaderCode);

        mProgram = GLES20.GlCreateProgram();             // create empty OpenGL Program
        GLES20.GlAttachShader(mProgram, vertexShader);   // add the vertex shader to program
        GLES20.GlAttachShader(mProgram, fragmentShader); // add the fragment shader to program
        GLES20.GlLinkProgram(mProgram);                  // create OpenGL program executables
    }

    public void Draw(float[] mvpMatrix) {
        // Add program to OpenGL environment
        GLES20.GlUseProgram(mProgram);

        // get handle to vertex shader's vPosition member
        mPositionHandle = GLES20.GlGetAttribLocation(mProgram, "vPosition");

        // Enable a handle to the triangle vertices
        GLES20.GlEnableVertexAttribArray(mPositionHandle);

        // Prepare the triangle coordinate data
        GLES20.GlVertexAttribPointer(
                mPositionHandle, COORDS_PER_VERTEX,
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

        // Draw the line
        GLES20.GlDrawElements(
                GLES20.GlTriangles, drawOrder.Length,
                GLES20.GlUnsignedShort, drawListBuffer);

        // Disable vertex array
        GLES20.GlDisableVertexAttribArray(mPositionHandle);
    }

}

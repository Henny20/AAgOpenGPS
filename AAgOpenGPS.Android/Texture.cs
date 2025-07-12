/*
 * Copyright (C) 2011 The Android Open Source Project
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;

using Android.Views;
using Android.Content;
using Android.Util;
using Android.Opengl;
using Android.App;
using Graph = Android.Graphics;
/*** rawresource
using System.IO;
using Android.Graphics;
using Android.Content;
*******/
using Java.Nio;

namespace AAgOpenGPS.Android;

class Texture {

    	private string vertexShaderCode =
            // This matrix member variable provides a hook to manipulate
            // the coordinates of the objects that use this vertex shader
            "uniform mat4 uMVPMatrix;" +
            "attribute vec4 vPosition;" +
            "attribute vec2 aTextureCoord;\n" +
            "varying vec2 vTextureCoord;\n" +
            "void main() {" +
            // The matrix must be included as a modifier of gl_Position.
            // Note that the uMVPMatrix factor *must be first* in order
            // for the matrix multiplication product to be correct.
            "  gl_Position = uMVPMatrix * vPosition;" +
            "  vTextureCoord = aTextureCoord;\n" +

            "}";

         private string fragmentShaderCode =
            "precision mediump float;" +
            //"uniform vec4 vColor;" +
            "varying vec2 vTextureCoord;\n" +
            "uniform sampler2D sTexture;\n" +
            "void main() {" +
//            "  gl_FragColor = vColor;" +
            "  gl_FragColor = texture2D(sTexture, vTextureCoord);\n" +

            "}";
    
    private FloatBuffer vertexBuffer;
	private ShortBuffer drawListBuffer;
	private int mProgram;
	private int mPositionHandle;
	//private int mColorHandle;
	private int maTextureHandle;
	private int mMVPMatrixHandle;
	
    // number of coordinates per vertex in this array
    static int COORDS_PER_VERTEX = 5;
    static float[] squareCoords = new float[] { 
             -1.0f,  0.3f, 0.0f, 0, 0,   // top left
             -1.0f,  0.0f, 0.0f, 0, 1,  // bottom left
             -0.7f,  0.0f, 0.0f, 1, 1,  // bottom right
             -0.7f,  0.3f, 0.0f, 1, 0 }; // top right

    private short[] drawOrder = new short[] { 
			0, 
			1, 
			2, 
			0, 
			2, 
			3
		}; // order to draw vertices

    private int vertexStride = COORDS_PER_VERTEX * 4; // 4 bytes per vertex

    //float color[] = { 0.2f, 0.709803922f, 0.898039216f, 1.0f };
    private int textureId;

    /**
     * Sets up the drawing object data for use in an OpenGL ES context.
     */
    public Texture (string path)
		{
		     this.textureId = loadTexture(path);
			// initialize vertex byte buffer for shape coordinates
			ByteBuffer bb = ByteBuffer.AllocateDirect (
			// (# of coordinate values * 4 bytes per float)
			        squareCoords.Length * 4);
			bb.Order (ByteOrder.NativeOrder ());
			vertexBuffer = bb.AsFloatBuffer ();
			vertexBuffer.Put (squareCoords);
			vertexBuffer.Position (0);

			// initialize byte buffer for the draw list
			ByteBuffer dlb = ByteBuffer.AllocateDirect (
			// (# of coordinate values * 2 bytes per short)
			        drawOrder.Length * 2);
			dlb.Order (ByteOrder.NativeOrder ());
			drawListBuffer = dlb.AsShortBuffer ();
			drawListBuffer.Put (drawOrder);
			drawListBuffer.Position (0);

			// prepare shaders and OpenGL program
			int vertexShader = MyGLRenderer.LoadShader (GLES20.GlVertexShader,
			                                           vertexShaderCode);
			int fragmentShader = MyGLRenderer.LoadShader (GLES20.GlFragmentShader,
			                                             fragmentShaderCode);

			mProgram = GLES20.GlCreateProgram ();             // create empty OpenGL Program
			GLES20.GlAttachShader (mProgram, vertexShader);   // add the vertex shader to program
			GLES20.GlAttachShader (mProgram, fragmentShader); // add the fragment shader to program
			GLES20.GlLinkProgram (mProgram);                  // create OpenGL program executables
			
			 //Enable blend
            GLES20.GlEnable(GLES20.GlBlend);
            //Uses to prevent transparent area to turn in black
            GLES20.GlBlendFunc(GLES20.GlOne, GLES20.GlOneMinusSrcAlpha);
            
		}

    /**
     * Encapsulates the OpenGL ES instructions for drawing this shape.
     *
     * @param mvpMatrix - The Model View Project matrix in which to draw
     * this shape.
     */
    public void Draw(float[] mvpMatrix) {
        GLES20.GlActiveTexture(GLES20.GlTexture0);
        GLES20.GlBindTexture(GLES20.GlTexture2d, textureId);
        // Add program to OpenGL environment
        GLES20.GlUseProgram(mProgram);

        // get handle to vertex shader's vPosition member
        mPositionHandle = GLES20.GlGetAttribLocation(mProgram, "vPosition");

        // Enable a handle to the triangle vertices
        GLES20.GlEnableVertexAttribArray(mPositionHandle);
        vertexBuffer.Position(0);
        // Prepare the triangle coordinate data
        GLES20.GlVertexAttribPointer(mPositionHandle, 3, GLES20.GlFloat, false, vertexStride, vertexBuffer);

        // get handle to fragment shader's vColor member
        maTextureHandle = GLES20.GlGetAttribLocation(mProgram, "aTextureCoord");

        vertexBuffer.Position(3);
        GLES20.GlVertexAttribPointer(maTextureHandle, 2, GLES20.GlFloat, false, vertexStride, vertexBuffer);
        MyGLRenderer.CheckGlError("glVertexAttribPointer maTextureHandle");
        GLES20.GlEnableVertexAttribArray(maTextureHandle);
        MyGLRenderer.CheckGlError("glEnableVertexAttribArray maTextureHandle");

        // get handle to shape's transformation matrix
        mMVPMatrixHandle = GLES20.GlGetUniformLocation(mProgram, "uMVPMatrix");
        MyGLRenderer.CheckGlError("glGetUniformLocation");

        // Apply the projection and view transformation
        GLES20.GlUniformMatrix4fv(mMVPMatrixHandle, 1, false, mvpMatrix, 0);
        MyGLRenderer.CheckGlError("glUniformMatrix4fv");

        // Draw the square
        GLES20.GlDrawElements(GLES20.GlTriangles, drawOrder.Length, GLES20.GlUnsignedShort, drawListBuffer);

        // Disable vertex array
        GLES20.GlDisableVertexAttribArray(mPositionHandle);
    }

    private int loadTexture(string path) {
        int[] textures = new int[100];
        GLES20.GlGenTextures(1, textures, 0);
        int mTextureID = textures[0];
        GLES20.GlBindTexture(GLES20.GlTexture2d, mTextureID);

        //GLES20.GlTexParameterf(GLES20.GL_TEXTURE_2D, GLES20.GL_TEXTURE_MIN_FILTER, GLES20.GL_NEAREST);
       // GLES20.GlTexParameterf(GLES20.GL_TEXTURE_2D, GLES20.GL_TEXTURE_MAG_FILTER, GLES20.GL_LINEAR);

       // GLES20.GlTexParameteri(GLES20.GL_TEXTURE_2D, GLES20.GL_TEXTURE_WRAP_S, GLES20.GL_REPEAT);
       // GLES20.GlTexParameteri(GLES20.GL_TEXTURE_2D, GLES20.GL_TEXTURE_WRAP_T, GLES20.GL_REPEAT);
        Graph.BitmapFactory.Options options = new Graph.BitmapFactory.Options();
        options.InScaled = false;
        
        var stream = Application.Context.Assets.Open(path);
        Graph.Bitmap bitmap = Graph.BitmapFactory.DecodeStream(stream, null, options);

       // GLES20.GlBindTexture(GLES20.GlTexture2d, textureHandle[0]);
        GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureMinFilter, GLES20.GlNearest);
        GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureMagFilter, GLES20.GlNearest);
        GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureWrapS, GLES20.GlClampToEdge);
        GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureWrapT, GLES20.GlClampToEdge);
        GLUtils.TexImage2D(GLES20.GlTexture2d, 0, bitmap, 0);
        bitmap.Recycle();
/***********
Stream is = mContext.Resources.OpenRawResource(Resource.Raw.robot56);
Bitmap bitmap;
try
{
    bitmap = BitmapFactory.DecodeStream(is);
}
finally
{
    try
    {
        is.Close();
    }
    catch (IOException e)
    {
        e.PrintStackTrace();
    }
}
******/
         return mTextureID;
    }
}

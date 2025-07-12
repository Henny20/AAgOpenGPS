using System;
using System.Linq;

using Android.Views;
using Android.Content;
using Android.Util;
using Android.Opengl;

using Java.Nio;

namespace AAgOpenGPS.Android
{
	class Plane
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
				////"vec2 uv = gl_FragCoord.xy / 800.0;" +
				////"vec2 tiledUV = fract(uv * 5.0);" +
				//"vec4 fragcolor = vec4(min(g.x , g.y) < 0.05);" +
				////"vec2 square = abs(tiledUV*2.-1.);" +
				////"vec2 sharpSquare = step(0.98,square);" +
  ////  "float result = sharpSquare.x+sharpSquare.y;" +
				"gl_FragColor = vColor;" +
				//"  gl_FragColor = mix(fragcolor, vColor, 0.5);" +
            	//// "gl_FragColor = mix(vec4(1.0)*result, vColor, 0.8);" +
				"}";
				
		private FloatBuffer vertexBuffer;
		private ShortBuffer drawListBuffer;
		private int mProgram;
		private int mPositionHandle;
		private int mColorHandle;
		private int mMVPMatrixHandle;
		private static float pSize = 30f;

		// number of coordinates per vertex in this array
		static int COORDS_PER_VERTEX = 3;
		static float[] squareCoords = new float[] { 
			-1.0f * pSize,  1.0f * pSize, 0.0f,   // top left
			-1.0f * pSize, -1.0f * pSize, 0.0f,   // bottom left
			1.0f * pSize, -1.0f * pSize, 0.0f,    // bottom right
			1.0f * pSize,  1.0f * pSize, 0.0f };  // top right

		private short[] drawOrder = new short[] { 
			0, 
			1, 
			2, 
			0, 
			2, 
			3
		}; // order to draw vertices

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
			0.5f, 
			0.5f, 
			0.5f, 
			1.0f
		};

		public Plane ()
		{
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
		}

		public void Draw (float[] mvpMatrix)
		{
			// Add program to OpenGL environment
			GLES20.GlUseProgram (mProgram);

			// get handle to vertex shader's vPosition member
			mPositionHandle = GLES20.GlGetAttribLocation (mProgram, "vPosition");

			// Enable a handle to the triangle vertices
			GLES20.GlEnableVertexAttribArray (mPositionHandle);

			// Prepare the triangle coordinate data
			GLES20.GlVertexAttribPointer (mPositionHandle, COORDS_PER_VERTEX,
			                             GLES20.GlFloat, false,
			                             vertexStride, vertexBuffer);

			// get handle to fragment shader's vColor member
			mColorHandle = GLES20.GlGetUniformLocation (mProgram, "vColor");

			// Set color for drawing the triangle
			GLES20.GlUniform4fv (mColorHandle, 1, color, 0);

			// get handle to shape's transformation matrix
			mMVPMatrixHandle = GLES20.GlGetUniformLocation (mProgram, "uMVPMatrix");
			MyGLRenderer.CheckGlError ("glGetUniformLocation");

			// Apply the projection and view transformation
			GLES20.GlUniformMatrix4fv (mMVPMatrixHandle, 1, false, mvpMatrix, 0);
			MyGLRenderer.CheckGlError ("glUniformMatrix4fv");

			// Draw the square
			GLES20.GlDrawElements (GLES20.GlTriangles, drawOrder.Length,
			                      GLES20.GlUnsignedShort, drawListBuffer);

			// Disable vertex array
			GLES20.GlDisableVertexAttribArray (mPositionHandle);
		}
		
		}
}


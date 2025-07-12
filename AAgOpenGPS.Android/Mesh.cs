using System;
using System.Collections.Generic;
using System.IO;

using Android.Views;
using Android.Content;
using Android.Util;
using Android.Opengl;
using Android.App;

using Java.Nio;

namespace AAgOpenGPS.Android
{
	class Mesh
	{
		private string vertexShaderCode =
	        "uniform mat4 uMVPMatrix;" +
			"attribute vec4 vPosition;" +
			"void main() {" +
			"  gl_Position = vPosition * uMVPMatrix;" +
			"}";
		private string fragmentShaderCode =
	        "precision mediump float;" +
			"void main() {" +
			"  gl_FragColor = vec4(1, 0.5, 0, 1.0);" +
			"}";
		private FloatBuffer verticesBuffer;
        private ShortBuffer facesBuffer;
      	private int mProgram;
		private int mPositionHandle;
		private int mMVPMatrixHandle;
		
		private List<String> verticesList;
        private List<String> facesList;		

		public Mesh()
		{
		
		    var scanner = new StreamReader(Application.Context.Assets.Open("cube.obj"));
			verticesList = new List<string>();
			facesList = new List<string>();

			// Loop through all its lines 
			string line;
			while ((line = scanner.ReadLine()) != null) {
				if (line.StartsWith("v ")) {
					// Add vertex line to list of vertices 
					verticesList.Add(line);
				} else if (line.StartsWith("f ")) {
					// Add face line to faces list 
					facesList.Add(line);
				}
			}
			// Close the scanner 
			scanner.Close();
	
			// Create buffer for vertices 
			ByteBuffer buffer1 = ByteBuffer.AllocateDirect(verticesList.Count * 3 * 4);
			buffer1.Order(ByteOrder.NativeOrder());
			verticesBuffer = buffer1.AsFloatBuffer();
			
			// Create buffer for faces 
			ByteBuffer buffer2 = ByteBuffer.AllocateDirect(facesList.Count * 3 * 2);
			buffer2.Order(ByteOrder.NativeOrder());
			facesBuffer = buffer2.AsShortBuffer();
			
			foreach (string vertex in verticesList)
			{
				string[] coords = vertex.Split(' '); // Split by space 
				float x = float.Parse(coords[1]);
				float y = float.Parse(coords[2]);
				float z = float.Parse(coords[3]);
				verticesBuffer.Put(x);
				verticesBuffer.Put(y);
				verticesBuffer.Put(z);
			}
			verticesBuffer.Position (0);;

			foreach (string face in facesList)
			{
				string[] vertexIndices = face.Split(' '); 
				foreach(var item in vertexIndices)
				{
					Console.Write("AAAAA  " + item.ToString() + "  ");
				}
				short vertex1 = short.Parse(vertexIndices[1]);
				short vertex2 = short.Parse(vertexIndices[2]);
				short vertex3 = short.Parse(vertexIndices[3]);
				facesBuffer.Put((short)(vertex1 - 1));
				facesBuffer.Put((short)(vertex2 - 1));
				facesBuffer.Put((short)(vertex3 - 1));
			}
			facesBuffer.Position(0);

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
			GLES20.GlVertexAttribPointer (mPositionHandle, 3,
			                             GLES20.GlFloat, false,
			                             3 * 4, verticesBuffer);
			// get handle to shape's transformation matrix
			mMVPMatrixHandle = GLES20.GlGetUniformLocation (mProgram, "uMVPMatrix");
			MyGLRenderer.CheckGlError ("glGetUniformLocation");

			// Apply the projection and view transformation
			GLES20.GlUniformMatrix4fv (mMVPMatrixHandle, 1, false, mvpMatrix, 0);
			MyGLRenderer.CheckGlError ("glUniformMatrix4fv");

			// Draw the square
			GLES20.GlDrawElements (GLES20.GlTriangles, facesList.Count * 3,
			                      GLES20.GlUnsignedShort, facesBuffer);

			// Disable vertex array
			GLES20.GlDisableVertexAttribArray (mPositionHandle);
		}
		
		}
}


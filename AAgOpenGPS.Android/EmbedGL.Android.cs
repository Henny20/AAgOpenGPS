using System;
using Avalonia.Platform;
using Avalonia.Android;
using AAgOpenGPS.Pages;
using AAgOpenGPS.Interfaces;
using AAgOpenGPS.ViewModels;
using AAgOpenGPS;
using Android.Opengl;
using Android.OS;
using Android.Util;
using Android.App;
using Graph = Android.Graphics;
using System.Numerics;
using System.Collections.Generic;
using System.Collections;

using Java.Nio;
using Javax.Microedition.Khronos.Opengles;

using Android.Views;


namespace AAgOpenGPS.Android;

public class EmbedGLAndroid : INativeDemoControl
{
    public IPlatformHandle CreateControl(IPlatformHandle parent, Func<IPlatformHandle> createDefault)
    {
        var parentContext = (parent as AndroidViewControlHandle)?.View.Context
            ?? global::Android.App.Application.Context;

        GLSurfaceView view = new GLSurfaceView(parentContext);
        view.SetEGLContextClientVersion(2);
        view.SetRenderer(new MyGLRenderer());
        view.SetOnTouchListener(new AOGTouchListener());
        return new AndroidViewControlHandle(view);
    }
}

public class AOGTouchListener
    : Java.Lang.Object
    , View.IOnTouchListener
{
    public bool OnTouch(View v, MotionEvent e)
    {
        if (e.Action == MotionEventActions.Down)
        {
            // do stuff
            return true;
        }
        if (e.Action == MotionEventActions.Up)
        {
            // do other stuff
            return true;
        }

        return false;
    }
}

class MyGLRenderer : Java.Lang.Object, GLSurfaceView.IRenderer
{
    /**************** template ***********
		public void OnDrawFrame (Javax.Microedition.Khronos.Opengles.IGL10? gl)
		{
			// Draw background color
			GLES20.GlClear ((int)GLES20.GlColorBufferBit);
		}

		public void OnSurfaceChanged (Javax.Microedition.Khronos.Opengles.IGL10? gl, int width, int height)
		{
			// Adjust the viewport based on geometry changes,
			// such as screen rotation
			GLES20.GlViewport (0, 0, width, height);

			float ratio = (float)width / height;
		}

		public void OnSurfaceCreated (Javax.Microedition.Khronos.Opengles.IGL10? gl, Javax.Microedition.Khronos.Egl.EGLConfig? config)
		{
			// Set the background frame color
			//GLES20.GlClearColor (0.0f, 0.0f, 0.0f, 1.0f);
			GLES20.GlClearColor(0.27f, 0.4f, 0.7f, 1.0f);
		}
	
    *************/

    private static string TAG = "MyGLRenderer";
    //private Triangle mTriangle;
    private Plane mPlane;
    private Plane bPlane;

    private Line mLine;
    private Line bLine;
    private GridHelper gGrid;
    private Texture mTexture;
    private Texture tTexture;
    private float[] mMVPMatrix = new float[16];
    private float[] mProjMatrix = new float[16];
    private float[] mVMatrix = new float[16];
    private float[] itMatrix = new float[16];
    private float[] mRotationMatrix = new float[16];
    private float ratio = 0;


    // private int[] textureHandle = new int[1];

    #region IRenderer implementation
    public void OnDrawFrame(Javax.Microedition.Khronos.Opengles.IGL10? gl)
    {
        // Draw background color
        GLES20.GlClear((int)GLES20.GlColorBufferBit | (int)GLES20.GlDepthBufferBit);

        System.Console.WriteLine("RATIO IS " + ratio);
        // if (Main.Instance.curve.isMakingCurve) Main.Instance.curve.DrawCurveNew();

        if (Main.Instance.ABLine.isMakingABLine)
        {
            mLine = Main.Instance.ABLine.DrawABLineNew();
        }

        Matrix.SetIdentityM(itMatrix, 0);
        // Matrix.TranslateM(itMatrix, 0, -0.1f, 0.6f, 0.0f);
        // Matrix.ScaleM(itMatrix, 0, 0.1f, 1.1f, 0.1f);
        //  mTexture.Draw(itMatrix);

        Matrix.SetLookAtM(mVMatrix, 0, 0, 0, 3, 0f, 0f, 0f, 0f, 1.0f, 0.0f);

        // Calculate the projection and view transformation
        Matrix.MultiplyMM(mMVPMatrix, 0, mProjMatrix, 0, mVMatrix, 0);

        //Model = Translation * Rotation;
        Matrix.TranslateM(mMVPMatrix, 0, -0.7f, 0.6f, 0.0f);
        mTexture.Draw(mMVPMatrix);

        //Matrix.RotateM(mMVPMatrix, 0, 40, 1.0f, 0.0f, 0.0f);
        // SetRotateM(float[]? rm, int rmOffset, float a, float x, float y, float z);
        Matrix.SetRotateM(mRotationMatrix, 0, 40, 1.0f, 0.0f, 0.0f);
        Matrix.MultiplyMM(mMVPMatrix, 0, mRotationMatrix, 0, mVMatrix, 0);
        // Draw square
        // mMVPMatrix = Matrix4ToArray(mModelViewProjectionMatrix);

        //  foreach (var item in mMVPMatrix) {
        //     Console.WriteLine("ITEM IS " + item);
        //}


        mPlane.Draw(mMVPMatrix);

        //  Matrix.ScaleM(itMatrix, 0, 2.0f, 2.0f, 0.0f);
        //  Matrix.TranslateM(itMatrix, 0, 0.5f, 0.0f, 0.0f);
        //   Matrix.RotateM(itMatrix, 0, 40, 1.0f, 0.0f, 0.0f);
        gGrid.Draw(mMVPMatrix);
        //Matrix.SetIdentityM(mVMatrix, 0); 
        // Create a rotation for the triangle
        //long time = SystemClock.UptimeMillis() % 4000L;
        //  float angle = 0.090f * ((int) time);
        //  Matrix.SetRotateM(mRotationMatrix, 0, 0, 0.6f, 0, -1.0f);

        // Combine the rotation matrix with the projection and camera view
        // Matrix.MultiplyMM(mMVPMatrix, 0, mRotationMatrix, 0, mMVPMatrix, 0);

        // Draw lines
        mLine.Draw(mMVPMatrix);
        //  bLine.Draw(mMVPMatrix);

        Matrix.SetIdentityM(itMatrix, 0);
        Matrix.RotateM(itMatrix, 0, 40, 1.0f, 0.0f, 0.0f);
        Matrix.TranslateM(itMatrix, 0, 0.85f, 0.0f, 0.0f);
        tTexture.Draw(itMatrix);
        Matrix.SetIdentityM(itMatrix, 0);


    }

    public void OnSurfaceChanged(Javax.Microedition.Khronos.Opengles.IGL10? gl, int width, int height)
    {
        // Adjust the viewport based on geometry changes,
        // such as screen rotation
        GLES20.GlViewport(0, 0, width, height);

        ratio = (float)width / height;

        // this projection matrix is applied to object coordinates
        // in the onDrawFrame() method
        Matrix.FrustumM(mProjMatrix, 0, -ratio, ratio, -1, 1, 3, 7);//using perspective view
                                                                    //Matrix.PerspectiveM(mProjMatrix, 0, 0.4f ,ratio, 50.0f, 520.0f);

    }

    public void OnSurfaceCreated(Javax.Microedition.Khronos.Opengles.IGL10? gl, Javax.Microedition.Khronos.Egl.EGLConfig config)
    {
        // Set the background frame color
        //GLES20.GlClearColor (0.0f, 0.0f, 0.0f, 1.0f);
        //skyblue
        GLES20.GlClearColor(0.0f, 0.7f, 0.9f, 1.0f);

        GLES20.GlLineWidth(2.0f);
        /***
        mLine = new Line();
        mLine.SetVerts(0.0f, -1.0f * 30, 0.0f, 0.0f, 1.0f * 30, 0.0f);
        mLine.SetColor(1.0f, 0.0f, 0.0f, 1.0f);
        bLine = new Line();
        bLine.SetVerts(0.1f, -1.0f * 30, 0.0f, 0.1f, 1.0f * 30, 0.0f);
        bLine.SetColor(0.0f, 1.0f, 0.0f, 1.0f);
        ****/
        mPlane = new Plane();
        mTexture = new Texture("speedo.png");
        tTexture = new Texture("tractor.png");
        gGrid = new GridHelper(10, 200);
    }
    #endregion

    public static int LoadShader(int type, string shaderCode)
    {
        // create a vertex shader type (GLES20.GL_VERTEX_SHADER)
        // or a fragment shader type (GLES20.GL_FRAGMENT_SHADER)
        int shader = GLES20.GlCreateShader(type);

        // add the source code to the shader and compile it
        GLES20.GlShaderSource(shader, shaderCode);
        GLES20.GlCompileShader(shader);

        return shader;
    }

    /**
    * Utility method for debugging OpenGL calls. Provide the name of the call
    * just after making it:
    *
    * <pre>
    * mColorHandle = GLES20.glGetUniformLocation(mProgram, "vColor");
    * MyGLRenderer.checkGlError("glGetUniformLocation");</pre>
    *
    * If the operation is not successful, the check throws an error.
    *
    * @param glOperation - Name of the OpenGL call to check.
    */
    public static void CheckGlError(string glOperation)
    {
        int error;
        while ((error = GLES20.GlGetError()) != GLES20.GlNoError)
        {
            Log.Error(TAG, glOperation + ": glError " + error);
            throw new Java.Lang.RuntimeException(glOperation + ": glError " + error);
        }
    }

    public float Angle
    {
        get;
        set;
    }

    // probeersel
    public static int loadTexture()
    {

        int[] textureHandle = new int[1];

        GLES20.GlGenTextures(1, textureHandle, 0); //init 1 texture storage handle 
        if (textureHandle[0] != 0)
        {
            //Android.Graphics cose class Matrix exists at both Android.Graphics and Android.OpenGL and this is only sample of using 
            Graph.BitmapFactory.Options options = new Graph.BitmapFactory.Options();
            options.InScaled = false; // No pre-scaling
                                      // Graph.Bitmap bitmap = Graph.BitmapFactory.DecodeResource(Context.Resources, Resource.Drawable.texture1, options);

            string basePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            Console.WriteLine("PATHHHHHHHHHHHHHHH" + basePath);
            //  Graph.Bitmap bitmap = Graph.BitmapFactory.DecodeFile(System.Environment.SpecialFolder.LocalApplicationData + "/test.png", options);
            var stream = Application.Context.Assets.Open("test.png");
            Graph.Bitmap bitmap = Graph.BitmapFactory.DecodeStream(stream, null, options);

            GLES20.GlBindTexture(GLES20.GlTexture2d, textureHandle[0]);
            GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureMinFilter, GLES20.GlNearest);
            GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureMagFilter, GLES20.GlNearest);
            GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureWrapS, GLES20.GlClampToEdge);
            GLES20.GlTexParameteri(GLES20.GlTexture2d, GLES20.GlTextureWrapT, GLES20.GlClampToEdge);
            GLUtils.TexImage2D(GLES20.GlTexture2d, 0, bitmap, 0);
            bitmap.Recycle();
        }

        if (textureHandle[0] == 0)
        {
            throw new Java.Lang.RuntimeException("Error loading texture.");
        }

        return textureHandle[0];
    }
    ///helpers
    private float[] Matrix4ToArray(Matrix4x4 matrix)
    {
        float[] data = new float[16];
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                data[i * 4 + j] = matrix[i, j];
            }
        }
        return data;
    }

    public static Graph.Matrix convertMatrixToAndroidGraphicsMatrix(float[] matrix4x4)
    {
        float[] values = {
        matrix4x4[0 * 4 + 0], matrix4x4[1 * 4 + 0], matrix4x4[3 * 4 + 0],
        matrix4x4[0 * 4 + 1], matrix4x4[1 * 4 + 1], matrix4x4[3 * 4 + 1],
        matrix4x4[0 * 4 + 3], matrix4x4[1 * 4 + 3], matrix4x4[3 * 4 + 3],
    };

        Graph.Matrix matrix = new Graph.Matrix();
        matrix.SetValues(values);
        return matrix;
    }

}







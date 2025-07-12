/*
 * Created by ARF on 05/11/2016.
 */
using Android.Opengl;
 
namespace AAgOpenGPS.Android; 

public class Mat4 {

    private float[] m_Data;

    public Mat4() {

        m_Data = new float[16];

        for (int i = 1; i < 15; ++i) {

            m_Data[i] = 0.0f;
        }
        m_Data[0] = 1.0f;
        m_Data[5] = 1.0f;
        m_Data[10]= 1.0f;
        m_Data[15]= 1.0f;
    }


    public float [] GetMat() {

        return m_Data;
    }


    public void Translate(float x, float y, float z) {

        Matrix.TranslateM(m_Data, 0, x,y,z);
    }


    public void Scale(float x) {

        Matrix.ScaleM(m_Data, 0, x ,x, x);
    }


    public void Scale(float x, float y, float z) {

        Matrix.ScaleM(m_Data, 0, x ,y, z);
    }
}

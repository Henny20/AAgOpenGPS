//using AAgOpenGPS.Properties;
using System.Numerics;

namespace AAgOpenGPS.Android
{
	public class CCamera
	{
		private readonly double camPosZ;

		public bool camFollowing;
		public int camMode = 0;

		public double camPitch;
		private double camPosX;
		private double camPosY;
		public double camSetDistance = -75;
		public double camSmoothFactor;

		//private double fixHeading;
		private double camYaw;

		public double gridZoom;
		public double offset;
		public double previousZoom = 25;

		public double zoomValue = 15;

		//private double camDelta = 0;

		public CCamera()
		{
		    //get the pitch of camera from settings
		   // camPitch = Settings.Default.setDisplay_camPitch;TODO
		   // zoomValue = Settings.Default.setDisplay_camZoom;TODO
		    camPosZ = 0.0;
		    camFollowing = true;
		  //  camSmoothFactor = Settings.Default.setDisplay_camSmooth * 0.004 + 0.2;TODO
		}

		public void setWorldCam(Matrix4x4 modelview, double _fixPosX, double _fixPosY, double _fixHeading)
		{
		 
		    camPosX = _fixPosX;
		    camPosY = _fixPosY;
		    camYaw = _fixHeading;

		    modelview = Matrix4x4.CreateTranslation(0, 0, (float)camSetDistance * 0.5f) * modelview;
		  //  modelview = Matrix4x4.CreateFromAxisAngle(new Vector3(1, 0, 0), (float)camPitch) * modelview; TODO
		    if (camFollowing)
		    {
		     //   modelview = Matrix4x4.CreateFromAxisAngle(new Vector3(0, 0, 1), (float)camYaw) * modelview; TODO
		        modelview = Matrix4x4.CreateTranslation((float)-camPosX, (float)-camPosY, (float)-camPosZ) * modelview;
		    }
		    else
		    {
		        modelview = Matrix4x4.CreateTranslation((float)-camPosX, (float)-camPosY, (float)-camPosZ) * modelview;
		    }
		}
	 }
}






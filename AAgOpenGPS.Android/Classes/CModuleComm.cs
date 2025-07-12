//﻿using AgOpenGPS.Core.Translations;
using AAgOpenGPS.ViewModels;

namespace AAgOpenGPS.Android
{
    public class CModuleComm
    {
        //copy of the mainform address
      //  private readonly FormGPS mf;
        public Main mf { get; set; }

        //Critical Safety Properties
        public bool isOutOfBounds = true;

        // ---- Section control switches to AOG  ---------------------------------------------------------
        //PGN - 32736 - 127.249 0x7FF9
        public byte[] ss = new byte[9];

        public byte[] ssP = new byte[9];

        public int
            swHeader = 0,
            swMain = 1,
            swAutoGr0 = 2,
            swAutoGr1 = 3,
            swNumSections = 4,
            swOnGr0 = 5,
            swOffGr0 = 6,
            swOnGr1 = 7,
            swOffGr1 = 8;

        public int pwmDisplay = 0;
        public double actualSteerAngleDegrees = 0;
        public int actualSteerAngleChart = 0, sensorData = -1;

        //for the workswitch
        public bool isWorkSwitchActiveLow, isRemoteWorkSystemOn, isWorkSwitchEnabled,
            isWorkSwitchManualSections, isSteerWorkSwitchManualSections, isSteerWorkSwitchEnabled;

        public bool workSwitchHigh, oldWorkSwitchHigh, steerSwitchHigh, oldSteerSwitchHigh, oldSteerSwitchRemote;

        //constructor
        public CModuleComm(Main _f)
        {
            mf = _f;
            //WorkSwitch logic
            isRemoteWorkSystemOn = false;

            //does a low, grounded out, mean on
            isWorkSwitchActiveLow = true;
        }
/************ only trouble
        //Called from "OpenGL.Designer.cs" when requied
        public void CheckWorkAndSteerSwitch()
        {
            //AutoSteerAuto button enable - Ray Bear inspired code - Thx Ray!
            if (mfahrs.isAutoSteerAuto && steerSwitchHigh != oldSteerSwitchRemote)
            {
                oldSteerSwitchRemote = steerSwitchHigh;
                //steerSwith is active low
                if (steerSwitchHigh == mfisBtnAutoSteerOn)
                {
                    mfbtnAutoSteer.PerformClick();
                }
            }

            if (isRemoteWorkSystemOn)
            {
                if (isWorkSwitchEnabled && (oldWorkSwitchHigh != workSwitchHigh))
                {
                    oldWorkSwitchHigh = workSwitchHigh;

                    if (workSwitchHigh != isWorkSwitchActiveLow)
                    {
                        if (isWorkSwitchManualSections)
                        {
                            if (mfmanualBtnState != btnStates.On)
                                mfbtnSectionMasterManual.PerformClick();
                        }
                        else
                        {
                            if (mfautoBtnState != btnStates.Auto)
                                mfbtnSectionMasterAuto.PerformClick();
                        }
                    }

                    else//Checks both on-screen buttons, performs click if button is not off
                    {
                        if (mfautoBtnState != btnStates.Off)
                            mfbtnSectionMasterAuto.PerformClick();
                        if (mfmanualBtnState != btnStates.Off)
                            mfbtnSectionMasterManual.PerformClick();
                    }
                }

                if (isSteerWorkSwitchEnabled && (oldSteerSwitchHigh != steerSwitchHigh))
                {
                    oldSteerSwitchHigh = steerSwitchHigh;

                    if ((mfisBtnAutoSteerOn && mfahrs.isAutoSteerAuto)
                        || !mfahrs.isAutoSteerAuto && !steerSwitchHigh)
                    {
                        if (isSteerWorkSwitchManualSections)
                        {
                            if (mfmanualBtnState != btnStates.On)
                                mfbtnSectionMasterManual.PerformClick();
                        }
                        else
                        {
                            if (mfautoBtnState != btnStates.Auto)
                                mfbtnSectionMasterAuto.PerformClick();
                        }
                    }

                    else//Checks both on-screen buttons, performs click if button is not off
                    {
                        if (mfautoBtnState != btnStates.Off)
                            mfbtnSectionMasterAuto.PerformClick();
                        if (mfmanualBtnState != btnStates.Off)
                            mfbtnSectionMasterManual.PerformClick();
                    }
                }
            }
        }
        **************/
    }
}

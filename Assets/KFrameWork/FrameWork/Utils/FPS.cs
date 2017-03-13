﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using KFrameWork;
using KUtils;

public class FPS : MonoBehaviour {


    void Start () {
        if(!MainLoop.getLoop().OpenFps){
           
            enabled = false;
        }
    }
        
    void OnGUI()
    {
        try
        {
            GUI.skin.label.normal.textColor = Color.black;
            GUI.Label(new Rect(Screen.width-150,0,150,60),string.Format("{0:F2} FPS",GameSyncCtr.mIns.Fps));
        }
        catch (Exception ex)
        {
            LogMgr.LogException(ex);
            this.enabled=false;
        }
        

    }
}

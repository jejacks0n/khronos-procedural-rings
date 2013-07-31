using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;

namespace Khronos
{

  public class ProceduralRingPartModule : PartModule
  {

    // Alerting
    float alertTime = 0;
    string alertText = null;


    public void alert(string message, float time)
    {
      alertTime = Time.time + time;
      alertText = message;
    }


    public void OnGUI()
    {
      if (!HighLogic.LoadedSceneIsEditor) return;

      if (Time.time < alertTime)
      {
        GUI.skin = HighLogic.Skin;
        GUIStyle style = new GUIStyle("Label");
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = 20;
        style.normal.textColor = Color.black;
        GUI.Label(new Rect(2, 2 + (Screen.height / 9), Screen.width, 50), alertText, style);
        style.normal.textColor = Color.yellow;
        GUI.Label(new Rect(0, Screen.height / 9, Screen.width, 50), alertText, style);
      }
    }
  }

}


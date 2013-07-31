using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;

namespace Khronos
{

  public class ProceduralRingsStrut : PartModule
  {
    [KSPField] public string  widthKey          = "w";
    [KSPField] public float   widthMin          = 0.2f;
    [KSPField] public float   widthMax          = 5f;
  
    [KSPField] public string  heightKey         = "h";
    [KSPField] public float   heightMin         = 0.2f;
    [KSPField] public float   heightMax         = 5f;
  
    [KSPField] public float   speedMultiplier   = 0.1f;
  
    [KSPField(isPersistant = true)] public float width = 1f;
    [KSPField(isPersistant = true)] public float height = 1f;

    public override string GetInfo()
    {
      string s = "Attach to a HyperRing Base and a toroid will be built in a different time stream and appear instantly.\n";
      if (!string.IsNullOrEmpty(widthKey)) s += "\nMouse over and hold '" + widthKey + "' to adjust the toroid width.";
      if (!string.IsNullOrEmpty(heightKey)) s += "\nMouse over and hold '" + heightKey + "' to adjust the toroid height.";
      return s;
    }


    public override void OnStart(StartState state)
    {
      if (state == StartState.None || (state & StartState.Editor) == 0) return;

      part.OnEditorAttach += OnEditorAttach;
    }


    public void OnMouseOver() {
      if (!HighLogic.LoadedSceneIsEditor || !part.isConnected) return;
  
      // todo: sizing should change weight and drag
      if (Input.GetKey(widthKey)) setWidth((Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")) * speedMultiplier);
      else if (Input.GetKey(heightKey)) setHeight((Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")) * speedMultiplier);
    }


    public void OnEditorAttach() {
      if (part.parent.GetComponent<ProceduralRingsBase>() != null) buildStrutsAndToroid();
      else alert("HyperRing Struts only work when attached to a HyperRing Base", 2);
    }


    void setWidth(float delta)
    {
      width += delta;
      width = Mathf.Max(width, widthMin);
      width = Mathf.Min(width, widthMax);
      calcShape();
    }
  
  
    void setHeight(float delta)
    {
      height += delta;
      height = Mathf.Max(height, heightMin);
      height = Mathf.Min(height, heightMax);
      calcShape();
    }

     
    void calcShape()
    {
      print(string.Format("[KPR] width {0}, height {1}", width, height));
    }


    void buildStrutsAndToroid()
    {
      print("!!!!!!!buildStrutsAndToroid");
    }


    // Alerting


    float alertTime = 0;
    string alertText = null;


    void alert(string message, float time)
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


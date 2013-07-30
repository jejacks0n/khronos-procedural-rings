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

      print("[KPR] Added strut part");
    }


    public void OnMouseOver() {
      if (!HighLogic.LoadedSceneIsEditor || !part.isConnected) return;
  
      // todo: sizing should change weight and drag
      if (Input.GetKey(widthKey))
      {
        setWidth((Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")) * speedMultiplier);
      }
      else if (Input.GetKey(heightKey))
      {
        setHeight((Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")) * speedMultiplier);
      }
    }
  
  
    void setWidth(float delta)
    {
      width += delta;
      width = Mathf.Max(width, widthMin);
      width = Mathf.Min(width, widthMax);

      print(string.Format("[KPR] adjusted width {0}", width));
      calcShape();
    }
  
  
    void setHeight(float delta)
    {
      height += delta;
      height = Mathf.Max(height, heightMin);
      height = Mathf.Min(height, heightMax);

      print(string.Format("[KPR] adjusted height {0}", height));
      calcShape();
    }


    void calcShape()
    {
    }
  }

}


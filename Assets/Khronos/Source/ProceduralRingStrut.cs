using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;

namespace Khronos
{

  public class ProceduralRingStrut : ProceduralRingPartModule
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
      string s = "Attach this strut to a HyperRing Base and a toroid will be built in a different time stream and appear instantly.\n";
      if (!string.IsNullOrEmpty(widthKey)) s += "\nMouse over and hold '" + widthKey + "' to adjust the toroid width.";
      if (!string.IsNullOrEmpty(heightKey)) s += "\nMouse over and hold '" + heightKey + "' to adjust the toroid height.";
      return s;
    }


    public override void OnStart(StartState state)
    {
      part.OnEditorAttach += OnEditorAttach;
      if (state == StartState.None || (state & StartState.Editor) == 0) return;
    }


    public void OnMouseOver() {
      if (!HighLogic.LoadedSceneIsEditor || !part.isConnected) return;
  
      // todo: sizing should change weight and drag
      if (Input.GetKey(widthKey)) setWidth((Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")) * speedMultiplier);
      else if (Input.GetKey(heightKey)) setHeight((Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")) * speedMultiplier);
    }


    public void OnEditorAttach() {
      // todo, this intentionally only happens once, but we may need to build each strut as a separate part, and only do a toroid pass once.
      if (part.symmetryMode > 0) return;

      if (part.parent.GetComponent<ProceduralRingBase>() != null)
      {
        buildStrut();
        buildToroid();
      }
      else alert("HyperRing Struts only work when attached to a HyperRing Base", 2);
    }


    public void OnDestroy()
    {
      print("[KPR] Strut OnDestroy");
    }


    private void setWidth(float delta)
    {
      width += delta;
      width = Mathf.Max(width, widthMin);
      width = Mathf.Min(width, widthMax);
      calcShape();
    }
  
  
    private void setHeight(float delta)
    {
      height += delta;
      height = Mathf.Max(height, heightMin);
      height = Mathf.Min(height, heightMax);
      calcShape();
    }

     
    private void calcShape()
    {
      print(string.Format("[KPR] width {0}, height {1}", width, height));
    }


    private void buildStrut()
    {
      print("[KPR] Strut buildStrut");
    }


    private void buildToroid()
    {
      print("[KPR] Strut buildToroid");
    }
  }

}


using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;

namespace Khronos
{

  public class ProceduralRingsBase : PartModule
  {
  //  [KSPField] public float   verticalStep    = 0.2f;
  //  [KSPField] public int     circleSegments  = 24;
  //  [KSPField] public float   sideThickness   = 0.05f;

    [KSPField] public float   baseSize         = 1.25f;

    [KSPField] public string  radiusKey        = "r";
    [KSPField] public float   radiusMin        = 1f;
    [KSPField] public float   radiusMax        = 30f;

    [KSPField] public float   speedMultiplier  = 0.5f;

    [KSPField] public int     outlineSlices   = 12;
    [KSPField] public Vector4 outlineColor    = new Vector4(0, 0, 0.2f, 1);
    [KSPField] public float   outlineWidth    = 0.05f;

    [KSPField(isPersistant = true)] public float radius = 5f;

    LineRenderer line = null;
    List<LineRenderer> outline=new List<LineRenderer>();
//    List<ConfigurableJoint> joints=new List<ConfigurableJoint>();

    public override string GetInfo()
    {
      string s = "Attach a HyperRing Strut and a toroid will be built in a different time stream and appear instantly.\n";
      if (!string.IsNullOrEmpty(radiusKey)) s += "\nMouse over and hold '" + radiusKey + "' to adjust radius.";
      return s;
    }


    public override void OnStart(StartState state)
    {
      if (state == StartState.None || (state & StartState.Editor) == 0) return;

      print("[KPR] Added base part");
      if (line) line.transform.Rotate(0, 90, 0);

      destroyOutline();
      for (int i = 0; i < outlineSlices; ++i)
      {
        var r = makeLineRenderer("fairing outline", outlineColor, outlineWidth);
        outline.Add(r);
        r.transform.Rotate(0, i * 360f / outlineSlices, 0);
      }
    }


    public void OnMouseOver()
    {
      if (!HighLogic.LoadedSceneIsEditor || !part.isConnected) return;
      if (!Input.GetKey(radiusKey)) return;

      setRadius((Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")) * speedMultiplier);
    }


    public void OnDestroy()
    {
      if (line) {
        UnityEngine.Object.Destroy(line.gameObject);
        line = null;
      }
      destroyOutline();
    }


    void setRadius(float delta)
    {
      radius += delta;
      radius = Mathf.Max(radius, radiusMin);
      radius = Mathf.Min(radius, radiusMax);

      print(string.Format("[KPR] adjusted radius {0}", radius));
      calcShape();
    }


    void calcShape()
    {
      print("[KPR] caclulating shape");
    }


    LineRenderer makeLineRenderer(string name, Color color, float wd)
    {
      var o=new GameObject(name);
      o.transform.parent = part.transform;
      o.transform.localPosition = Vector3.zero;
      o.transform.localRotation = Quaternion.identity;
      var r = o.AddComponent<LineRenderer>();
      r.useWorldSpace = false;
      r.material = new Material(Shader.Find("Particles/Additive"));
      r.SetColors(color, color);
      r.SetWidth(wd, wd);
      r.SetVertexCount(0);
      return r;
    }


    void destroyOutline()
    {
      foreach (var r in outline) UnityEngine.Object.Destroy(r.gameObject);
      outline.Clear();
    }
  }

}


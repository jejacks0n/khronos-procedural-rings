using UnityEngine;
using System.Collections.Generic;

namespace Khronos
{

  public class ProceduralRingHub : ProceduralRingPartModule
  {
    [KSPField] public float   hubSize         = 1.25f;

    [KSPField] public float   radiusMin        = 1f;
    [KSPField] public float   radiusMax        = 30f;

    [KSPField] public float   speedMultiplier  = 0.5f;
    [KSPField] public int     outlineSegments  = 32;

    [KSPField(isPersistant = true)] public float radius = 5f;

    public List<ProceduralRingStrut> struts = new List<ProceduralRingStrut>();

    public override string GetInfo()
    {
      string s = "Attach a HyperRing Strut and a toroid will be built in a different time stream and appear instantly.\n";
      if (!string.IsNullOrEmpty(radiusKey)) s += "\nMouse over and hold '" + radiusKey + "' to adjust radius.";
      return s;
    }


    public override void OnStart(StartState state)
    {
      if (state == StartState.None || (state & StartState.Editor) == 0)
      {}
      else
      {
        createOutline();
      }
    }


    public void OnMouseOver()
    {
      if (!HighLogic.LoadedSceneIsEditor || !part.isConnected) return;

      if (Input.GetKey(radiusKey))
      {
        setRadius((Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")));
//        foreach (var strut in struts) strut.updateOutline();
      }
//      if (Input.GetKey(widthKey)) setWidth((Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")));
//      else if (Input.GetKey(heightKey)) setHeight((Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")));
    }


    public void OnDestroy()
    {
      destroyOutline();
    }


    public float getRadius(bool includeOffset = false)
    {
      return radius - (hubSize / 2);
    }


    public void setRadius(float delta)
    {
      radius += delta * speedMultiplier;
      radius = Mathf.Max(radius, radiusMin);
      radius = Mathf.Min(radius, radiusMax);

      updateOutline();
    }


    private void createOutline()
    {
      destroyOutline();
      outline = makeLineRenderer("radial", outlineColor, outlineWidth, outlineSegments);
      updateOutline();
    }


    public void updateOutline()
    {
      float delta = (2.0f * Mathf.PI) / outlineSegments;
      float theta = 0;

      for (int i = 0; i <= outlineSegments; i++)
      {
        outline.SetPosition(i, new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta)));
        theta += delta;
      }

      foreach (var strut in struts) strut.updateOutline();
    }
  }

}


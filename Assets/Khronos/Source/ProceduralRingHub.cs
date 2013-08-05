using UnityEngine;
using System.Collections.Generic;

namespace Khronos
{
   
  public class ProceduralRingHub : ProceduralRingPartModule
  {
    [KSPField] public float   hubSize          = 1.25f;

    [KSPField] public float   radiusMin        = 1f;
    [KSPField] public float   radiusMax        = 30f;

    [KSPField] public int     outlineSegments  = 32;

    [KSPField(isPersistant = true)] public float radius = 5f;

    public override string GetInfo()
    {
      return "Attach a HyperRing Strut and a toroid will be built in a different time stream and appear instantly.\n" + keyboardControls();
    }


    public void OnMouseOver()
    {
      if (!HighLogic.LoadedSceneIsEditor) return;

      if (Input.GetKey(radiusKey)) setRadius((Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")));
      if (Input.GetKey(widthKey)) setWidth((Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")));
      else if (Input.GetKey(heightKey)) setHeight((Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")));
    }


    public override void enableForEditor()
    {
      createOutline();
      buildTorus();
    }


    public void buildTorus()
    {
//      mesh = Primitives.Torus()
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


    public void setWidth(float delta)
    {
    }


    public void setHeight(float delta)
    {
    }


    private void createOutline()
    {
      destroyOutline();
      outline = makeLineRenderer("radial", outlineColor, outlineWidth, outlineSegments);
      updateOutline();
    }


    private void updateOutline()
    {
      float delta = (2.0f * Mathf.PI) / outlineSegments;
      float theta = 0;

      for (int i = 0; i <= outlineSegments; i++)
      {
        outline.SetPosition(i, new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta)));
        theta += delta;
      }
      updateStrutOutlines();
    }


    private void updateStrutOutlines()
    {
      foreach (var child in part.children)
      {
        ProceduralRingStrut strut = child.GetComponent<ProceduralRingStrut>();
        if (strut != null)
        {
          strut.hub = this;
          strut.createOutline();
        }
      }
    }


    public override void drawMesh()
    {
      createOutline();
    }
  }

}


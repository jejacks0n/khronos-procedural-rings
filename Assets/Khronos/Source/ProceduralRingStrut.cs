using UnityEngine;

namespace Khronos
{ 
   
  public class ProceduralRingStrut : ProceduralRingPartModule
  {
    [KSPField] public float   widthMin          = 0.2f;
    [KSPField] public float   widthMax          = 5f;
     
    [KSPField] public float   heightMin         = 0.2f;
    [KSPField] public float   heightMax         = 5f;

    [KSPField] public string  meshName          = "strut";

    [KSPField(isPersistant = true)] public float width = 1f;
    [KSPField(isPersistant = true)] public float height = 1f;

    public ProceduralRingHub hub = null;

    public override string GetInfo()
    { 
      return "Attach this strut to a HyperRing Hub and a toroid will be built in a different time stream and appear instantly.\n" + keyboardControls();
    }


    public void OnEditorAttach() {
      hub = part.parent.GetComponent<ProceduralRingHub>();
      if (hub != null) {
        log("parent {0} {1}", part.parent.partName, part.parent.name);
        createOutline();
      }
      else
      {
        if (part.symmetryMode > 0) return;
        alert("HyperRing Struts only work when attached to a HyperRing Hub", 2);
      }
    }


    public void OnEditorDetach()
    {
      hub = null;
      destroyOutline();
    } 


    public override void OnDestroy()
    {
      hub = null;
      destroyOutline();
    }


    public override void enableForEditor()
    {
      part.OnEditorAttach += OnEditorAttach;
      part.OnEditorDetach += OnEditorDetach;

      if (part.parent) hub = part.parent.GetComponent<ProceduralRingHub>();
      if (hub && !part.isClone) createOutline();
    }


    public void createOutline()
    {
//      drawMesh();
//      return;
      destroyOutline();
      outline = makeLineRenderer("strut", outlineColor, outlineWidth);
      outline.transform.Rotate(0, -90, 0);
      updateOutline();

      float radius = hub.radius;
      int outlineSegments = 20;
      LineRenderer testline = makeLineRenderer("test", outlineColor, outlineWidth, outlineSegments);
//      testline.transform.rotation = Quaternion.Inverse(outline.transform.rotation);

      float delta = (2.0f * Mathf.PI) / outlineSegments;
      float theta = 0;

      for (int i = 0; i <= outlineSegments; i++)
      {
        testline.SetPosition(i, new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta)));
        theta += delta;
      }
    }


    private void updateOutline()
    {
      outline.SetPosition(1, new Vector3(0, 0, hub.getRadius(true) - 0.2f));
    }


    public override void drawMesh()
    {
//      if (hub) createOutline();
      if (!hub) return;

      MeshFilter mf = null;
      Mesh m = null;
      mf = part.FindModelComponent<MeshFilter>(meshName);
      if (mf) m = mf.mesh;

      if (!mf || !m)
      {
        error("no mesh matching '{0}' found in strut", part);
        return;
      }



    }
  }

}

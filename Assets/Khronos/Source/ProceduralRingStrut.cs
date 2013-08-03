using UnityEngine;

namespace Khronos
{ 
   
  public class ProceduralRingStrut : ProceduralRingPartModule
  {
    [KSPField] public float   widthMin          = 0.2f;
    [KSPField] public float   widthMax          = 5f;
     
    [KSPField] public float   heightMin         = 0.2f;
    [KSPField] public float   heightMax         = 5f;

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
        print(string.Format("[KPR] parent {0} {1}", part.parent.partName, part.parent.name));
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
      destroyOutline();
      outline = makeLineRenderer("strut", outlineColor, outlineWidth);
      outline.transform.Rotate(0, -90, 0);
      updateOutline();
    }


    private void updateOutline()
    {
      outline.SetPosition(1, new Vector3(0, 0, hub.getRadius(true) - 0.2f));
    }


    public override void drawMesh()
    {
      if (hub) createOutline();
    }
  }

}

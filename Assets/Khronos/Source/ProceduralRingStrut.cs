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

    public ProceduralRingHub hub = null;

    public override string GetInfo()
    {
      string s = "Attach this strut to a HyperRing Hub and a toroid will be built in a different time stream and appear instantly.\n";
      if (!string.IsNullOrEmpty(widthKey)) s += "\nMouse over and hold '" + widthKey + "' to adjust the toroid width.";
      if (!string.IsNullOrEmpty(heightKey)) s += "\nMouse over and hold '" + heightKey + "' to adjust the toroid height.";
      return s;
    }


    public override void OnStart(StartState state)
    {
      if (state == StartState.None || (state & StartState.Editor) == 0)
      {}
      else
      {
        part.OnEditorAttach += OnEditorAttach;
        part.OnEditorDetach += OnEditorDetach;
        if (hub = part.parent.GetComponent<ProceduralRingHub>() && !part.isClone)
        {
          createOutline();
          updateOutline();
        }
      }
    }


    public void OnEditorAttach() {
      if (hub = part.parent.GetComponent<ProceduralRingHub>())
      {
        createOutline();
        updateOutline();
      }
      else
      {
        if (part.symmetryMode > 0) return;
        alert("HyperRing Struts only work when attached to a HyperRing Hub", 2);
      }
    }


    public void OnEditorDetach()
    {
      destroyOutline();
    }


//    private void setWidth(float delta)
//    {
//      width += delta * speedMultiplier;
//      width = Mathf.Max(width, widthMin);
//      width = Mathf.Min(width, widthMax);
////      hub.setWidth(width);
//    }
//
//
//    private void setHeight(float delta)
//    {
//      height += delta * speedMultiplier;
//      height = Mathf.Max(height, heightMin);
//      height = Mathf.Min(height, heightMax);
////      hub.setHeight(height);
//    }


    private void createOutline()
    {
      destroyOutline();
      outline = makeLineRenderer("strut", outlineColor, outlineWidth);
      outline.transform.Rotate(0, -90, 0);
    }


    public void updateOutline()
    {
      outline.SetPosition(1, new Vector3(0, 0, hub.getRadius(true) - 0.2f));
    }
  }

}

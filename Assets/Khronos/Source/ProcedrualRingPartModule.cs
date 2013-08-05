using UnityEngine;

namespace Khronos
{

  public class ProceduralRingPartModule : PartModule
  {
    [KSPField] public Vector4 outlineColor     = Color.red; //new Vector4(0, 0, 0.2f, 1);
    [KSPField] public float   outlineWidth     = 0.05f;

    [KSPField] public string  radiusKey        = "r";
    [KSPField] public string  widthKey         = "w";
    [KSPField] public string  heightKey        = "h";

    [KSPField] public float   speedMultiplier  = 0.5f;

    protected LineRenderer outline = null;

    float alertTime = 0;
    string alertText = null;

    public override void OnStart(StartState state)
    {
      if (state == StartState.None) return;
      else if ((state & StartState.Editor) == 0) drawMesh();
      else enableForEditor();
    }


    public virtual void OnDestroy()
    {
      destroyOutline();
    }


    public virtual void destroyOutline()
    {
      if (!outline) return;
      UnityEngine.Object.Destroy(outline.gameObject);
      outline = null;
    }


    public virtual void enableForEditor()
    {
    }


    public virtual void drawMesh()
    {
    }


    protected string keyboardControls()
    {
      string s = "";
      if (!string.IsNullOrEmpty(radiusKey)) s += "\nMouse over and hold '" + radiusKey + "' to adjust radius.";
      if (!string.IsNullOrEmpty(widthKey))  s += "\nMouse over and hold '" + widthKey + "' to adjust the toroid width.";
      if (!string.IsNullOrEmpty(heightKey)) s += "\nMouse over and hold '" + heightKey + "' to adjust the toroid height.";
      return s;
    }


    protected LineRenderer makeLineRenderer(string name, Color color, float width, int vertexCount = 1)
    {
      var o = new GameObject(name);
      o.transform.parent = part.transform;
      o.transform.localPosition = Vector3.zero;
      o.transform.localRotation = Quaternion.identity;
      var r = o.AddComponent<LineRenderer>();
      r.useWorldSpace = false;
      r.material = new Material(Shader.Find("Particles/Additive"));
      r.SetColors(color, color);
      r.SetWidth(width, width);
      r.SetVertexCount(vertexCount + 1);
      return r;
    }


    // Logging / Alerting


    protected void log(string message, params object[] list)
    {
      message = "[KPR] " + message;
      if (list.Length > 0) message = string.Format(message, list);
      Debug.Log(message);
    }


    protected void error(string message, params object[] list)
    {
      message = "[KPR] " + message;
      if (list.Length > 0) message = string.Format(message, list);
      Debug.LogError(message);
    }


    protected void alert(string message, float time)
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


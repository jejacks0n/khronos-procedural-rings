using UnityEngine;

namespace Khronos
{

  public class ProceduralRingPartModule : PartModule
  {
    [KSPField] public Vector4 outlineColor     = Color.red; //new Vector4(0, 0, 0.2f, 1);
    [KSPField] public float   outlineWidth     = 0.05f;

    [KSPField] public string  radiusKey        = "r";

    protected LineRenderer outline = null;

    float alertTime = 0;
    string alertText = null;


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


    protected void destroyOutline()
    {
      if (!outline) return;
      UnityEngine.Object.Destroy(outline.gameObject);
      outline = null;
    }


    // Alerting


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


    protected void alert(string message, float time)
    {
      alertTime = Time.time + time;
      alertText = message;
    }
  }

}


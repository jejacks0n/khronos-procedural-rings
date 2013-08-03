using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;

//
//public class ProceduralFairingBase : PartModule
//{
//  [KSPField] public float outlineWidth=0.05f;
//  [KSPField] public int   outlineSlices=12;
//  [KSPField] public Vector4 outlineColor=new Vector4(0, 0, 0.2f, 1);
//  [KSPField] public float verticalStep=0.2f;
//  [KSPField] public float baseSize=1.25f;
//  [KSPField(isPersistant=true)] public float extraRadius=0.2f;
//  [KSPField] public int circleSegments=24;
//
//  [KSPField] public float sideThickness=0.05f;
//
//  [KSPField] public string extraRadiusKey="r";
//
//  float updateDelay=0;
//
//
//  LineRenderer line=null;
//
//  List<LineRenderer> outline=new List<LineRenderer>();
//
//
//  List<ConfigurableJoint> joints=new List<ConfigurableJoint>();
//
//
//  public void OnMouseOver()
//  {
//    if (HighLogic.LoadedSceneIsEditor)
//    {
//      if (part.isConnected && Input.GetKey(extraRadiusKey))
//      {
//        float old=extraRadius;
//        extraRadius+=(Input.GetAxis("Mouse X")+Input.GetAxis("Mouse Y"))*0.1f;
//        extraRadius=Mathf.Max(extraRadius, -1);
//        extraRadius=Mathf.Min(extraRadius, 100);
//        if (extraRadius!=old) updateDelay=0;
//      }
//    }
//  }
//
//
//  public override string GetInfo()
//  {
//    string s="Attach side fairings and they'll be shaped for your attached payload.\n"+
//      "Remember to add a decoupler if you need one.";
//
//    if (!string.IsNullOrEmpty(extraRadiusKey))
//      s+="\nMouse over and hold '"+extraRadiusKey+"' to adjust extra radius.";
//
//    return s;
//  }
//
//
//  public override void OnStart(StartState state)
//  {
//    if (state==StartState.None) return;
//
//    if ((state & StartState.Editor)!=0)
//    {
//      // line=makeLineRenderer("payload outline", Color.red, outlineWidth);
//      if (line) line.transform.Rotate(0, 90, 0);
//
//      destroyOutline();
//      for (int i=0; i<outlineSlices; ++i)
//      {
//        var r=makeLineRenderer("fairing outline", outlineColor, outlineWidth);
//        outline.Add(r);
//        r.transform.Rotate(0, i*360f/outlineSlices, 0);
//      }
//
//      updateDelay=0;
//    }
//  }
//
//
//  public void OnPartPack()
//  {
//    while (joints.Count>0)
//    {
//      int i=joints.Count-1;
//      var joint=joints[i]; joints.RemoveAt(i);
//      UnityEngine.Object.Destroy(joint);
//    }
//  }
//
//
//  public void OnPartUnpack()
//  {
//    if (!HighLogic.LoadedSceneIsEditor)
//    {
//      // strut side fairings together
//      var attached=part.findAttachNodes("connect");
//      for (int i=0; i<attached.Length; ++i)
//      {
//        var p=attached[i].attachedPart;
//        if (p==null || p.Rigidbody==null) continue;
//
//        // var sf=p.GetComponent<ProceduralFairingSide>();
//        // if (sf==null) continue;
//
//        var pp=attached[i>0 ? i-1 : attached.Length-1].attachedPart;
//        if (pp==null) continue;
//
//        var rb=pp.Rigidbody;
//        if (rb==null) continue;
//
//        var joint=p.gameObject.AddComponent<ConfigurableJoint>();
//        joint.xMotion=ConfigurableJointMotion.Locked;
//        joint.yMotion=ConfigurableJointMotion.Locked;
//        joint.zMotion=ConfigurableJointMotion.Locked;
//        joint.angularXMotion=ConfigurableJointMotion.Locked;
//        joint.angularYMotion=ConfigurableJointMotion.Locked;
//        joint.angularZMotion=ConfigurableJointMotion.Locked;
//        joint.projectionDistance=0.1f;
//        joint.projectionAngle=5;
//        joint.breakForce =p.breakingForce ;
//        joint.breakTorque=p.breakingTorque;
//        joint.connectedBody=rb;
//
//        joints.Add(joint);
//      }
//    }
//  }
//
//
//  LineRenderer makeLineRenderer(string name, Color color, float wd)
//  {
//    var o=new GameObject(name);
//    o.transform.parent=part.transform;
//    o.transform.localPosition=Vector3.zero;
//    o.transform.localRotation=Quaternion.identity;
//    var r=o.AddComponent<LineRenderer>();
//    r.useWorldSpace=false;
//    r.material=new Material(Shader.Find("Particles/Additive"));
//    r.SetColors(color, color);
//    r.SetWidth(wd, wd);
//    r.SetVertexCount(0);
//    return r;
//  }
//
//
//  void destroyOutline()
//  {
//    foreach (var r in outline) UnityEngine.Object.Destroy(r.gameObject);
//    outline.Clear();
//  }
//
//
//  public void OnDestroy()
//  {
//    if (line) { UnityEngine.Object.Destroy(line.gameObject); line=null; }
//    destroyOutline();
//  }
//
//
//  public void Update()
//  {
//    if (HighLogic.LoadedSceneIsEditor)
//    {
//      updateDelay-=Time.deltaTime;
//      if (updateDelay<=0) { recalcShape(); updateDelay=0.2f; }
//    }
//  }
//
//
//  static public Vector3[] buildFairingShape(float baseRad, float maxRad,
//    float cylStart, float cylEnd, float noseHeightRatio,
//    Vector4 baseConeShape, Vector4 noseConeShape,
//    int baseConeSegments, int noseConeSegments,
//    Vector4 vertMapping, float mappingScaleY)
//  {
//    float baseConeRad=maxRad-baseRad;
//    float tip=maxRad*noseHeightRatio;
//
//    var baseSlope=new BezierSlope(baseConeShape);
//    var noseSlope=new BezierSlope(noseConeShape);
//
//    float baseV0=vertMapping.x/mappingScaleY;
//    float baseV1=vertMapping.y/mappingScaleY;
//    float noseV0=vertMapping.z/mappingScaleY;
//    float noseV1=vertMapping.w/mappingScaleY;
//
//    var shape=new Vector3[1+(cylStart==0?0:baseConeSegments)+1+noseConeSegments];
//    int vi=0;
//
//    if (cylStart!=0)
//    {
//      for (int i=0; i<=baseConeSegments; ++i, ++vi)
//      {
//        float t=(float)i/baseConeSegments;
//        var p=baseSlope.interp(t);
//        shape[vi]=new Vector3(p.x*baseConeRad+baseRad, p.y*cylStart,
//          Mathf.Lerp(baseV0, baseV1, t));
//      }
//    }
//    else
//      shape[vi++]=new Vector3(baseRad, 0, baseV1);
//
//    for (int i=0; i<=noseConeSegments; ++i, ++vi)
//    {
//      float t=(float)i/noseConeSegments;
//      var p=noseSlope.interp(1-t);
//      shape[vi]=new Vector3(p.x*maxRad, (1-p.y)*tip+cylEnd,
//        Mathf.Lerp(noseV0, noseV1, t));
//    }
//
//    return shape;
//  }
//
//
//  static public Vector3[] buildInlineFairingShape(float baseRad, float maxRad, float topRad,
//    float cylStart, float cylEnd, float top,
//    Vector4 baseConeShape,
//    int baseConeSegments,
//    Vector4 vertMapping, float mappingScaleY)
//  {
//    float baseConeRad=maxRad-baseRad;
//    float  topConeRad=maxRad- topRad;
//
//    var baseSlope=new BezierSlope(baseConeShape);
//
//    float baseV0=vertMapping.x/mappingScaleY;
//    float baseV1=vertMapping.y/mappingScaleY;
//    float noseV0=vertMapping.z/mappingScaleY;
//
//    var shape=new Vector3[2+(cylStart==0?0:baseConeSegments+1)+(cylEnd==top?0:baseConeSegments+1)];
//    int vi=0;
//
//    if (cylStart!=0)
//    {
//      for (int i=0; i<=baseConeSegments; ++i, ++vi)
//      {
//        float t=(float)i/baseConeSegments;
//        var p=baseSlope.interp(t);
//        shape[vi]=new Vector3(p.x*baseConeRad+baseRad, p.y*cylStart,
//          Mathf.Lerp(baseV0, baseV1, t));
//      }
//    }
//
//    shape[vi++]=new Vector3(maxRad, cylStart, baseV1);
//    shape[vi++]=new Vector3(maxRad, cylEnd, noseV0);
//
//    if (cylEnd!=top)
//    {
//      for (int i=0; i<=baseConeSegments; ++i, ++vi)
//      {
//        float t=(float)i/baseConeSegments;
//        var p=baseSlope.interp(1-t);
//        shape[vi]=new Vector3(p.x*topConeRad+topRad, Mathf.Lerp(top, cylEnd, p.y),
//          Mathf.Lerp(baseV1, baseV0, t));
//      }
//    }
//
//    return shape;
//  }
//
//
//  void recalcShape()
//  {
//    // scan payload and build its profile
//    var scan=new PayloadScan(part, verticalStep, extraRadius);
//
//    AttachNode node=part.findAttachNode("top");
//    if (node!=null && node.attachedPart!=null)
//    {
//      scan.ofs=node.position.y;
//      scan.addPart(node.attachedPart, part);
//    }
//
//    for (int i=0; i<scan.payload.Count; ++i)
//    {
//      var cp=scan.payload[i];
//
//      // add connected payload parts
//      scan.addPart(cp.parent, cp);
//      foreach (var p in cp.children) scan.addPart(p, cp);
//
//      // scan part colliders
//      foreach (var coll in cp.FindModelComponents<Collider>())
//      {
//        if (coll.tag!="Untagged") continue; // skip ladders etc.
//        scan.addPayload(coll);
//      }
//    }
//
//    // check for reversed base for inline fairings
//    float topY=0;
//    ProceduralFairingBase topBase=null;
//    if (scan.targets.Count>0)
//    {
//      topBase=scan.targets[0].GetComponent<ProceduralFairingBase>();
//      topY=scan.w2l.MultiplyPoint3x4(topBase.part.transform.position).y;
//      if (topY<scan.ofs) topY=scan.ofs;
//    }
//
//    if (scan.profile.Count<=0)
//    {
//      // no payload
//      if (line) line.SetVertexCount(0);
//      foreach (var lr in outline) lr.SetVertexCount(0);
//      return;
//    }
//
//    // fill profile outline (for debugging)
//    if (line)
//    {
//      line.SetVertexCount(scan.profile.Count*2+2);
//
//      float prevRad=0;
//      int hi=0;
//      foreach (var r in scan.profile)
//      {
//        line.SetPosition(hi*2  , new Vector3(prevRad, hi*verticalStep+scan.ofs, 0));
//        line.SetPosition(hi*2+1, new Vector3(      r, hi*verticalStep+scan.ofs, 0));
//        hi++; prevRad=r;
//      }
//
//      line.SetPosition(hi*2  , new Vector3(prevRad, hi*verticalStep+scan.ofs, 0));
//      line.SetPosition(hi*2+1, new Vector3(      0, hi*verticalStep+scan.ofs, 0));
//    }
//
//    // check attached side parts and get params
//    var attached=part.findAttachNodes("connect");
//    int numSideParts=attached.Length;
//
//    var sideNode=attached.FirstOrDefault(n => n.attachedPart
//      && n.attachedPart.GetComponent<ProceduralFairingSide>());
//
//    float noseHeightRatio=2;
//    Vector4 baseConeShape=new Vector4(0.5f, 0, 1, 0.5f);
//    Vector4 noseConeShape=new Vector4(0.5f, 0, 1, 0.5f);
//    int baseConeSegments=1;
//    int noseConeSegments=1;
//    Vector2 mappingScale=new Vector2(1024, 1024);
//    Vector2 stripMapping=new Vector2(992, 1024);
//    Vector4  horMapping=new Vector4(0, 480, 512,  992);
//    Vector4 vertMapping=new Vector4(0, 160, 704, 1024);
//
//    if (sideNode!=null)
//    {
//      var sf=sideNode.attachedPart.GetComponent<ProceduralFairingSide>();
//      noseHeightRatio =sf.noseHeightRatio ;
//      baseConeShape   =sf.baseConeShape   ;
//      noseConeShape   =sf.noseConeShape   ;
//      baseConeSegments=sf.baseConeSegments;
//      noseConeSegments=sf.noseConeSegments;
//      mappingScale    =sf.mappingScale    ;
//      stripMapping    =sf.stripMapping    ;
//      horMapping      =sf.horMapping      ;
//      vertMapping     =sf.vertMapping     ;
//    }
//
//    // compute fairing shape
//    float baseRad=baseSize*0.5f;
//
//    float cylStart=0;
//    float maxRad=scan.profile.Max();
//
//    float topRad=0;
//    if (topBase!=null)
//    {
//      topRad=topBase.baseSize*0.5f;
//      maxRad=Mathf.Max(maxRad, topRad);
//    }
//
//    if (maxRad>baseRad)
//    {
//      // try to fit base cone as high as possible
//      cylStart=scan.ofs;
//      for (int i=1; i<scan.profile.Count; ++i)
//      {
//        float y=i*verticalStep+scan.ofs;
//        float r0=baseRad;
//        float k=(maxRad-r0)/y;
//
//        bool ok=true;
//        float r=r0+k*scan.ofs;
//        for (int j=0; j<i; ++j, r+=k*verticalStep)
//          if (scan.profile[j]>r) { ok=false; break; }
//
//        if (!ok) break;
//        cylStart=y;
//      }
//    }
//    else
//      maxRad=baseRad; // no base cone, just cylinder and nose
//
//    float cylEnd=scan.profile.Count*verticalStep+scan.ofs;
//
//    if (topBase!=null)
//    {
//      cylEnd=topY;
//
//      if (maxRad>topRad)
//      {
//        // try to fit top cone as low as possible
//        for (int i=scan.profile.Count-1; i>=0; --i)
//        {
//          float y=i*verticalStep+scan.ofs;
//          float r0=topRad;
//          float k=(maxRad-r0)/(y-topY);
//
//          bool ok=true;
//          float r=maxRad+k*verticalStep;
//          for (int j=i; j<scan.profile.Count; ++j, r+=k*verticalStep)
//            if (scan.profile[j]>r) { ok=false; break; }
//
//          if (!ok) break;
//
//          cylEnd=y;
//        }
//      }
//    }
//    else
//    {
//      // try to fit nose cone as low as possible
//      for (int i=scan.profile.Count-1; i>=0; --i)
//      {
//        float s=verticalStep/noseHeightRatio;
//
//        bool ok=true;
//        float r=maxRad-s;
//        for (int j=i; j<scan.profile.Count; ++j, r-=s)
//          if (scan.profile[j]>r) { ok=false; break; }
//
//        if (!ok) break;
//
//        float y=i*verticalStep+scan.ofs;
//        cylEnd=y;
//      }
//    }
//
//    if (cylStart>cylEnd) cylStart=cylEnd;
//
//    // build fairing shape line
//    Vector3[] shape;
//
//    if (topBase!=null)
//      shape=buildInlineFairingShape(baseRad, maxRad, topRad, cylStart, cylEnd, topY,
//        baseConeShape, baseConeSegments,
//        vertMapping, mappingScale.y);
//    else
//      shape=buildFairingShape(baseRad, maxRad, cylStart, cylEnd, noseHeightRatio,
//        baseConeShape, noseConeShape, baseConeSegments, noseConeSegments,
//        vertMapping, mappingScale.y);
//
//    if (sideNode==null)
//    {
//      // no side parts - fill fairing outlines
//      foreach (var lr in outline)
//      {
//        lr.SetVertexCount(shape.Length);
//        for (int i=0; i<shape.Length; ++i)
//          lr.SetPosition(i, new Vector3(shape[i].x, shape[i].y));
//      }
//    }
//    else
//    {
//      foreach (var lr in outline)
//        lr.SetVertexCount(0);
//    }
//
//    // rebuild side parts
//    int numSegs=circleSegments/numSideParts;
//    if (numSegs<2) numSegs=2;
//
//    foreach (var sn in attached)
//    {
//      var sp=sn.attachedPart;
//      if (!sp) continue;
//      var sf=sp.GetComponent<ProceduralFairingSide>();
//      if (!sf) continue;
//
//      var mf=sp.FindModelComponent<MeshFilter>("model");
//      if (!mf) { Debug.LogError("[ProceduralFairingBase] no model in side fairing", sp); continue; }
//
//      var nodePos=sn.position;
//
//      mf.transform.position=part.transform.position;
//      mf.transform.rotation=part.transform.rotation;
//      float ra=Mathf.Atan2(-nodePos.z, nodePos.x)*Mathf.Rad2Deg;
//      mf.transform.Rotate(0, ra, 0);
//
//      sf.meshPos=mf.transform.localPosition;
//      sf.meshRot=mf.transform.localRotation;
//      sf.numSegs=numSegs;
//      sf.numSideParts=numSideParts;
//      sf.baseRad=baseRad;
//      sf.maxRad=maxRad;
//      sf.cylStart=cylStart;
//      sf.cylEnd=cylEnd;
//      sf.topRad=topRad;
//      sf.inlineHeight=topY;
//      sf.sideThickness=sideThickness;
//      sf.rebuildMesh();
//    }
//  }
//}
//
//
////ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ//
//
//
//public class ProceduralFairingSide : PartModule
//{
//  [KSPField] public float ejectionDv=15;
//  [KSPField] public float ejectionTorque=0.2f;
//  [KSPField] public float ejectionLowDv=1;
//  [KSPField] public float ejectionLowTorque=0.01f;
//
//  [KSPField] public string ejectionPowerKey="f";
//  [KSPField(isPersistant=true)] public float ejectionPower=1;
//
//  [KSPField] public string ejectSoundUrl="Squad/Sounds/sound_decoupler_fire";
//  public FXGroup ejectFx;
//
//  [KSPField] public float noseHeightRatio=2;
//  [KSPField] public Vector4 baseConeShape=new Vector4(0.5f, 0, 1, 0.5f);
//  [KSPField] public Vector4 noseConeShape=new Vector4(0.5f, 0, 1, 0.5f);
//  [KSPField] public int baseConeSegments=5;
//  [KSPField] public int noseConeSegments=7;
//
//  [KSPField] public Vector2 mappingScale=new Vector2(1024, 1024);
//  [KSPField] public Vector2 stripMapping=new Vector2(992, 1024);
//  [KSPField] public Vector4   horMapping=new Vector4(0, 480, 512, 992);
//  [KSPField] public Vector4  vertMapping=new Vector4(0, 160, 704, 1024);
//
//  [KSPField] public float density=0.2f;
//  [KSPField] public float specificBreakingForce =2000;
//  [KSPField] public float specificBreakingTorque=2000;
//
//  [KSPField(isPersistant=true)] public int numSegs=12;
//  [KSPField(isPersistant=true)] public int numSideParts=2;
//  [KSPField(isPersistant=true)] public float baseRad=1.25f;
//  [KSPField(isPersistant=true)] public float  maxRad=1.50f;
//  [KSPField(isPersistant=true)] public float cylStart=0.5f;
//  [KSPField(isPersistant=true)] public float cylEnd  =2.5f;
//  [KSPField(isPersistant=true)] public float topRad=0;
//  [KSPField(isPersistant=true)] public float inlineHeight=0;
//  [KSPField(isPersistant=true)] public float sideThickness=0.05f;
//  [KSPField(isPersistant=true)] public Vector3 meshPos=Vector3.zero;
//  [KSPField(isPersistant=true)] public Quaternion meshRot=Quaternion.identity;
//
//
//  public override string GetInfo()
//  {
//    string s="Attach to Keramzit's fairing base to reshape.";
//
//    if (!string.IsNullOrEmpty(ejectionPowerKey))
//      s+="\nMouse over and press '"+ejectionPowerKey+"' to change ejection force.";
//
//    return s;
//  }
//
//
//  public override void OnStart(StartState state)
//  {
//    if (state==StartState.None) return;
//
//    ejectFx.audio=part.gameObject.AddComponent<AudioSource>();
//    ejectFx.audio.volume=GameSettings.SHIP_VOLUME;
//    ejectFx.audio.rolloffMode=AudioRolloffMode.Logarithmic;
//    ejectFx.audio.panLevel=1;
//    ejectFx.audio.maxDistance=100;
//    ejectFx.audio.loop=false;
//    ejectFx.audio.playOnAwake=false;
//
//    if (GameDatabase.Instance.ExistsAudioClip(ejectSoundUrl))
//      ejectFx.audio.clip=GameDatabase.Instance.GetAudioClip(ejectSoundUrl);
//    else
//      Debug.LogError("[ProceduralFairingSide] can't find sound: "+ejectSoundUrl, this);
//
//    if (state!=StartState.Editor) rebuildMesh();
//  }
//
//
//  public void rebuildMesh()
//  {
//    var mf=part.FindModelComponent<MeshFilter>("model");
//    if (!mf) { Debug.LogError("[ProceduralFairingSide] no model in side fairing", part); return; }
//
//    Mesh m=mf.mesh;
//    if (!m) { Debug.LogError("[ProceduralFairingSide] no mesh in side fairing", part); return; }
//
//    mf.transform.localPosition=meshPos;
//    mf.transform.localRotation=meshRot;
//
//    // build fairing shape line
//    float tip=maxRad*noseHeightRatio;
//
//    Vector3[] shape;
//    if (inlineHeight<=0)
//      shape=ProceduralFairingBase.buildFairingShape(
//        baseRad, maxRad, cylStart, cylEnd, noseHeightRatio,
//        baseConeShape, noseConeShape, baseConeSegments, noseConeSegments,
//        vertMapping, mappingScale.y);
//    else
//      shape=ProceduralFairingBase.buildInlineFairingShape(
//        baseRad, maxRad, topRad, cylStart, cylEnd, inlineHeight,
//        baseConeShape, baseConeSegments,
//        vertMapping, mappingScale.y);
//
//    // set up params
//    var dirs=new Vector3[numSegs+1];
//    for (int i=0; i<=numSegs; ++i)
//    {
//      float a=Mathf.PI*2*(i-numSegs*0.5f)/(numSideParts*numSegs);
//      dirs[i]=new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a));
//    }
//
//    float segOMappingScale=(horMapping.y-horMapping.x)/(mappingScale.x*numSegs);
//    float segIMappingScale=(horMapping.w-horMapping.z)/(mappingScale.x*numSegs);
//    float segOMappingOfs=horMapping.x/mappingScale.x;
//    float segIMappingOfs=horMapping.z/mappingScale.x;
//
//    if (numSideParts>2)
//    {
//      segOMappingOfs+=segOMappingScale*numSegs*(0.5f-1f/numSideParts);
//      segOMappingScale*=2f/numSideParts;
//      segIMappingOfs+=segIMappingScale*numSegs*(0.5f-1f/numSideParts);
//      segIMappingScale*=2f/numSideParts;
//    }
//
//    float stripU0=stripMapping.x/mappingScale.x;
//    float stripU1=stripMapping.y/mappingScale.x;
//
//    float ringSegLen=baseRad*Mathf.PI*2/(numSegs*numSideParts);
//    float topRingSegLen=topRad*Mathf.PI*2/(numSegs*numSideParts);
//
//    float collWidth=maxRad*Mathf.PI*2/(numSideParts*3);
//
//    int numMainVerts=(numSegs+1)*(shape.Length-1)+1;
//    int numMainFaces=numSegs*((shape.Length-2)*2+1);
//
//    int numSideVerts=shape.Length*2;
//    int numSideFaces=(shape.Length-1)*2;
//
//    int numRingVerts=(numSegs+1)*2;
//    int numRingFaces=numSegs*2;
//
//    if (inlineHeight>0)
//    {
//      numMainVerts=(numSegs+1)*shape.Length;
//      numMainFaces=numSegs*(shape.Length-1)*2;
//    }
//
//    int totalVerts=numMainVerts*2+numSideVerts*2+numRingVerts;
//    int totalFaces=numMainFaces*2+numSideFaces*2+numRingFaces;
//
//    if (inlineHeight>0)
//    {
//      totalVerts+=numRingVerts;
//      totalFaces+=numRingFaces;
//    }
//
//    var p=shape[shape.Length-1];
//    float topY=p.y, topV=p.z;
//
//    float collCenter=(cylStart+cylEnd)/2, collHeight=cylEnd-cylStart;
//    if (collHeight<=0) collHeight=Mathf.Min(topY-cylEnd, cylStart)/2;
//
//    // compute area
//    double area=0;
//    for (int i=1; i<shape.Length; ++i)
//      area+=(shape[i-1].x+shape[i].x)*(shape[i].y-shape[i-1].y)*Mathf.PI/numSideParts;
//
//    // set params based on volume
//    float volume=(float)(area*sideThickness);
//    part.mass=volume*density;
//    part.breakingForce =part.mass*specificBreakingForce;
//    part.breakingTorque=part.mass*specificBreakingTorque;
//
//    var offset=new Vector3(maxRad*0.7f, topY*0.5f, 0);
//    part.CoMOffset=part.transform.InverseTransformPoint(mf.transform.TransformPoint(offset));
//
//    // remove old colliders
//    foreach (var c in part.FindModelComponents<Collider>())
//      UnityEngine.Object.Destroy(c.gameObject);
//
//    // add new colliders
//    for (int i=-1; i<=1; ++i)
//    {
//      var obj=new GameObject("collider");
//      obj.transform.parent=mf.transform;
//      obj.transform.localPosition=Vector3.zero;
//      obj.transform.localRotation=Quaternion.AngleAxis(90f*i/numSideParts, Vector3.up);
//      var coll=obj.AddComponent<BoxCollider>();
//      coll.center=new Vector3(maxRad+sideThickness*0.5f, collCenter, 0);
//      coll.size=new Vector3(sideThickness, collHeight, collWidth);
//    }
//
//    {
//      // nose collider
//      float r=maxRad*0.2f;
//      var obj=new GameObject("nose_collider");
//      obj.transform.parent=mf.transform;
//      obj.transform.localPosition=new Vector3(r, cylEnd+tip-r*1.2f, 0);
//      obj.transform.localRotation=Quaternion.identity;
//
//      if (inlineHeight>0)
//      {
//        r=sideThickness*0.5f;
//        obj.transform.localPosition=new Vector3(maxRad+r, collCenter, 0);
//      }
//
//      var coll=obj.AddComponent<SphereCollider>();
//      coll.center=Vector3.zero;
//      coll.radius=r;
//    }
//
//    // build mesh
//    m.Clear();
//
//    var verts=new Vector3[totalVerts];
//    var uv=new Vector2[totalVerts];
//    var norm=new Vector3[totalVerts];
//    var tang=new Vector4[totalVerts];
//
//    if (inlineHeight<=0)
//    {
//      // tip vertex
//      verts[numMainVerts  -1].Set(0, topY+sideThickness, 0); // outside
//      verts[numMainVerts*2-1].Set(0, topY, 0); // inside
//      uv[numMainVerts  -1].Set(segOMappingScale*0.5f*numSegs+segOMappingOfs, topV);
//      uv[numMainVerts*2-1].Set(segIMappingScale*0.5f*numSegs+segIMappingOfs, topV);
//      norm[numMainVerts  -1]= Vector3.up;
//      norm[numMainVerts*2-1]=-Vector3.up;
//      // tang[numMainVerts  -1]= Vector3.forward;
//      // tang[numMainVerts*2-1]=-Vector3.forward;
//      tang[numMainVerts  -1]=Vector3.zero;
//      tang[numMainVerts*2-1]=Vector3.zero;
//    }
//
//    // main vertices
//    float noseV0=vertMapping.z/mappingScale.y;
//    float noseV1=vertMapping.w/mappingScale.y;
//    float noseVScale=1f/(noseV1-noseV0);
//    float oCenter=(horMapping.x+horMapping.y)/(mappingScale.x*2);
//    float iCenter=(horMapping.z+horMapping.w)/(mappingScale.x*2);
//
//    int vi=0;
//    for (int i=0; i<shape.Length-(inlineHeight<=0?1:0); ++i)
//    {
//      p=shape[i];
//
//      Vector2 n;
//      if (i==0) n=shape[1]-shape[0];
//      else if (i==shape.Length-1) n=shape[i]-shape[i-1];
//      else n=shape[i+1]-shape[i-1];
//      n.Set(n.y, -n.x);
//      n.Normalize();
//
//      for (int j=0; j<=numSegs; ++j, ++vi)
//      {
//        var d=dirs[j];
//        var dp=d*p.x+Vector3.up*p.y;
//        var dn=d*n.x+Vector3.up*n.y;
//        if (i==0 || i==shape.Length-1) verts[vi]=dp+d*sideThickness;
//        else verts[vi]=dp+dn*sideThickness;
//        verts[vi+numMainVerts]=dp;
//
//        float v=(p.z-noseV0)*noseVScale;
//        float uo=j*segOMappingScale+segOMappingOfs;
//        float ui=(numSegs-j)*segIMappingScale+segIMappingOfs;
//        if (v>0 && v<1)
//        {
//          float us=1-v;
//          uo=(uo-oCenter)*us+oCenter;
//          ui=(ui-iCenter)*us+iCenter;
//        }
//
//        uv[vi].Set(uo, p.z);
//        uv[vi+numMainVerts].Set(ui, p.z);
//
//        norm[vi]=dn;
//        norm[vi+numMainVerts]=-dn;
//        tang[vi].Set(-d.z, 0, d.x, 0);
//        tang[vi+numMainVerts].Set(d.z, 0, -d.x, 0);
//      }
//    }
//
//    // side strip vertices
//    float stripScale=Mathf.Abs(stripMapping.y-stripMapping.x)/(sideThickness*mappingScale.y);
//
//    vi=numMainVerts*2;
//    float o=0;
//    for (int i=0; i<shape.Length; ++i, vi+=2)
//    {
//      int si=i*(numSegs+1);
//
//      var d=dirs[0];
//      verts[vi]=verts[si];
//      uv[vi].Set(stripU0, o);
//      norm[vi].Set(d.z, 0, -d.x);
//
//      verts[vi+1]=verts[si+numMainVerts];
//      uv[vi+1].Set(stripU1, o);
//      norm[vi+1]=norm[vi];
//      tang[vi]=tang[vi+1]=(verts[vi+1]-verts[vi]).normalized;
//
//      if (i+1<shape.Length) o+=((Vector2)shape[i+1]-(Vector2)shape[i]).magnitude*stripScale;
//    }
//
//    vi+=numSideVerts-2;
//    for (int i=shape.Length-1; i>=0; --i, vi-=2)
//    {
//      int si=i*(numSegs+1)+numSegs;
//      if (i==shape.Length-1 && inlineHeight<=0) si=numMainVerts-1;
//
//      var d=dirs[numSegs];
//      verts[vi]=verts[si];
//      uv[vi].Set(stripU0, o);
//      norm[vi].Set(-d.z, 0, d.x);
//
//      verts[vi+1]=verts[si+numMainVerts];
//      uv[vi+1].Set(stripU1, o);
//      norm[vi+1]=norm[vi];
//      tang[vi]=tang[vi+1]=(verts[vi+1]-verts[vi]).normalized;
//
//      if (i>0) o+=((Vector2)shape[i]-(Vector2)shape[i-1]).magnitude*stripScale;
//    }
//
//    // ring vertices
//    vi=numMainVerts*2+numSideVerts*2;
//    o=0;
//    for (int j=numSegs; j>=0; --j, vi+=2, o+=ringSegLen*stripScale)
//    {
//      verts[vi]=verts[j];
//      uv[vi].Set(stripU0, o);
//      norm[vi]=-Vector3.up;
//
//      verts[vi+1]=verts[j+numMainVerts];
//      uv[vi+1].Set(stripU1, o);
//      norm[vi+1]=-Vector3.up;
//      tang[vi]=tang[vi+1]=(verts[vi+1]-verts[vi]).normalized;
//    }
//
//    if (inlineHeight>0)
//    {
//      // top ring vertices
//      o=0;
//      int si=(shape.Length-1)*(numSegs+1);
//      for (int j=0; j<=numSegs; ++j, vi+=2, o+=topRingSegLen*stripScale)
//      {
//        verts[vi]=verts[si+j];
//        uv[vi].Set(stripU0, o);
//        norm[vi]=Vector3.up;
//
//        verts[vi+1]=verts[si+j+numMainVerts];
//        uv[vi+1].Set(stripU1, o);
//        norm[vi+1]=Vector3.up;
//        tang[vi]=tang[vi+1]=(verts[vi+1]-verts[vi]).normalized;
//      }
//    }
//
//    // set vertex data to mesh
//    for (int i=0; i<totalVerts; ++i) tang[i].w=1;
//    m.vertices=verts;
//    m.uv=uv;
//    m.normals=norm;
//    m.tangents=tang;
//
//    m.uv2=null;
//    m.colors32=null;
//
//    var tri=new int[totalFaces*3];
//
//    // main faces
//    vi=0;
//    int ti1=0, ti2=numMainFaces*3;
//    for (int i=0; i<shape.Length-(inlineHeight<=0?2:1); ++i, ++vi)
//    {
//      p=shape[i];
//      for (int j=0; j<numSegs; ++j, ++vi)
//      {
//        tri[ti1++]=vi;
//        tri[ti1++]=vi+1+numSegs+1;
//        tri[ti1++]=vi+1;
//
//        tri[ti1++]=vi;
//        tri[ti1++]=vi  +numSegs+1;
//        tri[ti1++]=vi+1+numSegs+1;
//
//        tri[ti2++]=numMainVerts+vi;
//        tri[ti2++]=numMainVerts+vi+1;
//        tri[ti2++]=numMainVerts+vi+1+numSegs+1;
//
//        tri[ti2++]=numMainVerts+vi;
//        tri[ti2++]=numMainVerts+vi+1+numSegs+1;
//        tri[ti2++]=numMainVerts+vi  +numSegs+1;
//      }
//    }
//
//    if (inlineHeight<=0)
//    {
//      // main tip faces
//      for (int j=0; j<numSegs; ++j, ++vi)
//      {
//        tri[ti1++]=vi;
//        tri[ti1++]=numMainVerts-1;
//        tri[ti1++]=vi+1;
//
//        tri[ti2++]=numMainVerts+vi;
//        tri[ti2++]=numMainVerts+vi+1;
//        tri[ti2++]=numMainVerts+numMainVerts-1;
//      }
//    }
//
//    // side strip faces
//    vi=numMainVerts*2;
//    ti1=numMainFaces*2*3;
//    ti2=ti1+numSideFaces*3;
//    for (int i=0; i<shape.Length-1; ++i, vi+=2)
//    {
//      tri[ti1++]=vi;
//      tri[ti1++]=vi+1;
//      tri[ti1++]=vi+3;
//
//      tri[ti1++]=vi;
//      tri[ti1++]=vi+3;
//      tri[ti1++]=vi+2;
//
//      tri[ti2++]=numSideVerts+vi;
//      tri[ti2++]=numSideVerts+vi+3;
//      tri[ti2++]=numSideVerts+vi+1;
//
//      tri[ti2++]=numSideVerts+vi;
//      tri[ti2++]=numSideVerts+vi+2;
//      tri[ti2++]=numSideVerts+vi+3;
//    }
//
//    // ring faces
//    vi=numMainVerts*2+numSideVerts*2;
//    ti1=(numMainFaces+numSideFaces)*2*3;
//    for (int j=0; j<numSegs; ++j, vi+=2)
//    {
//      tri[ti1++]=vi;
//      tri[ti1++]=vi+1;
//      tri[ti1++]=vi+3;
//
//      tri[ti1++]=vi;
//      tri[ti1++]=vi+3;
//      tri[ti1++]=vi+2;
//    }
//
//    if (inlineHeight>0)
//    {
//      // top ring faces
//      vi+=2;
//      for (int j=0; j<numSegs; ++j, vi+=2)
//      {
//        tri[ti1++]=vi;
//        tri[ti1++]=vi+1;
//        tri[ti1++]=vi+3;
//
//        tri[ti1++]=vi;
//        tri[ti1++]=vi+3;
//        tri[ti1++]=vi+2;
//      }
//    }
//
//    m.triangles=tri;
//
//    if (!HighLogic.LoadedSceneIsEditor) m.Optimize();
//
//    part.SendEvent("FairingShapeChanged");
//  }
//
//
//  [KSPEvent(name = "Jettison", active=true, guiActive=true, guiActiveUnfocused=false, guiName="Jettison")]
//  public void Jettison()
//  {
//    if (part.parent)
//    {
//      foreach (var p in part.parent.children)
//      {
//        var joint=p.GetComponent<ConfigurableJoint>();
//        if (joint!=null && (joint.rigidbody==part.Rigidbody || joint.connectedBody==part.Rigidbody))
//          Destroy(joint);
//      }
//
//      part.decouple(0);
//
//      var tr=part.FindModelTransform("nose_collider");
//      if (tr)
//      {
//        part.Rigidbody.AddForce(tr.right*Mathf.Lerp(ejectionLowDv, ejectionDv, ejectionPower),
//          ForceMode.VelocityChange);
//        part.Rigidbody.AddTorque(-tr.forward*Mathf.Lerp(ejectionLowTorque, ejectionTorque, ejectionPower),
//          ForceMode.VelocityChange);
//      }
//      else
//        Debug.LogError("[ProceduralFairingSide] no 'nose_collider' in side fairing", part);
//
//      ejectFx.audio.Play();
//    }
//  }
//
//
//  public override void OnActive()
//  {
//    Jettison();
//  }
//
//
//  [KSPAction("Jettison", actionGroup=KSPActionGroup.None)]
//  public void ActionJettison(KSPActionParam param)
//  {
//    Jettison();
//  }
//
//
//  float osdMessageTime=0;
//  string osdMessageText=null;
//
//
//  public void OnMouseOver()
//  {
//    if (HighLogic.LoadedSceneIsEditor)
//    {
//      if (Input.GetKeyDown(ejectionPowerKey))
//      {
//        if (ejectionPower<0.25f) ejectionPower=0.5f;
//        else if (ejectionPower<0.75f) ejectionPower=1;
//        else ejectionPower=0;
//
//        foreach (var p in part.symmetryCounterparts)
//          p.GetComponent<ProceduralFairingSide>().ejectionPower=ejectionPower;
//
//        osdMessageTime=Time.time+0.5f;
//        osdMessageText="Fairing ejection force: ";
//
//        if (ejectionPower<0.25f) osdMessageText+="low";
//        else if (ejectionPower<0.75f) osdMessageText+="medium";
//        else osdMessageText+="high";
//      }
//    }
//  }
//
//
//  public void OnGUI()
//  {
//    if (!HighLogic.LoadedSceneIsEditor) return;
//
//    if (Time.time<osdMessageTime)
//    {
//      GUI.skin=HighLogic.Skin;
//      GUIStyle style=new GUIStyle("Label");
//      style.alignment=TextAnchor.MiddleCenter;
//      style.fontSize=20;
//      style.normal.textColor=Color.black;
//      GUI.Label(new Rect(2, 2+(Screen.height/9), Screen.width, 50), osdMessageText, style);
//      style.normal.textColor=Color.yellow;
//      GUI.Label(new Rect(0, Screen.height/9, Screen.width, 50), osdMessageText, style);
//    }
//  }
//}
//
//struct BezierSlope
//{
//  Vector2 p1, p2;
//
//  public BezierSlope(Vector4 v)
//  {
//    p1=new Vector2(v.x, v.y);
//    p2=new Vector2(v.z, v.w);
//  }
//
//  public Vector2 interp(float t)
//  {
//    Vector2 a=Vector2.Lerp(Vector2.zero, p1, t);
//    Vector2 b=Vector2.Lerp(p1, p2, t);
//    Vector2 c=Vector2.Lerp(p2, Vector2.one, t);
//
//    Vector2 d=Vector2.Lerp(a, b, t);
//    Vector2 e=Vector2.Lerp(b, c, t);
//
//    return Vector2.Lerp(d, e, t);
//  }
//}
//
//
//
//struct PayloadScan
//{
//  public List<float> profile;
//  public List<Part> payload;
//  public HashSet<Part> hash;
//
//  public List<Part> targets;
//
//  public Matrix4x4 w2l;
//
//  public float ofs, verticalStep, extraRadius;
//
//
//  public PayloadScan(Part p, float vs, float er)
//  {
//    profile=new List<float>();
//    payload=new List<Part>();
//    targets=new List<Part>();
//    hash=new HashSet<Part>();
//    hash.Add(p);
//    w2l=p.transform.worldToLocalMatrix;
//    ofs=0;
//    verticalStep=vs;
//    extraRadius=er;
//  }
//
//
//  public void addPart(Part p, Part prevPart)
//  {
//    if (p==null || hash.Contains(p)) return;
//    hash.Add(p);
//
//    // check for another fairing base
//    if (p.GetComponent<ProceduralFairingBase>()!=null)
//    {
//      AttachNode node=p.findAttachNode("top");
//      if (node!=null && node.attachedPart==prevPart)
//      {
//        // reversed base - potential inline fairing target
//        targets.Add(p);
//        return;
//      }
//    }
//
//    payload.Add(p);
//  }
//
//
//  public void addPayloadEdge(Vector3 v0, Vector3 v1)
//  {
//    float r0=Mathf.Sqrt(v0.x*v0.x+v0.z*v0.z)+extraRadius;
//    float r1=Mathf.Sqrt(v1.x*v1.x+v1.z*v1.z)+extraRadius;
//
//    float y0=(v0.y-ofs)/verticalStep;
//    float y1=(v1.y-ofs)/verticalStep;
//
//    if (y0>y1)
//    {
//      float tmp;
//      tmp=y0; y0=y1; y1=tmp;
//      tmp=r0; r0=r1; r1=tmp;
//    }
//
//    int h0=Mathf.FloorToInt(y0);
//    int h1=Mathf.FloorToInt(y1);
//    if (h1<0) return;
//    if (h1>=profile.Count) profile.AddRange(Enumerable.Repeat(0f, h1-profile.Count+1));
//
//    if (h0>=0) profile[h0]=Mathf.Max(profile[h0], r0);
//    profile[h1]=Mathf.Max(profile[h1], r1);
//
//    if (h0!=h1)
//    {
//      float k=(r1-r0)/(y1-y0);
//      float b=r0+k*(h0+1-y0);
//      float maxR=Mathf.Max(r0, r1);
//
//      for (int h=Math.Max(h0, 0); h<h1; ++h)
//      {
//        float r=Mathf.Min(k*(h-h0)+b, maxR);
//        profile[h  ]=Mathf.Max(profile[h  ], r);
//        profile[h+1]=Mathf.Max(profile[h+1], r);
//      }
//    }
//  }
//
//
//  public void addPayload(Bounds box, Matrix4x4 boxTm)
//  {
//    Matrix4x4 m=w2l*boxTm;
//
//    Vector3 p0=box.min, p1=box.max;
//    var verts=new Vector3[8];
//    for (int i=0; i<8; ++i)
//      verts[i]=m.MultiplyPoint3x4(new Vector3(
//        (i&1)!=0 ? p1.x : p0.x,
//        (i&2)!=0 ? p1.y : p0.y,
//        (i&4)!=0 ? p1.z : p0.z));
//
//    addPayloadEdge(verts[0], verts[1]);
//    addPayloadEdge(verts[2], verts[3]);
//    addPayloadEdge(verts[4], verts[5]);
//    addPayloadEdge(verts[6], verts[7]);
//
//    addPayloadEdge(verts[0], verts[2]);
//    addPayloadEdge(verts[1], verts[3]);
//    addPayloadEdge(verts[4], verts[6]);
//    addPayloadEdge(verts[5], verts[7]);
//
//    addPayloadEdge(verts[0], verts[4]);
//    addPayloadEdge(verts[1], verts[5]);
//    addPayloadEdge(verts[2], verts[6]);
//    addPayloadEdge(verts[3], verts[7]);
//  }
//
//
//  public void addPayload(Collider c)
//  {
//    var mc=c as MeshCollider;
//    var bc=c as BoxCollider;
//    if (mc)
//    {
//      // addPayload(mc.sharedMesh.bounds,
//      //   c.GetComponent<Transform>().localToWorldMatrix);
//      var m=w2l*mc.transform.localToWorldMatrix;
//      var verts=mc.sharedMesh.vertices;
//      var faces=mc.sharedMesh.triangles;
//      for (int i=0; i<faces.Length; i+=3)
//      {
//        var v0=m.MultiplyPoint3x4(verts[faces[i  ]]);
//        var v1=m.MultiplyPoint3x4(verts[faces[i+1]]);
//        var v2=m.MultiplyPoint3x4(verts[faces[i+2]]);
//        addPayloadEdge(v0, v1);
//        addPayloadEdge(v1, v2);
//        addPayloadEdge(v2, v0);
//      }
//    }
//    else if (bc)
//      addPayload(new Bounds(bc.center, bc.size),
//        bc.transform.localToWorldMatrix);
//    else
//    {
//      // Debug.Log("generic collider "+c);
//      addPayload(c.bounds, Matrix4x4.identity);
//    }
//  }
//}
//
//
////ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ//
//
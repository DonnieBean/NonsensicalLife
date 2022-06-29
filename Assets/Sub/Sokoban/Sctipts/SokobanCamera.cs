using Cinemachine;
using NonsensicalKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SokobanCamera : NonsensicalMono 
{
    protected override void Awake()
    {
        base.Awake();

        Subscribe<Vector3>("setCameraPosition", (p) =>transform.position=p );
        Subscribe<float>("setCameraSize",(f)=>GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize=f );
    }
}

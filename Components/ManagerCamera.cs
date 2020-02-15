using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct ManagerCamera : IComponentData
{
    public float3 PointerRayOrigin;
    public float3 PointerRayDirection;
}

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;

[AlwaysSynchronizeSystem]
public class CameraSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entities
            .WithoutBurst()
            .ForEach((ref ManagerCamera camera, in ManagerTag mainCamera) =>
            {
                var ray = UnityEngine.Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
                camera.PointerRayOrigin = new float3(ray.origin.x, ray.origin.y, ray.origin.z);
                camera.PointerRayDirection = new float3(ray.direction.x, ray.direction.y, ray.direction.z);
            }).Run();

        return default;
    }
}

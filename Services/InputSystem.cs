using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;

[AlwaysSynchronizeSystem]
public class InputSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entities
            .WithoutBurst()
            .ForEach((ref ManagerInput input, in ManagerTag mainCamera) =>
            {
                input.IsPointerDown = UnityEngine.Input.GetMouseButtonDown(0);
                input.IsPointerUp = UnityEngine.Input.GetMouseButtonUp(0);
                input.IsPointerHeldDown = UnityEngine.Input.GetMouseButton(0);
            }).Run();

        return default;
    }
}

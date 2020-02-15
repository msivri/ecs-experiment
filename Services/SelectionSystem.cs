using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;

[UpdateAfter(typeof(CameraSystem))]
[UpdateAfter(typeof(InputSystem))] 
public class SelectionSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var physicsWorldSystem = World.GetExistingSystem<Unity.Physics.Systems.BuildPhysicsWorld>();
        var collisionWorld = physicsWorldSystem.PhysicsWorld.CollisionWorld; 

        var handle = Entities
            .WithAll<ManagerTag>()
            .ForEach((ref ManagerSelection managerSelection, in ManagerCamera managerCamera, in ManagerInput managerInput) =>
            {
                if (managerInput.IsPointerDown)
                {
                    var rayFrom = managerCamera.PointerRayOrigin;
                    var rayTo = managerCamera.PointerRayDirection * 100f;
                    var input = new RaycastInput()
                    {
                        Start = rayFrom,
                        End = rayTo,
                        Filter = new CollisionFilter()
                        {
                            BelongsTo = ~0u,
                            CollidesWith = ~0u, // all 1s, so all layers, collide with everything
                            GroupIndex = 0
                        }
                    };
                    if (collisionWorld.CastRay(input, out var hit))
                    {
                        managerSelection.HasSelection = true;
                        managerSelection.SelectionRigidBodyIndex = hit.RigidBodyIndex;
                    }
                    else
                    {
                        managerSelection.HasSelection = false;
                        managerSelection.SelectionRigidBodyIndex = 0;
                    }
                }
            }).Schedule(inputDeps); 
         
        return handle;
    }
} 
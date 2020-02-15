using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

[UpdateAfter(typeof(SelectionSystem))]
[AlwaysSynchronizeSystem]
public class SelectableComponentSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var physicsWorldSystem = World.GetExistingSystem<Unity.Physics.Systems.BuildPhysicsWorld>();
        var collisionWorld = physicsWorldSystem.PhysicsWorld.CollisionWorld;
        var managerGroup = GetEntityQuery(ComponentType.ReadOnly<ManagerSelection>());
        var managerEntities = managerGroup.ToComponentDataArray<ManagerSelection>(Unity.Collections.Allocator.TempJob);
        var managerSelection = managerEntities[0]; 
        var managerSelectedEntity = managerSelection.HasSelection ? collisionWorld.Bodies[managerSelection.SelectionRigidBodyIndex].Entity : Entity.Null;

        var handle = Entities.ForEach((Entity entity, ref Selectable selectable) =>
        {
            if (!managerSelection.HasSelection || entity != managerSelectedEntity) selectable.IsSelected = false;
            else
                selectable.IsSelected = true;
            
        }).Schedule(inputDeps);


        handle.Complete();

        managerEntities.Dispose();

        return default;
    }
}
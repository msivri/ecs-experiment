using Unity.Entities;
using Unity.Jobs; 

[UpdateAfter(typeof(SelectionSystem))]
[AlwaysSynchronizeSystem]
public class SelectableComponentSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var physicsWorldSystem = World.GetExistingSystem<Unity.Physics.Systems.BuildPhysicsWorld>();
        var collisionWorld = physicsWorldSystem.PhysicsWorld.CollisionWorld;
        var managerQuery = GetEntityQuery(ComponentType.ReadOnly<ManagerSelection>());
        var managerEntities = managerQuery.ToComponentDataArray<ManagerSelection>(Unity.Collections.Allocator.TempJob);
        var managerEntity = managerEntities[0];
        var selectedEntity = managerEntity.HasSelection ? collisionWorld.Bodies[managerEntity.SelectionRigidBodyIndex].Entity : Entity.Null;

        var handle = Entities.ForEach((Entity entity, ref Selectable selectable) =>
        {
            if (!managerEntity.HasSelection || entity != selectedEntity) selectable.IsSelected = false;
            else
                selectable.IsSelected = true;

        }).Schedule(inputDeps);


        handle.Complete();

        managerEntities.Dispose();

        return default;
    }
}

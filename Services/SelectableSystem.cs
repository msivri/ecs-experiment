using Unity.Entities;
using Unity.Jobs; 

[UpdateAfter(typeof(SelectionSystem))]
[AlwaysSynchronizeSystem]
public class SelectableComponentSystem : JobComponentSystem
{
    private EntityQuery managerQuery;

    protected override void OnCreate()
    {
        base.OnCreate();  
        managerQuery = GetEntityQuery(ComponentType.ReadOnly<ManagerSelection>());
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var physicsWorldSystem = World.GetExistingSystem<Unity.Physics.Systems.BuildPhysicsWorld>();
        var collisionWorld = physicsWorldSystem.PhysicsWorld.CollisionWorld;
        var managerEntities = managerQuery.ToComponentDataArray<ManagerSelection>(Unity.Collections.Allocator.TempJob);
        var managerEntity = managerEntities[0];
        var hasSelection = managerEntity.HasSelection;
        var selectedEntity = hasSelection ? collisionWorld.Bodies[managerEntity.SelectionRigidBodyIndex].Entity : Entity.Null;
        managerEntities.Dispose();

        Entities.ForEach((Entity entity, ref Selectable selectable) =>
        {
            selectable.IsSelected = hasSelection && entity == selectedEntity; 
        }).Run(); 

        return default;
    }
}

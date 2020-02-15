using Unity.Entities;

[GenerateAuthoringComponent]
public struct ManagerSelection : IComponentData
{
    public bool HasSelection;
    public int SelectionRigidBodyIndex;
}

using Unity.Entities;

[GenerateAuthoringComponent]
public struct ManagerInput : IComponentData
{
    public bool IsPointerHeldDown;
    public bool IsPointerDown;
    public bool IsPointerUp;
}

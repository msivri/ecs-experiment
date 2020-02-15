using Unity.Entities;

[GenerateAuthoringComponent]
public struct Selectable :  IComponentData
{
    public bool IsSelected;
}

namespace SharedModels
{
    //type of EVENT message
    public interface SpecializationCreated
    {
        int Id { get; set; }
        string SpecializationName { get; set; }
        bool IsActive { get; set; }
    }

    public interface SpecializationUpdated
    {
        int Id { get; set; }
        string SpecializationName { get; set; }
        bool IsActive { get; set; }
    }

    public interface StatusChanged
    {
        int Id { get; set; }
        bool IsActive { get; set; }
    }
}
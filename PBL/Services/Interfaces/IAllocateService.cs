namespace PBL.Services.Interfaces
{
    public interface IAllocateService
    {
        void Allocate(int Id, string userEmail);
        void DeAllocate(int Id);
    }
}
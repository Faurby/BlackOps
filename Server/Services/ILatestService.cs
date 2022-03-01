public interface ILatestService
{
    Task<LatestDTO> GetAsync();
    Task<Status> UpdateAsync(LatestDTO dto);
}

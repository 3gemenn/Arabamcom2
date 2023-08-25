using Arabamcom2.DTOs;

namespace Arabamcom2.IService
{
    public interface IAdvertService 
    {
       Task<Result<AdvertDto>> GetAdvertById(IdDto id);
       Task<ResultList<AdvertCarDto>> GetAllAdvert(int pageSize, int pageNumber);
       Task<Result<AdvertDto>> InsertAdvert(AdvertDto dto);
       Task<bool> DeleteAdvert(IdDto id);
       Task<Result<AdvertDto>> UpdateAdvert(AdvertDto dto);
       Task<Result<LogDto>> CreateLogEntry(string controller, string action, string message);
    }
}

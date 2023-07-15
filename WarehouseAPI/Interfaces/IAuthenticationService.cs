using Microsoft.Extensions.Primitives;
using WarehouseAPI.Models.Dto;
using WarehouseAPI.Models;

namespace WarehouseAPI.Interfaces
{
    public interface IAuthenticationService
    {
        string RegisterUser(RegisterDataDto dto);
        LoggedUserRecordDto GenerateJwtAndGetUser(LoginDto dto);
        User GetLoggedUser(StringValues token);
    }
}

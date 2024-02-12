using Core.Domain;
using Microsoft.VisualBasic;
using WebApi.Models;

namespace WebApi.Mappers
{
    public static class UserWindMapper
    {
        public static UserWind MapFromModel(CreateOrEditUserWindRequest request, UserWind? userWind = null)
        {
            if (userWind == null)
            {
                userWind = new UserWind
                {
                    Id = Guid.NewGuid(),
                };
            }

            userWind.UserName = request.UserName;
            userWind.NormalizedUserName = Strings.UCase(request.UserName);
            userWind.Fio = request.Fio;
            userWind.Email = request.Email;
            userWind.NormalizedEmail = Strings.UCase(request.Email);
            userWind.Age = request.Age;
            userWind.About = request.About;
            userWind.FotoFileName= request.FotoFileName;

            return userWind;
        }
    }
}


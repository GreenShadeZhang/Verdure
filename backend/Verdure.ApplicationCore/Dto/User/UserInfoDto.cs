using System;
using System.Collections.Generic;
using System.Text;

namespace Verdure.ApplicationCore
{
    public class UserInfoDto
    {
        public string UserId { get; set; }
        public string Avatar { get; set; }
        public string NickName { get; set; }
        public string UserName { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public string Age { get; set; }
        public string Province { get; set; }
        public string JwtToken { get; set; }
        public UserInfoDto(ApplicationUser applicationUser)
        {
            if (applicationUser != null)
            {
                //UserId = applicationUser.Id;
                //Avatar = applicationUser.Avatar;
                //NickName = applicationUser.NickName;
                //UserName = applicationUser.UserName;
                //Gender = applicationUser.Gender;
                //City = applicationUser.City;
                //Province = applicationUser.Province;
            }
        }       
    }
}

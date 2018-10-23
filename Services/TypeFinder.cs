using System;
using System.Linq;
using System.Security.Claims;
using SADJZ.Models;

namespace SADJZ.Services
{

    public class TypeFinder
    {
        public static UserType TypeFromClaim(ClaimsPrincipal currentUser)
        {
            Claim typeString = currentUser.Claims.FirstOrDefault(c => c.Type == "Type");
            if (typeString != null){
            string claimValue = typeString.Value;
                if (claimValue != null){
                    return Enum.Parse<UserType>(claimValue);
                }else{
                    return UserType.User;

                }
            }else{
                return UserType.User;
            }
        }
    }


}
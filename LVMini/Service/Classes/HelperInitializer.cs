using LVMini.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace LVMini.Service.Classes
{
    public static class HelperInitializer
    {
        public static MyProfileModel ConstructMyProfileModel(IEnumerable<Claim> userClaims)
        {
            string userName = null;
            string email = null;
            string firstName = null;
            string lastName = null;
            foreach (var claim in userClaims)
            {
                if (claim.Type.Equals("given_name"))
                {
                    firstName = claim.Value;
                }
                if (claim.Type.Equals("family_name"))
                {
                    lastName = claim.Value;
                }
                if (claim.Type.Equals("email"))
                {
                    email = claim.Value;
                }
                if (claim.Type.Equals("username"))
                {
                    userName = claim.Value;
                }
            }

            MyProfileModel model = new MyProfileModel(userName, email, firstName, lastName);
            return model;
        }
    }
}

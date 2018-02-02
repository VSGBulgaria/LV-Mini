using System;
using System.Linq;
using Data.Service.Core.Entities;
using LVMiniAdminApi.Attributes;
using LVMiniAdminApi.Contracts;
using LVMiniAdminApi.Models;

namespace LVMiniAdminApi.Services
{
    public class ModifiedUserHandler : IModifiedUserHandler
    {
        public bool CheckTheChanges(User storedUser, BaseModifiedUserModel modifiedUserModel)
        {
            var modifiedUserChangeableProperties = modifiedUserModel.GetType().GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(Changeable)));
            var storedUserProperties = storedUser.GetType().GetProperties();
            var changed = true;
            foreach (var storedUserProperty in storedUserProperties)
            {
                foreach (var modifiedUserChangeableProperty in modifiedUserChangeableProperties)
                {
                    var userPropertyName = storedUserProperty.Name;
                    var modifiedPropertyName = modifiedUserChangeableProperty.Name;
                    if (String.Equals(userPropertyName
                        , modifiedPropertyName
                        , StringComparison.OrdinalIgnoreCase))
                    {
                        if(!storedUserProperty.GetValue(storedUser).Equals(modifiedUserChangeableProperty.GetValue(modifiedUserModel)))
                        {
                            changed = false;
                        }
                    }
                }
            }
            return changed;
        }

        public User SetChangesToStoredUser(User storedUser, BaseModifiedUserModel modifiedModel)
        {
            var modifiedUserChangeableProperties = modifiedModel.GetType().GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(Changeable)));
            var storedUserProperties = storedUser.GetType().GetProperties();
            foreach (var userProperty in storedUserProperties)
            {
                foreach (var modifiedUserChangeableProperty in modifiedUserChangeableProperties)
                {
                    var userPropertyName = userProperty.Name;
                    var modifiedPropertyName = modifiedUserChangeableProperty.Name;
                    if (String.Equals(userPropertyName
                        , modifiedPropertyName
                        , StringComparison.OrdinalIgnoreCase))
                    {
                        userProperty.SetValue(storedUser, modifiedUserChangeableProperty.GetValue(modifiedModel));
                    }
                }
            }
            return storedUser;
        }
    }
}

using System;
using System.Collections.Generic;
using LVMiniAdminApi.Models.UserModels;

namespace LVMiniAdminApi.Models.TeamModels
{
    public abstract class BaseTeamDto
    {
        private string _teamName;

        public string TeamName
        {
            get => this._teamName;
            set => _teamName = value ?? throw new ArgumentException("TeamDto name cannot be null.");
        }
        public bool IsActive { get; set; }

        public ICollection<UserDto> Users { get; set; }
    }
}

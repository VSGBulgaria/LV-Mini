using System;

namespace LVMiniAdminApi.Models.TeamModels
{
    public abstract class BaseTeam
    {
        private string _teamName;

        public string TeamName
        {
            get => this._teamName;
            set => _teamName = value ?? throw new ArgumentException("TeamDto name cannot be null.");
        }
        public bool IsActive { get; set; }
    }
}

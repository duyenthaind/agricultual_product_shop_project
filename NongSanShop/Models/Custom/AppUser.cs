using System;

namespace NongSanShop.Models.Custom
{
    public class AppUser
    {
        private readonly int _userId;
        private readonly string _username;
        private readonly AppRole _appRole;

        internal AppUser(int userId, string username, AppRole appRole)
        {
            this._userId = userId;
            this._username = username;
            this._appRole = appRole ?? throw new Exception("Role is not support or malformed role id");
        }

        public int UserId => _userId;

        public string Username => _username;

        public AppRole AppRole => _appRole;
    }
}
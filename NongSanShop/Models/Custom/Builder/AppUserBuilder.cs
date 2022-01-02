namespace NongSanShop.Models.Custom.Builder
{
    public class AppUserBuilder : ModelBuilder<AppUser>
    {
        private int _userId;
        private string _username;
        private AppRole _appRole;

        public override AppUser build()
        {
            return new AppUser(_userId, _username, _appRole);
        }

        public AppUserBuilder withUserId(int userId)
        {
            this._userId = userId;
            return this;
        }

        public AppUserBuilder withUserName(string username)
        {
            this._username = username;
            return this;
        }

        public AppUserBuilder withAppRole(int roleId)
        {
            _appRole = AppRole.getRoleFromRoleId(roleId);
            return this;
        }
    }
}
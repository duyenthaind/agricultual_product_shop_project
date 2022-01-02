using NongSanShop.Models.Custom.Enums;

namespace NongSanShop.Models.Custom
{
    public class AppRole
    {
        private static readonly string ROLE_USER = "user";
        private static readonly string ROLE_ADMIN = "admin";

        private readonly string _roleName;
        private readonly int _roleId;
        private readonly bool _isAdmin;

        public string RoleName => _roleName;

        public int RoleId => _roleId;

        public bool IsAdmin => _isAdmin;

        private AppRole(string roleName, int roleId, bool isAdmin)
        {
            _roleId = roleId;
            _roleName = roleName;
            _isAdmin = isAdmin;
        }

        public static AppRole getRoleFromRoleId(int roleId)
        {
            switch (roleId)
            {
                case (int) ERole.USER:
                    return new AppRole(ROLE_USER, roleId, false);
                case (int) ERole.AMDIN:
                    return new AppRole(ROLE_ADMIN, roleId, true);
                default:
                    return null;
            }
        }
    }
}
﻿namespace back_end.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<RolePermission> Roles { get; set; }
    }
}

﻿namespace E_commerce.Core.Entities
{
    public class UserRole : BaseEntity
    {
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public Guid RoleId { get; set; }
        public Role? Role { get; set; }
    }
}

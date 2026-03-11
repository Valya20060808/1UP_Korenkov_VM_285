using System.Collections.Generic;
using UP_Korenkov_VM_285.Models;

namespace UP_Korenkov_VM_285.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public int RoleId { get; set; }
        public Role? Role { get; set; }

        public ICollection<Request>? AssignedRequests { get; set; }
        public ICollection<Request>? ClientRequests { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}
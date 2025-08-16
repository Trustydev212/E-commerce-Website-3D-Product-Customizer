using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App_Code
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public bool? EmailVerified { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string AvatarPath
        {
            get { return Avatar; }
            set { Avatar = value; }
        }
        public DateTime? CreatedDate
        {
            get { return CreatedAt; }
            set { CreatedAt = value; }
        }
        public string Password { get; set; }
    }
} 
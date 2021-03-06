﻿using System.Collections.Generic;

namespace FacesStorage.Data.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string Nickname { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }
        public int RoleId { get; set; }

        public IList<Request> Requests { get; set; }
    }
}
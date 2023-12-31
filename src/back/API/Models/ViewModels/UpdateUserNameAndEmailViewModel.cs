﻿using System.ComponentModel.DataAnnotations;

namespace API.Models.ViewModels
{
    public class UpdateUserNameAndEmailViewModel
    {
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}

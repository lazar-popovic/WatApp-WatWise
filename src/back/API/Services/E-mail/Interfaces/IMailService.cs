﻿using API.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace API.Services.E_mail.Interfaces
{
    public interface IMailService
    {
        public void sendTokenEmployee(User user);
        public void sendTokenProsumer(User user);
       
    }
}

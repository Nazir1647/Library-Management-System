﻿using LMS.Models.Models;
using LMS.Tables.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Services.IServices
{
    public interface IAuthService
    {
        Task<dynamic> Login(UserCred user);
        Task<dynamic> Registration(UserModel user);
    }
}
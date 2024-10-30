﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccessObject;
using Repository.IRepository;
using Repository.Repository;
using Microsoft.AspNetCore.Authorization;

namespace KoiFarmRazorPage.Pages.Admin
{
    public class ViewAllUser : PageModel
    {
        private readonly IUserRepository userRepository;

        public ViewAllUser()
        {
            userRepository = new UserRepository();
        }

        public IList<User> User { get;set; } = new List<User>()!;

        public async Task OnGetAsync()
        {
            User = userRepository.GetUsers();
    }
}
}
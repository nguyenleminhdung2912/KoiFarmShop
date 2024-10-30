﻿using BusinessObject;
using DataAccessObject;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class KoiFishRepository : IKoiFishRepository
    {
        public List<KoiFish> GetKoiFishsByListString(string listString)
        => KoiFishDAO.GetKoiFishsByListString(listString);
    }
}
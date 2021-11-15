﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Salvo.Models;

namespace Salvo.Repositories
{
    public interface IPlayerRepository
    {
        public Player FindByEmail(string email);
    }
}

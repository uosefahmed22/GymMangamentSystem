﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.Models.Identity
{
    public class RevokeTokenRequest
    {
        public string RefreshToken { get; set; }
    }
}

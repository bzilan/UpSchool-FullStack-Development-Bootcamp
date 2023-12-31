﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpSchool.Console.Common;

namespace UpSchool.Console.Domain
{
    public class AppUser:IdentityUser<string>,IEntityBase,ICreatedByEntity
    {
        public string Id { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}

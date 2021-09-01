﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data.Entity.MasterData
{
    [Table("Color", Schema = "MasterData")]
    public class Color : Base
    {
        public string name { get; set; }
    }
}

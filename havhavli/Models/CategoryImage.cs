﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace havhavli.Models
{
    public class CategoryImage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public category category { get; set; }
    }
}

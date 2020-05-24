﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Models
{
    public class GenreVM
    {
        public int GenreId { get; set; }

        [DataType(DataType.Text)]
        public string Name { get; set; }
    }
}
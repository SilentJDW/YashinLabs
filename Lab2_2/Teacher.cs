﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2_2
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Course> Courses { get; set; } = new ();
    }
}

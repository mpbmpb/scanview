﻿using System;
namespace scanview.Models
{
    public class CheckedId
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsSelected { get; set; }
    }
}

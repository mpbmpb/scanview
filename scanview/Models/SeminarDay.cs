﻿using System;
namespace scanview.Models
{
    public class SeminarDay
    {
        public int SeminarId { get; set; }
        public virtual Seminar Seminar { get; set; }
        public int DayId { get; set; }
        public virtual Day Day { get; set; }
    }
}

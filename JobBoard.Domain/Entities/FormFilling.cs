﻿using JobBoard.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities
{
    public class FormFilling : DomainEntity 
    {
        public int JobApplicationId { get; set; }
        public JobApplication JobApplication { get; set; }
        public DateTimeOffset FillingDate { get; set; }

        public  ICollection<FieldFilling> FieldFillings { get; set; }  = new List<FieldFilling>();
    }
}

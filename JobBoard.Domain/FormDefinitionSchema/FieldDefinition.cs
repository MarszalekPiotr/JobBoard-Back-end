﻿using JobBoard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.FormDefinitionSchema
{
    public abstract class FieldDefinition
    {
        public string Name { get; set; } = string.Empty;
        public bool IsRequired { get; set; }
        public abstract  EnumFieldType EnumFieldType { get;  }
    }
}

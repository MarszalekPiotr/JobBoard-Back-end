using JobBoard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.FormDefinitionSchema
{
    public class PercentageFieldDefinition : FieldDefinition
    {   
        public PercentageFieldDefinition()
        {
           
        }
        public int MinValue { get; set; } = 0;
        public int MaxValue { get; set; } = 100;

        public override EnumFieldType EnumFieldType => EnumFieldType.Percentage;
    }
}

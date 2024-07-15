using JobBoard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.FormDefinitionSchema
{
    public class IntFieldDefinition : FieldDefinition
    {
        public IntFieldDefinition()
        {

        }
        // validation
        public int MinValue { get; set; } = 0;

        public int MaxValue { get; set; } = int.MaxValue;

        public override EnumFieldType EnumFieldType => EnumFieldType.Int;
    }
}

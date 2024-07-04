using JobBoard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.FormDefinitionSchema
{
    public class IntFieldDefinition : BaseFieldDefinition
    {
        public IntFieldDefinition()
        {

        }
        // validation
        public int MinValue = 0;
        public int MaxValue = int.MaxValue;

        public override EnumFieldType EnumFieldType => EnumFieldType.Int;
    }
}

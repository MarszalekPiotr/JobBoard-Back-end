using JobBoard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.FormDefinitionSchema
{
    public class StringFieldDefinition : BaseFieldDefinition
    {   
        public StringFieldDefinition()
        {
            
        }
        public int MaxLengthValue { get; set; } = 1000;

        public override EnumFieldType EnumFieldType => EnumFieldType.String;
    }
}

using JobBoard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.FormDefinitionSchema
{
    public class BoolFieldDefinition : FieldDefinition
    {
        public BoolFieldDefinition() 
        {
            
        }

        public override EnumFieldType EnumFieldType  => EnumFieldType.Bool;
    }
}

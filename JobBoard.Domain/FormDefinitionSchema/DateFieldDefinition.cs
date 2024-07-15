using JobBoard.Domain.Enums;
using JobBoard.Domain.FormDefinitionSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.FormDefinitionSchema
{
    public class DateFieldDefinition : FieldDefinition
    {  
        public DateFieldDefinition() 
        {
            
        }
        public DateTimeOffset MinDateValue { get; set; } 
        public DateTimeOffset MaxDateValue { get; set; }

        public override EnumFieldType EnumFieldType => EnumFieldType.Date;
    }
}

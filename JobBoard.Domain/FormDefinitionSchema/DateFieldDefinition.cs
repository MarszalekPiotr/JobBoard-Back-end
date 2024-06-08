using JobBoard.Domain.FormDefinitionSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.FormDefinitionSchema
{
    public class DateFieldDefinition : BaseFieldDefinition
    {  
        public DateFieldDefinition() 
        {
            this.EnumFieldType = Enums.EnumFieldType.Date;
        }
        public DateTimeOffset MinDateValue { get; set; } 
        public DateTimeOffset MaxDateValue { get; set; }
    }
}

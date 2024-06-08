using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.FormDefinitionSchema
{
    public class BoolFieldDefinition : BaseFieldDefinition
    {
        public BoolFieldDefinition() 
        {
            this.EnumFieldType = Enums.EnumFieldType.Bool;
        }
    }
}

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
            this.EnumFieldType = Enums.EnumFieldType.String;
        }
        public int MaxLengthValue = 1000;
    }
}

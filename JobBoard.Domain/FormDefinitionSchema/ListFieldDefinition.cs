using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.FormDefinitionSchema
{
    public class ListFieldDefinition : BaseFieldDefinition
    {   
        public ListFieldDefinition()
        {
            this.EnumFieldType = Enums.EnumFieldType.List;
        }
        public List<string> Values { get; set; } = new List<string>();
        public bool IsMultipleChoice { get; set; } = false;
    }
}

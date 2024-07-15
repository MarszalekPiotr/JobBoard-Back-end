using JobBoard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.FormDefinitionSchema
{
    public class ListFieldDefinition : FieldDefinition
    {
        public ListFieldDefinition()
        {

        }
        public List<string> Values { get; set; } = new List<string>();
        public bool IsMultipleChoice { get; set; } = false;

        public override EnumFieldType EnumFieldType => EnumFieldType.List;
    }
}

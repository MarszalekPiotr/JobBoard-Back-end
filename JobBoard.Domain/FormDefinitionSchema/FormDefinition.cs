using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.FormDefinitionSchema
{
    public class FormDefinition
    {
        public List<FieldDefinition> FieldDefinitions { get; set; } = new List<FieldDefinition>();
    }
}

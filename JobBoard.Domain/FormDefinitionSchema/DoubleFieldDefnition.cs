using JobBoard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.FormDefinitionSchema
{
    public class DoubleFieldDefnition : BaseFieldDefinition
    {   
        public DoubleFieldDefnition()
        {
            this.EnumFieldType = EnumFieldType.Double;
        }
        public double MinValue = 0;
        public double MaxValue = double.MaxValue;
    }
}

using FluentValidation;
using JobBoard.Application.Exceptions;
using JobBoard.Domain.FormDefinitionSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JobBoard.Application.Validators
{
    public static class CreateFormValidator
    {
        public static IRuleBuilderOptions<T, FormDefinition> FormDefinitionCorrect<T>(
            this IRuleBuilder<T, FormDefinition> ruleBuilder)
        {
            return  ruleBuilder.ChildRules(x => x.RuleForEach(x => x.FieldDefinitions).ChildRules(x => x.RuleFor(x => x.Name).NotEmpty()));
           // return ruleBuilder.Must(ValidateForm);
                
        }


        private static bool ValidateForm(FormDefinition formDefinition) 
        {
            if (!formDefinition.FieldDefinitions.All(field => field.Name.Length > 3)){
                return false;
              //  throw new ErrorException("Field Name must be at lest 4 characters length in field... nazwa fielda");
            }
            /// czy bool moze przyjsc jako null? czy jak nedzie pusty w w jsonie to sie nie bedzie dalo go przeparsowac?

            return true;
          
        }
    }
}

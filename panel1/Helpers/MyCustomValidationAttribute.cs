using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace panel1.Helpers
{
    public class MyCustomValidationAttribute: ValidationAttribute
    {
        //public MyCustomValidationAttribute(string text)
        //{
        //    Text = text;
        //}

        

        public string Text { get; set; }
        protected override ValidationResult? IsValid(object? value,ValidationContext validationContext)
        {
            if (value != null)
            {
                string name = value.ToString();
                if(name.Contains(Text))
                {
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult(ErrorMessage ?? "Custom Error MEssage");
        }
       
    }
}

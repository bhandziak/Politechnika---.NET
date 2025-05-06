using System.ComponentModel.DataAnnotations;

namespace EventRegisterProject.Attributes
{
    public class FutureDate : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is not DateTime date)
            {
                return false;
            }

            return date > DateTime.Now;
        }
    }
}

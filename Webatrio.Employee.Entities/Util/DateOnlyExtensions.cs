namespace Webatrio.Employee.Entities.Util
{
    public static class DateOnlyExtensions
    {
        public static int GetAge(this DateOnly birthdate) 
        {
            //todo inject the now to test better
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            // Calculate the age.
            var age = today.Year - birthdate.Year;

            // Go back to the year in which the person was born in case of a leap year
            if (birthdate > today.AddYears(-age)) 
                age--;

            return age;
        }
    }
}

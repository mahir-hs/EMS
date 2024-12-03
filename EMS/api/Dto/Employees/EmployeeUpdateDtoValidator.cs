namespace api.Dto.Employees
{
    public static class EmployeeUpdateDtoValidator
    {
        public static List<string> Validate(EmployeeUpdateDto entity)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(entity.FirstName))
                errors.Add("First Name is required.");
            if (string.IsNullOrWhiteSpace(entity.LastName))
                errors.Add("Last Name is required.");
            if (string.IsNullOrWhiteSpace(entity.Email) || !IsValidEmail(entity.Email))
                errors.Add("Valid Email is required.");
            if (string.IsNullOrWhiteSpace(entity.Phone) || !entity.Phone.All(char.IsDigit))
                errors.Add("Valid Phone number is required.");
            if (string.IsNullOrWhiteSpace(entity.Address))
                errors.Add("Address is required.");
            if (entity.DateOfBirth == default || entity.DateOfBirth > DateTime.UtcNow)
                errors.Add("Valid Date of Birth is required.");
            if (entity.DepartmentId.HasValue && entity.DepartmentId <= 0)
                errors.Add("Department ID must be greater than 0 if provided.");
            if (entity.DesignationId.HasValue && entity.DesignationId <= 0)
                errors.Add("Designation ID must be greater than 0 if provided.");

            return errors;
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}

namespace ECommerce.Shared.CommonResponses
{
    public class Error
    {
        public string Code { get; set; }
        public string Descreption { get; set; }
        public ErrorType ErrorType { get; set; }

        private Error(string code, string descreption, ErrorType errorType)
        {
            Code = code;
            Descreption = descreption;
            ErrorType = errorType;
        }
        #region Factory Methods 
        public static Error Faliure(
        string code = "Genaral.Faliure",
        string description = "A general Faliure Has Occured"
    )
        {
            return new Error(code, description, ErrorType.Failure);
        }

        public static Error Validation(
            string code = "Genaral.Validation",
            string description = "Validation Error Has Occured"
        )
        {
            return new Error(code, description, ErrorType.Validation);
        }

        public static Error NotFound(
            string code = "Genaral.NotFound",
            string description = "The Requested Resource was nit found"
        )
        {
            return new Error(code, description, ErrorType.NotFound);
        }

        public static Error UnAuthorized(
            string code = "Genaral.UnAuthorized",
            string description = "You are Not Authorized to perform this action"
        )
        {
            return new Error(code, description, ErrorType.UnAuthorized);
        }

        public static Error Forbidden(
            string code = "Genaral.Forbidden",
            string description = "You don't have the access to this resource,Access denied"
        )
        {
            return new Error(code, description, ErrorType.ForBidden);
        }

        public static Error InvalidCredintals(
            string code = "Genaral.InvalidCredintals",
            string description = "Your Credintals is not valid to reach this resource"
        )
        {
            return new Error(code, description, ErrorType.InvalidCredentials);
        }
        #endregion
    }
}
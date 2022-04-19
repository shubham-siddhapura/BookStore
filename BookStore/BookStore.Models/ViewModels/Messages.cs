using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Models.ViewModels
{
    public class Messages
    {
        public static string GeneralExceptionCode = "general_exception_message";
        public static string GeneralExceptionMessage = "The server encountered an error and could not complete your request.";

        public static string UserNotFoundCode = "not_found";
        public static string UserNotFoundMessage = "User Not Found";

        public static string InvalidCredentialsCode = "invalid_credentials";
        public static string InvalidCredentialsMessage = "Invalid username or password.";

    }
}

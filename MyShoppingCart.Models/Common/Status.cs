using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Models.Common
{
    public static class Status
    {
        #region Response Status

        public static readonly string OK = "ok";
        public static readonly string FAIL = "fail";
        public static readonly string ERROR = "error";
        public static readonly string WARNING = "warning";
        public static readonly string INFO = "info";

        #endregion
    }

    public static class AlertMessages
    {
        #region Regular Expressions
        public const string SaveSuccessful = "Data saved successfully";
        public const string Success = "Success";
        public const string Fail = "Fail";
        public const string UpdateSuccessful = "Data updated successfully";
        public const string DeleteSuccessful = "Data deleted successfully";
        public const string NotFound = "Data not found";
        public const string AlreadyExist = "{0} already exist";
        public const string Error = "An error occurred. Please try again";
        public const string LoginFailed = "Invalid login attempt. Please confirm your username and password and try again";
        public const string AccountUnapproved = "Your account is not approved yet. Please contact to admin.";
        public const string AccountBlocked = "Your account has been blocked. Please contact support.";
        public const string BookReturned = "Book returned successfully.";
  // Other messages...
        #endregion
    }
}

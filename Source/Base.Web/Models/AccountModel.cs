using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Base.Web.Models
{
    public class AccountModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool ValidacionAD { get; set; }
    }
}
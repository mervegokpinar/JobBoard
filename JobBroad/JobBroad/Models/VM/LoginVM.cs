using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobBroad.Models.VM
{
    public class LoginVM
    {
     
        [
            Required(ErrorMessage = "Enter Password."),
            DisplayName("Password")
        ]
        public string Password { get; set; }

        [
         Required(ErrorMessage = "Enter User Name"),
         DisplayName("User Name")
     ]
        public string Name { get; set; }
        public int MyProperty { get; set; }


    }
}
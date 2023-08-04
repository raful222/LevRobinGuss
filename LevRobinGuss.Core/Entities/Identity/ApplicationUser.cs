using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LevRobinGuss.Core.Entities.Identity
{
    public class ApplicationUser : IdentityUser<int>,IAuditable
    {
        public ApplicationUser()
        {
            Logins = new HashSet<ApplicationUserLogin>();
            UserRoles = new HashSet<ApplicationUserRole>();
            Tokens = new HashSet<ApplicationUserToken>();
            Claims = new HashSet<ApplicationUserClaim>();
        }

        [Display(Name = "דוא\"ל")]
        [Remote("IsAlreadySigned", "account", AdditionalFields = nameof(Id), ErrorMessage = "דוא\"ל זה כבר קיים", HttpMethod = "POST")]
        public override string Email { get; set; }

        [Display(Name = "שם פרטי")]
        public string FirstName { get; set; }

        [Display(Name = "שם משפחה")]
        public string? LastName { get; set; }

        [Display(Name = "טלפון")]
        public override string? PhoneNumber { get; set; }

        public DateTime CreatedAt { get; set; }

        [Display(Name = "תאריך עדכון")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        public DateTime UpdatedAt { get; set; }

        public string MetaMaskKey { get; set; }

        public double AmountTokenBuy { get; set; }

        public virtual ICollection<ApplicationUserClaim> Claims { get; set; }

        public virtual ICollection<ApplicationUserLogin> Logins { get; set; }

        public virtual ICollection<ApplicationUserToken> Tokens { get; set; }

        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}

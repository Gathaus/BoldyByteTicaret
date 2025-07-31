using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ECommerceApp.Domain.Entities
{
    public class User : IdentityUser
    {
        [MaxLength(100)]
        public string FirstName { get; set; } = "";
        
        [MaxLength(100)]
        public string LastName { get; set; } = "";
        
        public DateTime? DateOfBirth { get; set; }
        
        public Gender? Gender { get; set; }
        
        [MaxLength(500)]
        public string ProfileImageUrl { get; set; } = "";
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        
        // Computed Properties
        public string FullName => $"{FirstName} {LastName}".Trim();
        
        public int? Age 
        {
            get
            {
                if (!DateOfBirth.HasValue) return null;
                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Value.Year;
                if (DateOfBirth.Value.Date > today.AddYears(-age)) age--;
                return age;
            }
        }
    }

    public enum Gender
    {
        Male = 1,
        Female = 2,
        Other = 3,
        PreferNotToSay = 4
    }
} 
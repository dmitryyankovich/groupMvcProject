﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class WishList: Identity
    {
        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be maximum {1} characters long.")]
        public string Name { get; set; }

        [Required]
        public string Link { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public ICollection<Gift> Gifts { get; set; }
        public ICollection<View> Views { get; set; }
        public ICollection<Vote> Votes { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}

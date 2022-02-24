﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Models
{
	public class OrderHeader
	{
		[Key]
		public int Id { get; set; }

		public int UserId { get; set; }

		[ForeignKey("UserId")]
        public virtual ApplicationUser ApplcationUser { get; set; }

		[Required]
		public DateTime OrderDate { get; set; }

		[Required]
		[Display(Name = "Order Total")]
		[DisplayFormat(DataFormatString = "{0:C}")]
		public double OrderTotal { get; set; }

		[Required]
		[Display(Name = "Delivery Time")]
		public DateTime DeliveryTime { get; set; }

		[Required]
		public DateTime DeliveryDate { get; set; }

		public string Status { get; set; }

		[Display(Name = "Delivery Name")]
		public string DeliveryName { get; set; }

		[Display(Name = "Phone Number")]
		public string PhoneNumber { get; set; }

        public string TransactionsId { get; set; }
	}			
}




﻿using System;
using ApplicationCore.Models;

namespace FoodDelivery.ViewModels
{
	public class OrderDetailsViewModel
	{
		public OrderHeader OrderHeader { get; set; }
		public List<OrderDetails> OrderDetails { get; set; }		
	}
}


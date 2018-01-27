﻿using SpaProject.Data.Items;
using SpaProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace SpaProject.Data
{
	public class SpaMappingProfile : Profile
	{

		public SpaMappingProfile()
		{
			CreateMap<Order, OrderViewModel>()
				.ForMember(o => o.OrderId, ex => ex.MapFrom(o => o.Id))
				.ReverseMap();

			CreateMap<OrderItem, OrderItemViewModel>()
				.ReverseMap();
		}

	}
}

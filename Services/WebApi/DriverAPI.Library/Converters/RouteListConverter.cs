﻿using DriverAPI.Library.DTOs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Vodovoz.Domain.Logistic;

namespace DriverAPI.Library.Converters
{
	public class RouteListConverter
	{
		private readonly ILogger<RouteListConverter> _logger;
		private readonly DeliveryPointConverter _deliveryPointConverter;
		private readonly RouteListStatusConverter _routeListStatusConverter;
		private readonly RouteListAddressStatusConverter _routeListAddressStatusConverter;
		private readonly RouteListCompletionStatusConverter _routeListCompletionStatusConverter;

		public RouteListConverter(ILogger<RouteListConverter> logger,
			DeliveryPointConverter deliveryPointConverter,
			RouteListStatusConverter routeListStatusConverter,
			RouteListAddressStatusConverter routeListAddressStatusConverter,
			RouteListCompletionStatusConverter routeListCompletionStatusConverter)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_deliveryPointConverter = deliveryPointConverter ?? throw new ArgumentNullException(nameof(deliveryPointConverter));
			_routeListStatusConverter = routeListStatusConverter ?? throw new ArgumentNullException(nameof(routeListStatusConverter));
			_routeListAddressStatusConverter = routeListAddressStatusConverter ?? throw new ArgumentNullException(nameof(routeListAddressStatusConverter));
			_routeListCompletionStatusConverter = routeListCompletionStatusConverter ?? throw new ArgumentNullException(nameof(routeListCompletionStatusConverter));
		}

		public RouteListDto convertToAPIRouteList(RouteList routeList, IEnumerable<KeyValuePair<string, int>> itemsToReturn)
		{
			var result = new RouteListDto()
			{
				ForwarderFullName = routeList.Forwarder?.FullName ?? "Нет",
				CompletionStatus = _routeListCompletionStatusConverter.convertToAPIRouteListCompletionStatus(routeList.Status)
			};

			if(result.CompletionStatus == RouteListDtoCompletionStatus.Completed)
			{
				result.CompletedRouteList = new CompletedRouteListDto()
				{
					RouteListId = routeList.Id,
					RouteListStatus = _routeListStatusConverter.convertToAPIRouteListStatus(routeList.Status),
					CashMoney = routeList.Addresses
						.Where(rla => rla.Status == RouteListItemStatus.Completed
							&& rla.Order.PaymentType == Vodovoz.Domain.Client.PaymentType.cash)
						.Sum(rla => rla.Order.ActualTotalSum),
					TerminalMoney = routeList.Addresses
						.Where(rla => rla.Status == RouteListItemStatus.Completed
							&& rla.Order.PaymentType == Vodovoz.Domain.Client.PaymentType.Terminal)
						.Sum(rla => rla.Order.ActualTotalSum),
					TerminalOrdersCount = routeList.Addresses
						.Where(rla => rla.Status == RouteListItemStatus.Completed
							&& rla.Order.PaymentType == Vodovoz.Domain.Client.PaymentType.Terminal)
						.Count(),
					FullBottlesToReturn = routeList.Addresses
						.Where(rla => rla.Status == RouteListItemStatus.Canceled
							|| rla.Status == RouteListItemStatus.Overdue)
						.Sum(rla => rla.Order.Total19LBottlesToDeliver),
					EmptyBottlesToReturn = routeList.Addresses
						.Sum(rla => rla.DriverBottlesReturned ?? 0),
				};

				result.CompletedRouteList.OrdersReturnItems = itemsToReturn.Select(pair => new OrdersReturnItemDto() { Name = pair.Key, Count = pair.Value });
			}
			else
			{
				if (result.CompletionStatus == RouteListDtoCompletionStatus.Incompleted)
				{
					var routelistAddresses = new List<RouteListAddressDto>();

					foreach (var address in routeList.Addresses.OrderBy(address => address.IndexInRoute))
					{
						routelistAddresses.Add(convertToAPIRouteListAddress(address));
					}
					
					result.IncompletedRouteList = new IncompletedRouteListDto()
					{
						RouteListId = routeList.Id,
						RouteListStatus = _routeListStatusConverter.convertToAPIRouteListStatus(routeList.Status),
						RouteListAddresses = routelistAddresses
					};
				}
			}

			return result;
		}

		private RouteListAddressDto convertToAPIRouteListAddress(RouteListItem routeListAddress)
		{
			return new RouteListAddressDto()
			{
				Id = routeListAddress.Id,
				Status = _routeListAddressStatusConverter.convertToAPIRouteListAddressStatus(routeListAddress.Status),
				DeliveryIntervalStart = routeListAddress.Order.DeliveryDate + routeListAddress.Order.DeliverySchedule.From ?? DateTime.MinValue,
				DeliveryIntervalEnd = routeListAddress.Order.DeliveryDate + routeListAddress.Order.DeliverySchedule.To ?? DateTime.MinValue,
				OrderId = routeListAddress.Order.Id,
				Address = _deliveryPointConverter.ExtractAPIAddressFromDeliveryPoint(routeListAddress.Order.DeliveryPoint)
			};
		}
	}
}

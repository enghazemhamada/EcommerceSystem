using EcommerceSystem.DTOs;
using EcommerceSystem.Models;
using EcommerceSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceSystem.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class OrderController : ControllerBase
	{
		private readonly IOrderRepository orderRepository;

		public OrderController(IOrderRepository orderRepository)
        {
			this.orderRepository = orderRepository;
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public ActionResult<GeneralResponse> GetAllOrders()
		{
			List<Order> orders = orderRepository.GetAll();
			List<ShowOrderDto> ordersDto = new List<ShowOrderDto>();

			foreach(Order order in orders)
			{
				ShowOrderDto orderDto = new ShowOrderDto();
				orderDto.Id = order.Id;
				orderDto.UserId = order.UserId;
				orderDto.Status = order.Status;
				orderDto.OrderDate = order.OrderDate;
				orderDto.TotalAmount = order.TotalAmount;

				ordersDto.Add(orderDto);
			}
			GeneralResponse generalResponse = new GeneralResponse() { IsSuccess = true, Data = ordersDto };
			return generalResponse;
		}

		[HttpPost]
		public ActionResult<GeneralResponse> AddOrder(OrderDto orderDto)
		{
			GeneralResponse generalResponse = new GeneralResponse();
			if(ModelState.IsValid && orderDto.OrderItemsDto.Count > 0)
			{
				Order order = new Order() { Status = OrderStatus.Pending, OrderDate = DateTime.Now };
				order.UserId = orderDto.UserId;

				decimal totalAmount = 0;
				List<OrderItem> orderItems = new List<OrderItem>();
				foreach(var item in orderDto.OrderItemsDto)
				{
					totalAmount += item.Quantity * item.UnitPrice;

					OrderItem orderItem = new OrderItem();
					orderItem.ProductId = item.ProductId;
					orderItem.Quantity = item.Quantity;
					orderItem.UnitPrice = item.UnitPrice;
					
					orderItems.Add(orderItem);
				}
				order.TotalAmount = totalAmount;
				order.OrderItems = orderItems;
				try
				{
					orderRepository.Add(order);
					orderRepository.Save();

					generalResponse.IsSuccess = true;
					generalResponse.Data = "Created done";
					return generalResponse;
				}
				catch(Exception ex)
				{
					generalResponse.IsSuccess = false;
					generalResponse.Data = ex.Message;
					return generalResponse;
				}
			}
			generalResponse.IsSuccess = false;
			generalResponse.Data = ModelState;
			return generalResponse;
		}

		[HttpGet("user/{userId}")]
		public ActionResult<GeneralResponse> GetOrdersByUserId(string userId)
		{
			List<Order> orders = orderRepository.GetOrdersByUserId(userId);
			GeneralResponse generalResponse = new GeneralResponse();
			if(orders.Count > 0)
			{
				List<ShowOrderDto> ordersDto = new List<ShowOrderDto>();
				foreach(Order order in orders)
				{
					ShowOrderDto orderDto = new ShowOrderDto();
					orderDto.Id = order.Id;
					orderDto.UserId = order.UserId;
					orderDto.Status = order.Status;
					orderDto.OrderDate = order.OrderDate;
					orderDto.TotalAmount = order.TotalAmount;

					ordersDto.Add(orderDto);
				}

				generalResponse.IsSuccess = true;
				generalResponse.Data = ordersDto;
				return generalResponse;
			}
			generalResponse.IsSuccess = false;
			generalResponse.Data = "Orders not found";
			return generalResponse;
		}

		[HttpGet("{orderId}")]
		public ActionResult<GeneralResponse> GetOrderDetails(int orderId)
		{
			Order order = orderRepository.GetOrderWithItemsById(orderId);
			GeneralResponse generalResponse = new GeneralResponse();
			if(order != null)
			{
				ShowOrderWithItemsDto orderWithItemsDto = new ShowOrderWithItemsDto();
				orderWithItemsDto.Id = order.Id;
				orderWithItemsDto.UserId = order.UserId;
				orderWithItemsDto.Status = order.Status;
				orderWithItemsDto.OrderDate = order.OrderDate;
				orderWithItemsDto.TotalAmount = order.TotalAmount;

				List<ShowOrderItemDto> orderItemsDto = new List<ShowOrderItemDto>();
				foreach(OrderItem item in order.OrderItems)
				{
					ShowOrderItemDto orderItemDto = new ShowOrderItemDto();
					orderItemDto.Id = item.Id;
					orderItemDto.ProductId = item.ProductId;
					orderItemDto.Quantity = item.Quantity;
					orderItemDto.OrderId = item.OrderId;
					orderItemDto.UnitPrice = item.UnitPrice;

					orderItemsDto.Add(orderItemDto);
				}
				orderWithItemsDto.OrderItems = orderItemsDto;

				generalResponse.IsSuccess = true;
				generalResponse.Data = orderWithItemsDto;
				return generalResponse;
			}
			generalResponse.IsSuccess = false;
			generalResponse.Data = "Id invalid";
			return generalResponse;
		}

		[HttpPut("{id}/status")]
		[Authorize(Roles = "Admin")]
		public ActionResult<GeneralResponse> UpdateOrderStatus(int id, UpdateOrderStatusDto orderStatusDto)
		{
			GeneralResponse generalResponse = new GeneralResponse();
			if(ModelState.IsValid)
			{
				Order order = orderRepository.GetOrderById(id);
				if(order != null)
				{
					if(Enum.TryParse<OrderStatus>(orderStatusDto.Status, true, out var result))
					{
						order.Status = result;
						orderRepository.Update(order);
						orderRepository.Save();

						generalResponse.IsSuccess = true;
						generalResponse.Data = "Updated Done";
						return generalResponse;
					}
					generalResponse.IsSuccess = false;
					generalResponse.Data = "Order Status invalid";
					return generalResponse;
				}
				generalResponse.IsSuccess = false;
				generalResponse.Data = "Id invalid";
				return generalResponse;
			}
			generalResponse.IsSuccess = false;
			generalResponse.Data = ModelState;
			return generalResponse;
		}
    }
}

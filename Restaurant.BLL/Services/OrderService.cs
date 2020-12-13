using System.Collections.Generic;
using AutoMapper;
using Restaurant.BLL.DTOobjects;
using Restaurant.BLL.Interfaces;
using Restaurant.BLL.Profiles;
using Restaurant.DAL.Interfaces;

namespace Restaurant.BLL.Services
{
    public class OrderService : IOrderService
    {
        private IUnitOfWork _db;
        private Mapper _mapper;
        
        public void Dispose()
        {
            _db.Dispose();
        }
        
        public OrderService(IUnitOfWork db)
        {
            _db = db;
            
            _mapper = new Mapper(
                new MapperConfiguration(
                    cfg=>cfg
                        .AddProfile<OrderProfile>()));
        }
        
        public IEnumerable<OrderDTO> GetAll()
        {
            var orders = _mapper
                .Map<IEnumerable<OrderDTO>>(
                    _db.Orders.GetAll());
            return orders;
        }
    }
}
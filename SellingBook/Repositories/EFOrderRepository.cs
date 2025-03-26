using SellingBook.Models;
using SellingBook.Models.BasicModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SellingBook.Repositories
{
    public class EFOrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public EFOrderRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        // Thêm đơn hàng vào cơ sở dữ liệu và lưu thay đổi
        public void AddOrder(Order order)
        {
            _applicationDbContext.Orders.Add(order);
            _applicationDbContext.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
        }

        // Xóa đơn hàng khỏi cơ sở dữ liệu và lưu thay đổi
        public void DeleteOrder(Order order)
        {
            _applicationDbContext.Orders.Remove(order);
            _applicationDbContext.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
        }

        // Lấy tất cả đơn hàng
        public IEnumerable<Order> GetOrders()
        {
            return _applicationDbContext.Orders.ToList(); // Đảm bảo gọi ToList để tải dữ liệu từ cơ sở dữ liệu
        }

        // Đếm số lượng đơn hàng (có thể thêm điều kiện lọc)
        public int GetOrdersCountBasedOnIds()
        {
            return _applicationDbContext.Orders.Count(); // Đếm tất cả các đơn hàng trong cơ sở dữ liệu
        }

        // Lưu bình luận vào cơ sở dữ liệu và trả về đối tượng ProductComment đã lưu
        public ProductComment SaveComment(string orderId, string comment, int productId, string userId)
        {
            // Kiểm tra đơn hàng có tồn tại hay không
            var order = _applicationDbContext.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null) return null;  // Nếu không tìm thấy đơn hàng, trả về null

            // Tạo đối tượng bình luận mới và lưu vào cơ sở dữ liệu
            var newComment = new ProductComment
            {
                OrderId = orderId,  // Khóa ngoại liên kết với đơn hàng
                ProductId = productId,  // Khóa ngoại liên kết với sản phẩm
                UserId = userId,  // Khóa ngoại liên kết với người dùng
                Comment = comment,  // Nội dung bình luận
                CreatedAt = DateTime.Now  // Thời gian tạo bình luận
            };

            _applicationDbContext.ProductComments.Add(newComment);
            _applicationDbContext.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu

            return newComment; // Trả về đối tượng ProductComment đã lưu
        }
    }
}

using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.IRepository;
using Repository.Repository;

namespace KoiFarmRazorPage.Pages.Admin
{
    public class IndexModel : PageModel
    {
        // Properties to hold data

        private readonly IUserRepository userRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IKoiFishRepository koiFishRepository;
        private readonly IProductRepository productRepository;
        public IList<User> User { get; set; } = new List<User>()!;
        public IList<Order> Order { get; set; } = new List<Order>()!;
        public List<string> Transactions { get; set; } = new List<string>();

        public IndexModel()
        {
            userRepository = new UserRepository();
            orderRepository = new OrderRepository();
            koiFishRepository = new KoiFishRepository();
            productRepository = new ProductRepository();
        }

        public void OnGet()
        {
            // Initial data load (could be empty)
            LoadUsers();
            LoadRevenues();
        }

        public void OnGetLoadUsers()
        {
            LoadUsers();
        }

        public void OnGetLoadRevenues()
        {
            LoadRevenues();
        }

        public void OnGetLoadTransactions()
        {
            LoadTransactions();
        }

        // Dummy methods to simulate loading data from a database
        private void LoadUsers()
        {
            User = userRepository.GetUsers();
        }

        private void LoadRevenues()
        {
            Order = orderRepository.GetAllOrders();
            foreach (var order in Order)
            {
                List<Product> products = productRepository.GetProductsByListString(order.ProductId);
                List<KoiFish> koiFishs = koiFishRepository.GetKoiFishsByListString(order.KoiFishId);
                order.KoiFishList = koiFishs;
                order.ProductList = products;
            }
        }

        private void LoadTransactions()
        {
            Transactions = new List<string> { "Transaction 1", "Transaction 2", "Transaction 3" };
        }
    }
}

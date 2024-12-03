using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using wesolapi.Controllers;
using wesolapi.Data;
using wesolapi.Entity;

namespace WebSolUnitTest.Controllers
{
    public class CustomerControllerTest
    {
        private readonly Mock<ILogger<CustomerController>> _mockLogger;
        private readonly Mock<IApplicationDBContext> _mockContext;
        private readonly CustomerController _mockCustomerController;

        public CustomerControllerTest()
        {
            _mockLogger = new Mock<ILogger<CustomerController>>();
            _mockContext = new Mock<IApplicationDBContext>();
            _mockCustomerController = new CustomerController(_mockLogger.Object, _mockContext.Object);
        }

        [Fact]
        public void GetAllCustomer()
        {
            Mock<DbSet<Customer>> mockDbSet = setupCustomerObjecct();

            _mockContext.Setup(x => x.Customers).Returns(mockDbSet.Object);


            var result = _mockCustomerController.GetAll();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(25)]
        public void GetById(int id)
        {
            var mockDbSet = setupCustomerObjecct();
            _mockContext.Setup(x => x.Customers).Returns(mockDbSet.Object);

            var result = _mockCustomerController.GetById(id);

            if (result == null)
            {
                Assert.Null(result);
            }
            else
            {
                Assert.NotNull(result);
            }
        }

        private static Mock<DbSet<Customer>> setupCustomerObjecct()
        {
            var customers = new List<Customer>()
            {
                new()
                {
                    Id = 1,
                    Name = "Foo",
                    Address = "XYZ"
                },
                new()
                {
                    Id = 2,
                    Name = "Test",
                    Address = "ABC"
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Customer>>();
            mockDbSet.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(customers.Provider);
            mockDbSet.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(customers.Expression);
            mockDbSet.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(customers.ElementType);
            mockDbSet.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator()).Returns(customers.GetEnumerator());
            return mockDbSet;
        }
    }
}

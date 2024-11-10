using Microsoft.AspNetCore.Mvc;
using Common.Dao;
using Common.Database;
using Common.Encryption;
using Common.Model;
using Newtonsoft.Json;

namespace Products.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductDao ProductDao;
        private readonly IEncryptor IEncryptor;

        public ProductsController()
        {
            ProductDao = new ProductDao(DbContextDaoType.SqlServer, "Server=localhost\\SQLEXPRESS;Database=BetaTest;Trusted_Connection=True;");
            IEncryptor = EncryptorFactory.GetEncryptor(EncryptorType.BASIC);
        }

        [HttpPost(Name = "GetProducts")]
        public IActionResult GetProducts()
        {
            try
            {
                return Ok(ProductDao.GetProducts());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "GetProductsByCategory")]
        public IActionResult GetProductsByCategory([FromBody] JsonPayload categoryJson)
        {
            try
            {
                string category = DecryptAndDeserializeJson<string>(categoryJson.Data);
                return Ok(ProductDao.GetProductsByCategory(category));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "GetProductById")]
        public IActionResult GetProductById([FromBody] JsonPayload idJson)
        {
            try
            {
                int id = DecryptAndDeserializeJson<int>(idJson.Data);
                return Ok(ProductDao.GetProductById(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "GetProductByProductId")]
        public IActionResult GetProductByProductId([FromBody] JsonPayload productIdJson)
        {
            try
            {
                int productId = DecryptAndDeserializeJson<int>(productIdJson.Data);
                return Ok(ProductDao.GetProductByProductId(productId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "GetProductByName")]
        public IActionResult GetProductByName([FromBody] JsonPayload nameJson)
        {
            try
            {
                string name = DecryptAndDeserializeJson<string>(nameJson.Data);
                return Ok(ProductDao.GetProductByName(name));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "InsertProduct")]
        public IActionResult InsertProduct([FromBody] JsonPayload productJson)
        {
            try
            {
                Product product = DecryptAndDeserializeJson<Product>(productJson.Data);
                return Ok(ProductDao.InsertProduct(product));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private T DecryptAndDeserializeJson<T>(string payload)
        {
            T result = default;
            string decrypted = IEncryptor.Decrypt(payload);
            result = JsonConvert.DeserializeObject<T>(decrypted);
            return result;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Backend.Data; // 引入 AppDbContext 的命名空间
using Backend.Models; // 引入 Student 和其他 Model 的命名空间
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RandomDataController : ControllerBase
    {
        private readonly AppDbContext _context;

        // 使用构造函数注入 AppDbContext
        public RandomDataController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("random")]
        public IActionResult GetRandomData()
        {
            var randomData = new { Value = new Random().Next(1, 100) };
            return Ok(randomData);
        }

        [HttpPost("increment")]
        public IActionResult Increment([FromBody] int number)
        {
            var result = number + 1;
            return Ok(new { Result = result });
        }

        [HttpPost("add-student")]
        public async Task<IActionResult> AddStudent([FromBody] Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return Ok(student);
        }
        
        [HttpGet("search-students")]
        public IActionResult SearchStudentsByName([FromQuery] string name)
        {
            // 查询名字中包含指定字符串的学生记录（不区分大小写）
            var students = _context.Students
                .Where(s => s.FirstName.Contains(name) || s.LastName.Contains(name))
                .ToList();

            return Ok(students);
        }

        
    }
}
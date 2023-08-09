using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace Assignment1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private static List<string> A = new List<string>()
        {
        "EmployeA","EmployeB","EmployeeC"};
        //Get: api/Test
        [HttpGet("all")]
        public IActionResult Get()
        {
           
            return Ok(A);
        }
        //Get: api/Test/1
        [HttpGet("allbyId")]
        public IActionResult GetById(int  id)
        {
            if (id >= 0 && id < A.Count) {
                return Ok(A[id]);
            }
            else
            {
                return BadRequest();
            }
        }
        //Post: api/Test
        [HttpPost("create")]
        public IActionResult Post([FromBody]string neA)
        {
            A.Add(neA);
            return Created("", neA);
        }
        //Put: api/Test/1
        [HttpPut("updatebyid")]
        public IActionResult Update(int id, [FromBody] string uA)
        {
            if (id >= 0 && id < A.Count)
            {
                A[id] = uA;
                return Ok(A);
            }
            else
            {
                return BadRequest();
            }
        }
        //Delete: api/Test/1
        [HttpDelete("Deletebyid")]
        public IActionResult Delete(int id) 
        {
            if (id >= 0 && id < A.Count)
            {
                A.RemoveAt(id);
                return Ok(A);
            }
            else
            {
                return BadRequest();
            }
        }
       
        }
    }


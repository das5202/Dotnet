using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment1.Controllers
{

    [ApiController]
    [Route("api/[Controller]")]
    public class DummyController : ControllerBase
    {
        private List<string> dev = new List<string>()
{
"number1",
"number2",
"number3"
};

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            return Ok(dev);
        }
        [HttpGet("getbyId")]
        public IActionResult GetById(int id)
        {
            if (id >= 0 && id < dev.Count)
            {
                return Ok(dev[id]);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost("create")]
        public IActionResult create([FromBody] string newdev)
        {
            dev.Add(newdev);
            return Created("", newdev);
        }
        [HttpPut("updateById")]
        public IActionResult Update(int id, [FromBody] string updev)
        {
            if (id >= 0 && id < dev.Count)
            {
                dev[id] = updev;
                return Ok(updev);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpDelete("deletebyid")]
        public IActionResult DeleteById(int id)
        {
            if (id >= 0 && id < dev.Count)
            {
                dev.RemoveAt(id);
                return Ok(dev);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}




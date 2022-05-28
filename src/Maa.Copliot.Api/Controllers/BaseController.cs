using MaaCopilot.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace MaaCopilot.Controllers
{
    public abstract class BaseController<T, U, V> : Controller
        where T : class
        where U : class
        where V : class
    {
        protected readonly IService<T, U> _service;
        public BaseController(IService<T, U> service)
        {
            _service = service;
        }

        [HttpPost]
        public virtual async Task<IActionResult> CreateAsync([FromBody] T request)
        {
            var res = await _service.AddAsync(request).ConfigureAwait(false);
            return res.Success ? Ok(res) : BadRequest(res.Message);
        }
    }
}
using Lookeach.Application.Commands.Categories;
using Lookeach.Application.Queries.Categories;
using Lookeach.Core.ViewModels.Categories;
using Lookeach.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Lookeach.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryCreateModel categoryCreateModel)
        {
            ResponseViewModel responseViewModel = new ResponseViewModel();
            var rs = await _mediator.Send(new CreateCategoryCommand(categoryCreateModel));
            if (rs != null)
            {
                responseViewModel.IsFailure = false;
                responseViewModel.IsSuccess = true;
                responseViewModel.Error = null!;
                responseViewModel.Value = JsonConvert.SerializeObject(rs);
                return StatusCode(StatusCodes.Status201Created, responseViewModel);
            }
            responseViewModel.IsFailure = true;
            responseViewModel.IsSuccess = false;
            responseViewModel.Value = null!;
            responseViewModel.Error = new ErrorViewModel
            {
                Code = 500,
                Message = "Can't create new Category"
            };
            return StatusCode(StatusCodes.Status500InternalServerError, responseViewModel);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromForm] CategoryUpdateModel categoryUpdateModel)
        {
            ResponseViewModel responseViewModel = new ResponseViewModel();
            var rs = await _mediator.Send(new UpdateCategoryCommand(categoryUpdateModel));
            if (rs != null)
            {
                responseViewModel.IsFailure = false;
                responseViewModel.IsSuccess = true;
                responseViewModel.Error = null!;
                responseViewModel.Value = JsonConvert.SerializeObject(rs);
                return Ok(responseViewModel);
            }
            responseViewModel.IsFailure = true;
            responseViewModel.IsSuccess = false;
            responseViewModel.Value = null!;
            responseViewModel.Error = new ErrorViewModel
            {
                Code = 500,
                Message = "Can't update category"
            };
            return StatusCode(StatusCodes.Status500InternalServerError, responseViewModel);
        }

        [HttpGet("{id}/category")]
        public async Task<IActionResult> GetCategory([FromRoute] Guid id)
        {
            var rs = await _mediator.Send(new GetCategoryByIdQuery(id));
            ResponseViewModel responseViewModel = new ResponseViewModel
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = JsonConvert.SerializeObject(rs)
            };
            return Ok(responseViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories(string? name, int? pageIndex, int? pageSize)
        {
            var rs = await _mediator.Send(new GetCategoriesQuery(name, pageIndex, pageSize));
            ResponseViewModel responseViewModel = new ResponseViewModel
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = JsonConvert.SerializeObject(rs)
            };
            return Ok(responseViewModel);
        }
    }
}

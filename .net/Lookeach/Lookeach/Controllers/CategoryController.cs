using Lookeach.Application.Commands.Categories;
using Lookeach.Application.Queries.Categories;
using Lookeach.Core.ViewModels.Categories;
using Lookeach.Core.ViewModels.Colors;
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

        [HttpPost("category/create")]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryCreateModel categoryCreateModel)
        {
            ResponseBase<CategoryViewModel> responseViewModel = new ResponseBase<CategoryViewModel>();
            var rs = await _mediator.Send(new CreateCategoryCommand(categoryCreateModel));
            if (rs != null)
            {
                responseViewModel.IsFailure = false;
                responseViewModel.IsSuccess = true;
                responseViewModel.Error = null!;
                responseViewModel.Value = rs;
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

        [HttpPut("category/update")]
        public async Task<IActionResult> UpdateCategory([FromForm] CategoryUpdateModel categoryUpdateModel)
        {
            ResponseBase<CategoryViewModel> responseViewModel = new ResponseBase<CategoryViewModel>();
            var rs = await _mediator.Send(new UpdateCategoryCommand(categoryUpdateModel));
            if (rs != null)
            {
                responseViewModel.IsFailure = false;
                responseViewModel.IsSuccess = true;
                responseViewModel.Error = null!;
                responseViewModel.Value = rs;
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

        [HttpGet("category/{id}")]
        public async Task<IActionResult> GetCategory([FromRoute] Guid id)
        {
            var rs = await _mediator.Send(new GetCategoryByIdQuery(id));
            ResponseBase<CategoryViewModel> responseViewModel = new ResponseBase<CategoryViewModel>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value =rs
            };
            return Ok(responseViewModel);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories(string? name, int? pageIndex, int? pageSize)
        {
            var rs = await _mediator.Send(new GetCategoriesQuery(name, pageIndex, pageSize));
            ResponseBase<List<CategoryViewModel>> responseViewModel = new ResponseBase<List<CategoryViewModel>>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value =rs
            };
            return Ok(responseViewModel);
        }
    }
}

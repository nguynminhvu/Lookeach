using Lookeach.Application.Commands.Brands;
using Lookeach.Application.Features.Brands.Commands;
using Lookeach.Application.Features.Brands.Models;
using Lookeach.Application.Queries.Brands;
using Lookeach.Application.ViewModels.Brands;
using Lookeach.AuthOptions;
using Lookeach.Constants;
using Lookeach.Core.Enums;
using Lookeach.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Lookeach.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BrandController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("brands")]
        public async Task<IActionResult> GetBrands(string? name, int? pageIndex, int? pageSize)
        {
            List<BrandViewModel> rs = new List<BrandViewModel>();
            if (!string.IsNullOrEmpty(name))
            {
                rs = await _mediator.Send(new GetBrandsByNameQuery(name, pageIndex, pageSize));
            }
            else
            {
                rs = await _mediator.Send(new GetAllBrandQuery(pageIndex, pageSize));
            }
            ResponseBase<List<BrandViewModel>> responseViewModel = new ResponseBase<List<BrandViewModel>>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = rs
            };
            return Ok(responseViewModel);
        }

        [HttpGet("brand/{id}")]
        public async Task<IActionResult> GetBrandById([FromRoute] Guid id)
        {
            var rs = await _mediator.Send(new GetBrandByIdQuery(id));
            ResponseBase<BrandViewModel> responseViewModel = new ResponseBase<BrandViewModel>
            {
                Error = null!,
                IsFailure = false,
                IsSuccess = true,
                Value = rs
            };
            return Ok(responseViewModel);
        }

        /// <summary>
        /// Create new Brand with BrandCreateModel
        /// </summary>
        /// <param name="brandCreateModel"></param>
        /// <returns></returns>
        [LookeachAuthorize(
            new string[] { ActionConstant.WRITE }
            )]
        [HttpPost("brand/create")]
        public async Task<IActionResult> CreateBrand([FromForm] BrandCreateModel brandCreateModel)
        {
            var rs = await _mediator.Send(new CreateBrandCommand(brandCreateModel));
            ResponseViewModel responseViewModel = new();
            if (rs != null)
            {
                responseViewModel.Error = null!;
                responseViewModel.IsFailure = false;
                responseViewModel.IsSuccess = true;
                responseViewModel.Value = JsonConvert.SerializeObject(rs);
                return StatusCode(StatusCodes.Status201Created, responseViewModel);
            }
            responseViewModel.Error = new ErrorViewModel
            {
                Code = 500,
                Message = "Can't create new Brand, try again"
            };
            responseViewModel.IsFailure = true;
            responseViewModel.IsSuccess = false;
            responseViewModel.Value = null!;
            return StatusCode(StatusCodes.Status500InternalServerError, responseViewModel);
        }

        /// <summary>
        /// Update brand with parameter BrandUpdateModel, no pass image parameter then this Brand has no image
        /// </summary>
        /// <param name="brandUpdateModel"></param>
        /// <returns></returns>
        [LookeachAuthorize(
          new string[] { ActionConstant.OVERRIDE }
          )]
        [Authorize(Roles = "Merchant")]
        [HttpPut("brand/update")]
        public async Task<IActionResult> UpdateBrand([FromForm] BrandUpdateModel brandUpdateModel)
        {
            var rs = await _mediator.Send(new UpdateBrandCommand(brandUpdateModel));
            ResponseViewModel responseViewModel = new();
            if (rs != null)
            {
                responseViewModel.Error = null!;
                responseViewModel.IsFailure = false;
                responseViewModel.IsSuccess = true;
                responseViewModel.Value = JsonConvert.SerializeObject(rs);
                return StatusCode(StatusCodes.Status200OK, responseViewModel);
            }
            responseViewModel.Error = new ErrorViewModel
            {
                Code = 500,
                Message = "Can't update Brand, try again"
            };
            responseViewModel.IsFailure = true;
            responseViewModel.IsSuccess = false;
            responseViewModel.Value = null!;
            return StatusCode(StatusCodes.Status500InternalServerError, responseViewModel);
        }
    }
}

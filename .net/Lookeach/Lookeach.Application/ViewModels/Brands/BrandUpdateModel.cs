using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lookeach.Application.ViewModels.Brands
{
    public class BrandUpdateModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}

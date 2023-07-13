using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Queries.GetById
{
    public class ProductGetByIdQuery
    {
        public Guid Id { get; set; }
        public ProductGetByIdQuery(int Id)
        {
            Id = Id;
        }
    }
}

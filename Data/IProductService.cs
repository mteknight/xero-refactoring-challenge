using System;

using RefactorThis.Domain;

namespace RefactorThis.Data
{
    public interface IProductService
    {
        Product Get(Guid id);
    }
}

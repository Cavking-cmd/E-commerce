using System.Linq.Expressions;
using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;
using E_commerce.Core.Entities;

namespace E_commerce.Services.Interfaces
{
    public interface IVendorService
    {
        Task<BaseResponse<VendorDto>> CreateVendor(CreateVendorRequestModel model);
        Task<BaseResponse<VendorDto>> GetAsync(Guid id);
        Task<BaseResponse<ICollection<VendorDto>>> GetAll();
        Task<BaseResponse<VendorDto>> UpdateVendor(UpdateVendorRequestModel model);
        Task<BaseResponse<bool>> DeleteVendor(Guid id);
    }
}

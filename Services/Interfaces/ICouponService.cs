using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;
using E_commerce.Core.Entities;
using E_commerce.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_commerce.Services.Interfaces
{
    public interface ICouponService
    {
        Task<BaseResponse<CouponDto>> CreateCoupon(CreateCouponRequestModel model);
        Task<BaseResponse<CouponDto>> GetCouponAsync(string code);
        Task<BaseResponse<Coupon>> RedeemCouponAsync(string code);
    }
}
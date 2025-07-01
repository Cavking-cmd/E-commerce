using System;
using System.Linq;
using System.Threading.Tasks;
using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;
using E_commerce.Core.Entities;
using E_commerce.Core.Entities.Enums;
using E_commerce.Repositories.Interfaces;
using E_commerce.Services.Interfaces;

namespace E_commerce.Services.Implementations
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _couponRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CouponService(ICouponRepository couponRepository, IUnitOfWork unitOfWork)
        {
            _couponRepository = couponRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<CouponDto>> CreateCoupon(CreateCouponRequestModel model)
        {
            try
            {
                if (model == null)
                {
                    return new BaseResponse<CouponDto>
                    {
                        Message = "Coupon model cannot be null.",
                        Status = false,
                        Data = null
                    };
                }

                if (string.IsNullOrWhiteSpace(model.Code))
                {
                    return new BaseResponse<CouponDto>
                    {
                        Message = "Coupon code is required.",
                        Status = false,
                        Data = null
                    };
                }

                if (model.DiscountPercentage <= 0 || model.DiscountPercentage > 100)
                {
                    return new BaseResponse<CouponDto>
                    {
                        Message = "Discount percentage must be between 0 and 100.",
                        Status = false,
                        Data = null
                    };
                }

                if (model.ValidFrom >= model.ValidUntil)
                {
                    return new BaseResponse<CouponDto>
                    {
                        Message = "ValidUntil must be after ValidFrom.",
                        Status = false,
                        Data = null
                    };
                }

                var existingCoupon = await _couponRepository.GetCouponAsync(c => c.Code == model.Code);
                if (existingCoupon != null)
                {
                    return new BaseResponse<CouponDto>
                    {
                        Message = "Coupon code already exists.",
                        Status = false,
                        Data = null
                    };
                }

                var coupon = new Coupon
                {
                    Code = model.Code,
                    DiscountPercentage = model.DiscountPercentage,
                    ValidFrom = model.ValidFrom,
                    ValidUntil = model.ValidUntil,
                    Status = CouponEnum.Active,
                    Orders = new System.Collections.Generic.List<Order>()
                };

                await _couponRepository.CreateAsync(coupon);
                await _unitOfWork.SaveChangesAsync();

                var couponDto = new CouponDto
                {
                    Code = coupon.Code,
                    DiscountPercentage = coupon.DiscountPercentage,
                    ValidFrom = coupon.ValidFrom,
                    ValidUntil = coupon.ValidUntil,
                    Status = coupon.Status
                };

                return new BaseResponse<CouponDto>
                {
                    Message = "Coupon created successfully",
                    Status = true,
                    Data = couponDto
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<CouponDto>
                {
                    Message = ex.Message,
                    Status = false,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<CouponDto>> GetCouponAsync(string code)
        {
            var coupon = await _couponRepository.GetCouponAsync(c => c.Code == code && !c.Orders.Any());
            if (coupon == null)
            {
                return new BaseResponse<CouponDto>
                {
                    Message = "Coupon not found or already used.",
                    Status = false,
                    Data = null
                };
            }

            var couponDto = new CouponDto
            {
                Code = coupon.Code,
                DiscountPercentage = coupon.DiscountPercentage,
                ValidFrom = coupon.ValidFrom,
                ValidUntil = coupon.ValidUntil,
                Status = coupon.Status
            };

            return new BaseResponse<CouponDto>
            {
                Message = "Coupon retrieved successfully",
                Status = true,
                Data = couponDto
            };
        }
        public async Task<BaseResponse<Coupon>> RedeemCouponAsync(string code)
        {
            try
            {
                var coupon = await _couponRepository.GetCouponAsync(c =>
                    c.Code == code &&
                    c.Status == CouponEnum.Active &&
                    c.ValidFrom <= DateTime.UtcNow &&
                    c.ValidUntil >= DateTime.UtcNow &&
                    !c.Orders.Any());

                if (coupon == null)
                {
                    return new BaseResponse<Coupon>
                    {
                        Message = "Invalid, expired, or already used coupon.",
                        Status = false,
                        Data = null
                    };
                }

                // Mark the coupon as used
                coupon.Status = CouponEnum.Redeemed;

                await _couponRepository.Update(coupon);
                await _unitOfWork.SaveChangesAsync();

                return new BaseResponse<Coupon>
                {
                    Message = "Coupon redeemed successfully",
                    Status = true,
                    Data = coupon
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Coupon>
                {
                    Message = ex.Message,
                    Status = false,
                    Data = null
                };
            }
        }
    }
}
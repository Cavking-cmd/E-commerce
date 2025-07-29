using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;
using E_commerce.Core.Entities;
using E_commerce.Repositories.Interfaces;
using E_commerce.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce.Services.Implementations
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<BaseResponse<ReviewDto>> CreateReviewAsync(ReviewDto model)
        {
            try
            {
                var review = new Review
                {
                    Id = Guid.NewGuid(),
                    ProductId = model.ProductId,
                    //UserId = model.UserId,
                    Rating = model.Rating,
                    Comment = model.Comment
                };

                await _reviewRepository.CreateAsync(review);

                model.Id = review.Id;

                return new BaseResponse<ReviewDto>
                {
                    Message = "Review created successfully.",
                    Status = true,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ReviewDto>
                {
                    Message = $"An error occurred: {ex.Message}",
                    Status = false,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteReviewAsync(Guid id)
        {
            try
            {
                var review = await _reviewRepository.GetReviewByIdAsync(id);
                if (review == null)
                {
                    return new BaseResponse<bool>
                    {
                        Message = "Review not found.",
                        Status = false,
                        Data = false
                    };
                }

                await _reviewRepository.SoftDeleteAsync(review);

                return new BaseResponse<bool>
                {
                    Message = "Review deleted successfully.",
                    Status = true,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>
                {
                    Message = $"An error occurred: {ex.Message}",
                    Status = false,
                    Data = false
                };
            }
        }

        public async Task<BaseResponse<ICollection<ReviewDto>>> GetAllReviewsAsync()
        {
            try
            {
                var reviews = await _reviewRepository.GetAllReviewsAsync();
                var reviewDtos = reviews.Select(r => new ReviewDto
                {
                    Id = r.Id,
                    ProductId = r.ProductId,
                    //UserId = r.UserId,
                    Rating = r.Rating,
                    Comment = r.Comment
                }).ToList();

                return new BaseResponse<ICollection<ReviewDto>>
                {
                    Message = "Reviews retrieved successfully.",
                    Status = true,
                    Data = reviewDtos
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ICollection<ReviewDto>>
                {
                    Message = $"An error occurred: {ex.Message}",
                    Status = false,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<ReviewDto>> GetReviewAsync(Guid id)
        {
            try
            {
                var review = await _reviewRepository.GetReviewByIdAsync(id);
                if (review == null)
                {
                    return new BaseResponse<ReviewDto>
                    {
                        Message = "Review not found.",
                        Status = false,
                        Data = null
                    };
                }

                var reviewDto = new ReviewDto
                {
                    Id = review.Id,
                    ProductId = review.ProductId,
                    //UserId = review.UserId,
                    Rating = review.Rating,
                    Comment = review.Comment
                };

                return new BaseResponse<ReviewDto>
                {
                    Message = "Review retrieved successfully.",
                    Status = true,
                    Data = reviewDto
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ReviewDto>
                {
                    Message = $"An error occurred: {ex.Message}",
                    Status = false,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<ReviewDto>> UpdateReviewAsync(ReviewDto model)
        {
            try
            {
                var review = await _reviewRepository.GetReviewByIdAsync(model.Id);
                if (review == null)
                {
                    return new BaseResponse<ReviewDto>
                    {
                        Message = "Review not found.",
                        Status = false,
                        Data = null
                    };
                }

                review.Rating = model.Rating;
                review.Comment = model.Comment;
                ;

                await _reviewRepository.Update(review);

                return new BaseResponse<ReviewDto>
                {
                    Message = "Review updated successfully.",
                    Status = true,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ReviewDto>
                {
                    Message = $"An error occurred: {ex.Message}",
                    Status = false,
                    Data = null
                };
            }
        }
    }
}

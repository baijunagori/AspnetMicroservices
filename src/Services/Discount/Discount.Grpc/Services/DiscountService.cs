using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository discountRepository;
        private readonly ILogger logger;
        private readonly IMapper mapper;

        public DiscountService(IDiscountRepository discountRepository, ILogger logger, IMapper mapper)
        {
            this.discountRepository = discountRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await discountRepository.GetDiscount(request.ProductName);
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={ request.ProductName } not found"));
            }
            logger.LogInformation($"Discount is retrieved for ProductName = {coupon.ProductName} and Amount = {coupon.Amount}");
            var couponModel = mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = mapper.Map<Coupon>(request.Coupon);
            await discountRepository.CreateDiscount(coupon);
            logger.LogInformation($"Discount created successfully for ProductName = {coupon.ProductName}");

            var couponModel = mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = mapper.Map<Coupon>(request.Coupon);
            await discountRepository.UpdateDiscount(coupon);
            logger.LogInformation($"Discount updated successfully for ProductName = {coupon.ProductName}");

            var couponModel = mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var deleted = await discountRepository.DeleteDiscount(request.ProductName);
            var response = new DeleteDiscountResponse
            {
                Success = deleted
            };
            return response;
        }

    }
}

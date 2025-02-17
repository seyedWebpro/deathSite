using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Context;
using api.Middleware;
using deathSite.Model;
using deathSite.View.Factors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace deathSite.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class FactorsController : ControllerBase
    {
        private readonly ILogger<FactorsController> _logger;
        private readonly apiContext _context;


        public FactorsController(ILogger<FactorsController> logger, apiContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: api/Factors
        [HttpGet]
        public async Task<IActionResult> GetAllFactors()
        {
            var factors = await _context.Factors
                .Include(p => p.User)
                .Include(p => p.UserPackage)
                .Select(f => new FactorsViewDto
                {
                    Id = f.Id,
                    UserId = f.UserId,
                    UserPackageId = f.UserPackageId,
                    UserName = f.UserName,
                    TransactionDate = f.TransactionDate,
                    Amount = f.Amount,
                    Status = f.Status,
                    TransactionType = f.TransactionType,
                    TrackingNumber = f.TrackingNumber,
                    PaymentGateway = f.PaymentGateway,
                    Description = f.Description,
                    OrderId = f.OrderId
                })
                .ToListAsync();

            return Ok(new { StatusCode = 200, Message = "فاکتورها با موفقیت دریافت شدند.", Data = factors });
        }

        // GET: api/Factors/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFactorById(int id)
        {
            var factor = await _context.Factors
                .Include(p => p.User)
                .Include(p => p.UserPackage)
                .Where(i => i.Id == id)
                .Select(f => new FactorsViewDto
                {
                    Id = f.Id,
                    UserId = f.UserId,
                    UserPackageId = f.UserPackageId,
                    UserName = f.UserName,
                    TransactionDate = f.TransactionDate,
                    Amount = f.Amount,
                    Status = f.Status,
                    TransactionType = f.TransactionType,
                    TrackingNumber = f.TrackingNumber,
                    PaymentGateway = f.PaymentGateway,
                    Description = f.Description,
                    OrderId = f.OrderId
                })
                .FirstOrDefaultAsync();

            if (factor == null)
            {
                return NotFound(new { StatusCode = 404, Message = "فاکتور با شناسه مورد نظر یافت نشد." });
            }

            return Ok(new { StatusCode = 200, Message = "فاکتور با موفقیت دریافت شد.", Data = factor });
        }

        // GET: api/Factors/User/{userId}
        [HttpGet("User/{userId}")]
        public async Task<IActionResult> GetFactorsByUserId(int userId)
        {
            var userExists = await _context.users.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                return NotFound(new { StatusCode = 404, Message = "کاربر با شناسه مورد نظر یافت نشد." });
            }

            var factors = await _context.Factors
                .Where(i => i.UserId == userId)
                .Include(p => p.UserPackage)
                .Select(f => new FactorsViewDto
                {
                    Id = f.Id,
                    UserId = f.UserId,
                    UserPackageId = f.UserPackageId,
                    UserName = f.UserName,
                    TransactionDate = f.TransactionDate,
                    Amount = f.Amount,
                    Status = f.Status,
                    TransactionType = f.TransactionType,
                    TrackingNumber = f.TrackingNumber,
                    PaymentGateway = f.PaymentGateway,
                    Description = f.Description,
                    OrderId = f.OrderId
                })
                .ToListAsync();

            return Ok(new { StatusCode = 200, Message = "فاکتورهای کاربر با موفقیت دریافت شدند.", Data = factors });
        }

        // POST: api/Factors/Filter
        [HttpPost("Filter")]
        public async Task<IActionResult> FilterFactors([FromBody] FactorsFilterDto filterDto)
        {
            var query = _context.Factors
                .Include(p => p.User)
                .Include(p => p.UserPackage)
                .AsQueryable();

            if (filterDto.FromDate.HasValue)
            {
                query = query.Where(i => i.TransactionDate >= filterDto.FromDate.Value);
            }

            if (filterDto.ToDate.HasValue)
            {
                query = query.Where(i => i.TransactionDate <= filterDto.ToDate.Value);
            }

            if (filterDto.UserId.HasValue)
            {
                query = query.Where(i => i.UserId == filterDto.UserId.Value);
            }

            if (!string.IsNullOrEmpty(filterDto.Status))
            {
                query = query.Where(i => i.Status == filterDto.Status);
            }

            if (!string.IsNullOrEmpty(filterDto.PaymentGateway))
            {
                query = query.Where(i => i.PaymentGateway == filterDto.PaymentGateway);
            }

            if (!string.IsNullOrEmpty(filterDto.TransactionType))
            {
                query = query.Where(i => i.TransactionType == filterDto.TransactionType);
            }

            var factors = await query
                .Select(f => new FactorsViewDto
                {
                    Id = f.Id,
                    UserId = f.UserId,
                    UserPackageId = f.UserPackageId,
                    UserName = f.UserName,
                    TransactionDate = f.TransactionDate,
                    Amount = f.Amount,
                    Status = f.Status,
                    TransactionType = f.TransactionType,
                    TrackingNumber = f.TrackingNumber,
                    PaymentGateway = f.PaymentGateway,
                    Description = f.Description,
                    OrderId = f.OrderId
                })
                .ToListAsync();

            return Ok(new { StatusCode = 200, Message = "فاکتورهای فیلتر شده با موفقیت دریافت شدند.", Data = factors });
        }

        // POST: api/Factors
        [HttpPost]
        public async Task<IActionResult> CreateFactor([FromBody] FactorsCreateDto factorDto)
        {
            var validationResult = HelperMethods.HandleValidationErrors(ModelState);
            if (validationResult != null)
            {
                return validationResult;
            }

            var userExists = await _context.users.AnyAsync(u => u.Id == factorDto.UserId);
            if (!userExists)
            {
                return BadRequest(new { StatusCode = 400, Message = "کاربر با شناسه مورد نظر یافت نشد." });
            }

            if (factorDto.UserPackageId.HasValue)
            {
                var userPackageExists = await _context.UserPackages.AnyAsync(up => up.Id == factorDto.UserPackageId.Value);
                if (!userPackageExists)
                {
                    return BadRequest(new { StatusCode = 400, Message = "پکیج کاربر با شناسه مورد نظر یافت نشد." });
                }
            }

            var factor = new Factors
            {
                UserId = factorDto.UserId,
                UserPackageId = factorDto.UserPackageId,
                UserName = factorDto.UserName,
                TransactionDate = factorDto.TransactionDate,
                Amount = factorDto.Amount,
                Status = factorDto.Status,
                TransactionType = factorDto.TransactionType,
                TrackingNumber = factorDto.TrackingNumber,
                PaymentGateway = factorDto.PaymentGateway,
                Description = factorDto.Description,
                OrderId = factorDto.OrderId
            };

            _context.Factors.Add(factor);
            await _context.SaveChangesAsync();

            var result = new FactorsViewDto
            {
                Id = factor.Id,
                UserId = factor.UserId,
                UserPackageId = factor.UserPackageId,
                UserName = factor.UserName,
                TransactionDate = factor.TransactionDate,
                Amount = factor.Amount,
                Status = factor.Status,
                TransactionType = factor.TransactionType,
                TrackingNumber = factor.TrackingNumber,
                PaymentGateway = factor.PaymentGateway,
                Description = factor.Description,
                OrderId = factor.OrderId
            };

            return Ok(new { StatusCode = 201, Message = "فاکتور با موفقیت ایجاد شد.", Data = result });
        }

        // PUT: api/Factors/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFactor(int id, [FromBody] FactorsUpdateDto factorDto)
        {
            var factor = await _context.Factors.FindAsync(id);
            if (factor == null)
            {
                return NotFound(new { StatusCode = 404, Message = "فاکتور با شناسه مورد نظر یافت نشد." });
            }

            // Update only the fields that are not null
            if (factorDto.UserId.HasValue)
            {
                var userExists = await _context.users.AnyAsync(u => u.Id == factorDto.UserId.Value);
                if (!userExists)
                {
                    return BadRequest(new { StatusCode = 400, Message = "کاربر با شناسه مورد نظر یافت نشد." });
                }
                factor.UserId = factorDto.UserId.Value;
            }

            if (factorDto.UserPackageId.HasValue)
            {
                var userPackageExists = await _context.UserPackages.AnyAsync(up => up.Id == factorDto.UserPackageId.Value);
                if (!userPackageExists)
                {
                    return BadRequest(new { StatusCode = 400, Message = "پکیج کاربر با شناسه مورد نظر یافت نشد." });
                }
                factor.UserPackageId = factorDto.UserPackageId;
            }

            if (factorDto.UserName != null)
            {
                factor.UserName = factorDto.UserName;
            }

            if (factorDto.TransactionDate.HasValue)
            {
                factor.TransactionDate = factorDto.TransactionDate.Value;
            }

            if (factorDto.Amount.HasValue)
            {
                factor.Amount = factorDto.Amount.Value;
            }

            if (factorDto.Status != null)
            {
                factor.Status = factorDto.Status;
            }

            if (factorDto.TransactionType != null)
            {
                factor.TransactionType = factorDto.TransactionType;
            }

            if (factorDto.TrackingNumber != null)
            {
                factor.TrackingNumber = factorDto.TrackingNumber;
            }

            if (factorDto.PaymentGateway != null)
            {
                factor.PaymentGateway = factorDto.PaymentGateway;
            }

            if (factorDto.Description != null)
            {
                factor.Description = factorDto.Description;
            }

            if (factorDto.OrderId.HasValue)
            {
                factor.OrderId = factorDto.OrderId.Value;
            }

            await _context.SaveChangesAsync();

            var result = new FactorsViewDto
            {
                Id = factor.Id,
                UserId = factor.UserId,
                UserPackageId = factor.UserPackageId,
                UserName = factor.UserName,
                TransactionDate = factor.TransactionDate,
                Amount = factor.Amount,
                Status = factor.Status,
                TransactionType = factor.TransactionType,
                TrackingNumber = factor.TrackingNumber,
                PaymentGateway = factor.PaymentGateway,
                Description = factor.Description,
                OrderId = factor.OrderId
            };

            return Ok(new { StatusCode = 200, Message = "فاکتور با موفقیت بروزرسانی شد.", Data = result });
        }

        // DELETE: api/Factors/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFactor(int id)
        {
            var factor = await _context.Factors.FindAsync(id);
            if (factor == null)
            {
                return NotFound(new { StatusCode = 404, Message = "فاکتور با شناسه مورد نظر یافت نشد." });
            }

            _context.Factors.Remove(factor);
            await _context.SaveChangesAsync();

            return Ok(new { StatusCode = 200, Message = "فاکتور با موفقیت حذف شد." });
        }

        // GET: api/Factors/Dashboard/Summary
        [HttpGet("Dashboard/Summary")]
        public async Task<IActionResult> GetDashboardSummary()
        {
            var today = DateTime.Today;
            var lastMonth = today.AddMonths(-1);
            var lastYear = today.AddYears(-1);

            var totalFactors = await _context.Factors.CountAsync();
            var successfulFactors = await _context.Factors.CountAsync(i => i.Status == "Success");
            var totalAmount = await _context.Factors
                .Where(i => i.Status == "Success")
                .SumAsync(i => i.Amount);

            var todayFactors = await _context.Factors
                .Where(i => i.TransactionDate.Date == today)
                .CountAsync();

            var lastMonthFactors = await _context.Factors
                .Where(i => i.TransactionDate >= lastMonth && i.TransactionDate <= today)
                .CountAsync();

            var yearlyFactors = await _context.Factors
                .Where(i => i.TransactionDate >= lastYear && i.TransactionDate <= today)
                .CountAsync();

            var summary = new
            {
                TotalFactors = totalFactors,
                SuccessfulFactors = successfulFactors,
                SuccessRate = totalFactors > 0 ? (double)successfulFactors / totalFactors * 100 : 0,
                TotalAmount = totalAmount,
                TodayFactors = todayFactors,
                LastMonthFactors = lastMonthFactors,
                YearlyFactors = yearlyFactors
            };

            return Ok(new { StatusCode = 200, Message = "اطلاعات داشبورد با موفقیت دریافت شد.", Data = summary });
        }

        // GET: api/Factors/Statistics/Monthly
        [HttpGet("Statistics/Monthly")]
        public async Task<IActionResult> GetMonthlyStatistics(int year)
        {
            if (year <= 0)
            {
                year = DateTime.Now.Year;
            }

            var statistics = await _context.Factors
                .Where(i => i.TransactionDate.Year == year && i.Status == "Success")
                .GroupBy(i => i.TransactionDate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Count = g.Count(),
                    TotalAmount = g.Sum(i => i.Amount)
                })
                .OrderBy(s => s.Month)
                .ToListAsync();

            return Ok(new { StatusCode = 200, Message = "آمار ماهانه با موفقیت دریافت شد.", Data = statistics });
        }

    }
}
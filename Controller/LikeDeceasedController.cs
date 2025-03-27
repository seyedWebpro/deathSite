using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Context;
using deathSite.Model;
using deathSite.View.Dead;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace deathSite.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class LikeDeceasedController : ControllerBase
    {

        private readonly ILogger<LikeDeceasedController> _logger;
        private readonly apiContext _context;


        public LikeDeceasedController(ILogger<LikeDeceasedController> logger, apiContext context)
        {
            _logger = logger;
            _context = context;
        }


        [HttpPost("like-toggle")]
        public async Task<IActionResult> ToggleLike([FromBody] LikeDeceasedRequest request)
        {
            var user = await _context.users.FindAsync(request.UserId);
            var deceased = await _context.Deceaseds.FindAsync(request.DeceasedId);

            if (user == null || deceased == null)
            {
                return NotFound(new
                {
                    status = 404,
                    message = "کاربر یا متوفی یافت نشد."
                });
            }

            // بررسی اینکه آیا متوفی قبلاً لایک شده است
            var existingLike = await _context.LikeDeceaseds
                .FirstOrDefaultAsync(l => l.UserId == request.UserId && l.DeceasedId == request.DeceasedId);

            if (existingLike != null)
            {
                _context.LikeDeceaseds.Remove(existingLike);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    status = 200,
                    message = "لایک حذف شد.",
                    isLiked = false
                });
            }
            else
            {
                var like = new LikeDeceased
                {
                    UserId = request.UserId,
                    DeceasedId = request.DeceasedId,
                    LikedAt = DateTime.UtcNow
                };

                _context.LikeDeceaseds.Add(like);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    status = 200,
                    message = "متوفی با موفقیت لایک شد.",
                    isLiked = true
                });
            }
        }

        [HttpGet("{deceasedId}/likes/count")]
        public async Task<IActionResult> GetDeceasedLikesCount(int deceasedId)
        {
            int count = await _context.LikeDeceaseds
                .CountAsync(l => l.DeceasedId == deceasedId);

            return Ok(new
            {
                status = 200,
                deceasedId,
                likesCount = count
            });
        }

        [HttpGet("{deceasedId}/likes/users")]
        public async Task<IActionResult> GetUsersWhoLikedDeceased(int deceasedId)
        {
            var users = await _context.LikeDeceaseds
                .Where(l => l.DeceasedId == deceasedId)
                .Select(l => new
                {
                    l.User.Id,
                    FullName = l.User.firstName + " " + l.User.lastName
                })
                .ToListAsync();

            return Ok(new
            {
                status = 200,
                deceasedId,
                users
            });
        }

        [HttpGet("{deceasedId}/likes/hasLiked/{userId}")]
        public async Task<IActionResult> HasUserLikedDeceased(int userId, int deceasedId)
        {
            bool hasLiked = await _context.LikeDeceaseds
                .AnyAsync(l => l.UserId == userId && l.DeceasedId == deceasedId);

            return Ok(new
            {
                status = 200,
                userId,
                deceasedId,
                hasLiked
            });
        }


    }
}
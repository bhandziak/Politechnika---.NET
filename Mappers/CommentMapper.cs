using CarWorkshopProjekt.Data;
using CarWorkshopProjekt.DTOs;
using Riok.Mapperly.Abstractions;

namespace CarWorkshopProjekt.Mappers
{
    [Mapper]
    public partial class CommentMapper
    {
        //[HttpPost("addComment")]
        public Comment MapToEntity(AddComment commentDto)
        {
            return new Comment
            {
                CommentId = Guid.NewGuid(),
                ServiceOrderId = Guid.Parse(commentDto.ServiceOrderId),
                UserId = commentDto.UserId,
                Content = commentDto.Content,
                TimestampComment = DateTime.UtcNow
            };
        }

        //[HttpGet("getAll")]
        public ReturnComment MapToDto(Comment comment, string role)
        {
            return new ReturnComment
            {
                CommentId = comment.CommentId,
                Content = comment.Content,
                TimestampComment = comment.TimestampComment,
                User = new ReturnUser
                {
                    Id = comment.User.Id,
                    UserName = comment.User.UserName,
                    Role = role
                }
            };
        }
    }
}

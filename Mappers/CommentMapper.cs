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
    }
}

using api.Models;
using api.Mappers;
using api.Dtos.Comment;
using System.Collections.Generic;

namespace api.Mappers;

public static class CommentMappers
{
    public static CommentDto ToCommentDto(this Comment comment)
    {
        return new CommentDto()
        {
            Id = comment.Id,
            Title = comment.Title,
            Content = comment.Content,
            CreatedOn = comment.CreatedOn,
            CryptoAssetId = comment.CryptoAssetId
        };
    }

    public static Comment ToCommentFromCreateDto(this CreateCommentDto createCommentDto, int cryptoAssetId)
    {
        return new Comment
        {
            Title = createCommentDto.Title,
            Content = createCommentDto.Content,
            CryptoAssetId = cryptoAssetId
        };
    }

}
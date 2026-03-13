namespace order_api.Application.DTO
{
    public record UserInfoResponseDto(
        string Name,
        string Email,
        string Address
    );

    public record UserInfoDto(
        string Id,
        string Name,
        string Email,
        string Address,
        DateTime CreatedAt
    );
}

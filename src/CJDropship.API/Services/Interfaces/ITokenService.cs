namespace CJDropship.API.Services.Interfaces
{
    public interface ITokenService
    {
        public string? token { get; set; }

        Task<string?> GetTokenAsync();
    }
}

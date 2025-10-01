namespace ERestaurant.Application.Services.Middleware.Interfaces
{
    public interface IRequestLanguage
    {
        public string TwoLetter { get; }
        public bool IsArabic { get; }
    }
}

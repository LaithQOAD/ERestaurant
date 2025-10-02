namespace ERestaurant.Application.Services.MiddlewareInterfaces
{
    public interface IRequestLanguage
    {
        public string TwoLetter { get; }
        public bool IsArabic { get; }
    }
}

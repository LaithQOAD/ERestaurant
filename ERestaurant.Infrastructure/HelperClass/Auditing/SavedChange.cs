namespace ERestaurant.Infrastructure.HelperClass.Auditing
{
    public readonly record struct SavedChange(string Entity, Guid Id, string Action);

}

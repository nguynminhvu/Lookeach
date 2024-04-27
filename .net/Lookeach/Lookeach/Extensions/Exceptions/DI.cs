namespace Lookeach.Extensions.Exceptions
{
    public static class DI
    {
        public static void UseLookeachException(this IApplicationBuilder application)
        {

            application.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}

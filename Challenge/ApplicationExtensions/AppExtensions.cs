using Challenge.QueueMessage;

namespace Challenge.ApplicationExtensions
{
    public static class AppExtensions
    {
        private static Receiver _receiver { get; set; }

        public static IApplicationBuilder UseRabbitMQListener(this IApplicationBuilder app)
        {
            _receiver = app.ApplicationServices.GetService<Receiver>();
            var lifetime = app.ApplicationServices.GetService<IHostApplicationLifetime>();
            lifetime.ApplicationStarted.Register(OnStarted);
            lifetime.ApplicationStopping.Register(OnStopping);
            return app;
        }

        private static void OnStarted()
        {
            _receiver.Register();
        }

        private static void OnStopping()
        {
            _receiver.Unregister();
        }
    }
}

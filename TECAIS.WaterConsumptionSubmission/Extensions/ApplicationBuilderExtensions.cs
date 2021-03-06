﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TECAIS.RabbitMq;
using TECAIS.WaterConsumptionSubmission.Handlers;
using TECAIS.WaterConsumptionSubmission.Models.Events;

namespace TECAIS.WaterConsumptionSubmission.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder ConfigureEventBus(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetService<IEventBus>();
            var applicationLifeTime = app.ApplicationServices.GetService<IApplicationLifetime>();
            eventBus.Subscribe<Measurement, MeasurementReceivedEventHandler>("water");
            applicationLifeTime.ApplicationStopping.Register(() => OnStopping(eventBus));
            return app;
        }

        private static void OnStopping(IEventBus eventBus)
        {
            eventBus.Deregister();
        }
    }
}
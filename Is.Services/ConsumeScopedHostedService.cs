using Is.Services.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Is.Services
{
    public class ConsumeScopedHostedService : IHostedService
    {
        private readonly IServiceProvider _service;

        public ConsumeScopedHostedService(IServiceProvider serviceProvider)
        {
            this._service = serviceProvider;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await DoWork();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        private async Task DoWork()
        {
            using(var scope = _service.CreateScope()) {
                var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IBackgroundEmailSender>();
                await scopedProcessingService.DoWork();
            };
        }
    }
}

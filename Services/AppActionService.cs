using back_end.Mediators.Machines;
using back_end.Models;
using back_end.Repositories;
using back_end.Services.Location;
using back_end.Services.ZKEM_Machine;

namespace back_end.Services
{
    public class AppActionService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public AppActionService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var machinesMediator = scope.ServiceProvider.GetRequiredService<IMachinesMediator>();
            var companySetupRepository = scope.ServiceProvider.GetRequiredService<IRepository<CompanySetup>>();
            var locationService = scope.ServiceProvider.GetRequiredService<ILocationService>();

            machinesMediator.ConnectToAllMachines();

            var companySetup = companySetupRepository.GetByFilter(x => x.Id == 1).FirstOrDefault();
            locationService.setCompanyCoordinates(companySetup);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var machinesMediator = scope.ServiceProvider.GetRequiredService<IMachinesMediator>();

            machinesMediator.DisconnectFromAllMachines();

            return Task.CompletedTask;
        }
    }
}

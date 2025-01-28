using AutoMapper;
using back_end.Models;
using back_end.Repositories;
using back_end.Services.ZKEM_Machine;
using back_end.ViewModels.Machine;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace back_end.Mediators.Machines
{
    public class MachinesMediator : IMachinesMediator
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;
        private readonly ConcurrentDictionary<int, IZKMachineService> _zkMachineServices;

        public MachinesMediator(IServiceProvider serviceProvider, IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _mapper = mapper;
            _zkMachineServices = new ConcurrentDictionary<int, IZKMachineService>();
        }

        public List<Machine> GetAllMachines()
        {
            using var scope = _serviceProvider.CreateScope();
            var machineRepository = scope.ServiceProvider.GetRequiredService<IRepository<Machine>>();
            return machineRepository.GetAll().ToList();
        }

        public Machine GetMachineByCode(int machineCode)
        {
            using var scope = _serviceProvider.CreateScope();
            var machineRepository = scope.ServiceProvider.GetRequiredService<IRepository<Machine>>();
            return machineRepository.GetByFilter(x => x.machineCode == machineCode).FirstOrDefault();
        }

        public Machine CreateMachine(MachineViewModel machineViewModel)
        {
            using var scope = _serviceProvider.CreateScope();
            var machineRepository = scope.ServiceProvider.GetRequiredService<IRepository<Machine>>();

            var machine = _mapper.Map<Machine>(machineViewModel);
            machineRepository.Add(machine);
            machineRepository.SaveChanges();
            return machine;
        }

        public bool UpdateMachine(int machineCode, MachineViewModel machineViewModel)
        {
            using var scope = _serviceProvider.CreateScope();
            var machineRepository = scope.ServiceProvider.GetRequiredService<IRepository<Machine>>();

            var machine = machineRepository.GetByFilter(x => x.machineCode == machineCode).FirstOrDefault();
            if (machine == null)
            {
                return false;
            }

            _mapper.Map(machineViewModel, machine);
            machineRepository.Update(machine);
            machineRepository.SaveChanges();

            if (_zkMachineServices.TryGetValue(machineCode, out var zkService))
            {
                zkService.setDeviceNetwork(machine);
            }

            return true;
        }

        public bool ConnectToMachine(int machineCode)
        {
            var machine = GetMachineByCode(machineCode);
            if (machine == null)
            {
                return false;
            }

            var zkService = GetOrCreateMachineService(machine);
            zkService.setDeviceNetwork(machine);
            return zkService.isConnected();
        }

        public void ConnectToAllMachines()
        {
            using var scope = _serviceProvider.CreateScope();
            var machineRepository = scope.ServiceProvider.GetRequiredService<IRepository<Machine>>();
            var machines = machineRepository.GetAll().ToList();

            foreach (var machine in machines)
            {
                var zkService = GetOrCreateMachineService(machine);
                zkService.setDeviceNetwork(machine);
            }
        }

        public IZKMachineService GetMachineService(int machineCode)
        {
            _zkMachineServices.TryGetValue(machineCode, out var zkService);
            return zkService;
        }

        public void DisconnectFromMachine(int machineCode)
        {
            if (_zkMachineServices.TryRemove(machineCode, out var zkService))
            {
                zkService.Dispose();
            }
        }

        public void DisconnectFromAllMachines()
        {
            foreach (var machineCode in _zkMachineServices.Keys)
            {
                if (_zkMachineServices.TryRemove(machineCode, out var zkService))
                {
                    zkService.Dispose();
                }
            }
        }

        private IZKMachineService GetOrCreateMachineService(Machine machine)
        {
            return _zkMachineServices.GetOrAdd(machine.machineCode, machineCode =>
            {
                using var scope = _serviceProvider.CreateScope();
                return scope.ServiceProvider.GetRequiredService<IZKMachineService>();
            });
        }

        public bool DeleteMachine(int machineCode)
        {
            using var scope = _serviceProvider.CreateScope();
            var machineRepository = scope.ServiceProvider.GetRequiredService<IRepository<Machine>>();

            var machine = GetMachineByCode(machineCode);
            if (machine == null)
            {
                return false;
            }

            DisconnectFromMachine(machineCode);

            machineRepository.Delete(machine);
            machineRepository.SaveChanges();

            return true;
        }
    }
}

using back_end.Models;
using back_end.ViewModels.Machine;

namespace back_end.Mediators.Machines
{
    public interface IMachinesMediator
    {
        List<Machine> GetAllMachines();
        Machine GetMachineByCode(int machineCode);
        Machine CreateMachine(MachineViewModel machineViewModel);
        bool UpdateMachine(int machineCode, MachineViewModel machineViewModel);
        bool ConnectToMachine(int machineCode);
        void ConnectToAllMachines();
        void DisconnectFromMachine(int machineCode);
        void DisconnectFromAllMachines();
        bool DeleteMachine(int machineCode);
    }
}

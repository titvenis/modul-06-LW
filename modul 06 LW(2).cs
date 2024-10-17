using System;

public class Computer
{
    public string CPU { get; set; }
    public string RAM { get; set; }
    public string Storage { get; set; }
    public string GPU { get; set; }
    public string OS { get; set; }
    public string Cooling { get; set; }
    public string PowerSupply { get; set; }

    public override string ToString()
    {
        return $"Компьютер: CPU - {CPU}, RAM - {RAM}, Накопитель - {Storage}, GPU - {GPU}, ОС - {OS}, Охлаждение - {Cooling}, БП - {PowerSupply}";
    }
}

public interface IComputerBuilder
{
    void SetCPU();
    void SetRAM();
    void SetStorage();
    void SetGPU();
    void SetOS();
    void SetCooling();
    void SetPowerSupply();
    Computer GetComputer();
}

public class OfficeComputerBuilder : IComputerBuilder
{
    private Computer _computer = new Computer();

    public void SetCPU() => _computer.CPU = "Intel i3";
    public void SetRAM() => _computer.RAM = "8GB";
    public void SetStorage() => _computer.Storage = "1TB HDD";
    public void SetGPU() => _computer.GPU = "Integrated";
    public void SetOS() => _computer.OS = "Windows 10";
    public void SetCooling() => _computer.Cooling = "Air Cooling";
    public void SetPowerSupply() => _computer.PowerSupply = "500W";

    public Computer GetComputer() => _computer;
}

public class GamingComputerBuilder : IComputerBuilder
{
    private Computer _computer = new Computer();

    public void SetCPU() => _computer.CPU = "Intel i9";
    public void SetRAM() => _computer.RAM = "32GB";
    public void SetStorage() => _computer.Storage = "1TB SSD";
    public void SetGPU() => _computer.GPU = "NVIDIA RTX 3080";
    public void SetOS() => _computer.OS = "Windows 11";
    public void SetCooling() => _computer.Cooling = "Liquid Cooling";
    public void SetPowerSupply()
    {
        if (_computer.GPU == "NVIDIA RTX 3080")
        {
            _computer.PowerSupply = "850W";
        }
        else
        {
            throw new InvalidOperationException("Несовместимая видеокарта и блок питания");
        }
    }

    public Computer GetComputer() => _computer;
}

public class ComputerDirector
{
    private IComputerBuilder _builder;

    public ComputerDirector(IComputerBuilder builder)
    {
        _builder = builder;
    }

    public void ConstructComputer()
    {
        _builder.SetCPU();
        _builder.SetRAM();
        _builder.SetStorage();
        _builder.SetGPU();
        _builder.SetOS();
        _builder.SetCooling();
        _builder.SetPowerSupply();
    }

    public Computer GetComputer()
    {
        return _builder.GetComputer();
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Создаем офисный компьютер
        IComputerBuilder officeBuilder = new OfficeComputerBuilder();
        ComputerDirector director = new ComputerDirector(officeBuilder);
        director.ConstructComputer();
        Computer officeComputer = director.GetComputer();
        Console.WriteLine(officeComputer);

        // Создаем игровой компьютер
        IComputerBuilder gamingBuilder = new GamingComputerBuilder();
        director = new ComputerDirector(gamingBuilder);
        director.ConstructComputer();
        Computer gamingComputer = director.GetComputer();
        Console.WriteLine(gamingComputer);
    }
}

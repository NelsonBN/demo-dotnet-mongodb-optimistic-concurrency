namespace Demo.WebAPI.Models;

public class CounterVersion
{
    public Guid Id { get; set; }
    public int Value { get; set; }

    public Guid Version { get; set; }
}


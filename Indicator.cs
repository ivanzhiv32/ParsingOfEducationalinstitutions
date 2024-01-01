public struct Indicator
{
    public string number;
    public string name;
    public string unit_measure;
    public double value;

    public Indicator(string number, string name, string unit_measure, double value)
    {
        this.number = number;
        this.name = name;
        this.unit_measure = unit_measure;
        this.value = value;
    }
}
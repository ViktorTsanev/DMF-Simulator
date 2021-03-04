
public class BoardModel
{
    public Information[] information { get; set; }
    public Electrode[] electrodes { get; set; }
    public object actuators { get; set; }
    public object sensors { get; set; }
    public Input[] inputs { get; set; }
    public Output[] outputs { get; set; }
    public object droplets { get; set; }
    public object bubbles { get; set; }
    public object unclassified { get; set; }
}

public class Information
{
    public string platform_name { get; set; }
    public string platform_type { get; set; }
    public string platform_ID { get; set; }
    public int sizeX { get; set; }
    public int sizeY { get; set; }
}

public class Electrode
{
    public string name { get; set; }
    public int ID { get; set; }
    public int electrodeID { get; set; }
    public int driverID { get; set; }
    public int shape { get; set; }
    public int positionX { get; set; }
    public int positionY { get; set; }
    public int sizeX { get; set; }
    public int sizeY { get; set; }
    public int status { get; set; }
    public object corners { get; set; }
}

public class Input
{
    public string name { get; set; }
    public int ID { get; set; }
    public int inputID { get; set; }
    public int positionX { get; set; }
    public int positionY { get; set; }
}

public class Output
{
    public string name { get; set; }
    public int ID { get; set; }
    public int outputID { get; set; }
    public int positionX { get; set; }
    public int positionY { get; set; }
}

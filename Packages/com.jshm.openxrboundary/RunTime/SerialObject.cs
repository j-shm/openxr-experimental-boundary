
public class SerialObject
{
    public float[] position = new float[3];
    public float[] rotation = new float[3];
    public float[] scale = new float[3];
    public SerialObject(float[] position, float[] rotation, float[] scale)
    {
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
    }
}
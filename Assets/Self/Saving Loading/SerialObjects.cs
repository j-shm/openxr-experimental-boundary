public class SerialObjects
{
    public SerialObject[] objects;
    public float[] position = new float[3];
    public SerialObjects(SerialObject[] objects, float[] position)
    {
        this.objects = objects;
        this.position = position;
    }
}
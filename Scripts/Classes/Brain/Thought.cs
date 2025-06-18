public class Thought
{
    public string description;
    public float duration;

    public Thought(string description, float duration = 2f)
    {
        this.description = description;
        this.duration = duration;
    }
}
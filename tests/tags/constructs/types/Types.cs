namespace DefaultNamespace;

using System;

public interface IPerson
{
    public int Height { get; set; }
}

interface ILogger
{
    void Foo(string message);
    void Bar(string message) => Console.WriteLine(message);
}

public abstract class Animal
{
    public abstract void Run();
    
    public virtual void Speak()
    {
    }
}

public class Cow : Animal
{
    public override void Run()
    {
    }
}

public class Person : IPerson
{
    private int _height;
    
    public int Height
    {
        get
        {
            return _height;
        }
        set
        {
            _height = value;
        }
    }

    public string Name { get; set; }
    public string Age { get; }
    public bool Cool => true;

    public void Reset()
    {
        Name = "foo";
    }

    public bool Uncool => !Cool;

    private class Nested
    {
    }
}

public struct Actress
{
    public string Name;
}

public record Movie(string Title, string Director)
{
    public Movie Sample()
    {
        return new Movie("Jurassic Park", "Steven Spielberg") with {Title = "Saving Private Ryan"};
    }
} 

public enum Rating
{
    Great,
    Okay
}


[Flags]
public enum Genre
{
    Action,
    Documentary,
    Thriller
}

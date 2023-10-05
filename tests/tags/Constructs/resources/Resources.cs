namespace DefaultNamespace;

using System;
using System.IO;

public class Resources : IDisposable
{
    public void ManualDispose()
    {
        var reader = File.OpenText("tmp.json");
        reader.Dispose();
    }
    
    public void AutomaticDisposeInBlock()
    {
        using (var reader = File.OpenText("tmp.json"))
        {            
        }
    }
    
    public void AutomaticDispose()
    {
        using var reader = File.OpenText("tmp.json");
    }
    
    public void Dispose()
    {
    }
}
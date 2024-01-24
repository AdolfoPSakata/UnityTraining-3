using System.Collections.Generic;

public class ServiceLocator : IServiceLocator
{
    private IDictionary<object, object> services;
    
    internal ServiceLocator()
    {
        services = new Dictionary<object, object>();

        //services.Add(typeof(IUIController), new UIController());
    }

    public T GetService<T>()
    {
        try
        {
            return (T)services[typeof(T)];
        }
        catch (KeyNotFoundException)
        {
            throw new System.Exception ("Service not Available. Call dev to complain about it");
        }
    }
}
 

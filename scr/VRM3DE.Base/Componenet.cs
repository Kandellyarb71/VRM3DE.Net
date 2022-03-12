namespace VRM3DE
{
    public interface IUpdateable
    {
        public void OnUpdate();
    }
    
    public interface IDataComponent
    {
        
    }
    
    public interface IDataComponent<Type> where Type : IDataComponent
    {

    }

    public abstract class ActionComponent
    {

    }
}
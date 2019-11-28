namespace Test2Xml.Commands
{
    public abstract class CommandBase
    {
        protected Options Options { get; private set; }
        public abstract void Execute();

        protected CommandBase(Options options)
        {
            Options = options;
        }

        protected void LogVerbose(string message)
        {
            Program.LogVerbose(message);
        }    
    }
}

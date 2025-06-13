namespace SupportHelper.Communication.Requests
{
    public class RequestMachine
    {
        public string Command { get; init; }

        private RequestMachine(string command)
        {
            Command = command;
        }

        public static RequestMachine Create(string command)
        {
            return new RequestMachine(command);
        }
    }
}

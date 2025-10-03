namespace NotificationMotorRentService.EventProcessor
{
    public interface IProcessaEvento
    {
        void Process(string mensagem);
    }
}

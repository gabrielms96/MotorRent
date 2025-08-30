namespace NotificationMotorRentService.EventProcessor
{
    public interface IProcessaEvento
    {
        void Processa(string mensagem);
    }
}

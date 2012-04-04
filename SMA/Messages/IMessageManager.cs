namespace SMA.Messages
{
    public interface IMessageManager : IEcouteur
    {
        void start();
        void stop();

        void consommer();
        void faire(Message msg);
    }
}

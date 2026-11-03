namespace EmotePlayer
{
    public class PlayerManager
    {
        public PlayerController Controller { get; private set; }
        public PlayerCharacter Character { get; private set; }

        public virtual void SetPlayer(PlayerController controller)
        {
            Controller = controller;
            Character = controller?.GetPlayerCharacter();
        }
        public virtual void Refresh()
        {
        }
    }
}

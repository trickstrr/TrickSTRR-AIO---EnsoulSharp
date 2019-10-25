namespace TrickSTRR.AIO.Kalista
{
    using EnsoulSharp;
    using EnsoulSharp.SDK;

    public class Program
    {
        public static void Main()
        {
            GameEvent.OnGameLoad += delegate
            {
                if (ObjectManager.Player.CharacterName != "Kalista")
                    return;

                Kalista.OnLoad();
            };
        }
    }
}

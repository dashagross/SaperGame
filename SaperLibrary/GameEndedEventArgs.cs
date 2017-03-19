namespace SaperLibrary
{
    public class GameEndedEventArgs
    {
        public GameEndStates e;
        
        public GameEndedEventArgs(GameEndStates e)
        {
            this.e = e;
        }
    }
}
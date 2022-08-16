namespace SRC.HTTP.Models
{
    public class LocalGameStateRequestModel
    {
        public int CurrentStage;

        public static LocalGameStateRequestModel Create(SRC.Core.GMApplication app)
        {
            return new()
            {
                CurrentStage = app.GameState.Stage
            };
        }
    }
}

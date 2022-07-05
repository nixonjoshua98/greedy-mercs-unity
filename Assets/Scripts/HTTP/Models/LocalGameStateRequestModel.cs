namespace GM.HTTP.Models
{
    public class LocalGameStateRequestModel
    {
        public int CurrentStage;

        public static LocalGameStateRequestModel Create(GM.Core.GMApplication app)
        {
            return new()
            {
                CurrentStage = app.GameState.Stage
            };
        }
    }
}

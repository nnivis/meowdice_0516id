namespace CodeBase.Infrastructure.SceneLoad
{
    public interface ILoadingScreen
    {
        public void Show();

        public void SetProgress(float value);

        public void Hide();
    }
}
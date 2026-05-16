using System.Collections;

namespace CodeBase.Infrastructure.SceneLoad
{
    public interface IStartupSequence
    {
        public IEnumerator Run();
    }
}
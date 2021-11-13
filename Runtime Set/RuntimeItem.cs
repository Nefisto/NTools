// This came from Ryan Hipple talk: https://www.youtube.com/watch?v=raQ3iHhE_Kk&t=184s 

namespace NTools
{
    public class RuntimeItem : LazyBehavior
    {
        public RuntimeSet runtimeSet;

        private void OnEnable()
            => runtimeSet.Add(this);

        private void OnDisable()
            => runtimeSet.Remove(this);
    }
}

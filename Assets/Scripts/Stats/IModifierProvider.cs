using System.Collections.Generic;

namespace RPG.Stats
{
    public interface IModifierProvider
    {

        //Enumerat over floats to do a foreach loop
        IEnumerable<float> GetAdditiveModifiers(Stat stat);

        IEnumerable<float> GetPercentageModifiers(Stat stat);

    }


}



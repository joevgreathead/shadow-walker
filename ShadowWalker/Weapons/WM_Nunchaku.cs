using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShadowWalker
{
    class WM_Nunchaku : MeleeWeaponManager, WeaponBehaviorManager
    {
        public float useWeapon(/*PlayerObjects p, IEnemyObjects e*/)
        {
            Console.Out.WriteLine("Enter The Dragon, Nunchuck POWER!");
            return 10.0f;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShadowWalker
{
    class WP_M4 : PrimaryWeaponManager, WeaponBehaviorManager
    {
        public float useWeapon(/*PlayerObjects p, IEnemyObjects e*/)
        {
            Console.Out.WriteLine("TinkaTinkaTinka Pow!, M4 Carbine Fired!!!");
            return 70.0f;
        }
    }
}

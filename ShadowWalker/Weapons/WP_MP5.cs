using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShadowWalker
{
    class WP_MP5 : PrimaryWeaponManager, WeaponBehaviorManager
    {
        public float useWeapon(/*PlayerObjects p, IEnemyObjects e*/)
        {
            Console.Out.WriteLine("Rat a tat tat, MP5 Fired!!!");
            return 40.0f;
        }
    }
}

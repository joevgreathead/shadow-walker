using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShadowWalker
{
    class WP_P90 : PrimaryWeaponManager, WeaponBehaviorManager
    {
        public float useWeapon(/*PlayerObjects p, IEnemyObjects e*/)
        {
            Console.Out.WriteLine("TAK ATAK KATAK, P90 Fired!!!");
            return 50.0f;
        }
    }
}

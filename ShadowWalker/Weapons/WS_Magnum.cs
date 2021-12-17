using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShadowWalker
{
    class WS_Magnum : SecondaryWeaponManager, WeaponBehaviorManager
    {
        public float useWeapon(/*PlayerObjects p, IEnemyObjects e*/)
        {
            Console.Out.WriteLine("Bang!, 357 Magnum Fired!!!");
            return 25.0f;
        }
    }
}

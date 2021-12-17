using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShadowWalker
{
    class WM_CombatKnife : MeleeWeaponManager, WeaponBehaviorManager
    {
        public float useWeapon(/*PlayerObjects p, IEnemyObjects e*/)
        {
            Console.Out.WriteLine("Slashy Slashy Stab COMBAT KNIFE!!!!");
            return 15.0f;
        }
    }
}

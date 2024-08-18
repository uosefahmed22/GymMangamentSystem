using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.Enums.Business
{
    public enum MembershipType
    {
        // Gold: 1-month access, includes full gym access and fitness classes.
        Gold_1Month,

        // Gold: 3-month access, includes full gym access, fitness classes, and 2 personal training sessions.
        Gold_3Months,

        // Gold: 6-month access, includes full gym access, fitness classes, 5 personal training sessions, and a nutrition plan.
        Gold_6Months,

        // Platinum: 1-month access, includes all Gold benefits plus unlimited personal training sessions.
        Platinum_1Month,

        // Platinum: 3-month access, includes all Gold benefits, unlimited personal training, and access to VIP lounge.
        Platinum_3Months,

        // Platinum: 6-month access, includes all Gold benefits, unlimited personal training, VIP lounge access, and a wellness consultation.
        Platinum_6Months,

        // Silver: 1-month access, includes gym access only.
        Silver_1Month,

        // Silver: 3-month access, includes gym access and 1 fitness class per week.
        Silver_3Months,

        // Silver: 6-month access, includes gym access and 2 fitness classes per week.
        Silver_6Months,

        // Bronze: 1-month access, includes gym access only.
        Bronze_1Month,
        // Bronze: 3-month access, includes gym access and 1 fitness class per week.
        Bronze_3Months,
        // Bronze: 6-month access, includes gym access and 2 fitness classes per week.
        Bronze_6Months,
        // Diamond: 1-month access, includes all Gold benefits plus unlimited personal training sessions.
        Diamond_1Month,
        // Diamond: 3-month access, includes all Gold benefits, unlimited personal training, and access to VIP lounge.
        Diamond_3Months,
        // Diamond: 6-month access, includes all Gold benefits, unlimited personal training, VIP lounge access, and a wellness consultation.
        Diamond_6Months
    }
}

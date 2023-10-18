using Microsoft.OpenApi.Any;
using System.Xml.Linq;

namespace GiftDice_Backend.Data
{
    public static class StaticUsers
    {
        public static List<string> AllUsers { get; set; } = new List<string>()
        {
            "Kamil", "Ania", "Oliwka", "Danusia", "Tomek", "Arek", "Asia"
        };

        public static List<string> UsersToRoll { get; set; } = new List<string>()
        {
            "Kamil", "Ania", "Oliwka", "Danusia", "Tomek", "Arek", "Asia"
        };

        public static List<UserCode> UserCodes { get; set; } = new List<UserCode>
        {
            new UserCode { Code = 2137420, Name = "Kamil", CanRoll = true },
            new UserCode { Code = 4653461, Name = "Ania", CanRoll = true  },
            new UserCode { Code = 4512312, Name = "Oliwka", CanRoll = true  },
            new UserCode { Code = 1643346, Name = "Danusia", CanRoll = true },
            new UserCode { Code = 6533233, Name = "Tomek", CanRoll = true },
            new UserCode { Code = 1531531, Name = "Arek", CanRoll = true },
            new UserCode { Code = 4622452, Name = "Asia", CanRoll = true },
        };

        public static string Roll(int code)
        {
            var currentUser = UserCodes.FirstOrDefault(user => user.Code == code);
            if (currentUser == null || !AllUsers.Contains(currentUser.Name))
                throw new Exception("Niepoprawny kod");

            if (!currentUser.CanRoll)
                throw new Exception($"Juz losowales/as, wylosowales/as '{currentUser.RolledUser}'");

            if (UsersToRoll.Count == 0)
            {
                if (UserCodes.Any(user => user.RolledUser == ""))
                    throw new Exception($"Wystapil paradoks losowania nieparzystego. Zresetowano liste, losowanie nalezy powtorzyc");
                else
                    throw new Exception($"Nieznany blad, nalezy powtorzyc losowanie");
            }

            bool hasDuplicateRolledUser = UserCodes
                .Where(user => user.RolledUser != "")
                .GroupBy(user => user.RolledUser)
                .Any(group => group.Count() > 1);

            if (hasDuplicateRolledUser )
                throw new Exception($"Nieznany blad, nalezy powtorzyc losowanie");

            var temporaryRollingList = UsersToRoll.Where(user => user != currentUser.Name).ToList();
            var rolledUser = temporaryRollingList.OrderBy(g => Guid.NewGuid()).First();
            UsersToRoll.Remove(rolledUser);
            currentUser.CanRoll = false;
            currentUser.RolledUser = rolledUser;

            return rolledUser.ToString();
        }
    }

    public class UserCode
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public bool CanRoll { get; set; }

        public string RolledUser { get; set; } = "";
    }
}

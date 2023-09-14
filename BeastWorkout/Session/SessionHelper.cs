using BeastWorkout.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace BeastWorkout.Session
{
    public static class SessionHelper
    {
        public static Person person { get; private set; }

        public static void SetUser(this ISession session, Person value)
        {
            session.SetString("Person", JsonConvert.SerializeObject(value));
            person = value;
        }

        public static Person GetUser(this ISession session)
        {
            return JsonConvert.DeserializeObject<Person>(session.GetString("Person"));
        }

        public static void Disconnect(this ISession session)
        {

            session.Remove("AccessToken");
            session.Clear();
            person = null;
        }
    }
}

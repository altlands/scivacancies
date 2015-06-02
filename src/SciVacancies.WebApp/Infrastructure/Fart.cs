using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.WebApp.Infrastructure
{
    public class Fart : IFart
    {
        public string GetFartSound()
        {
            return "loud";
        }
    }

    public interface IFart
    {
        string GetFartSound();
    }
}

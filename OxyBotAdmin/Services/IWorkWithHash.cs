using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxyBotAdmin.Services
{
    public interface IWorkWithHash
    {
        string CalculateHash(string inputString);
    }
}

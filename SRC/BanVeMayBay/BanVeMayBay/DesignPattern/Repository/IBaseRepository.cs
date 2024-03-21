using BanVeMayBay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanVeMayBay.DesignPattern.Repository
{
    public interface IBaseRepository
    {
        string RandomPassword(int numberUpTo); //apply in line ... (...Controller)
        void TongtienDAT(string makh, BANVEMAYBAYEntities db); //apply in line ... (...Controller)
    }
}

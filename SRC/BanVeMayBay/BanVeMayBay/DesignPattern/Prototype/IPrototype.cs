using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanVeMayBay.DesignPattern.Prototype
{
    public interface IPrototype
    {
        IPrototype Clone(); //apply in line 159 Admin/PostController
    }
}

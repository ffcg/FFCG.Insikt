using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Api;

namespace GameEngine
{
    public interface IUserActionHandler<in TUserAction> where TUserAction : UserAction
    {
        void Handle(TUserAction userAction);
    }
}

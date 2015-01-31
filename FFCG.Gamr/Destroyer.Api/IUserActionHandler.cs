using Destroyer.Actions;

namespace Destroyer
{
    public interface IUserActionHandler<in TUserAction> where TUserAction : UserAction
    {
        void Handle(TUserAction userAction);
    }
}

namespace Ataoge.Data
{
    public interface IPermissionAssign
    {
        int ResourceId {get;}

        int RoleId {get;}

        int Operation {get;}

        int IsRefused {get;}
    }
}